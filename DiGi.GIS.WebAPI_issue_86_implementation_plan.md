# Implementation plan — DiGi.GIS.WebAPI #86

> **Live-verify polygon-based area count and AdministrativeAreal2DId scope (#83) on api.digiproject.uk**
> Issue: [#86](https://github.com/DiGi-Organization/DiGi.GIS.WebAPI/issues/86) · `type: test`, `priority: high`, `ai: light`, `status: blocked` · assignee `ZiolkowskiJakub`
> Kind: **production verification, no code change** (`GitHub - Issues.md` §3; `Coding - Deployed WebAPI.md`). Investigated 2026-11-18 against the issue, its comments, the code and the live API (read-only GETs only).

---

## 1. Verdict

The issue is **ready to finish**. Its blockers are done and the build it was waiting for is live. What is left is a short run of
POST requests plus the reporting steps, all of which the issue's last comment lists as "Remaining here after the #90 deploy".

| # | Finding | Evidence | Effect |
|---|---|---|---|
| F1 | **Blockers closed.** #83, #89 and #90 are closed (`blocked_by` GET on #86 lists all three as closed). | `gh api .../issues/86/dependencies/blocked_by` | Remove the `status: blocked` label when the run starts (`GitHub - Labels.md`). |
| F2 | **The #89 fix is deployed.** Live `DiGi.GIS.PostgreSQL` = `0.8.11.20261118081736` (service `0.8.8.20261118081858`, started 2026-11-18 08:33 UTC). The fix `c9dea3d` was committed 2026-11-17 09:02 +01:00. | `GET /information/version` | #90's first check passes. The build timestamp is newer than the fix, but `/version` without the key shows no commit hash. Treat the first 55413 result as the proof. |
| F3 | **Reference-count anchors (read-only GET, 2026-11-18).** 55413 → **6 435**, 55426 → **11 682**, 55626 → **6 435**. Each came back 200 in about 0.24 s. | `GET gis/building2d/building2dreferencesbyadministrativeareal2did?administrativeareal2did=<id>`, counted from the response | Expected values for the three Subdivision rows. They match #90's figures. |
| F4 | **Every remaining check is a POST.** `countbyadministrativeareal2did`, `aggregatesummary/singlevalue` and `tablebyfiltergroup` are `HttpPost`. | `BuildingDataController.cs:1182`, `:1580`, `:1736` | Needs **explicit authorization** from the user before the run (`Coding - Deployed WebAPI.md` §2). Not given yet. |
| F5 | **Two of the issue's expectations are stale.** (a) Mokotów 55426 is **11 682**, not about 6 435 (already corrected in the comments). (b) The 55626 row is already measured: **6 435**, moved from 2 466. | Issue comments | Use the corrected figures. Do not re-open either point. |
| F6 | **The #83 mutual-exclusivity guard is already verified** on `tablebyfiltergroup` and `aggregatesummary/singlevalue` (400 with the expected message). The `countbyadministrativeareal2did` body has no `CountyId` property, so it cannot be tested there. | Comment of 2026-11-17 | Not repeated. |
| F7 | **Request body names matter.** A renamed or unknown property is silently ignored (`Coding - WebAPI Contracts.md` §2). Use the exact names from the swagger operation. | Comment note on the 204 probe | Read the schemas from `/swagger/gis/swagger.json` before the run. |

---

## 2. Steps

Order: pre-flight (read-only) → **authorization gate** → run the POSTs one at a time → compare → report → clean up.
All calls are sequential, never parallel (`Coding - Deployed WebAPI.md` §4). Use `curl.exe`, not WebFetch.

### 2.1 Pre-flight (read-only, no authorization needed)
1. `GET /information/version` — record the `DiGi.GIS.PostgreSQL` and `DiGi.GIS.WebAPI` build stamps. They must be at or after
   `0.8.11.20261118081736`.
2. Read the three operations and their schemas, not the whole document:
   ```powershell
   $doc = Invoke-RestMethod "https://api.digiproject.uk/swagger/gis/swagger.json"
   $doc.paths."/gis/buildingdata/countbyadministrativeareal2did" | ConvertTo-Json -Depth 20
   ```
   Do the same for `aggregatesummary/singlevalue` and `tablebyfiltergroup`, and for the body schemas they reference.
3. Re-take the reference-count anchors from F3 if more than a day has passed.

### 2.2 Authorization gate (stop here until the user says yes)
Ask once, listing the exact calls: **8 POSTs** (the six area counts in the table below, plus the two filtered reads) and
their `commandtimeout` (default 600 s). They only read data. Do not run anything until a clear yes.

### 2.3 Run (one request at a time, record status and wall-clock `time_total`)
Step 2 — `POST gis/buildingdata/countbyadministrativeareal2did`:

| Body | Expect |
|---|---|
| `{"AdministrativeAreal2DId":55413}` | 200, **6 435** (equals the reference count, F3) |
| 55413 + `building_specific_functions Contains "church"` | 200, **≤ 7**, nonzero |
| `{"AdministrativeAreal2DId":55540}` | 716 (unchanged) |
| `{"AdministrativeAreal2DId":55626}` | 6 435 (unchanged) |
| `{"AdministrativeAreal2DId":85412}` | 2 505 (unchanged) |
| `{"AdministrativeAreal2DId":55417}` | 155 307 = `GET countbycountyid?countyid=55417` |
| `{"AdministrativeAreal2DId":55426}` | 200, **11 682** (equals F3) |

Step 3 — area scope with `AdministrativeAreal2DId: 55413`:
- `aggregatesummary/singlevalue` (`columnUniqueId "reference"`, function 4): equals the unfiltered count, **6 435**.
- `tablebyfiltergroup` with the church filter: row count equals the filtered count from step 2.
- `CountyId` together with `AdministrativeAreal2DId`: 400. Already verified (F6). Repeat only if the build changed.

Step 4 — wall-clock of 55413 and 55426 comes from the same runs. Single-shot figures from one machine are not guarantees; say so.

### 2.4 Decide the outcome
- **All rows match:** go to 2.5.
- **Any mismatch** (a 204, a count different from the reference count, a 500): do not fix it here. Re-run that one call alone first
  (a lone 500 on a cold partition is usually a timeout). If it persists, file a bug issue with type + priority + `ai:` labels, assignee
  `ZiolkowskiJakub`, the evidence table and a `blocked_by` link from #86. Then stop (`GitHub - Issues.md` §5).

### 2.5 Report (step 5 of the issue)
1. **Comment on #83** with the final figures table. Body through `--body-file`, UTF-8 without BOM, LF (`GitHub - Issues.md` §1).
2. **Update the worked recipes (#58).** The recipe text lives in the controller XML doc comments in
   `DiGi.GIS.WebAPI/Classes/Controller/BuildingDataController.cs`. The count and filtered-read recipes already document
   `AdministrativeAreal2DId` (lines about 1176, 1573 and 1726). Check whether the recipes use a `CountyId`-only example
   where the area scope would now be shorter. Add a verified example, such as "Bemowo district: 6 435", **only if a recipe is
   missing one**. Do not change any signature or route. If the XML changes, follow `XML Documentation.md`: one `<summary>`,
   params in signature order.
3. **Resolution comment on #86:** commits and builds, the measured table, timings, mismatches (none, or the new issue
   numbers). Then close the issue (`GitHub - Issues.md` §3). Closing is not a release request and not a reason to bump versions.

### 2.6 Clean-up and commit
- Delete scratch request bodies and response files.
- Code change only if 2.5.2 edits the controller: build with `BuildAll.ps1 -Summary -Solution DiGi.GIS.WebAPI`, then
  `CheckGuidelineRules.ps1 -Repo DiGi.GIS.WebAPI -Changed`. Commit on the active branch `0.8.9`, never a new branch or PR; push only when asked
  (`GitHub - Branches and Commits.md`).
- This plan file and the other untracked plan files are not committed (pre-commit plan-file sweep).

---

## 3. Guideline alignment

| Guideline | How the plan follows it |
|---|---|
| `Coding - Deployed WebAPI.md` §2, §4 | Read-only GETs freely; POSTs only after explicit authorization; sequential calls; re-test each 500 alone; production measured only through `api.digiproject.uk`. |
| `GitHub - Issues.md` §1, §2, §3, §5 | Bodies by `--body-file`; premises checked against the code and live API (F5); resolution comment; a mismatch becomes its own bug issue with a `blocked_by` link. |
| `GitHub - Labels.md` | Remove `status: blocked` once the dependencies are closed. |
| `Coding - WebAPI Contracts.md` §2 | Body property names confirmed from Swagger because unknown ones are ignored (F7). |
| `Coding - GIS Administrative Data.md` | County code is not a key. The plan uses ids only (55413, 55426, …). |
| `GitHub - Branches and Commits.md` | Active branch `0.8.9`, no new branch, worktree or PR, no push or release unless asked. |
| `XML Documentation.md` | Applies only if the #58 recipe text in the controller changes. |

## 4. Risks
- **A request can time out or answer 500 on a cold partition.** Re-test alone before calling it a defect.
- **The 55413 church filter may return 0 legitimately if the filter column or value differs.** Compare against the same filter on
  `tablebyfiltergroup`, and check the column name in the stored table.
- **The `/version` stamp does not prove the code.** The first 55413 result is the real proof that #89 is live.
