# Implementation Plan — Issue #101: `GET /gis/buildingheatdemand/summariesbycountyids`

> Sub-issue of DiGi-Organization/DiGi.Analytical#10 (Monthly Heat Demand).
> Sibling: DiGi-Organization/DiGi.GIS.PostgreSQL#146 (done — provides the query and result type in the main assembly).

---

## 1. Context

This issue adds a read-only route to `DiGi.GIS.WebAPI` that serves the summary of stored heat-demand results, so the production run (DiGi.GIS.PostgreSQL#144) can measure it through `api.digiproject.uk`.

### What the sibling issue (#146) provides (done)

| Item | Assembly | Namespace |
|---|---|---|
| `BuildingHeatDemandCountyResult` (22 fields) | `DiGi.GIS.PostgreSQL` (main) | `DiGi.GIS.PostgreSQL.Classes` |
| `Query.BuildingHeatDemandCountyResultsAsync` (static extension on `NpgsqlConnection?`) | `DiGi.GIS.PostgreSQL` (main) | `DiGi.GIS.PostgreSQL` |
| `Constants.TableName.BuildingHeatDemand` | `DiGi.GIS.PostgreSQL` (main) | `DiGi.GIS.PostgreSQL` |

Query method signature:

```csharp
public static async Task<List<BuildingHeatDemandCountyResult>?> BuildingHeatDemandCountyResultsAsync(
    this NpgsqlConnection? npgsqlConnection,
    IEnumerable<int>? countyIds,
    int? methodVersion = null,
    IEnumerable<short>? yearBuiltBounds = null,
    int commandTimeout = 600,
    CancellationToken cancellationToken = default)
```

- `npgsqlConnection` null → returns `null`
- Table missing → returns empty list
- `yearBuiltBounds` is sorted and de-duplicated inside the query (via `SortedSet<short>`)

### Key constraint

**No reference to `DiGi.GIS.PostgreSQL.Energy`** in `DiGi.GIS.WebAPI`. Pulling it in would add the solar stack and its NuGet packages to `DiGi.WebAPI.WindowsService` for a read that needs none of them.

### Connection strategy

The controller injects `BuildingSolarRadiationPostgreSQLConverter` (registered as a singleton by `GISPostgreSQLConverterManager` with the Storage `ConnectionData`) and uses its `ConnectionData` to open a connection — the same way the solar summary reaches its Storage database. No new DI registration is needed.

### Already available (no new references required)

| Dependency | Status |
|---|---|
| `DiGi.GIS.PostgreSQL` (main) | Already referenced in `DiGi.GIS.WebAPI.csproj` |
| `DiGi.Analytical.Building` (for `BuildingType` enum) | Already referenced in `DiGi.GIS.WebAPI.csproj` |
| `DiGi.WebAPI` (base class `WebAPIController`) | Already referenced |
| `Npgsql` | Already referenced |

---

## 2. Files to Create

### 2.1 Controller

**Path:** `DiGi.GIS.WebAPI/DiGi.GIS.WebAPI/Classes/Controller/BuildingHeatDemandController.cs`

#### Class structure

```csharp
using DiGi.GIS.PostgreSQL;
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.PostgreSQL.Classes;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Serves the summary of the stored heat-demand results (<c>building_heat_demand</c>).
    /// <para>A read open to every visitor: counts and aggregates only, no building data leaves.
    /// The production measure of a heat-demand run (DiGi.Analytical#10): the run keeps its
    /// counts in memory, and production is read only through this API, never through a
    /// configuration file (<c>Coding - PostgreSQL.md</c> §6).</para>
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class BuildingHeatDemandController : DiGi.WebAPI.Classes.WebAPIController
    {
        private readonly BuildingSolarRadiationPostgreSQLConverter buildingSolarRadiationPostgreSQLConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingHeatDemandController"/> class.
        /// This is the controller's only public constructor: MVC activates it through
        /// <c>ActivatorUtilities</c>, and a second one would answer every action with 500
        /// (<c>Coding - WebAPI Contracts.md</c> §5).
        /// </summary>
        /// <param name="buildingSolarRadiationPostgreSQLConverter">
        /// The Storage-database converter holding the <see cref="ConnectionData"/> this controller
        /// uses to open its own connection for the heat-demand summary query.
        /// </param>
        public BuildingHeatDemandController(BuildingSolarRadiationPostgreSQLConverter buildingSolarRadiationPostgreSQLConverter)
        {
            this.buildingSolarRadiationPostgreSQLConverter = buildingSolarRadiationPostgreSQLConverter;
        }

        // ... GetSummariesByCountyIdsAsync action ...
    }
}
```

#### Action method — `GetSummariesByCountyIdsAsync`

**Attributes:**

```csharp
[HttpGet("summariesbycountyids", Name = $"{nameof(BuildingHeatDemandController)}_{nameof(GetSummariesByCountyIdsAsync)}")]
[ApiExplorerSettings(IgnoreApi = false)]
[ProducesResponseType(typeof(List<BuildingHeatDemandCountyResult>), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
```

**Signature:**

```csharp
public async Task<IActionResult> GetSummariesByCountyIdsAsync(
    [FromQuery(Name = "countyids")] List<int>? countyIds,
    [FromQuery(Name = "methodversion")] int? methodVersion = null,
    [FromQuery(Name = "yearbuiltbounds")] List<short>? yearBuiltBounds = null,
    [Minimum(0), FromQuery(Name = "commandtimeout")] int commandTimeout = 600,
    CancellationToken cancellationToken = default)
```

**XML documentation** (on the method and every parameter):

```xml
/// <summary>
/// Summarises what each of the named county parts holds per method version, building type and year-built band:
/// the rows stored, the shaded and unshaded counts, the total and per-area heating energy need with its
/// deciles, the design heat load, the quality counts (excluded components, glazing fallbacks, missing
/// irradiation), and the first and last write.
/// <para>The production measure of a heat-demand run (DiGi.Analytical#10): the run keeps its counts in memory,
/// and production is read only through this API, never through a configuration file (<c>Coding - PostgreSQL.md</c> §6).
/// Counts and aggregates only - no building data leaves. Open to every visitor, like the other <c>summariesbycountyids</c> reads.</para>
/// <para>Naming no county part summarises every part holding results. A part holding none is absent rather than
/// present with zeros, and a missing table answers an empty list.</para>
/// </summary>
/// <param name="countyIds">The identifiers of the county parts to summarise, repeated once per part
/// (<c>countyids=55417&amp;countyids=53477</c>). Omit to summarise every part.</param>
/// <param name="methodVersion">The method version to summarise; omit to summarise every version, one row each.</param>
/// <param name="yearBuiltBounds">The ascending band bounds of the year built (e.g. the TABULA PL periods),
/// repeated once per bound. Omit for one band of every year. Must be strictly ascending after de-duplication;
/// a descending or non-ascending sequence is refused with HTTP 400.</param>
/// <param name="commandTimeout">The timeout in seconds for the execution of each command.
/// A value of 0 disables the timeout; a negative value is refused with HTTP 400. Defaults to 600 seconds.</param>
/// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
/// <returns>A task that represents the asynchronous operation. 200 with a JSON list of
/// <see cref="BuildingHeatDemandCountyResult"/> (empty when nothing is stored or the table is missing),
/// 400 for a negative timeout or non-ascending year-built bounds, 503 for a transient database failure, or 500.</returns>
```

**Body logic** (following `BuildingSolarRadiationController.GetSummariesByCountyIdsAsync` exactly):

```
1. Log start: "{Type}:{Name} started for {CountyCount} county parts"

2. Guard: commandTimeout < 0
   → Warning log: "{Type}:{Name} rejected: commandtimeout cannot be negative"
   → return BadRequest()

3. Guard: yearBuiltBounds not strictly ascending after de-duplication
   → Deduplicate (keep first occurrence), then check each element > previous
   → Warning log: "{Type}:{Name} rejected: yearbuiltbounds not strictly ascending after de-duplication"
   → return BadRequest()

4. try:
     ConnectionData? connectionData = this.buildingSolarRadiationPostgreSQLConverter.ConnectionData;
     if (connectionData is null)
       → results = null
     else
       → using NpgsqlConnection npgsqlConnection = DiGi.PostgreSQL.Create.NpgsqlConnection(connectionData);
         → results = await npgsqlConnection.BuildingHeatDemandCountyResultsAsync(
               countyIds is null || countyIds.Count == 0 ? null : countyIds,
               methodVersion,
               yearBuiltBounds,
               commandTimeout,
               cancellationToken);

5. catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
   → throw

6. catch (NpgsqlException exception) when (exception.IsTransient)
   → Log: "{Type}:{Name} failed (transient database failure)"
   → HttpContext.Response.Headers["Retry-After"] = "30"
   → return StatusCode(503, "Database temporarily unavailable; retry shortly")

7. catch (Exception exception)
   → Log: "{Type}:{Name} failed"
   → return StatusCode(500, "Internal server error during database query")

8. results is null
   → Error log: "{Type}:{Name} failed: no connection to the Storage database"
   → return StatusCode(500, "Heat demand summaries could not be read")

9. results.Count == 0
   → return Content("[]", "application/json")

10. string? json = Core.Convert.ToSystem_String(results)
    if (string.IsNullOrWhiteSpace(json))
      → Error log: "{Type}:{Name}: {Count} summaries could not be serialized"
      → return StatusCode(500, "Heat demand summaries could not be serialized")

11. Log: "{Type}:{Name} returns {Count} summaries"
    → return Content(json, "application/json")
```

**`yearBuiltBounds` validation detail:**

```csharp
if (yearBuiltBounds is not null && yearBuiltBounds.Count > 1)
{
    List<short> yearBuiltBounds_Deduplicated = [];
    foreach (short bound in yearBuiltBounds)
    {
        if (!yearBuiltBounds_Deduplicated.Contains(bound))
        {
            yearBuiltBounds_Deduplicated.Add(bound);
        }
    }

    for (int i = 1; i < yearBuiltBounds_Deduplicated.Count; i++)
    {
        if (yearBuiltBounds_Deduplicated[i] <= yearBuiltBounds_Deduplicated[i - 1])
        {
            Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Warning,
                "{Type}:{Name} rejected: yearbuiltbounds not strictly ascending after de-duplication",
                nameof(BuildingHeatDemandController), nameof(GetSummariesByCountyIdsAsync));
            return BadRequest();
        }
    }
}
```

Edge cases:
- `null` or empty → skip validation (means "one band of every year")
- Single element → valid (strictly ascending trivially)
- `[1946, 1946, 1967]` → dedup → `[1946, 1967]` → valid
- `[1967, 1946]` → dedup → `[1967, 1946]` → **400**
- `[1946, 1986, 1967]` → dedup → `[1946, 1986, 1967]` → **400**

---

## 3. Files to Modify

### 3.1 `DiGi.Test/DiGi.GIS.WebAPI.xUnit/Facts/FlooredQueryParameter.cs`

**Change 1** — Named list (`GisControllers_FlooredQueryParameters_CarryMinimum`): add:

```csharp
(typeof(BuildingHeatDemandController), nameof(BuildingHeatDemandController.GetSummariesByCountyIdsAsync), "commandTimeout", 0, false),
```

**Change 2** — Assembly-wide count (`GisControllers_CommandTimeoutParameters_AllFloored`): update:

```csharp
// 76 since buildingheatdemand/summariesbycountyids (DiGi.GIS.WebAPI#101).
Assert.Equal(76, count);
```

### 3.2 `DiGi.Test/DiGi.GIS.WebAPI.xUnit/Facts/RequiredQueryParameter.cs`

Add to the **optional parameters** list (`GisControllers_OptionalQueryParameters_AreNotBindRequired`):

```csharp
(typeof(BuildingHeatDemandController), nameof(BuildingHeatDemandController.GetSummariesByCountyIdsAsync), "countyIds"),
(typeof(BuildingHeatDemandController), nameof(BuildingHeatDemandController.GetSummariesByCountyIdsAsync), "methodVersion"),
(typeof(BuildingHeatDemandController), nameof(BuildingHeatDemandController.GetSummariesByCountyIdsAsync), "yearBuiltBounds"),
```

> All three are nullable and optional — no `[BindRequired]`. `commandTimeout` has a default and is covered by the floored fact.

---

## 4. New Test File

### 4.1 `DiGi.Test/DiGi.GIS.WebAPI.xUnit/Facts/BuildingHeatDemandController_Summaries.cs`

Following `BuildingSolarRadiationController_Summaries.cs` exactly.

```csharp
using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.WebAPI.Classes;
using DiGi.PostgreSQL.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.xUnit
{
    public partial class Facts
    {
        // Fact 1: QueryParameterNames [Fact]
        // Fact 2: Failures [Fact]
        // Fact 3: RoundTrip [MediumSkippableFact]
    }
}
```

#### Fact 1: `BuildingHeatDemandController_Summaries_QueryParameterNames`

- **Type:** `[Fact]`
- **Purpose:** Pins the `[FromQuery]` names: `["countyids", "methodversion", "yearbuiltbounds", "commandtimeout"]`. A renamed parameter is silently ignored and leaves the parameter at its default, so the production measure would summarise every county without saying so (`Coding - WebAPI Contracts.md` §1).
- **Reference:** DiGi.GIS.WebAPI#101.
- **Implementation:** Reflect `BuildingHeatDemandController.GetSummariesByCountyIdsAsync`, collect all `[FromQuery]` names, assert equality with the expected list.

#### Fact 2: `BuildingHeatDemandController_Summaries_Failures`

- **Type:** `[Fact]`
- **Purpose:** Verifies the failure answers without a database:
  - Negative `commandTimeout` → `BadRequestResult`
  - Descending `yearBuiltBounds` (e.g. `[1967, 1946]`) → `BadRequestResult`
  - Valid params but no connection → `ObjectResult` with status 500 (never an empty list, which would read as "nothing stored")
- **Reference:** DiGi.GIS.WebAPI#101.
- **Setup:**
  ```csharp
  using GISWebAPIConfigurationFileWatcher watcher = new(SolarRadiationConfigurationFilePath(true));
  BuildingSolarRadiationPostgreSQLConverter converter_NoConnection = new(null);
  BuildingHeatDemandController controller = new(converter_NoConnection);
  ```
- **Assertions:**
  ```csharp
  Assert.IsType<BadRequestResult>(await controller.GetSummariesByCountyIdsAsync([1], null, null, -1));
  Assert.IsType<BadRequestResult>(await controller.GetSummariesByCountyIdsAsync([1], null, [1967, 1946], 30));

  ObjectResult objectResult = Assert.IsType<ObjectResult>(await controller.GetSummariesByCountyIdsAsync([1], null, null, 30));
  Assert.Equal(500, objectResult.StatusCode);
  ```

#### Fact 3: `BuildingHeatDemandController_Summaries_RoundTrip`

- **Type:** `[MediumSkippableFact]` (two live database servers; drops scratch partitions before and after)
- **Purpose:** Verifies the summary read end-to-end on the throwaway Storage test database:
  - Stored results round-trip through `ContentResult.Content` back into `List<BuildingHeatDemandCountyResult>`
  - Assert on `CountyId`, `MethodVersion`, `BuildingType`, `YearBuiltMin`, `YearBuiltMax`, `BuildingCount`, `ShadedCount`, `UnshadedCount`, `HeatingEnergyNeed`, `FirstCreatedAt`
  - A part holding nothing → `ContentResult` with `"[]"`
- **Reference:** DiGi.GIS.WebAPI#101.
- **Setup:**
  ```csharp
  (ConnectionData connectionData_Main, ConnectionData connectionData_Storage) = SolarRadiationTestConnectionDatas();
  using GISWebAPIConfigurationFileWatcher watcher = new(SolarRadiationConfigurationFilePath(true));
  BuildingHeatDemandController controller = new(new BuildingSolarRadiationPostgreSQLConverter(connectionData_Storage));
  ```
- **Seed:** Use `BuildingHeatDemandPostgreSQLConverter` (from the Energy assembly, available in the test project) to store a scratch result under a scratch county part.
- **Cleanup:** Drop scratch partitions in `finally`.

---

## 5. Implementation Order

| Step | Action | Guideline reference |
|---|---|---|
| 1 | Run `digi-status` to confirm branch and workspace state | `GitHub - Branches and Commits.md` |
| 2 | Create `BuildingHeatDemandController.cs` | `Coding - General.md` §1, §2; `Coding - WebAPI Contracts.md` §1–§5; `XML Documentation.md` |
| 3 | Build `DiGi.GIS.WebAPI` — verify 0 warnings, no `DiGi.GIS.PostgreSQL.Energy` reference | `digi-verify` step 2 |
| 4 | Create `BuildingHeatDemandController_Summaries.cs` test file | `Coding - Automatic Tests.md` |
| 5 | Update `FlooredQueryParameter.cs` (named list + count) | — |
| 6 | Update `RequiredQueryParameter.cs` (optional parameters list) | — |
| 7 | Run `CheckGuidelineRules.ps1 -Changed` | `digi-verify` step 1 |
| 8 | Run `TestSummary.ps1 -Library DiGi.GIS.WebAPI.csproj -Project DiGi.GIS.WebAPI.xUnit` | `digi-verify` step 2 |
| 9 | Run `BuildAll.ps1 -Summary` to verify all consumers build | `digi-verify` step 3 |
| 10 | Commit on the active version branch (no branch/worktree/PR unless asked) | `GitHub - Branches and Commits.md` |
| 11 | Post-deployment: verify `/swagger/gis/swagger.json` lists the route; call for 46769, 49654, 53477, 55417 → 200 | `Coding - Deployed WebAPI.md` |

---

## 6. Guideline Compliance Checklist

| Guideline | Requirement | How met |
|---|---|---|
| **Coding - General** §1.1–1.5 | No `var`, block-scoped namespaces, target-typed `new()`, collection expressions `[]`, 0 warnings | Controller and tests use explicit types, block-scoped namespaces, `[]` |
| **Coding - General** §1.3 | Type-prefixed naming | `buildingSolarRadiationPostgreSQLConverter`, `yearBuiltBounds_Deduplicated` |
| **Coding - General** §1.6 | ≤ 7 params on one line | Action has 5 params |
| **Coding - General** §1.8 | `CancellationToken` last | Last parameter |
| **Coding - General** §1.21 | Serilog `"{Type}:{Name} …"` templates | All log statements |
| **Coding - General** §1.22 | Rethrow only requested cancellation, before general catch | `catch (OperationCanceledException)` first |
| **Coding - General** §2 | No service classes; data in `/Classes` | Controller in `/Classes/Controller/` |
| **Coding - WebAPI Contracts** §1 | Renamed `[FromQuery]` is silently ignored | QueryParameterNames fact pins the names |
| **Coding - WebAPI Contracts** §2 | Nullable binding, `null` rejected | `countyIds`, `methodVersion`, `yearBuiltBounds` are nullable |
| **Coding - WebAPI Contracts** §5 | One public constructor | Single constructor |
| **Coding - WebAPI Contracts** | `Content(...)` not `Ok(x)` | `Content(json, "application/json")` |
| **Coding - WebAPI Contracts** | Failure statuses never read as empty | `null` → 500, not `[]` |
| **Coding - PostgreSQL** §6 | Production read only through the API | This route serves the production measure |
| **Coding - GIS Administrative Data** | County part ids, not codes | `countyIds` are `administrative_areal_2d.id` |
| **XML Documentation** | Every public member, params in signature order | Action + all params documented |
| **Coding - Automatic Tests** §2 | `partial class Facts`, `<summary>` on every fact, no `using Xunit;` | Test file follows this |
| **Coding - Automatic Tests** §4 | Build library before `dotnet test`; `TestSummary.ps1` | Step 8 |
| **Coding - Automatic Tests** | `[Fact]` ≤ 2 s, `[MediumSkippableFact]` for live DB | QueryParameterNames and Failures are `[Fact]`; RoundTrip is `[MediumSkippableFact]` |
| **GitHub - Branches and Commits** | Commit on active version branch | Step 10 |
| **GitHub - Issues** | Mandatory labels, assignee | Already set on issue #101 |

---

## 7. Acceptance Criteria Mapping

| Criterion | Verification step |
|---|---|
| Route implemented and documented; 0 warnings; no `DiGi.GIS.PostgreSQL.Energy` reference | Step 3 (build + grep) |
| Facts green through `TestSummary.ps1` | Step 8 |
| `/swagger/gis/swagger.json` lists the route | Step 11 (post-deployment) |
| Call for 46769, 49654, 53477, 55417 answers 200 (empty list before the run) | Step 11 (post-deployment) |

---

## 8. Risks & Notes

1. **Connection disposal**: The query method takes an open `NpgsqlConnection`. The controller must dispose it. Use `using` around the connection. The solar pattern uses the converter's instance method which opens and disposes internally — here the controller does it directly.

2. **`yearBuiltBounds` validation**: The query sorts and de-duplicates internally (via `SortedSet<short>`), so it handles any order. The route's 400 for non-ascending bounds is a **contract choice** of the route — it rejects inputs the query could handle, to give the client early feedback.

3. **`BuildingType` enum**: `BuildingHeatDemandCountyResult.BuildingType` is `DiGi.Analytical.Building.Regulation.Enums.BuildingType`. The WebAPI already references `DiGi.Analytical.Building`, so no new reference is needed.

4. **DI registration**: No new DI registration is needed. `BuildingSolarRadiationPostgreSQLConverter` is already registered as a singleton by `GISPostgreSQLConverterManager`.

5. **`[TransientDatabaseFailure]` attribute**: Applied at class level (like the solar controller). Catches transient `NpgsqlException` and returns 503 + `Retry-After: 30`. The explicit `catch (NpgsqlException)` in the action is a belt-and-suspenders measure (the solar controller has both).

6. **Test project references**: The test project (`DiGi.GIS.WebAPI.xUnit`) already references both `DiGi.GIS.WebAPI` and `DiGi.GIS.PostgreSQL.Energy` (for the converter used in seeding). No new references needed.
