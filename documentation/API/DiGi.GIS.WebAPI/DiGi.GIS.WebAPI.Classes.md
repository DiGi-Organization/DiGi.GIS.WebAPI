#### [DiGi\.GIS\.WebAPI](DiGi.GIS.WebAPI.Overview.md 'DiGi\.GIS\.WebAPI\.Overview')

## DiGi\.GIS\.WebAPI\.Classes Namespace
### Classes

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController'></a>

## AddressPointController Class

Web API controller for PRG address points: stores the address points of a county and searches buildings by address\.

```csharp
public class AddressPointController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → AddressPointController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.AddressPointController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.AddressPointPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## AddressPointController\(GISWebAPIConfigurationFileWatcher, AddressPointPostgreSQLConverter, Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the [AddressPointController](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AddressPointController 'DiGi\.GIS\.WebAPI\.Classes\.AddressPointController') class\.

```csharp
public AddressPointController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.AddressPointPostgreSQLConverter addressPointPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.AddressPointController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.AddressPointPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher holding the access key and the [AllowUpdateAddressPoint](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateAddressPoint 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateAddressPoint') flag\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.AddressPointController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.AddressPointPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).addressPointPostgreSQLConverter'></a>

`addressPointPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AddressPointPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.addresspointpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AddressPointPostgreSQLConverter')

The converter of the address point table\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.AddressPointController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.AddressPointPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The converter the building footprints the points are matched to are read through\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.AddressPointController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.AddressPointPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter the county part polygons and the sibling parts of a county are read through\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.GetAddressPointsByReferenceAsync(string,System.Threading.CancellationToken)'></a>

## AddressPointController\.GetAddressPointsByReferenceAsync\(string, CancellationToken\) Method

Retrieves the address points matched to one building: the reverse of [GetBuildingsByAddressAsync\(string, string, Nullable&lt;int&gt;, Nullable&lt;int&gt;, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AddressPointController.GetBuildingsByAddressAsync(string,string,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.AddressPointController\.GetBuildingsByAddressAsync\(string, string, System\.Nullable\<int\>, System\.Nullable\<int\>, System\.Threading\.CancellationToken\)')\.

A building may hold any number of address points; a building with none answers an empty array.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAddressPointsByReferenceAsync(string? reference, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.GetAddressPointsByReferenceAsync(string,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The `building_2d` reference of the building\. Required\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.GetAddressPointsByReferenceAsync(string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the matches \(an empty array when none\), 400 for a missing reference, or 500 when the lookup could not run\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.GetBuildingsByAddressAsync(string,string,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## AddressPointController\.GetBuildingsByAddressAsync\(string, string, Nullable\<int\>, Nullable\<int\>, CancellationToken\) Method

Searches the buildings by address: a free\-text street or locality name, tolerating typos, missing diacritics and partial names, optionally narrowed by house number and county part\.

Answers the matched address points in rank order - similarity first, then an exact house number before a prefix match (`12` finds `12A` and `12/3`, not `120`). Each [DiGi\.GIS\.Classes\.AddressPointMatch](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.addresspointmatch 'DiGi\.GIS\.Classes\.AddressPointMatch') carries the reference and guid of its building together with the address point; points matched to no building are left out. Read the building geometry through `gis/building2d/itemsbyreferences`.

[countyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AddressPointController.GetBuildingsByAddressAsync(string,string,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).countyId 'DiGi\.GIS\.WebAPI\.Classes\.AddressPointController\.GetBuildingsByAddressAsync\(string, string, System\.Nullable\<int\>, System\.Nullable\<int\>, System\.Threading\.CancellationToken\)\.countyId') filters on one county <b>part</b>: a county with disconnected territory is stored as one row per part, so search such a county without the filter, or once per part.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetBuildingsByAddressAsync(string? text, string? houseNumber, System.Nullable<int> countyId, System.Nullable<int> limit=20, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.GetBuildingsByAddressAsync(string,string,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).text'></a>

`text` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The street or locality name to search for, as typed\. Required\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.GetBuildingsByAddressAsync(string,string,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).houseNumber'></a>

`houseNumber` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The house number to narrow the result to, or null for any\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.GetBuildingsByAddressAsync(string,string,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The county part id to narrow the result to, or null for every county\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.GetBuildingsByAddressAsync(string,string,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The maximum number of results, from 1 to 100\. Out\-of\-range values are refused, not clamped\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.GetBuildingsByAddressAsync(string,string,System.Nullable_int_,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the matches \(an empty array when none\), or 400 for a missing or empty text, a text that normalises to nothing, or an invalid limit or county id\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken)'></a>

## AddressPointController\.UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\) Method

Stores the PRG address points of one county: assigns every point to the county part it lies in, matches it to the nearest `building_2d` footprint of that part and writes the match\.

The county part is resolved per point <b>by geometry</b>, never by taking a single id on trust (DiGi.GIS.WebAPI#25): the given ids are first widened to every polygon part of the counties they name, so a caller sending one part of a multi-part county does not lose the points lying in its siblings. A point inside none of the parts is counted as outside and not written.

Answers 401 for a wrong key and, distinctly logged, for a disabled [AllowUpdateAddressPoint](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateAddressPoint 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateAddressPoint') flag; 503 with `Retry-After` for a transient database failure.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, int[]? countyIds, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array of [DiGi\.GIS\.Classes\.AddressPoint](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.addresspoint 'DiGi\.GIS\.Classes\.AddressPoint') items to store\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The county part ids the points belong to, one occurrence per part\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the [DiGi\.GIS\.PostgreSQL\.Classes\.AddressPointUpdateResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.addresspointupdateresult 'DiGi\.GIS\.PostgreSQL\.Classes\.AddressPointUpdateResult'), 204 for an empty body, 400 for invalid input, 401, 500 or 503\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointsPostTask'></a>

## AddressPointsPostTask Class

Provides functionality to handle the asynchronous posting of multiple PRG [DiGi\.GIS\.Classes\.AddressPoint](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.addresspoint 'DiGi\.GIS\.Classes\.AddressPoint') objects to the GIS PostgreSQL database\.

```csharp
public class AddressPointsPostTask : DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask<DiGi.GIS.Classes.AddressPoint>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.GIS\.Classes\.AddressPoint](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.addresspoint 'DiGi\.GIS\.Classes\.AddressPoint')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → AddressPointsPostTask
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.AddressPointsPostTask.AddressPointsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## AddressPointsPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the AddressPointsPostTask class\.

```csharp
public AddressPointsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AddressPointsPostTask.AddressPointsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') used to manage PostgreSQL GIS operations\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.AddressPointsPostTask.CountyIds'></a>

## AddressPointsPostTask\.CountyIds Property

Gets or sets the identifiers of the county rows the address points belong to \- every polygon part of one county, never a county code: a multi\-part county holds one row per part, and the server assigns each point to the part it lies in\.

```csharp
public System.Collections.Generic.IEnumerable<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.AddressPointsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AddressPoint_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken)'></a>

## AddressPointsPostTask\.ExecuteAsync\(IEnumerable\<AddressPoint\>, IEnumerable\<int\>, LongProgressWrapper, CancellationToken\) Method

Asynchronously executes the task of posting address points to the database, keyed by county identifiers\.

```csharp
protected System.Threading.Tasks.Task<bool> ExecuteAsync(System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.AddressPoint>? values, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AddressPointsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AddressPoint_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.AddressPoint](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.addresspoint 'DiGi\.GIS\.Classes\.AddressPoint')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of [DiGi\.GIS\.Classes\.AddressPoint](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.addresspoint 'DiGi\.GIS\.Classes\.AddressPoint') instances to post\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AddressPoint_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the address points belong to\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AddressPoint_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).longProgressWrapper'></a>

`longProgressWrapper` [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper')

A [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper') tracking the progress of the operation\.

<a name='DiGi.GIS.WebAPI.Classes.AddressPointsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AddressPoint_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if all batches were posted successfully; otherwise, false\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController'></a>

## AdministrativeAreal2DController Class

Web API controller for administrative area 2D operations, providing endpoints to retrieve, filter, and update administrative area data\.

```csharp
public class AdministrativeAreal2DController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → AdministrativeAreal2DController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.AdministrativeAreal2DController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## AdministrativeAreal2DController\(GISWebAPIConfigurationFileWatcher, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the [AdministrativeAreal2DController](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DController') class\.

```csharp
public AdministrativeAreal2DController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.AdministrativeAreal2DController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher for the GIS PostgreSQL Web API\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.AdministrativeAreal2DController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter used for administrative area 2D PostgreSQL operations\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferenceByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetAdministrativeAreal2DReferenceByCodeAsync\(string, Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Gets an administrative area reference by its code and type\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DReferenceByCodeAsync(string? code, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferenceByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique code of the administrative area\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferenceByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional type of the administrative area to filter the search\. Omit to resolve the code without a type filter\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferenceByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferenceByIdAsync(int,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetAdministrativeAreal2DReferenceByIdAsync\(int, CancellationToken\) Method

Retrieves an administrative area reference by its identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DReferenceByIdAsync(int id, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferenceByIdAsync(int,System.Threading.CancellationToken).id'></a>

`id` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the administrative area reference to retrieve\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferenceByIdAsync(int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencePathByIdAsync(int,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetAdministrativeAreal2DReferencePathByIdAsync\(int, CancellationToken\) Method

Retrieves the administrative area reference path by its identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DReferencePathByIdAsync(int id, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencePathByIdAsync(int,System.Threading.CancellationToken).id'></a>

`id` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the administrative area reference path to retrieve\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencePathByIdAsync(int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencePathsByNameAsync(string,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetAdministrativeAreal2DReferencePathsByNameAsync\(string, CancellationToken\) Method

Retrieves administrative area reference paths by name\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DReferencePathsByNameAsync(string text, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencePathsByNameAsync(string,System.Threading.CancellationToken).text'></a>

`text` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The search text used to find matching administrative area reference paths\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencePathsByNameAsync(string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

### Remarks
Use `GET referencepathsbyname?text=...` for a simpler query\-string form without the `_type` body discriminator\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencePathsByNameParameterAsync(DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetAdministrativeAreal2DReferencePathsByNameParameterAsync\(AdministrativeAreal2DReferencePathsByNameParameter, CancellationToken\) Method

Retrieves administrative area reference paths by name parameter\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DReferencePathsByNameParameterAsync(DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter administrativeAreal2DReferencePathsByNameParameter, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencePathsByNameParameterAsync(DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter,System.Threading.CancellationToken).administrativeAreal2DReferencePathsByNameParameter'></a>

`administrativeAreal2DReferencePathsByNameParameter` [AdministrativeAreal2DReferencePathsByNameParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DReferencePathsByNameParameter')

The parameter containing the search term for querying administrative areas by name\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencePathsByNameParameterAsync(DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync\(Nullable\<AdministrativeArealType\>, Nullable\<int\>, Nullable\<bool\>, CancellationToken\) Method

Retrieves all administrative area references filtered by administrative area type\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Nullable<int> parentId, System.Nullable<bool> uniqueCode, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The administrative area type used to filter the references\. Bound as nullable so an omitted parameter can be rejected: a non\-nullable binding would silently take [DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Country](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype.country 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Country'), because that is `default` of the enum while [DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Undefined](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype.undefined 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Undefined') is \-1\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).parentId'></a>

`parentId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional parent identifier used for filtering\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).uniqueCode'></a>

`uniqueCode` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional flag indicating whether to filter by unique code\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetAdministrativeAreal2DReferencesByCodeAsync\(string, Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves administrative area references by their code\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DReferencesByCodeAsync(string code, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique identifier or code used to retrieve the administrative area references\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional filter specifying the type of administrative area\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByIdsAsync(System.Collections.Generic.List_int_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetAdministrativeAreal2DReferencesByIdsAsync\(List\<int\>, CancellationToken\) Method

Retrieves administrative area references by a list of identifiers\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DReferencesByIdsAsync(System.Collections.Generic.List<int> ids, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByIdsAsync(System.Collections.Generic.List_int_,System.Threading.CancellationToken).ids'></a>

`ids` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

The list of unique identifiers of the administrative areas to retrieve\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByIdsAsync(System.Collections.Generic.List_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByNameParameterAsync(DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetAdministrativeAreal2DReferencesByNameParameterAsync\(AdministrativeAreal2DReferencesByNameParameter, CancellationToken\) Method

Retrieves administrative area references by name parameter\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DReferencesByNameParameterAsync(DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter administrativeAreal2DReferencesByNameParameter, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByNameParameterAsync(DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter,System.Threading.CancellationToken).administrativeAreal2DReferencesByNameParameter'></a>

`administrativeAreal2DReferencesByNameParameter` [AdministrativeAreal2DReferencesByNameParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DReferencesByNameParameter')

The parameter containing the search term for querying administrative area references by name\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetAdministrativeAreal2DReferencesByNameParameterAsync(DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

### Remarks
Use `GET referencesbyname?text=...` for a simpler query\-string form without the `_type` body discriminator\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetBoundingBox2DAsync(System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetBoundingBox2DAsync\(CancellationToken\) Method

Asynchronously retrieves the 2D bounding box enclosing country administrative areas\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetBoundingBox2DAsync(System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetBoundingBox2DAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the called method to indicate that the operation should be canceled\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetCodesAsync()'></a>

## AdministrativeAreal2DController\.GetCodesAsync\(\) Method

Retrieves all available administrative area codes\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCodesAsync();
```

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetCountAsync(System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetCountAsync\(CancellationToken\) Method

Retrieves the count of administrative areas\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountAsync(System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetCountAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetIdByCodeAsync\(string, Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves the identifier for a given code\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetIdByCodeAsync(string code, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique code of the administrative area\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional type of the administrative area to filter the search\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdsByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetIdsByAdministrativeArealTypeAsync\(Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves all identifiers for a given administrative area type\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetIdsByAdministrativeArealTypeAsync(System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdsByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The administrative area type\. Bound as nullable so an omitted parameter can be rejected: a non\-nullable binding would silently take [DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Country](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype.country 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Country'), because that is `default` of the enum while [DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Undefined](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype.undefined 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Undefined') is \-1\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdsByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdsByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetIdsByCodeAsync\(string, Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves all identifiers for a given code\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetIdsByCodeAsync(string code, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdsByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The code of the administrative area\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdsByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional type of the administrative area to filter the search\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetIdsByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetItemByCodeAsync\(string, Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves an administrative area item by its code and optional type\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemByCodeAsync(string code, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique code of the administrative area to retrieve\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional type of the administrative area to filter the search\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemByIdAsync(int,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetItemByIdAsync\(int, CancellationToken\) Method

Asynchronously retrieves an administrative area item by its unique identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemByIdAsync(int id, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemByIdAsync(int,System.Threading.CancellationToken).id'></a>

`id` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The integer identifier of the administrative area item to retrieve\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemByIdAsync(int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetItemsByAdministrativeArealTypeAsync\(Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves all administrative area items filtered by administrative area type\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByAdministrativeArealTypeAsync(System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The administrative area type used to filter the results\. Bound as nullable so an omitted parameter can be rejected: a non\-nullable binding would silently take [DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Country](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype.country 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Country'), because that is `default` of the enum while [DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Undefined](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype.undefined 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\.Undefined') is \-1\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByAdministrativeArealTypeAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetItemsByBoundingBoxAsync\(double, double, double, double, Nullable\<double\>, Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves administrative area items within a specified bounding box\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByBoundingBoxAsync(double x_1, double y_1, double x_2, double y_2, System.Nullable<double> tolerance, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).x_1'></a>

`x_1` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X\-coordinate of the first corner of the bounding box\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).y_1'></a>

`y_1` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y\-coordinate of the first corner of the bounding box\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).x_2'></a>

`x_2` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X\-coordinate of the second corner of the bounding box\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).y_2'></a>

`y_2` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y\-coordinate of the second corner of the bounding box\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional tolerance value for the spatial query\. If not provided, a default macro distance is used\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional filter to restrict results to a specific type of administrative area\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetItemsByCircleAsync\(double, double, Nullable\<double\>, Nullable\<double\>, Nullable\<double\>, Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves administrative area items within a specified circle\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByCircleAsync(double x, double y, System.Nullable<double> radius, System.Nullable<double> diameter, System.Nullable<double> tolerance, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).x'></a>

`x` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X\-coordinate of the center point of the search circle\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).y'></a>

`y` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y\-coordinate of the center point of the search circle\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).radius'></a>

`radius` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The radius of the search circle, in metres\. Capped at [MaximumRadius](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MaximumRadius 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MaximumRadius')\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).diameter'></a>

`diameter` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The diameter of the search circle, in metres, used only when [radius](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).radius 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DController\.GetItemsByCircleAsync\(double, double, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType\>, System\.Threading\.CancellationToken\)\.radius') is absent\. Capped at twice [MaximumRadius](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MaximumRadius 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MaximumRadius')\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The tolerance value for the spatial query\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The type of administrative area to retrieve\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing a list of administrative area items if found, or an error response\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetItemsByCodeAsync\(string, Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves administrative area items filtered by code\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByCodeAsync(string code, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The code used to filter the administrative area items\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional type of administrative area to filter by\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCodeAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing a list of matching administrative area items, or an error response if the code is invalid or no items are found\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCodesAsync(System.Collections.Generic.List_string_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetItemsByCodesAsync\(List\<string\>, CancellationToken\) Method

Retrieves administrative area items filtered by multiple codes\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByCodesAsync(System.Collections.Generic.List<string> codes, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCodesAsync(System.Collections.Generic.List_string_,System.Threading.CancellationToken).codes'></a>

`codes` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

The list of codes used to filter the administrative area items\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByCodesAsync(System.Collections.Generic.List_string_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByIdsAsync(System.Collections.Generic.List_int_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetItemsByIdsAsync\(List\<int\>, CancellationToken\) Method

Retrieves administrative area items filtered by a list of identifiers\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByIdsAsync(System.Collections.Generic.List<int> ids, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByIdsAsync(System.Collections.Generic.List_int_,System.Threading.CancellationToken).ids'></a>

`ids` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

The list of identifiers used to retrieve the administrative area items\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByIdsAsync(System.Collections.Generic.List_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByPointAsync(double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetItemsByPointAsync\(double, double, Nullable\<double\>, Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Retrieves administrative area items at or near a specified point\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByPointAsync(double x, double y, System.Nullable<double> tolerance, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByPointAsync(double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).x'></a>

`x` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X\-coordinate of the search point\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByPointAsync(double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).y'></a>

`y` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y\-coordinate of the search point\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByPointAsync(double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional tolerance distance to use when searching for items near the specified point\. If null, a default macro distance is used\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByPointAsync(double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional type filter for the administrative area items to be retrieved\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetItemsByPointAsync(double,double,System.Nullable_double_,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetReferencePathsByNameAsync(string,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetReferencePathsByNameAsync\(string, CancellationToken\) Method

Retrieves administrative area reference paths by name as a query string\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetReferencePathsByNameAsync(string text, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetReferencePathsByNameAsync(string,System.Threading.CancellationToken).text'></a>

`text` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The search text used to find matching administrative area reference paths\. Required\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetReferencePathsByNameAsync(string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

### Remarks
Case\-insensitive substring match on the administrative area name\. Equivalent to `POST administrativeareal2Dreferencepathsbynameparameter` without the `_type` discriminator\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetReferencesByNameAsync(string,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetReferencesByNameAsync\(string, CancellationToken\) Method

Retrieves administrative area references by name as a query string\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetReferencesByNameAsync(string text, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetReferencesByNameAsync(string,System.Threading.CancellationToken).text'></a>

`text` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The search text used to find matching administrative area references\. Required\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetReferencesByNameAsync(string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

### Remarks
Case\-insensitive substring match on the administrative area name\. Equivalent to `POST administrativeareal2Dreferencesbynameparameter` without the `_type` discriminator\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetSubCodesAsync(string,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.GetSubCodesAsync\(string, CancellationToken\) Method

Retrieves subcodes for a given code\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSubCodesAsync(string code, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetSubCodesAsync(string,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The administrative area code used to retrieve the associated subcodes\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.GetSubCodesAsync(string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.UpdateItemAsync\(JsonObject, string, CancellationToken\) Method

Updates a single administrative area item\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemAsync(System.Text.Json.Nodes.JsonObject? jsonObject, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Threading.CancellationToken).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data used to update the administrative area item\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken)'></a>

## AdministrativeAreal2DController\.UpdateItemsAsync\(JsonArray, string, CancellationToken\) Method

Updates multiple administrative area items\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the administrative area items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by other objects or threads to receive notice of cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter'></a>

## AdministrativeAreal2DReferencePathsByNameParameter Class

Represents a parameter containing text for querying administrative area reference paths\.

```csharp
public class AdministrativeAreal2DReferencePathsByNameParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → AdministrativeAreal2DReferencePathsByNameParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter.AdministrativeAreal2DReferencePathsByNameParameter()'></a>

## AdministrativeAreal2DReferencePathsByNameParameter\(\) Constructor

Initializes a new instance of the [AdministrativeAreal2DReferencePathsByNameParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DReferencePathsByNameParameter') class\.

```csharp
public AdministrativeAreal2DReferencePathsByNameParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter.AdministrativeAreal2DReferencePathsByNameParameter(string)'></a>

## AdministrativeAreal2DReferencePathsByNameParameter\(string\) Constructor

Initializes a new instance of the [AdministrativeAreal2DReferencePathsByNameParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DReferencePathsByNameParameter') class with the specified text \(search phrase\)\.

```csharp
public AdministrativeAreal2DReferencePathsByNameParameter(string text);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter.AdministrativeAreal2DReferencePathsByNameParameter(string).text'></a>

`text` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to search for\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter.AdministrativeAreal2DReferencePathsByNameParameter(System.Text.Json.Nodes.JsonObject)'></a>

## AdministrativeAreal2DReferencePathsByNameParameter\(JsonObject\) Constructor

Initializes a new instance of the [AdministrativeAreal2DReferencePathsByNameParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DReferencePathsByNameParameter') class using an [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public AdministrativeAreal2DReferencePathsByNameParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter.AdministrativeAreal2DReferencePathsByNameParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing the parameter values\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencePathsByNameParameter.Text'></a>

## AdministrativeAreal2DReferencePathsByNameParameter\.Text Property

Text to search for in the names of the administrative areal 2D reference paths\. The search is case\-insensitive and matches any path whose name contains the specified text\.

```csharp
public string? Text { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter'></a>

## AdministrativeAreal2DReferencesByNameParameter Class

Represents a parameter containing text for querying administrative area references\.

```csharp
public class AdministrativeAreal2DReferencesByNameParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → AdministrativeAreal2DReferencesByNameParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter.AdministrativeAreal2DReferencesByNameParameter()'></a>

## AdministrativeAreal2DReferencesByNameParameter\(\) Constructor

Initializes a new instance of the [AdministrativeAreal2DReferencesByNameParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DReferencesByNameParameter') class\.

```csharp
public AdministrativeAreal2DReferencesByNameParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter.AdministrativeAreal2DReferencesByNameParameter(string)'></a>

## AdministrativeAreal2DReferencesByNameParameter\(string\) Constructor

Initializes a new instance of the [AdministrativeAreal2DReferencesByNameParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DReferencesByNameParameter') class with the specified text\.

```csharp
public AdministrativeAreal2DReferencesByNameParameter(string text);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter.AdministrativeAreal2DReferencesByNameParameter(string).text'></a>

`text` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to search for\.

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter.AdministrativeAreal2DReferencesByNameParameter(System.Text.Json.Nodes.JsonObject)'></a>

## AdministrativeAreal2DReferencesByNameParameter\(JsonObject\) Constructor

Initializes a new instance of the [AdministrativeAreal2DReferencesByNameParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DReferencesByNameParameter') class using a [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') instance\.

```csharp
public AdministrativeAreal2DReferencesByNameParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter.AdministrativeAreal2DReferencesByNameParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing the parameter values\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DReferencesByNameParameter.Text'></a>

## AdministrativeAreal2DReferencesByNameParameter\.Text Property

Gets or sets the text to search for in the names of administrative areal 2D references\.

```csharp
public string? Text { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DsPostTask'></a>

## AdministrativeAreal2DsPostTask Class

Provides functionality to asynchronously post a collection of [DiGi\.GIS\.Classes\.AdministrativeAreal2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.administrativeareal2d 'DiGi\.GIS\.Classes\.AdministrativeAreal2D') objects to the PostgreSQL database\.

```csharp
public class AdministrativeAreal2DsPostTask : DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask<DiGi.GIS.Classes.AdministrativeAreal2D>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.GIS\.Classes\.AdministrativeAreal2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.administrativeareal2d 'DiGi\.GIS\.Classes\.AdministrativeAreal2D')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → AdministrativeAreal2DsPostTask
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DsPostTask.AdministrativeAreal2DsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## AdministrativeAreal2DsPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the AdministrativeAreal2DsPostTask class\.

```csharp
public AdministrativeAreal2DsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DsPostTask.AdministrativeAreal2DsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The GIS PostgreSQL Web API manager used to handle database operations\.

<a name='DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter'></a>

## AggregateSummariesRequestParameter Class

Parameter class containing options for a batch of single\-value database aggregation queries: several measures over one shared partition and filter, served in one request \(DiGi\-Organization/DiGi\.GIS\.WebAPI\#56\)\.

```csharp
public class AggregateSummariesRequestParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → AggregateSummariesRequestParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter.AggregateSummariesRequestParameter()'></a>

## AggregateSummariesRequestParameter\(\) Constructor

Initializes a new instance of the [AggregateSummariesRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.AggregateSummariesRequestParameter') class\.

```csharp
public AggregateSummariesRequestParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter.AggregateSummariesRequestParameter(System.Text.Json.Nodes.JsonObject)'></a>

## AggregateSummariesRequestParameter\(JsonObject\) Constructor

Initializes a new instance of the [AggregateSummariesRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.AggregateSummariesRequestParameter') class using a [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public AggregateSummariesRequestParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter.AggregateSummariesRequestParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter.CountyId'></a>

## AggregateSummariesRequestParameter\.CountyId Property

Gets or sets the target partition identifier \(County ID\)\. If null, aggregation is performed across all partitions\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

### Example
10365

<a name='DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter.FilterGroup'></a>

## AggregateSummariesRequestParameter\.FilterGroup Property

Gets or sets the optional dynamic hierarchical filters applied to every measure\.

```csharp
public DiGi.PostgreSQL.Table.Classes.FilterGroup? FilterGroup { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

<a name='DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter.Measures'></a>

## AggregateSummariesRequestParameter\.Measures Property

Gets or sets the single\-value aggregate measures to compute, each pairing a target column with its function\.

The answers arrive in this order, one value per measure; a SQL `NULL` aggregate (for example `Avg` over zero rows) is preserved as JSON `null`. Multi-value measures and histograms are not accepted here. An empty list, or a list above the cap, is refused with 400 (DiGi-Organization/DiGi.GIS.WebAPI#56).

```csharp
public System.Collections.Generic.List<DiGi.PostgreSQL.Table.Classes.AggregateMeasure> Measures { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[DiGi\.PostgreSQL\.Table\.Classes\.AggregateMeasure](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.aggregatemeasure 'DiGi\.PostgreSQL\.Table\.Classes\.AggregateMeasure')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

### Example
\[\{ "ColumnUniqueId": "floor\_area", "SinglevalueAggregateFunction": "Avg" \}\]

<a name='DiGi.GIS.WebAPI.Classes.Building2DController'></a>

## Building2DController Class

Web API controller for building 2D operations, providing endpoints to retrieve, filter, and update building 2D data\.

```csharp
public class Building2DController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → Building2DController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.Building2DController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## Building2DController\(GISWebAPIConfigurationFileWatcher, Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the [Building2DController](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DController 'DiGi\.GIS\.WebAPI\.Classes\.Building2DController') class\.

```csharp
public Building2DController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.Building2DController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher for the GIS PostgreSQL Web API\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.Building2DController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The converter used for Building 2D data operations in PostgreSQL\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.Building2DController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter used to resolve administrative area codes to county identifiers\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.CountAsync(DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter,System.Threading.CancellationToken)'></a>

## Building2DController\.CountAsync\(CountByAdministrativeAreal2DIdsParameter, CancellationToken\) Method

Asynchronously counts the number of buildings based on the administrative areal 2D identifiers\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> CountAsync(DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter countByAdministrativeAreal2DIdsParameter, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.CountAsync(DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter,System.Threading.CancellationToken).countByAdministrativeAreal2DIdsParameter'></a>

`countByAdministrativeAreal2DIdsParameter` [CountByAdministrativeAreal2DIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.CountByAdministrativeAreal2DIdsParameter')

The parameter object containing the collection of administrative areal 2D identifiers\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.CountAsync(DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferenceByIdAsync(long,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## Building2DController\.GetBuilding2DReferenceByIdAsync\(long, Nullable\<int\>, CancellationToken\) Method

Asynchronously retrieves a building 2D reference by its unique identifier and an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetBuilding2DReferenceByIdAsync(long id, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferenceByIdAsync(long,System.Nullable_int_,System.Threading.CancellationToken).id'></a>

`id` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The unique identifier of the building\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferenceByIdAsync(long,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional integer representing the county identifier used to filter the search\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferenceByIdAsync(long,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferenceByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## Building2DController\.GetBuilding2DReferenceByReferenceAsync\(string, Nullable\<int\>, CancellationToken\) Method

Retrieves a building 2D reference by its unique reference code and an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetBuilding2DReferenceByReferenceAsync(string reference, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferenceByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string of the building to retrieve\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferenceByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional integer identifier of the county used to filter the search\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferenceByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferencesByAdministrativeAreal2DIdAsync(int,System.Threading.CancellationToken)'></a>

## Building2DController\.GetBuilding2DReferencesByAdministrativeAreal2DIdAsync\(int, CancellationToken\) Method

Retrieves building 2D references filtered by administrative area 2D identifier\. Can be used for relatively small number of buildings\.

An area above the subdivision level resolves through its <b>Subdivision children</b> (`building_2d.subdivision_id` membership); a <b>Subdivision</b> resolves through its <b>polygon</b> - the buildings whose bounding-box centre it contains - because the subdivision layer nests (a city, its districts and their neighbourhoods are all subdivisions of one municipality) and `subdivision_id` files each building under a single container, so membership cannot name a district's buildings (DiGi.GIS.PostgreSQL#75). Parity with `point2dsbyadministrativeareal2Did`.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetBuilding2DReferencesByAdministrativeAreal2DIdAsync(int administrativeAreal2DId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferencesByAdministrativeAreal2DIdAsync(int,System.Threading.CancellationToken).administrativeAreal2DId'></a>

`administrativeAreal2DId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the administrative area 2D used to filter the building references\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferencesByAdministrativeAreal2DIdAsync(int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferencesByPagingParameterAsync(DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter,System.Threading.CancellationToken)'></a>

## Building2DController\.GetBuilding2DReferencesByPagingParameterAsync\(Building2DReferencesByPagingParameter, CancellationToken\) Method

Retrieves a paginated list of building 2D references\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetBuilding2DReferencesByPagingParameterAsync(DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter building2DReferencesByPagingParameter, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferencesByPagingParameterAsync(DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter,System.Threading.CancellationToken).building2DReferencesByPagingParameter'></a>

`building2DReferencesByPagingParameter` [Building2DReferencesByPagingParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter 'DiGi\.GIS\.WebAPI\.Classes\.Building2DReferencesByPagingParameter')

The parameter containing paging options, including county identifier, cursor, and page size\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetBuilding2DReferencesByPagingParameterAsync(DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning a list of building 2D references\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetCentroidsByAdministrativeAreal2DIdAsync(int,int,System.Threading.CancellationToken)'></a>

## Building2DController\.GetCentroidsByAdministrativeAreal2DIdAsync\(int, int, CancellationToken\) Method

Retrieves the bounding\-box centres of every building of the given administrative area as one compact, columnar document: `{"References":[…],"CountyIds":[…],"X":[…],"Y":[…]}`, the i\-th entry of every array describing the same building, coordinates rounded to 0\.01 m\.

The same buildings, area resolution and status codes as `point2dsbyadministrativeareal2Did` (see its remarks), without a `_type` and four property names per building. That answer relays as 15.9 MB for county 1465 (155 307 buildings), and its client only ever keeps these four values (DiGi.GIS.WebAPI.UI#29). The full endpoint stays for callers that deserialize `Building2DCentroid` objects.

An area with no buildings answers 200 with four empty arrays; a failed area lookup answers 404. Rows without a reference or a county identifier are left out ([ToSystem\_JsonObject\(this IEnumerable&lt;Building2DCentroid&gt;\)](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Convert.ToSystem_JsonObject(thisSystem.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DCentroid_) 'DiGi\.GIS\.WebAPI\.Convert\.ToSystem\_JsonObject\(this System\.Collections\.Generic\.IEnumerable\<DiGi\.GIS\.PostgreSQL\.Classes\.Building2DCentroid\>\)')).

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCentroidsByAdministrativeAreal2DIdAsync(int administrativeAreal2DId, int commandTimeout=30, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetCentroidsByAdministrativeAreal2DIdAsync(int,int,System.Threading.CancellationToken).administrativeAreal2DId'></a>

`administrativeAreal2DId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the administrative area 2D whose buildings are returned\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetCentroidsByAdministrativeAreal2DIdAsync(int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetCentroidsByAdministrativeAreal2DIdAsync(int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetCountyPartMismatchesAsync(string,int,System.Threading.CancellationToken)'></a>

## Building2DController\.GetCountyPartMismatchesAsync\(string, int, CancellationToken\) Method

Asynchronously counts, for every polygon part of every multi\-part county, the buildings it holds whose bounding box lies outside that part\.

A county code names one `administrative_areal_2d` row per polygon part, and a building filed under the wrong one is unreachable through every read that filters on `county_id`. A building whose stored box does not intersect the box of the part holding it cannot be inside that part, so what this counts is certainly misfiled - and it costs no geometry.

The count is a lower bound, not a total: a building inside the box of its part but outside the polygon needs the polygon to settle and is not counted. Read it as the number of rows known to be wrong, before and after a repair.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountyPartMismatchesAsync(string? code=null, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetCountyPartMismatchesAsync(string,int,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional county code to restrict the measurement to\. When omitted every multi\-part code is measured\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetCountyPartMismatchesAsync(string,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetCountyPartMismatchesAsync(string,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning one entry per county part holding buildings\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetEstimatedCountsByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken)'></a>

## Building2DController\.GetEstimatedCountsByCountyIdsAsync\(List\<int\>, int, CancellationToken\) Method

Asynchronously returns the estimated number of buildings held by each of the named county parts\.

Read from the planner's row estimate of each county's `building_2d` partition, not counted: the whole country answers in one catalogue query, where an exact count reads every partition. The estimate is as fresh as the partition's last analyse, which is close enough for a coarse share such as a coverage marker and not for a figure anyone reconciles against.

The values come back in the order the identifiers were given, one per identifier, so a caller pairs them up by position. A value is `null` where the part has no partition or the partition has never been analysed - an unknown, never a zero.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetEstimatedCountsByCountyIdsAsync(System.Collections.Generic.List<int>? countyIds, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetEstimatedCountsByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

The county part identifiers, as a repeated `countyids` parameter\. At least one and at most [MaximumEstimatedCountCountyCount](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Building2D.MaximumEstimatedCountCountyCount 'DiGi\.GIS\.WebAPI\.Constants\.Building2D\.MaximumEstimatedCountCountyCount')\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetEstimatedCountsByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetEstimatedCountsByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with one estimate or `null` per identifier, 400 for invalid parameters, 503 on a transient database failure, or 500 when the estimates could not be read\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByIdAsync(long,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## Building2DController\.GetItemByIdAsync\(long, Nullable\<int\>, CancellationToken\) Method

Retrieves a building 2D item by its identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemByIdAsync(long id, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByIdAsync(long,System.Nullable_int_,System.Threading.CancellationToken).id'></a>

`id` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The unique identifier of the building 2D item to retrieve\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByIdAsync(long,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional county identifier associated with the building\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByIdAsync(long,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByPointAsync(double,double,System.Nullable_double_,System.Threading.CancellationToken)'></a>

## Building2DController\.GetItemByPointAsync\(double, double, Nullable\<double\>, CancellationToken\) Method

Retrieves a building 2D item at or near a specified point\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemByPointAsync(double x, double y, System.Nullable<double> tolerance, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByPointAsync(double,double,System.Nullable_double_,System.Threading.CancellationToken).x'></a>

`x` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate of the search point\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByPointAsync(double,double,System.Nullable_double_,System.Threading.CancellationToken).y'></a>

`y` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate of the search point\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByPointAsync(double,double,System.Nullable_double_,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional tolerance distance in meters to use when searching for the item near the specified point\. If not provided, NaN, or non\-positive, a default tolerance of 0\.5 meters is used\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByPointAsync(double,double,System.Nullable_double_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing the building 2D item if found, or an error response\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## Building2DController\.GetItemByReferenceAsync\(string, Nullable\<int\>, CancellationToken\) Method

Asynchronously retrieves a building 2D item by its reference code and an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemByReferenceAsync(string reference, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string used to locate the building 2D item\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional identifier of the county associated with the building\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken)'></a>

## Building2DController\.GetItemsByBoundingBoxAsync\(double, double, double, double, Nullable\<double\>, CancellationToken\) Method

Retrieves building 2D items within a specified bounding box\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByBoundingBoxAsync(double x_1, double y_1, double x_2, double y_2, System.Nullable<double> tolerance, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).x_1'></a>

`x_1` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X\-coordinate of the first corner of the bounding box\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).y_1'></a>

`y_1` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y\-coordinate of the first corner of the bounding box\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).x_2'></a>

`x_2` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X\-coordinate of the second corner of the bounding box\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).y_2'></a>

`y_2` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y\-coordinate of the second corner of the bounding box\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional tolerance value for the spatial query\. If not provided or NaN, a default macro distance is used\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBuilding2DReferencesAsync(System.Text.Json.Nodes.JsonArray,System.Threading.CancellationToken)'></a>

## Building2DController\.GetItemsByBuilding2DReferencesAsync\(JsonArray, CancellationToken\) Method

Retrieves building 2D items by their references\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByBuilding2DReferencesAsync(System.Text.Json.Nodes.JsonArray? jsonArray, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBuilding2DReferencesAsync(System.Text.Json.Nodes.JsonArray,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the building 2D references to retrieve\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByBuilding2DReferencesAsync(System.Text.Json.Nodes.JsonArray,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken)'></a>

## Building2DController\.GetItemsByCircleAsync\(double, double, Nullable\<double\>, Nullable\<double\>, Nullable\<double\>, CancellationToken\) Method

Retrieves building 2D items within a specified circle\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByCircleAsync(double x, double y, System.Nullable<double> radius, System.Nullable<double> diameter, System.Nullable<double> tolerance, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).x'></a>

`x` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X\-coordinate of the center of the circle\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).y'></a>

`y` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y\-coordinate of the center of the circle\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).radius'></a>

`radius` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The radius of the search circle, in metres\. Capped at [MaximumRadius](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MaximumRadius 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MaximumRadius')\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).diameter'></a>

`diameter` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The diameter of the search circle, in metres, used only when [radius](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).radius 'DiGi\.GIS\.WebAPI\.Classes\.Building2DController\.GetItemsByCircleAsync\(double, double, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Threading\.CancellationToken\)\.radius') is absent\. Capped at twice [MaximumRadius](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MaximumRadius 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MaximumRadius')\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The tolerance value to be applied to the search area\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCountyIdAsync(int,System.Threading.CancellationToken)'></a>

## Building2DController\.GetItemsByCountyIdAsync\(int, CancellationToken\) Method

Retrieves building 2D items for a specified county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByCountyIdAsync(int countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCountyIdAsync(int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the county\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByCountyIdAsync(int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result contains the building 2D items as a JSON response, or a 404 status if no items are found\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## Building2DController\.GetItemsByReferencesAsync\(IEnumerable\<string\>, Nullable\<int\>, CancellationToken\) Method

Asynchronously retrieves building 2D items for each of the provided references and an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable<string>? references, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of unique reference strings used to identify the 2D buildings\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional integer representing the county ID to filter the results\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning a list of building 2D items\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetPoint2DsByAdministrativeAreal2DIdAsync(int,int,System.Threading.CancellationToken)'></a>

## Building2DController\.GetPoint2DsByAdministrativeAreal2DIdAsync\(int, int, CancellationToken\) Method

Retrieves the bounding\-box centres of every building of the given administrative area, keyed by reference and county partition, for fast 2D dot rendering\.

Area resolution (parity with `building2Dreferencesbyadministrativeareal2Did`): an area above the subdivision level goes through its <b>Subdivision children</b>, not geometry - one with no subdivisions answers 200 with an empty array, which is not the same as "the area holds no buildings"; it means the resolution found no subdivision to filter by. A <b>Subdivision</b> goes through its <b>polygon</b> instead - the buildings whose bounding-box centre it contains - because the subdivision layer nests and `subdivision_id` membership cannot name a district's buildings (DiGi.GIS.PostgreSQL#75); for it, an empty array does mean the polygon holds no building centre. A [Microsoft\.AspNetCore\.Http\.StatusCodes\.Status404NotFound](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.statuscodes.status404notfound 'Microsoft\.AspNetCore\.Http\.StatusCodes\.Status404NotFound') means the area lookup itself failed, not that the area is empty. Rows whose bounding box is NULL are skipped. The JSONB geometry column is never read on either path.

A cold partition can exceed the default timeout (precedent: issue #27) - retry once with a higher `commandtimeout` before treating a timeout as a defect.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetPoint2DsByAdministrativeAreal2DIdAsync(int administrativeAreal2DId, int commandTimeout=30, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetPoint2DsByAdministrativeAreal2DIdAsync(int,int,System.Threading.CancellationToken).administrativeAreal2DId'></a>

`administrativeAreal2DId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the administrative area 2D used to filter the building centroids\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetPoint2DsByAdministrativeAreal2DIdAsync(int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetPoint2DsByAdministrativeAreal2DIdAsync(int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetPoint2DsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## Building2DController\.GetPoint2DsByReferencesAsync\(IEnumerable\<string\>, Nullable\<int\>, CancellationToken\) Method

Retrieves Point2D coordinates by their references\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetPoint2DsByReferencesAsync(System.Collections.Generic.IEnumerable<string>? references, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetPoint2DsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of reference strings used to identify the Point2D objects\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetPoint2DsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional identifier for the county associated with the coordinates\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetPoint2DsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferenceDuplicatesAsync(int,int,System.Threading.CancellationToken)'></a>

## Building2DController\.GetReferenceDuplicatesAsync\(int, int, CancellationToken\) Method

Asynchronously retrieves duplicate building references that occur across multiple counties, ordered by collision count descending\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetReferenceDuplicatesAsync(int limit=100, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferenceDuplicatesAsync(int,int,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of duplicate references to return\. Defaults to 100\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferenceDuplicatesAsync(int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferenceDuplicatesAsync(int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning a list of duplicate building references\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferencesByCountyIdAsync(int,System.Nullable_int_,int,System.Threading.CancellationToken)'></a>

## Building2DController\.GetReferencesByCountyIdAsync\(int, Nullable\<int\>, int, CancellationToken\) Method

Retrieves references of the building2Ds filtered by county Id\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetReferencesByCountyIdAsync(int countyId, System.Nullable<int> subdivisionId=null, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferencesByCountyIdAsync(int,System.Nullable_int_,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the county used to filter the building 2D references\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferencesByCountyIdAsync(int,System.Nullable_int_,int,System.Threading.CancellationToken).subdivisionId'></a>

`subdivisionId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional unique identifier of the subdivision used to further filter the building 2D references\. When set, the buildings are read by the subdivision's polygon \(every container that holds a building answers\), not by the stored `subdivision_id` \- the subdivision layer is nested, so a building filed under its neighbourhood is still returned for its district and city\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferencesByCountyIdAsync(int,System.Nullable_int_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferencesByCountyIdAsync(int,System.Nullable_int_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferenceUniquenessSummaryAsync(int,System.Threading.CancellationToken)'></a>

## Building2DController\.GetReferenceUniquenessSummaryAsync\(int, CancellationToken\) Method

Asynchronously retrieves overall building reference uniqueness metrics across all partitions in the database\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetReferenceUniquenessSummaryAsync(int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferenceUniquenessSummaryAsync(int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.GetReferenceUniquenessSummaryAsync(int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning the building reference uniqueness summary\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Nullable_int_,string,System.Threading.CancellationToken)'></a>

## Building2DController\.UpdateItemAsync\(JsonObject, string, Nullable\<int\>, string, CancellationToken\) Method

Updates a single building 2D item\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemAsync(System.Text.Json.Nodes.JsonObject? jsonObject, string? code, System.Nullable<int> countyId=null, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Nullable_int_,string,System.Threading.CancellationToken).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data to update the building 2D item\. This value can be null\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Nullable_int_,string,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The code identifying the specific building 2D item to be updated\. This value can be null\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Nullable_int_,string,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional county identifier associated with the building\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Nullable_int_,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemAsync(System.Text.Json.Nodes.JsonObject,string,System.Nullable_int_,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken)'></a>

## Building2DController\.UpdateItemsAsync\(JsonArray, string, string, CancellationToken\) Method

Updates multiple building 2D items based on the provided JSON array and identification code\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string? code, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the building 2D items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The identification code required for the update operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,string,System.Threading.CancellationToken)'></a>

## Building2DController\.UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, string, CancellationToken\) Method

Updates multiple building 2D items in the database for the given county rows\.

Naming several parts leaves the part of each building to geometry, which is the only thing that can decide it. Passing [code](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,string,System.Threading.CancellationToken).code 'DiGi\.GIS\.WebAPI\.Classes\.Building2DController\.UpdateItemsByCountyIdsAsync\(System\.Text\.Json\.Nodes\.JsonArray, int\[\], string, string, System\.Threading\.CancellationToken\)\.code') as well does not change that answer - it narrows the candidates to the parts of that county before the geometry runs, and it is what the row stores in its own `code` column.

Without it a building of a multi-part county is resolved against every county whose extent reaches it, which costs one query returning whole county polygons <b>per building</b> rather than one set of polygons per county. That is the difference between minutes and hours over a county of tens of thousands of buildings, so an importer that knows the code should send it.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, int[]? countyIds, string? code=null, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the building 2D items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The identifiers of the county rows the buildings belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,string,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional county code the buildings came from\. It narrows the geometric decision and is stored on the row; the parts named in [countyIds](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,string,System.Threading.CancellationToken).countyIds 'DiGi\.GIS\.WebAPI\.Classes\.Building2DController\.UpdateItemsByCountyIdsAsync\(System\.Text\.Json\.Nodes\.JsonArray, int\[\], string, string, System\.Threading\.CancellationToken\)\.countyIds') still decide\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter'></a>

## Building2DReferencesByPagingParameter Class

Parameter class containing options for keyset\-paginated building 2D reference queries\.

```csharp
public class Building2DReferencesByPagingParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → Building2DReferencesByPagingParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter.Building2DReferencesByPagingParameter()'></a>

## Building2DReferencesByPagingParameter\(\) Constructor

Initializes a new instance of the [Building2DReferencesByPagingParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter 'DiGi\.GIS\.WebAPI\.Classes\.Building2DReferencesByPagingParameter') class\.

```csharp
public Building2DReferencesByPagingParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter.Building2DReferencesByPagingParameter(DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter)'></a>

## Building2DReferencesByPagingParameter\(Building2DReferencesByPagingParameter\) Constructor

Initializes a new instance of the [Building2DReferencesByPagingParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter 'DiGi\.GIS\.WebAPI\.Classes\.Building2DReferencesByPagingParameter') class by copying properties from another instance\.

```csharp
public Building2DReferencesByPagingParameter(DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter building2DReferencesByPagingParameter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter.Building2DReferencesByPagingParameter(DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter).building2DReferencesByPagingParameter'></a>

`building2DReferencesByPagingParameter` [Building2DReferencesByPagingParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter 'DiGi\.GIS\.WebAPI\.Classes\.Building2DReferencesByPagingParameter')

The parameter instance to copy properties from\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter.Building2DReferencesByPagingParameter(System.Text.Json.Nodes.JsonObject)'></a>

## Building2DReferencesByPagingParameter\(JsonObject\) Constructor

Initializes a new instance of the [Building2DReferencesByPagingParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter 'DiGi\.GIS\.WebAPI\.Classes\.Building2DReferencesByPagingParameter') class using a [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public Building2DReferencesByPagingParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter.Building2DReferencesByPagingParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter.CountyId'></a>

## Building2DReferencesByPagingParameter\.CountyId Property

Gets or sets the target partition identifier \(County ID\)\.

```csharp
public int CountyId { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

### Example
10365

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter.Cursor'></a>

## Building2DReferencesByPagingParameter\.Cursor Property

Gets or sets the pagination cursor tracking the last processed building reference\.

```csharp
public string? Cursor { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
BLDG\-12345

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter.PageSize'></a>

## Building2DReferencesByPagingParameter\.PageSize Property

Gets or sets the maximum count of references per page\. Defaults to 250\.

```csharp
public int PageSize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

### Example
100

<a name='DiGi.GIS.WebAPI.Classes.Building2DReferencesByPagingParameter.SubdivisionId'></a>

## Building2DReferencesByPagingParameter\.SubdivisionId Property

Gets or sets the target subdivision identifier \(Subdivision ID\)\. Leave null to return references for the whole county\.

```csharp
public System.Nullable<int> SubdivisionId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

### Example
1035

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask'></a>

## Building2DsPostTask Class

Provides functionality to handle the asynchronous posting of multiple [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') objects to the GIS PostgreSQL database\.

```csharp
public class Building2DsPostTask : DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask<DiGi.GIS.Classes.Building2D>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → Building2DsPostTask
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.Building2DsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## Building2DsPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the Building2DsPostTask class\.

```csharp
public Building2DsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.Building2DsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') used to manage PostgreSQL GIS operations\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.Code'></a>

## Building2DsPostTask\.Code Property

Gets or sets the code associated with the building 2D post task\.

A code does not identify a single county row - a multi-part county holds one row per polygon part - so set [CountyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DsPostTask.CountyId 'DiGi\.GIS\.WebAPI\.Classes\.Building2DsPostTask\.CountyId') instead wherever the identifier is already known. [CountyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DsPostTask.CountyId 'DiGi\.GIS\.WebAPI\.Classes\.Building2DsPostTask\.CountyId') takes precedence when both are set.

```csharp
public string? Code { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.CountyId'></a>

## Building2DsPostTask\.CountyId Property

Gets or sets the identifier of the county row the buildings belong to\. When set it is used in preference to [Code](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DsPostTask.Code 'DiGi\.GIS\.WebAPI\.Classes\.Building2DsPostTask\.Code'), which leaves the server to choose between the rows of a multi\-part county\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken)'></a>

## Building2DsPostTask\.ExecuteAsync\(IEnumerable\<Building2D\>, int, LongProgressWrapper, CancellationToken\) Method

Asynchronously executes the task of posting building 2D objects to the database, keyed by county identifier\.

```csharp
protected System.Threading.Tasks.Task<bool> ExecuteAsync(System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.Building2D>? values, int countyId, DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') instances to post\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the buildings belong to\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).longProgressWrapper'></a>

`longProgressWrapper` [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper')

A [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper') tracking the progress of the operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if all batches were posted successfully; otherwise, false\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken)'></a>

## Building2DsPostTask\.ExecuteAsync\(IEnumerable\<Building2D\>, string, LongProgressWrapper, CancellationToken\) Method

Asynchronously executes the task of posting building 2D objects to the database, keyed by administrative code\.

```csharp
protected System.Threading.Tasks.Task<bool> ExecuteAsync(System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.Building2D>? values, string? code, DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') instances to post\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The administrative code associated with the buildings\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).longProgressWrapper'></a>

`longProgressWrapper` [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper')

A [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper') tracking the progress of the operation\.

<a name='DiGi.GIS.WebAPI.Classes.Building2DsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController'></a>

## BuildingController Class

Provides API endpoints for managing and updating Building data stored in a PostgreSQL database\.

```csharp
public class BuildingController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → BuildingController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.BuildingController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## BuildingController\(GISWebAPIConfigurationFileWatcher, BuildingPostgreSQLConverter, Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the BuildingController class\.

```csharp
public BuildingController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.BuildingPostgreSQLConverter buildingPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.BuildingController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher used to monitor changes to the PostgreSQL Web API configuration\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.BuildingController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).buildingPostgreSQLConverter'></a>

`buildingPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingPostgreSQLConverter')

The converter for Building objects when interacting with a PostgreSQL database\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.BuildingController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The converter for Building2D objects, used to read which county row a reference is already filed under\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.BuildingController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter for administrative areal 2D data when interacting with a PostgreSQL database\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken)'></a>

## BuildingController\.ContainsByReferencesAsync\(List\<string\>, Nullable\<int\>, Nullable\<bool\>, CancellationToken\) Method

Asynchronously checks for the existence of a collection of building references, optionally filtered by a county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> ContainsByReferencesAsync(System.Collections.Generic.List<string>? references, System.Nullable<int> countyId, System.Nullable<bool> inverted, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

A list of strings representing the building references to be checked\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional county identifier used to filter the search\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).inverted'></a>

`inverted` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

A boolean indicating whether to return missing references \(true\) or existing references \(false\)\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing the set of matching reference strings\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetCountAsync(System.Nullable_int_,bool,bool,int,System.Threading.CancellationToken)'></a>

## BuildingController\.GetCountAsync\(Nullable\<int\>, bool, bool, int, CancellationToken\) Method

Asynchronously retrieves the count of building records from the database, optionally filtered by a county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountAsync(System.Nullable<int> countyId, bool estimated=false, bool analyze=false, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetCountAsync(System.Nullable_int_,bool,bool,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional integer identifier of the county to filter the count; if null, the count is retrieved across all counties\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetCountAsync(System.Nullable_int_,bool,bool,int,System.Threading.CancellationToken).estimated'></a>

`estimated` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to read the estimated count from database statistics for faster execution on large partitions\. An unanalysed partition returns 204 NoContent\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetCountAsync(System.Nullable_int_,bool,bool,int,System.Threading.CancellationToken).analyze'></a>

`analyze` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to run an analysis operation before fetching the estimated count to ensure higher accuracy\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetCountAsync(System.Nullable_int_,bool,bool,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetCountAsync(System.Nullable_int_,bool,bool,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing the row count as a long integer, 204 NoContent when the partition exists but is unanalysed, or 404 NotFound when the county partition does not exist\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByLatestCreatedAtAsync(System.Nullable_int_,System.Threading.CancellationToken)'></a>

## BuildingController\.GetItemByLatestCreatedAtAsync\(Nullable\<int\>, CancellationToken\) Method

Asynchronously retrieves the building with the latest created date for an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemByLatestCreatedAtAsync(System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByLatestCreatedAtAsync(System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional integer representing the county ID to filter the results\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByLatestCreatedAtAsync(System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken)'></a>

## BuildingController\.GetItemByReferenceAsync\(string, Nullable\<int\>, Nullable\<double\>, Nullable\<double\>, Nullable\<double\>, Nullable\<double\>, CancellationToken\) Method

Asynchronously retrieves the single most relevant building for the provided reference and an optional county identifier\.

When the X, Y or Z coordinates are provided they are used to break ties between candidates resolved from the reference.

When the reference cannot be resolved and a point is provided, a spatial fallback search limited in X and Y by the maximum distance is performed.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemByReferenceAsync(string? reference, System.Nullable<int> countyId, System.Nullable<double> x=null, System.Nullable<double> y=null, System.Nullable<double> z=null, System.Nullable<double> maxDistance=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string used to identify the building\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional integer representing the county ID to filter the results\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).x'></a>

`x` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional X coordinate of the point used to break ties and to locate the building when the reference cannot be resolved\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).y'></a>

`y` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional Y coordinate of the point used to break ties and to locate the building when the reference cannot be resolved\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).z'></a>

`z` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional Z coordinate of the point used to break ties and to locate the building when the reference cannot be resolved\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).maxDistance'></a>

`maxDistance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional distance used to inflate the point in X and Y into the bounding box of the spatial fallback search\. Defaults to 1\.0 when not provided or invalid\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## BuildingController\.GetItemsByReferenceAsync\(string, Nullable\<int\>, CancellationToken\) Method

Asynchronously retrieves buildings based on a provided reference and an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByReferenceAsync(string? reference, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string used to identify the buildings\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional integer representing the county ID to filter the results\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## BuildingController\.GetItemsByReferencesAsync\(IEnumerable\<string\>, Nullable\<int\>, CancellationToken\) Method

Asynchronously retrieves the single most relevant building for each of the provided references\.

Several rows can share one reference (different level of detail or year); each reference is reduced to one building ranked by level of detail and then by year, matching the behaviour of [GetItemByReferenceAsync\(string, Nullable&lt;int&gt;, Nullable&lt;double&gt;, Nullable&lt;double&gt;, Nullable&lt;double&gt;, Nullable&lt;double&gt;, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.BuildingController\.GetItemByReferenceAsync\(string, System\.Nullable\<int\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Threading\.CancellationToken\)') when no coordinates are supplied.

References without a matching building are omitted, so an empty array is a valid response and does not indicate an error.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable<string>? references, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of unique reference strings used to identify the buildings\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional integer representing the county ID to filter the results\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken)'></a>

## BuildingController\.UpdateItemsAsync\(JsonArray, string, string, CancellationToken\) Method

Updates multiple building items based on the provided JSON array and identification code\.

A county code does not identify a single county row: BDOT10k stores a county whose territory is disconnected as one feature per polygon part, and every part becomes its own row. Every part the code names is passed on, and each building is filed under the part it actually belongs to - see [UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.BuildingController\.UpdateItemsByCountyIdsAsync\(System\.Text\.Json\.Nodes\.JsonArray, int\[\], string, System\.Threading\.CancellationToken\)') for how that is decided.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string? code, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the building items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The identification code required for the update operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') representing the result of the update operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken)'></a>

## BuildingController\.UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\) Method

Updates multiple building items in the database for the given county rows\.

The unambiguous counterpart of [UpdateItemsAsync\(JsonArray, string, string, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.BuildingController\.UpdateItemsAsync\(System\.Text\.Json\.Nodes\.JsonArray, string, string, System\.Threading\.CancellationToken\)'): it takes county identifiers rather than a code, so the caller states which rows are in play instead of leaving the server to derive them.

A single identifier is taken as stated and every building is filed under it. Several identifiers are the polygon parts of one multi-part county, and each building is then filed under the part it belongs to, decided in two steps:

1. the part already holding the building's `building_2d` row, probed lowest part first. That row was filed by geometry when it was imported, and reusing its answer keeps both tables keyed by the same `(county_id, reference)` pair - a building filed under a part its footprint is not stored in reads back as missing.

2. geometry, for a building no part holds a 2D row for: the part containing its bounding box, else the nearest part, else the part it overlaps most. Done by the converter, which drops a building it cannot place rather than filing it under a guess - such a building is reported as a rejection, not silently omitted.

The county-part lookup (step 1) not running is answered as a distinct 500 naming it, before any geometry fallback runs, and a transient database failure as 503 with `Retry-After` - an unreachable database must not look like a decision by geometry.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, int[]? countyIds, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the building items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The identifiers of the county rows the buildings belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') representing the result of the update operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter'></a>

## BuildingDataByFilterGroupParameter Class

Parameter class containing options for building data queries using dynamic hierarchical filters\.

```csharp
public class BuildingDataByFilterGroupParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → BuildingDataByFilterGroupParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter.BuildingDataByFilterGroupParameter()'></a>

## BuildingDataByFilterGroupParameter\(\) Constructor

Initializes a new instance of the [BuildingDataByFilterGroupParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByFilterGroupParameter') class\.

```csharp
public BuildingDataByFilterGroupParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter.BuildingDataByFilterGroupParameter(System.Text.Json.Nodes.JsonObject)'></a>

## BuildingDataByFilterGroupParameter\(JsonObject\) Constructor

Initializes a new instance of the [BuildingDataByFilterGroupParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByFilterGroupParameter') class using an [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public BuildingDataByFilterGroupParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter.BuildingDataByFilterGroupParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter.ColumnUniqueIds'></a>

## BuildingDataByFilterGroupParameter\.ColumnUniqueIds Property

Gets or sets the optional list of column unique identifiers to project in the result\.

```csharp
public System.Collections.Generic.List<string>? ColumnUniqueIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter.CountyId'></a>

## BuildingDataByFilterGroupParameter\.CountyId Property

Gets or sets the county partition to read; `null` reads across all partitions\.

The table is partitioned by `county_id`. Omitting this (and not filtering `county_id` in [FilterGroup](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter.FilterGroup 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByFilterGroupParameter\.FilterGroup')) makes the read span every partition of the national table; supply it unless a cross-partition read is intended.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

### Example
10365

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter.FilterGroup'></a>

## BuildingDataByFilterGroupParameter\.FilterGroup Property

Gets or sets the dynamic hierarchical filter group to apply to the database query\.

```csharp
public DiGi.PostgreSQL.Table.Classes.FilterGroup FilterGroup { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter'></a>

## BuildingDataByPagingParameter Class

Parameter class containing options for paged building data queries, in reference \(keyset\) or physical order\.

```csharp
public class BuildingDataByPagingParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → BuildingDataByPagingParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.BuildingDataByPagingParameter()'></a>

## BuildingDataByPagingParameter\(\) Constructor

Initializes a new instance of the [BuildingDataByPagingParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByPagingParameter') class\.

```csharp
public BuildingDataByPagingParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.BuildingDataByPagingParameter(System.Text.Json.Nodes.JsonObject)'></a>

## BuildingDataByPagingParameter\(JsonObject\) Constructor

Initializes a new instance of the [BuildingDataByPagingParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByPagingParameter') class using an [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public BuildingDataByPagingParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.BuildingDataByPagingParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.ColumnUniqueIds'></a>

## BuildingDataByPagingParameter\.ColumnUniqueIds Property

Gets or sets the list of column unique identifiers to project in the result\.

```csharp
public System.Collections.Generic.List<string>? ColumnUniqueIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

### Example
\["building\_id", "address"\]

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.CountyId'></a>

## BuildingDataByPagingParameter\.CountyId Property

Gets or sets the target partition identifier \(County ID\)\.

```csharp
public int CountyId { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

### Example
10365

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.Cursor'></a>

## BuildingDataByPagingParameter\.Cursor Property

Gets or sets where the page continues from; null or omitted starts at the beginning of the county part\.

In reference order it is the previous page's last `reference`, verbatim. In physical order it is the value of the previous response's `DiGi-Next-Cursor` header - an opaque heap position such as `(412,7)` - or, after a response that came back in reference order without the header, the last row's `reference`.

```csharp
public string? Cursor { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
30FA023C\-190B\-87D7\-E053\-CA2BA8C08B17

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.PageSize'></a>

## BuildingDataByPagingParameter\.PageSize Property

Gets or sets the maximum count of rows per page\. Defaults to 250\.

Capped because a building data row carries every derived column of a building, so a page is far heavier than its row count suggests. Ask for more pages rather than a bigger one.

```csharp
public int PageSize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

### Example
100

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.PhysicalOrder'></a>

## BuildingDataByPagingParameter\.PhysicalOrder Property

Gets or sets a value indicating whether the part is read in physical \(heap\) order rather than by `reference`\. Defaults to false, the reference order this endpoint has always answered\.

Physical order reads the part sequentially instead of one random heap read per row. Measured on production: 368-654 s for a cold 155 307-row part in reference order, against about 15 s per 100 000 rows read sequentially (DiGi.GIS.WebAPI.UI#29). Use it to read a whole part when row order does not matter. The next cursor then arrives in the `DiGi-Next-Cursor` response header, and the endpoint falls back to reference order where physical order is unavailable (see the action's remarks).

```csharp
public bool PhysicalOrder { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

### Example
true

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter'></a>

## BuildingDataByReferencesParameter Class

Represents a parameter containing references for querying building data\.

```csharp
public class BuildingDataByReferencesParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → BuildingDataByReferencesParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter.BuildingDataByReferencesParameter()'></a>

## BuildingDataByReferencesParameter\(\) Constructor

Initializes a new instance of the [BuildingDataByReferencesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByReferencesParameter') class\.

```csharp
public BuildingDataByReferencesParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter.BuildingDataByReferencesParameter(DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter)'></a>

## BuildingDataByReferencesParameter\(BuildingDataByReferencesParameter\) Constructor

Initializes a new instance of the [BuildingDataByReferencesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByReferencesParameter') class using an existing [BuildingDataByReferencesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByReferencesParameter') object\.

```csharp
public BuildingDataByReferencesParameter(DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter buildingDataByReferencesParameter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter.BuildingDataByReferencesParameter(DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter).buildingDataByReferencesParameter'></a>

`buildingDataByReferencesParameter` [BuildingDataByReferencesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByReferencesParameter')

The parameter object to copy data from\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter.BuildingDataByReferencesParameter(System.Text.Json.Nodes.JsonObject)'></a>

## BuildingDataByReferencesParameter\(JsonObject\) Constructor

Initializes a new instance of the [BuildingDataByReferencesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByReferencesParameter') class using a JSON object\.

```csharp
public BuildingDataByReferencesParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter.BuildingDataByReferencesParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing parameter data\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter.ColumnUniqueIds'></a>

## BuildingDataByReferencesParameter\.ColumnUniqueIds Property

Gets or sets the unique identifiers of the columns \(Column\.UniqueId\)\. All columns will be returned if the collection is null or empty\.
Required if performance is a concern and the column unique identifiers are available; otherwise, the column unique identifiers will be determined by the building data PostgreSQL converter\.

```csharp
public System.Collections.Generic.IEnumerable<string>? ColumnUniqueIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter.CountyId'></a>

## BuildingDataByReferencesParameter\.CountyId Property

Gets or sets the county identifier\.
Required if performance is a concern and the county identifier is available; otherwise, the county identifier will be determined by the building data PostgreSQL converter\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter.References'></a>

## BuildingDataByReferencesParameter\.References Property

Gets or sets the references for the building data parameter\.

```csharp
public System.Collections.Generic.IEnumerable<string> References { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter'></a>

## BuildingDataBySubdivisionIdsParameter Class

Represents a parameter containing subdivision ids for querying building data\.

```csharp
public class BuildingDataBySubdivisionIdsParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → BuildingDataBySubdivisionIdsParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter.BuildingDataBySubdivisionIdsParameter()'></a>

## BuildingDataBySubdivisionIdsParameter\(\) Constructor

Initializes a new instance of the [BuildingDataBySubdivisionIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataBySubdivisionIdsParameter') class\.

```csharp
public BuildingDataBySubdivisionIdsParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter.BuildingDataBySubdivisionIdsParameter(DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter)'></a>

## BuildingDataBySubdivisionIdsParameter\(BuildingDataBySubdivisionIdsParameter\) Constructor

Initializes a new instance of the [BuildingDataBySubdivisionIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataBySubdivisionIdsParameter') class using an existing [BuildingDataBySubdivisionIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataBySubdivisionIdsParameter') object\.

```csharp
public BuildingDataBySubdivisionIdsParameter(DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter buildingDataBySubdivisionIdsParameter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter.BuildingDataBySubdivisionIdsParameter(DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter).buildingDataBySubdivisionIdsParameter'></a>

`buildingDataBySubdivisionIdsParameter` [BuildingDataBySubdivisionIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataBySubdivisionIdsParameter')

The parameter object to copy data from\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter.BuildingDataBySubdivisionIdsParameter(System.Text.Json.Nodes.JsonObject)'></a>

## BuildingDataBySubdivisionIdsParameter\(JsonObject\) Constructor

Initializes a new instance of the [BuildingDataBySubdivisionIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataBySubdivisionIdsParameter') class using a JSON object\.

```csharp
public BuildingDataBySubdivisionIdsParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter.BuildingDataBySubdivisionIdsParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing parameter data\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter.ColumnUniqueIds'></a>

## BuildingDataBySubdivisionIdsParameter\.ColumnUniqueIds Property

Gets or sets the unique identifiers of the columns \(Column\.UniqueId\)\. All columns will be returned if the collection is null or empty\.
Required if performance is a concern and the column unique identifiers are available; otherwise, the column unique identifiers will be determined by the building data PostgreSQL converter\.

```csharp
public System.Collections.Generic.IEnumerable<string>? ColumnUniqueIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter.SubdivisionIds'></a>

## BuildingDataBySubdivisionIdsParameter\.SubdivisionIds Property

Gets or sets the subdivision ids for the building data

```csharp
public System.Collections.Generic.IEnumerable<int> SubdivisionIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController'></a>

## BuildingDataController Class

Controller responsible for handling API requests related to building data retrieved from a PostgreSQL database\.

```csharp
public class BuildingDataController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → BuildingDataController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.BuildingDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## BuildingDataController\(GISWebAPIConfigurationFileWatcher, BuildingDataPostgreSQLConverter, Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the BuildingDataController class\.

Both converters and the configuration watcher are taken on the one constructor, because the coverage read compares two tables that sit in different databases and write operations require authorization. A second constructor is not an option: a controller with more than one public constructor fails activation and answers 500 on every one of its endpoints.

```csharp
public BuildingDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter buildingDataPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.BuildingDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher') used to verify authorization and permissions for write operations\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.BuildingDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).buildingDataPostgreSQLConverter'></a>

`buildingDataPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingdatapostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataPostgreSQLConverter')

The [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingdatapostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataPostgreSQLConverter') used to handle building data operations and database conversions\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.BuildingDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter') used to read the buildings a county holds, which is the other half of the coverage comparison\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.BuildingDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter') used to widen a row no named part holds to every part of the named county\.
### Fields

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.referenceCount_Maximum'></a>

## BuildingDataController\.referenceCount\_Maximum Field

The largest number of references one request may ask for\.

The whole collection travels into a single statement, so an unbounded list is an unbounded statement. A caller with more than this to ask about should page rather than widen the request.

```csharp
private const int referenceCount_Maximum = 10000;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetAggregateSummariesAsync(DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetAggregateSummariesAsync\(AggregateSummariesRequestParameter, int, CancellationToken\) Method

Computes several single\-value aggregate measures \- different columns, different functions \- over one shared partition and filter in one request, served as one database command on one connection \(DiGi\-Organization/DiGi\.GIS\.WebAPI\#56\)\.

The answers arrive as a JSON array in the request's measure order; a SQL `NULL` aggregate (for example `Avg` over zero rows) is preserved as JSON `null`. An empty `Measures` list, a list above the cap of [MaximumAggregateSummariesMeasureCount](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.BuildingData.MaximumAggregateSummariesMeasureCount 'DiGi\.GIS\.WebAPI\.Constants\.BuildingData\.MaximumAggregateSummariesMeasureCount'), or a measure naming a column the stored table does not hold is refused with 400 - a measure failure answers the whole request, never a partial result (DiGi-Organization/DiGi.GIS.WebAPI#56).

Out of scope by design: per-measure filters, multi-value measures (`SplitValueDistribution`, `SplitDistinctCount`) and histograms (DiGi-Organization/DiGi.GIS.WebAPI#56).

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAggregateSummariesAsync(DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter aggregateSummariesRequestParameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetAggregateSummariesAsync(DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter,int,System.Threading.CancellationToken).aggregateSummariesRequestParameter'></a>

`aggregateSummariesRequestParameter` [AggregateSummariesRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.AggregateSummariesRequestParameter')

The parameter containing the shared county identifier, the shared filter, and the ordered list of measures\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetAggregateSummariesAsync(DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetAggregateSummariesAsync(DiGi.GIS.WebAPI.Classes.AggregateSummariesRequestParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning the measures as a JSON array in request order\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCategoriesAsync(int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetCategoriesAsync\(int, CancellationToken\) Method

Asynchronously retrieves all available building data column categories\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCategoriesAsync(int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCategoriesAsync(int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCategoriesAsync(int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnReferencesAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetColumnReferencesAsync\(List\<string\>, int, CancellationToken\) Method

Asynchronously retrieves all column references, optionally filtered by the specified categories\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetColumnReferencesAsync(System.Collections.Generic.List<string>? categories=null, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnReferencesAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken).categories'></a>

`categories` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

An optional list of category names to filter the column references by\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnReferencesAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnReferencesAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning a list of column references\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsAsync(int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetColumnsAsync\(int, CancellationToken\) Method

Asynchronously retrieves all available column definitions for building data\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetColumnsAsync(int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsAsync(int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsAsync(int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsByCategoriesAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetColumnsByCategoriesAsync\(List\<string\>, int, CancellationToken\) Method

Asynchronously retrieves all columns filtered by the specified categories\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetColumnsByCategoriesAsync(System.Collections.Generic.List<string>? categories=null, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsByCategoriesAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken).categories'></a>

`categories` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

An optional list of category names to filter the columns by\. If null, the filtering behavior is determined by the underlying data source\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsByCategoriesAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsByCategoriesAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsByCategoriesParameterAsync(DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetColumnsByCategoriesParameterAsync\(ColumnsByCategoriesParameter, int, CancellationToken\) Method

Retrieves all columns with given categories by columns by categories parameter \(which contains categories\)\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetColumnsByCategoriesParameterAsync(DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter columnsByCategoriesParameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsByCategoriesParameterAsync(DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter,int,System.Threading.CancellationToken).columnsByCategoriesParameter'></a>

`columnsByCategoriesParameter` [ColumnsByCategoriesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter 'DiGi\.GIS\.WebAPI\.Classes\.ColumnsByCategoriesParameter')

The parameter containing the categories for querying columns\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsByCategoriesParameterAsync(DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnsByCategoriesParameterAsync(DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
Column [DiGi\.PostgreSQL\.Table\.Classes\.Column](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.column 'DiGi\.PostgreSQL\.Table\.Classes\.Column')

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnUniqueIdsAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetColumnUniqueIdsAsync\(List\<string\>, int, CancellationToken\) Method

Retrieves the unique identifiers for columns, optionally filtered by the specified categories\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetColumnUniqueIdsAsync(System.Collections.Generic.List<string>? categories=null, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnUniqueIdsAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken).categories'></a>

`categories` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

An optional list of category names used to filter the column references\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnUniqueIdsAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetColumnUniqueIdsAsync(System.Collections.Generic.List_string_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByAdministrativeAreal2DIdAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetCountByAdministrativeAreal2DIdAsync\(BuildingDataCountByAdministrativeAreal2DIdParameter, int, CancellationToken\) Method

Asynchronously counts the building data rows of an administrative area, optionally narrowed by a dynamic hierarchical filter group, in one call\.

An invalid filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator (DiGi-Organization/DiGi.GIS.WebAPI#54).

Resolution: (1) the area reference is read by id, and a missing one answers 404. (2) An area <b>above</b> the Subdivision level (Country, Voivodeship, County, Municipality) is resolved to its Subdivision children, grouped by the county partition they sit in. (3) An area that <b>is</b> a Subdivision is counted by membership, `subdivision_id = id`. (4) Per distinct county - the `building_data` partition key - one count runs with `subdivision_id IN (that county's subdivisions)` plus the caller's filters, and the counts are <b>summed</b>. The counties are counted strictly one after another, never in parallel, because concurrent requests exhaust the Npgsql pool and turn healthy endpoints into 500s. (5) An area above the Subdivision level that resolves to no Subdivision answers 204.

Duration (measured 2026-10-14 on the estate; figures are measurements, not guarantees): a single county area returns in under a second; a voivodeship (typically 10–20 counties) takes roughly 1–3 minutes; a country-level area spans 400+ counties and exceeds normal client timeouts. `commandtimeout` applies per command, not per request: each sequential per-county count gets its own timeout, so total wall-clock can approach `N × commandtimeout` in the worst case.

What the count is not. `subdivision_id` files a building under its <b>smallest</b> containing subdivision and the Subdivision layer nests, so for a nested Subdivision (a district whose neighbourhoods are also subdivisions) the count covers only the buildings filed under that exact id and undercounts the district (DiGi.GIS.PostgreSQL#75, #77); a polygon-based count is a follow-up. Rows with `subdivision_id IS NULL` are excluded, so the count of an area is not the count of every building inside its polygon. A building filed in a county partition other than the one its subdivision belongs to (a subdivision of a neighbouring county) is not reached, because each county is counted with only its own subdivisions.

An unfiltered body is valid (`FilterGroup` omitted counts every building the area resolves to). At most [MaximumAdministrativeAreal2DCountyCount](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.BuildingData.MaximumAdministrativeAreal2DCountyCount 'DiGi\.GIS\.WebAPI\.Constants\.BuildingData\.MaximumAdministrativeAreal2DCountyCount') distinct counties are iterated; an area spanning more answers 400 naming that limit and nothing is counted. 204 means <b>the resolution found no subdivision to filter by - not evidence that the area holds no buildings</b>. 404 means the area does not exist, or no connection could be built, or a filter column could not be resolved. A filter value that cannot be converted to its column's type answers 400.

A client that sends the same `In` filter shape as a JSON array is subject to DiGi-Organization/DiGi.PostgreSQL#10; this endpoint builds its own subdivision list server-side and does not depend on that fix.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountByAdministrativeAreal2DIdAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter parameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByAdministrativeAreal2DIdAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter,int,System.Threading.CancellationToken).parameter'></a>

`parameter` [BuildingDataCountByAdministrativeAreal2DIdParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataCountByAdministrativeAreal2DIdParameter')

The parameter containing the administrative area identifier and the optional dynamic hierarchical filters\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByAdministrativeAreal2DIdAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of each command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByAdministrativeAreal2DIdAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the count as a bare JSON integer, 204 NoContent when no subdivision was found, 404 NotFound, or 400/503/500 on the guarded and failure paths\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetCountByCountyIdAsync\(int, bool, bool, int, CancellationToken\) Method

Asynchronously retrieves the number of building data rows stored for one county\.

The cheapest question that can be asked of the table, and the one that separates a county no run has reached from one a run reached and wrote nothing for.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountByCountyIdAsync(int countyId, bool estimated=false, bool analyze=false, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county to count\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).estimated'></a>

`estimated` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Reads the planner's row estimate instead of counting the rows\. Far faster on a partition of millions and accurate to a few percent, but it reflects the last time the partition was analysed rather than this moment\. An unanalysed partition returns 204 NoContent\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).analyze'></a>

`analyze` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to perform an ANALYZE operation before reading the estimate to ensure statistics are current\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the count, 204 NoContent when the partition exists but is unanalysed, or 404 NotFound when the county has no partition\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetCountByFilterGroupAsync\(BuildingDataCountByFilterGroupParameter, int, CancellationToken\) Method

Asynchronously counts the building data rows of a county partition, or of the whole table, matching an optional dynamic hierarchical filter group\.

The table is partitioned by `county_id`. Supply [CountyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter.CountyId 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataCountByFilterGroupParameter\.CountyId') to scope the read to one county partition; omitting it (and not filtering `county_id` in the filter group) makes the read span every partition of the national table, which is the cost of an unscoped cross-partition read. Measured 2026-10-14 on the estate: with a `CountyId` the count returns in 0.1 s; without one it exceeds 300 s (client timeout). The sibling `GET countbycountyid` refuses an unscoped request with 400 (`[BindRequired]`); this endpoint accepts it. Figures are measurements, not guarantees.

The answer is a bare JSON integer: the `COUNT(*)` of the scope, so it does not depend on any column being non-null and it never needs the magic `reference` column or an aggregate function. A `null` or empty filter group counts the whole partition scope; a filter that matches nothing answers `0` with a `200`, not an error status.

An invalid filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator (DiGi-Organization/DiGi.GIS.WebAPI#54).

De-duplication across county parts is a separate concern (`referenceuniquenesssummary`) and is not applied here.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter parameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter,int,System.Threading.CancellationToken).parameter'></a>

`parameter` [BuildingDataCountByFilterGroupParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataCountByFilterGroupParameter')

The parameter containing the optional partition \(county\) and the optional dynamic hierarchical filters\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the count as a bare JSON integer, 404 NotFound when no connection could be built or a filter column could not be resolved, or 400/503/500 on the guarded and failure paths\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountyIdsByReferenceAsync(string,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetCountyIdsByReferenceAsync\(string, int, CancellationToken\) Method

Asynchronously retrieves the counties whose building data holds a row for one reference\.

A reference addresses one building of one county, so more than one identifier coming back means the reference was written outside the county it belongs to.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountyIdsByReferenceAsync(string reference, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountyIdsByReferenceAsync(string,int,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The building reference to look up\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountyIdsByReferenceAsync(string,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCountyIdsByReferenceAsync(string,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the county identifiers in ascending order, or 404 when the reference is not stored\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCoverageByCountyIdAsync(int,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetCoverageByCountyIdAsync\(int, int, CancellationToken\) Method

Asynchronously measures what one county's building data holds against the buildings that county actually has\.

What a row count cannot answer: how much was left out, and how much of that no run could have reached. A shortfall larger than the unresolved subdivision count is a run that did not finish what it could have.

Reads both databases - the buildings from the main one and their data from the storage one - so it costs more than a count. Call it per county rather than in a sweep.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCoverageByCountyIdAsync(int countyId, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCoverageByCountyIdAsync(int,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county to measure\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCoverageByCountyIdAsync(int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetCoverageByCountyIdAsync(int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataCoverageResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingdatacoverageresult 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataCoverageResult'), or 404 when either side could not be read\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetDuplicateReferencesAsync(int,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetDuplicateReferencesAsync\(int, int, CancellationToken\) Method

Asynchronously retrieves the references the building data holds under more than one county, ordered by collision count descending\.

Expected to come back empty. A reference addresses one building of one county, so anything listed here was written outside the county it belongs to and nothing removes it afterwards.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetDuplicateReferencesAsync(int limit=100, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetDuplicateReferencesAsync(int,int,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of references to return\. Defaults to 100\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetDuplicateReferencesAsync(int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetDuplicateReferencesAsync(int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the duplicated references, or 404 when there are none\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetHistogramSummaryAsync(DiGi.GIS.WebAPI.Classes.HistogramRequestParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetHistogramSummaryAsync\(HistogramRequestParameter, int, CancellationToken\) Method

Generates a value range distribution histogram for a specific building data column inside a county partition, applying optional dynamic filters\.

Each row of the answer is one bucket - `{bucket, rangeStart, rangeEnd, count}`, the actual minimum and maximum of the values the bucket holds and their count. The buckets are of equal value width unless the body asks for [DiGi\.PostgreSQL\.Table\.Enums\.HistogramBucketing\.EqualCount](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.enums.histogrambucketing.equalcount 'DiGi\.PostgreSQL\.Table\.Enums\.HistogramBucketing\.EqualCount') (`HistogramBucketing: 1`), which gives every bucket the same number of rows and keeps a skewed column's resolution where its rows are (DiGi-Organization/DiGi.GIS.WebAPI#35).

An invalid target or filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator. A scope that holds no non-NULL values keeps its documented 404 empty answer (DiGi-Organization/DiGi.GIS.WebAPI#54).

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetHistogramSummaryAsync(DiGi.GIS.WebAPI.Classes.HistogramRequestParameter histogramRequestParameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetHistogramSummaryAsync(DiGi.GIS.WebAPI.Classes.HistogramRequestParameter,int,System.Threading.CancellationToken).histogramRequestParameter'></a>

`histogramRequestParameter` [HistogramRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.HistogramRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.HistogramRequestParameter')

The parameter containing the target column, county identifier, desired bucket count, bucketing rule, and optional dynamic filters\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetHistogramSummaryAsync(DiGi.GIS.WebAPI.Classes.HistogramRequestParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetHistogramSummaryAsync(DiGi.GIS.WebAPI.Classes.HistogramRequestParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning the histogram bucket list as a JSON array\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetMultivalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetMultivalueAggregateSummaryAsync\(MultivalueAggregateRequestParameter, int, CancellationToken\) Method

Computes multi\-value statistical summaries \(SplitDistinctCount, SplitValueDistribution\) on a partition column\.

An invalid target or filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator (DiGi-Organization/DiGi.GIS.WebAPI#54).

Group-by count: `SplitValueDistribution` (`multivalueAggregateFunction = 1`) on a single-value column returns one `{ "item", "count" }` object per distinct value, ordered `count` descending. Worked example (2026-10-12, partition `countyId 53477`, gmina Łomianki): `columnUniqueId "municipality_name"` answers `[{ "item": "Łomianki", "count": 10502 }, { "item": "Stare Babice", "count": 7976 }, …]`.

The result is capped at 1000 groups plus one trailing truncation-marker row (drop the last element when the array is that long). A non-text column is cast to its text form and distributed over it, so a boolean such as `is_residential` yields `true`/`false` items rather than a failure (DiGi-Organization/DiGi.PostgreSQL#11).

`SplitDistinctCount` (`multivalueAggregateFunction = 0`) counts the distinct split sub-items instead of returning them. For the plain distinct-value list (no split, no count), use `uniquevalues`.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetMultivalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter multivalueAggregateRequestParameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetMultivalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter,int,System.Threading.CancellationToken).multivalueAggregateRequestParameter'></a>

`multivalueAggregateRequestParameter` [MultivalueAggregateRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.MultivalueAggregateRequestParameter')

The parameter containing target column, multi\-value aggregate function, county identifier, and optional separator\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetMultivalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetMultivalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning the aggregate result as a JSON node\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetSinglevalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetSinglevalueAggregateSummaryAsync\(SinglevalueAggregateRequestParameter, int, CancellationToken\) Method

Computes single\-value statistical summaries \(Avg, Sum, Min, Max, Count, DistinctCount\) on a partition column\.

An invalid target or filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator; a valid filter that matches nothing keeps its 200 empty aggregate (DiGi-Organization/DiGi.GIS.WebAPI#54).

Row count: to count the matching rows, set `columnUniqueId "reference"` and `singlevalueAggregateFunction 4` (`Count`) - `reference` is the always-present column and `Count` tallies the non-null rows, so the pair is the plain "how many rows" recipe. `singlevalueAggregateFunction` values: 0 `Avg` (mean of a numeric column), 1 `Sum` (total), 2 `Min`, 3 `Max`, 4 `Count` (non-null row count), 5 `DistinctCount` (unique values).

Count with a filter - "residential buildings over 100 m² in gmina Łomianki" (2026-10-12, partition `countyId 53477`): `{ "columnUniqueId": "reference", "singlevalueAggregateFunction": 4, "countyId": 53477, "filterGroup": { "logicalOperator": 0, "filterConditions": [ { "columnUniqueId": "is_residential", "filterOperator": 0, "value": true }, { "columnUniqueId": "floor_area", "filterOperator": 2, "value": 100 } ] } }` answers `6798` (unfiltered anchor `13719`; DiGi-Organization/DiGi.GIS.WebAPI#53).

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSinglevalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter singlevalueAggregateRequestParameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetSinglevalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter,int,System.Threading.CancellationToken).singlevalueAggregateRequestParameter'></a>

`singlevalueAggregateRequestParameter` [SinglevalueAggregateRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.SinglevalueAggregateRequestParameter')

The parameter containing target column, single\-value aggregate function, and county identifier\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetSinglevalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetSinglevalueAggregateSummaryAsync(DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning the aggregate result as a JSON node\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataByPagingParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetTableByBuildingDataByPagingParameterAsync\(BuildingDataByPagingParameter, int, CancellationToken\) Method

Retrieves one page of a county part's building data, following a cursor from page to page\.

<b>Reference order</b> (default, [PhysicalOrder](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.PhysicalOrder 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByPagingParameter\.PhysicalOrder') false): rows ordered by `reference`. The cursor is the previous page's last `reference`, and a page shorter than `PageSize` ends the part. Every row is one random heap read, which on a large part is slow: part 55417 (155 307 rows) took 368-654 s cold (DiGi.GIS.WebAPI.UI#29).

<b>Physical order</b> ([PhysicalOrder](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter.PhysicalOrder 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByPagingParameter\.PhysicalOrder') true): rows in heap order, read sequentially. The next cursor is returned in the `DiGi-Next-Cursor` response header ([NextCursor](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Header.NextCursor 'DiGi\.GIS\.WebAPI\.Constants\.Header\.NextCursor')), and a response without the header ends the part. The page may still carry the part's last rows. Row order is unspecified. When the physical read is unavailable - a database older than PostgreSQL 14, or a `Cursor` that is not a physical position - the same request is answered by the reference-ordered page for that `Cursor`, without the header. A client that keeps asking for physical order and follows the last row's `reference` whenever the header is absent from a full page therefore pages correctly either way.

<b>Concurrent writes.</b> Each request reads its own snapshot. In physical order a row rewritten during a walk moves: it is repeated when its new version lands ahead of the walk and missed when it lands behind. Clients dedup on `(reference, county_id)`, which covers the repeats. The misses are the price of the sequential read and matter only while the part is being rewritten.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetTableByBuildingDataByPagingParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter buildingDataByPagingParameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataByPagingParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter,int,System.Threading.CancellationToken).buildingDataByPagingParameter'></a>

`buildingDataByPagingParameter` [BuildingDataByPagingParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByPagingParameter')

The parameter containing paging options, including column projections, county identifier, cursor, page size and order\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataByPagingParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataByPagingParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByPagingParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning the populated table\. In physical order the next cursor, if any, is in the `DiGi-Next-Cursor` header\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataByReferencesParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetTableByBuildingDataByReferencesParameterAsync\(BuildingDataByReferencesParameter, int, CancellationToken\) Method

Retrieves a building data table by building data by references parameter \(column unique ids, county id and references\)\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetTableByBuildingDataByReferencesParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter buildingDataByReferencesParameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataByReferencesParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter,int,System.Threading.CancellationToken).buildingDataByReferencesParameter'></a>

`buildingDataByReferencesParameter` [BuildingDataByReferencesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByReferencesParameter')

The parameter containing references for querying building data, including column unique identifiers, county identifier, and specific references\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataByReferencesParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataByReferencesParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByReferencesParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') representing the result of the operation, typically containing a [DiGi\.Core\.IO\.Table\.Classes\.Table](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.table.classes.table 'DiGi\.Core\.IO\.Table\.Classes\.Table') if found\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataBySubdivisionIdsParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetTableByBuildingDataBySubdivisionIdsParameterAsync\(BuildingDataBySubdivisionIdsParameter, int, CancellationToken\) Method

Retrieves a building data table by building data by subdivision ids parameter \(column unique ids, subdivision ids\)\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetTableByBuildingDataBySubdivisionIdsParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter buildingDataBySubdivisionIdsParameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataBySubdivisionIdsParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter,int,System.Threading.CancellationToken).buildingDataBySubdivisionIdsParameter'></a>

`buildingDataBySubdivisionIdsParameter` [BuildingDataBySubdivisionIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataBySubdivisionIdsParameter')

The parameter containing the subdivision IDs and optional column unique identifiers for querying building data\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataBySubdivisionIdsParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByBuildingDataBySubdivisionIdsParameterAsync(DiGi.GIS.WebAPI.Classes.BuildingDataBySubdivisionIdsParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetTableByFilterGroupAsync\(BuildingDataByFilterGroupParameter, int, CancellationToken\) Method

Retrieves a building data table filtered by the specified dynamic hierarchical filters\.

The table is partitioned by `county_id`. Supply [CountyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter.CountyId 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByFilterGroupParameter\.CountyId') to scope the read to one county partition; omitting it (and not filtering `county_id` in the filter group) makes the read span every partition of the national table, which is the cost of an unscoped cross-partition read. A caller `county_id` condition in the filter group is left as-is and the `CountyId` condition is ANDed with it, so a conflicting pair answers empty.

An invalid filter column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator (DiGi-Organization/DiGi.GIS.WebAPI#54).

Per-operator value shapes: each `FilterCondition`'s `Value` takes a scalar for the comparison operators, a JSON array for `In`/`NotIn`, and a case-insensitive substring for `Contains`; an unconvertible value is refused with 400 (see `FilterCondition.Value` for the full contract; DiGi-Organization/DiGi.PostgreSQL#13).

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetTableByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter buildingDataByFilterGroupParameter, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter,int,System.Threading.CancellationToken).buildingDataByFilterGroupParameter'></a>

`buildingDataByFilterGroupParameter` [BuildingDataByFilterGroupParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataByFilterGroupParameter')

The parameter containing the dynamic filter group, the optional county partition, and the optional column unique identifiers\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByFilterGroupAsync(DiGi.GIS.WebAPI.Classes.BuildingDataByFilterGroupParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning the populated filtered table\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByReferenceAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetTableByReferenceAsync\(string, Nullable\<int\>, int, CancellationToken\) Method

Retrieves a building data table for one specific building\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetTableByReferenceAsync(string reference, System.Nullable<int> countyId=null, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByReferenceAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Building reference

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByReferenceAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The unique identifier of the county for which building belongs to\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByReferenceAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetTableByReferenceAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning the populated filtered table with data for sigle building\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetUniqueValuesAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetUniqueValuesAsync\(string, Nullable\<int\>, int, CancellationToken\) Method

Retrieves unique values for a specified column unique identifier and an optional county identifier\.

An invalid column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator; a scope that holds nothing keeps its documented 404 empty answer (DiGi-Organization/DiGi.GIS.WebAPI#54).

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetUniqueValuesAsync(string columnUniqueId, System.Nullable<int> countyId=null, int commandTimeout=90, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetUniqueValuesAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken).columnUniqueId'></a>

`columnUniqueId` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique identifier of the column from which to retrieve unique values\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetUniqueValuesAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional integer identifier of the county used to filter the results\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetUniqueValuesAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 90 seconds; a national \(unfiltered\) unique\-values scan runs well past that and should pass `commandtimeout=600` explicitly\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetUniqueValuesAsync(string,System.Nullable_int_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetUniqueValuesByColumnUniqueIdParameterAsync(DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter,int,System.Threading.CancellationToken)'></a>

## BuildingDataController\.GetUniqueValuesByColumnUniqueIdParameterAsync\(UniqueValuesByColumnUniqueIdParameter, int, CancellationToken\) Method

Retrieves unique values for a given [UniqueValuesByColumnUniqueIdParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter 'DiGi\.GIS\.WebAPI\.Classes\.UniqueValuesByColumnUniqueIdParameter') \(column unique id and optionally county id\), applying optional dynamic filters\.

An invalid column, or a filter value the column's type cannot convert, is refused with 400 Bad Request naming the offending column and, where known, the operator; a scope that holds nothing keeps its documented 404 empty answer (DiGi-Organization/DiGi.GIS.WebAPI#54).

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetUniqueValuesByColumnUniqueIdParameterAsync(DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter uniqueValuesByColumnUniqueIdParameter, int commandTimeout=90, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetUniqueValuesByColumnUniqueIdParameterAsync(DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter,int,System.Threading.CancellationToken).uniqueValuesByColumnUniqueIdParameter'></a>

`uniqueValuesByColumnUniqueIdParameter` [UniqueValuesByColumnUniqueIdParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter 'DiGi\.GIS\.WebAPI\.Classes\.UniqueValuesByColumnUniqueIdParameter')

The parameter containing the column unique identifier, optional county identifier, and optional dynamic filters\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetUniqueValuesByColumnUniqueIdParameterAsync(DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 90 seconds; a national \(unfiltered\) unique\-values scan runs well past that and should pass `commandtimeout=600` explicitly\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.GetUniqueValuesByColumnUniqueIdParameterAsync(DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') representing the result of the operation, typically a list of unique values or a not found status\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonObject,int[],int,string,System.Threading.CancellationToken)'></a>

## BuildingDataController\.UpdateItemsByCountyIdsAsync\(JsonObject, int\[\], int, string, CancellationToken\) Method

Asynchronously updates building data for the specified county identifiers\.

The identifiers are the parts of one county in play, and each row is filed under the part already holding the `building_2d` row its reference names, probed lowest part first - whether one identifier arrived or several, since naming one part is not evidence the county has one. That row was filed by geometry when it was imported, so reusing its answer keeps both tables keyed by the same `(county_id, reference)` pair.

A row no named part holds is widened to every part of the county its parts name, and only a row no part of the county holds is left unwritten - nothing states where it belongs, and storing it under a guessed part is the state this replaced.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonObject? jsonObject, int[]? countyIds, int commandTimeout=600, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonObject,int[],int,string,System.Threading.CancellationToken).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing the table structure and data to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonObject,int[],int,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The identifiers of the county rows the building data belongs to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonObject,int[],int,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the database command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonObject,int[],int,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonObject,int[],int,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation with an [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing an [UpdateItemsResult](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UpdateItemsResult 'DiGi\.GIS\.WebAPI\.Classes\.UpdateItemsResult')\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter'></a>

## BuildingDataCountByAdministrativeAreal2DIdParameter Class

Parameter class containing options for counting the building data rows of an administrative area matching an optional dynamic hierarchical filter group\.

```csharp
public class BuildingDataCountByAdministrativeAreal2DIdParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → BuildingDataCountByAdministrativeAreal2DIdParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter.BuildingDataCountByAdministrativeAreal2DIdParameter()'></a>

## BuildingDataCountByAdministrativeAreal2DIdParameter\(\) Constructor

Initializes a new instance of the [BuildingDataCountByAdministrativeAreal2DIdParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataCountByAdministrativeAreal2DIdParameter') class\.

```csharp
public BuildingDataCountByAdministrativeAreal2DIdParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter.BuildingDataCountByAdministrativeAreal2DIdParameter(System.Text.Json.Nodes.JsonObject)'></a>

## BuildingDataCountByAdministrativeAreal2DIdParameter\(JsonObject\) Constructor

Initializes a new instance of the [BuildingDataCountByAdministrativeAreal2DIdParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataCountByAdministrativeAreal2DIdParameter') class using an [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public BuildingDataCountByAdministrativeAreal2DIdParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter.BuildingDataCountByAdministrativeAreal2DIdParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter.AdministrativeAreal2DId'></a>

## BuildingDataCountByAdministrativeAreal2DIdParameter\.AdministrativeAreal2DId Property

Gets or sets the identifier of the administrative area whose buildings are counted\.

Nullable so that an omitted property is distinguishable from a legitimate value and can be refused with HTTP 400 rather than silently meaning `0`.

```csharp
public System.Nullable<int> AdministrativeAreal2DId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

### Example
53469

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByAdministrativeAreal2DIdParameter.FilterGroup'></a>

## BuildingDataCountByAdministrativeAreal2DIdParameter\.FilterGroup Property

Gets or sets the optional dynamic hierarchical filters applied on top of the area; `null` \(or an empty group\) counts every building the area resolves to\.

```csharp
public DiGi.PostgreSQL.Table.Classes.FilterGroup? FilterGroup { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter'></a>

## BuildingDataCountByFilterGroupParameter Class

Parameter class containing options for counting building data rows matching an optional dynamic hierarchical filter group\.

```csharp
public class BuildingDataCountByFilterGroupParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → BuildingDataCountByFilterGroupParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter.BuildingDataCountByFilterGroupParameter()'></a>

## BuildingDataCountByFilterGroupParameter\(\) Constructor

Initializes a new instance of the [BuildingDataCountByFilterGroupParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataCountByFilterGroupParameter') class\.

```csharp
public BuildingDataCountByFilterGroupParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter.BuildingDataCountByFilterGroupParameter(System.Text.Json.Nodes.JsonObject)'></a>

## BuildingDataCountByFilterGroupParameter\(JsonObject\) Constructor

Initializes a new instance of the [BuildingDataCountByFilterGroupParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter 'DiGi\.GIS\.WebAPI\.Classes\.BuildingDataCountByFilterGroupParameter') class using an [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public BuildingDataCountByFilterGroupParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter.BuildingDataCountByFilterGroupParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter.CountyId'></a>

## BuildingDataCountByFilterGroupParameter\.CountyId Property

Gets or sets the optional partition \(county\) to count; `null` counts across all partitions\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

### Example
10365

<a name='DiGi.GIS.WebAPI.Classes.BuildingDataCountByFilterGroupParameter.FilterGroup'></a>

## BuildingDataCountByFilterGroupParameter\.FilterGroup Property

Gets or sets the optional dynamic hierarchical filters to apply prior to counting; `null` \(or an empty group\) counts the whole partition scope\.

```csharp
public DiGi.PostgreSQL.Table.Classes.FilterGroup? FilterGroup { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController'></a>

## BuildingModelController Class

Web API controller for building model operations, providing endpoints to retrieve building model data\.

```csharp
public class BuildingModelController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → BuildingModelController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.BuildingModelController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## BuildingModelController\(GISWebAPIConfigurationFileWatcher, BuildingModelPostgreSQLConverter, Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the [BuildingModelController](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingModelController 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelController') class\.

```csharp
public BuildingModelController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter buildingModelPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.BuildingModelController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher for the GIS PostgreSQL Web API\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.BuildingModelController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).buildingModelPostgreSQLConverter'></a>

`buildingModelPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingModelPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingmodelpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingModelPostgreSQLConverter')

The converter used for building model data operations in PostgreSQL\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.BuildingModelController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The converter used for Building 2D data operations in PostgreSQL\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.BuildingModelController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingModelPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter used to resolve an administrative area code to its county identifier\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetCountByCountyIdAsync(int,int,System.Threading.CancellationToken)'></a>

## BuildingModelController\.GetCountByCountyIdAsync\(int, int, CancellationToken\) Method

Asynchronously retrieves the number of building model rows stored for one county part\.

Compared against `gis/Building2D/referencesbycountyid` it tells whether a county's regeneration covered every building; the count is exact, so it walks the partition.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountByCountyIdAsync(int countyId, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetCountByCountyIdAsync(int,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county part \(the `building_2d.county_id` value, one per polygon part\)\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetCountByCountyIdAsync(int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetCountByCountyIdAsync(int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the row count, or 404 when the county has no building model partition\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken)'></a>

## BuildingModelController\.GetItemsByCircleAsync\(double, double, Nullable\<double\>, Nullable\<double\>, Nullable\<double\>, CancellationToken\) Method

Retrieves the building models stored in the database for all buildings within a specified circle\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByCircleAsync(double x, double y, System.Nullable<double> radius, System.Nullable<double> diameter, System.Nullable<double> tolerance=1E-06, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).x'></a>

`x` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X\-coordinate of the center point of the search circle\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).y'></a>

`y` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y\-coordinate of the center point of the search circle\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).radius'></a>

`radius` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The radius of the search circle\. Either radius or diameter must be supplied; if both are supplied, radius takes precedence\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).diameter'></a>

`diameter` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The diameter of the search circle\. Either radius or diameter must be supplied; if both are supplied, radius takes precedence\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional tolerance value for the spatial query\. If not provided, the default distance tolerance is used\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Nullable_long_,System.Threading.CancellationToken)'></a>

## BuildingModelController\.GetItemsByReferencesAsync\(IEnumerable\<string\>, Nullable\<int\>, Nullable\<long\>, CancellationToken\) Method

Retrieves building models stored in the database for the specified references\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable<string>? references, System.Nullable<int> countyId, System.Nullable<long> limit=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Nullable_long_,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The building references identifying the building models to retrieve\. This value can be null\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Nullable_long_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional county identifier used to narrow the search\. This value can be null\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Nullable_long_,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional maximum number of building models to retrieve\. This value can be null\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,System.Nullable_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetLatestCreatedAtByCountyIdAsync(int,int,System.Threading.CancellationToken)'></a>

## BuildingModelController\.GetLatestCreatedAtByCountyIdAsync\(int, int, CancellationToken\) Method

Asynchronously retrieves the most recent creation stamp among the building model rows of one county part\.

The model JSON carries no timestamp, so this is the only way to tell through the API whether a county holds the rows of the latest regeneration run or still those of an earlier one.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetLatestCreatedAtByCountyIdAsync(int countyId, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetLatestCreatedAtByCountyIdAsync(int,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county part \(the `building_2d.county_id` value, one per polygon part\)\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetLatestCreatedAtByCountyIdAsync(int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.GetLatestCreatedAtByCountyIdAsync(int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the stamp as an ISO 8601 UTC string, or 404 when the county has no building model partition or it holds no rows\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateAsync(System.Collections.Generic.List_DiGi.Analytical.Building.Classes.BuildingModel_,int)'></a>

## BuildingModelController\.UpdateAsync\(List\<BuildingModel\>, int\) Method

Writes the given building models to the partition of a single county row, replacing whatever those buildings already held there\.

Shared by both update actions so the county row is resolved once, by the action, and this method never has to guess one.

<b>A post replaces rather than adds.</b> A model row is addressed by the identifier of the model it holds, and a model is handed a fresh one whenever it is created, so the write itself always appends - see [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReferencedObject&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreferencedobject-1 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReferencedObject\`1'). Left at that, regenerating a county would add a model to every building instead of replacing its own, so what the buildings already held is read first and removed once the write has succeeded. A building therefore ends up holding exactly the models this call sent for it.

The identifiers are read before the write and deleted after it, deliberately in that order: an interrupted call then leaves the building holding both its old and its new model, which is recoverable, rather than holding neither.

```csharp
private System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateAsync(System.Collections.Generic.List<DiGi.Analytical.Building.Classes.BuildingModel> buildingModels, int countyId);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateAsync(System.Collections.Generic.List_DiGi.Analytical.Building.Classes.BuildingModel_,int).buildingModels'></a>

`buildingModels` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

The building models to write\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateAsync(System.Collections.Generic.List_DiGi.Analytical.Building.Classes.BuildingModel_,int).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the models belong to\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken)'></a>

## BuildingModelController\.UpdateItemsAsync\(JsonArray, string, string, CancellationToken\) Method

Updates multiple building model items in the database, keyed by administrative area code\.

A county code does not identify a single county row: BDOT10k stores a county whose territory is disconnected as one feature per polygon part, and every part becomes its own row. Every part the code names is passed on, and each model is filed under the part it actually belongs to - see [UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelController\.UpdateItemsByCountyIdsAsync\(System\.Text\.Json\.Nodes\.JsonArray, int\[\], string, System\.Threading\.CancellationToken\)') for how that is decided.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string? code, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the building models to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The administrative area code the building models belong to, resolved server\-side to a county identifier\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken)'></a>

## BuildingModelController\.UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\) Method

Updates multiple building model items in the database for the given county rows\.

The unambiguous counterpart of [UpdateItemsAsync\(JsonArray, string, string, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelController\.UpdateItemsAsync\(System\.Text\.Json\.Nodes\.JsonArray, string, string, System\.Threading\.CancellationToken\)'): it takes county identifiers rather than a code, so the caller states which rows are in play instead of leaving the server to derive them.

The identifiers are the parts of one county in play, and each model is filed under the part already holding the `building_2d` row its reference names, probed lowest part first - whether one identifier arrived or several, since naming one part is not evidence the county has one. That row was filed by geometry when it was imported, so reusing its answer keeps both tables keyed by the same `(county_id, reference)` pair.

A model no named part holds is widened to every part of the county its parts name, and only a model no part of the county holds is left unwritten - nothing states where it belongs, and storing it under a guessed part is the state this replaced.

Nothing filed is answered as a failure, not a quiet no-op: the county-part lookup not running answers a distinct 500 naming it, a transient database failure answers 503 with `Retry-After`, and a non-empty batch in which no model could be filed under any county answers 500. This is the endpoint behind a regeneration that once posted 33 687 models into an unreachable database and reported success.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, int[]? countyIds, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the building models to be updated\. This value can be null\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The identifiers of the county rows the building models belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask'></a>

## BuildingModelsPostTask Class

Provides functionality to handle the asynchronous posting of [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') collections to the PostgreSQL database\.

```csharp
public class BuildingModelsPostTask : DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask<DiGi.Analytical.Building.Classes.BuildingModel>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → BuildingModelsPostTask
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.BuildingModelsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## BuildingModelsPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [BuildingModelsPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask') class\.

```csharp
public BuildingModelsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.BuildingModelsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the server\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.Code'></a>

## BuildingModelsPostTask\.Code Property

Gets or sets the administrative area code the building models belong to\. It is resolved server\-side to a county identifier\.

A code does not identify a single county row - a multi-part county holds one row per polygon part - so set [CountyIds](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.CountyIds 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask\.CountyIds') instead wherever the identifiers are already known. [CountyIds](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.CountyIds 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask\.CountyIds') takes precedence when both are set.

```csharp
public string? Code { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.CountyIds'></a>

## BuildingModelsPostTask\.CountyIds Property

Gets or sets the identifiers of the county rows the building models belong to \- normally every polygon part of one county, since a single id is not evidence the code has one part\. When set it is used in preference to [Code](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.Code 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask\.Code'), which lets the server resolve the code to every part\.

```csharp
public System.Collections.Generic.HashSet<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.HashSet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken)'></a>

## BuildingModelsPostTask\.ExecuteAsync\(IEnumerable\<BuildingModel\>, string, LongProgressWrapper, CancellationToken\) Method

Asynchronously executes the task of posting building models to the database in memory\-size\-split batches\.

```csharp
protected System.Threading.Tasks.Task<bool> ExecuteAsync(System.Collections.Generic.IEnumerable<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels, string? code, DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).buildingModels'></a>

`buildingModels` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') instances to post\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The administrative area code the building models belong to\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).longProgressWrapper'></a>

`longProgressWrapper` [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper')

A [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper') tracking the progress of the operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if all batches were posted successfully; otherwise, false\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken)'></a>

## BuildingModelsPostTask\.ExecuteAsync\(IEnumerable\<BuildingModel\>, IEnumerable\<int\>, LongProgressWrapper, CancellationToken\) Method

Asynchronously executes the task of posting building models to the database in memory\-size\-split batches, keyed by county identifier\.

```csharp
protected System.Threading.Tasks.Task<bool> ExecuteAsync(System.Collections.Generic.IEnumerable<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).buildingModels'></a>

`buildingModels` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') instances to post\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the building models belong to \- normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).longProgressWrapper'></a>

`longProgressWrapper` [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper')

A [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper') tracking the progress of the operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if all batches were posted successfully; otherwise, false\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController'></a>

## BuildingSolarRadiationController Class

Serves the stored solar radiation of buildings \(`building_solar_radiation`\): a read open to every visitor, and a write that only a signed\-in user can make\.

A stored result is keyed by county part, building model id, neighbour radius and solver version. Its weather members describe the stored summary, not the key, so a client compares them with its own weather source and rebuilds the summary from the stored shading factors when they differ.

```csharp
public class BuildingSolarRadiationController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → BuildingSolarRadiationController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.BuildingSolarRadiationController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingSolarRadiationPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore)'></a>

## BuildingSolarRadiationController\(GISWebAPIConfigurationFileWatcher, BuildingSolarRadiationPostgreSQLConverter, Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter, SecurityKeyManager, TokenRevocationStore\) Constructor

Initializes a new instance of the [BuildingSolarRadiationController](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController 'DiGi\.GIS\.WebAPI\.Classes\.BuildingSolarRadiationController') class\. This is the controller's only public constructor: MVC activates it through `ActivatorUtilities`, and a second one would answer every action with 500 \(`Coding - WebAPI Contracts.md` §5\)\.

```csharp
public BuildingSolarRadiationController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.BuildingSolarRadiationPostgreSQLConverter buildingSolarRadiationPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter, DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager=null, DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.BuildingSolarRadiationController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingSolarRadiationPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration watcher holding [AllowUpdateSolarRadiationData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateSolarRadiationData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateSolarRadiationData')\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.BuildingSolarRadiationController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingSolarRadiationPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).buildingSolarRadiationPostgreSQLConverter'></a>

`buildingSolarRadiationPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingSolarRadiationPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingsolarradiationpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingSolarRadiationPostgreSQLConverter')

The converter of the `building_solar_radiation` table \(Storage database\)\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.BuildingSolarRadiationController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingSolarRadiationPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The converter of the `building_2d` table \(Main database\), which decides the county part a write is filed under\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.BuildingSolarRadiationController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingSolarRadiationPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter of the `administrative_areal_2d` table \(Main database\), which widens that decision to the sibling parts of the county\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.BuildingSolarRadiationController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingSolarRadiationPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).securityKeyManager'></a>

`securityKeyManager` [DiGi\.WebAPI\.Classes\.SecurityKeyManager](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.securitykeymanager 'DiGi\.WebAPI\.Classes\.SecurityKeyManager')

The user extension's security key manager; `null` when the user extension is not loaded, in which case every write is refused with 401\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.BuildingSolarRadiationController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.BuildingSolarRadiationPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).tokenRevocationStore'></a>

`tokenRevocationStore` [DiGi\.WebAPI\.Classes\.TokenRevocationStore](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.tokenrevocationstore 'DiGi\.WebAPI\.Classes\.TokenRevocationStore')

The user extension's token revocation store; `null` when the user extension is not loaded, in which case every write is refused with 401\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync(System.Nullable_long_,System.Nullable_int_,System.Nullable_double_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken)'></a>

## BuildingSolarRadiationController\.GetSolarRadiationByBuildingModelIdAsync\(Nullable\<long\>, Nullable\<int\>, Nullable\<double\>, Nullable\<int\>, Nullable\<bool\>, CancellationToken\) Method

Reads the stored solar radiation of a building model\.

Open to every visitor, like the building model read the panel already uses. Every parameter is bound as nullable, and a missing one answers 400 rather than meaning `default` (`Coding - WebAPI Contracts.md` §2). The row is looked for under [countyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync(System.Nullable_long_,System.Nullable_int_,System.Nullable_double_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).countyId 'DiGi\.GIS\.WebAPI\.Classes\.BuildingSolarRadiationController\.GetSolarRadiationByBuildingModelIdAsync\(System\.Nullable\<long\>, System\.Nullable\<int\>, System\.Nullable\<double\>, System\.Nullable\<int\>, System\.Nullable\<bool\>, System\.Threading\.CancellationToken\)\.countyId') and then under every county part, because a building of a multi-part county may be filed under a sibling part.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSolarRadiationByBuildingModelIdAsync(System.Nullable<long> id, System.Nullable<int> countyId, System.Nullable<double> radius, System.Nullable<int> solverVersion, System.Nullable<bool> includeShadingFactors, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync(System.Nullable_long_,System.Nullable_int_,System.Nullable_double_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).id'></a>

`id` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The id of the building model\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync(System.Nullable_long_,System.Nullable_int_,System.Nullable_double_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The county part the building is filed under\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync(System.Nullable_long_,System.Nullable_int_,System.Nullable_double_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).radius'></a>

`radius` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The neighbour radius, in m, exactly as stored \(the producer rounds it to 0\.1 m\); it is not rounded here\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync(System.Nullable_long_,System.Nullable_int_,System.Nullable_double_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).solverVersion'></a>

`solverVersion` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The solver version\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync(System.Nullable_long_,System.Nullable_int_,System.Nullable_double_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).includeShadingFactors'></a>

`includeShadingFactors` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

True to include every surface's shading factors \(8 760 bytes each, base64\); omitted or false leaves them null, which is all a client needs when the stored summary matches its weather source\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.GetSolarRadiationByBuildingModelIdAsync(System.Nullable_long_,System.Nullable_int_,System.Nullable_double_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the [DiGi\.GIS\.Classes\.BuildingSolarRadiationResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.buildingsolarradiationresult 'DiGi\.GIS\.Classes\.BuildingSolarRadiationResult'), 204 when nothing is stored for the key, 400 for a missing or invalid parameter, 503 for a transient database failure, or 500\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.SetSolarRadiationAsync(System.Text.Json.Nodes.JsonObject,System.Threading.CancellationToken)'></a>

## BuildingSolarRadiationController\.SetSolarRadiationAsync\(JsonObject, CancellationToken\) Method

Stores the solar radiation of a building on behalf of the signed\-in visitor, replacing the result stored for the same key\.

The bearer token identifies the user, as for `yearbuiltdata/setuseryearbuilt`, and [AllowUpdateSolarRadiationData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateSolarRadiationData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateSolarRadiationData') gates the write; each refusal is logged distinctly so the cause of a 4xx is recoverable where the status is shared. The server stamps [DiGi\.GIS\.Classes\.BuildingSolarRadiationResult\.UserEmail](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.buildingsolarradiationresult.useremail 'DiGi\.GIS\.Classes\.BuildingSolarRadiationResult\.UserEmail') from the token, ignoring any value in the body, and sets [DiGi\.GIS\.Classes\.BuildingSolarRadiationResult\.CreatedAt](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.buildingsolarradiationresult.createdat 'DiGi\.GIS\.Classes\.BuildingSolarRadiationResult\.CreatedAt') to now.

The body is the DiGi JSON of a [DiGi\.GIS\.Classes\.BuildingSolarRadiationResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.buildingsolarradiationresult 'DiGi\.GIS\.Classes\.BuildingSolarRadiationResult') (`ToJsonObject()`, with `_type`), bound as a [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') so the host's System.Text.Json formatter never sees the DiGi layout. The county part is resolved from the body's reference through `building_2d`, widened to the sibling parts of the body's `CountyId`; the body's part is only the candidate (`Coding - GIS Administrative Data.md`).

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SetSolarRadiationAsync(System.Text.Json.Nodes.JsonObject? jsonObject, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.SetSolarRadiationAsync(System.Text.Json.Nodes.JsonObject,System.Threading.CancellationToken).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The result to store\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingSolarRadiationController.SetSolarRadiationAsync(System.Text.Json.Nodes.JsonObject,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the id of the stored row; 401 without a valid user token; 400 for a disabled flag or an invalid body; 404 when no part of the county holds the reference; 503 for a transient database failure; or 500 when the write failed\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask'></a>

## BuildingsPostTask Class

Provides functionality to handle the asynchronous posting of [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') collections to the PostgreSQL database\.

```csharp
public class BuildingsPostTask : DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask<DiGi.CityGML.Classes.Building>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → BuildingsPostTask
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.BuildingsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## BuildingsPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [BuildingsPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingsPostTask 'DiGi\.GIS\.WebAPI\.Classes\.BuildingsPostTask') class\.

```csharp
public BuildingsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.BuildingsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The GIS PostgreSQL Web API manager used to handle data persistence\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.Code'></a>

## BuildingsPostTask\.Code Property

Gets or sets the code associated with the buildings post task\.

A code does not identify a single county row - a multi-part county holds one row per polygon part - so set [CountyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingsPostTask.CountyId 'DiGi\.GIS\.WebAPI\.Classes\.BuildingsPostTask\.CountyId') instead wherever the identifier is already known. [CountyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingsPostTask.CountyId 'DiGi\.GIS\.WebAPI\.Classes\.BuildingsPostTask\.CountyId') takes precedence when both are set.

```csharp
public string? Code { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.CountyId'></a>

## BuildingsPostTask\.CountyId Property

Gets or sets the identifier of the county row the buildings belong to\. When set it is used in preference to [Code](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingsPostTask.Code 'DiGi\.GIS\.WebAPI\.Classes\.BuildingsPostTask\.Code'), which leaves the server to choose between the rows of a multi\-part county\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken)'></a>

## BuildingsPostTask\.ExecuteAsync\(IEnumerable\<Building\>, int, LongProgressWrapper, CancellationToken\) Method

Asynchronously executes the task of posting building objects to the database, keyed by county identifier\.

```csharp
protected System.Threading.Tasks.Task<bool> ExecuteAsync(System.Collections.Generic.IEnumerable<DiGi.CityGML.Classes.Building>? values, int countyId, DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') instances to post\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the buildings belong to\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).longProgressWrapper'></a>

`longProgressWrapper` [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper')

A [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper') tracking the progress of the operation\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The task result is true if all batches were posted successfully; otherwise, false\.

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken)'></a>

## BuildingsPostTask\.ExecuteAsync\(IEnumerable\<Building\>, string, LongProgressWrapper, CancellationToken\) Method

Asynchronously executes the task of posting building objects to the database\.

```csharp
protected System.Threading.Tasks.Task<bool> ExecuteAsync(System.Collections.Generic.IEnumerable<DiGi.CityGML.Classes.Building>? values, string? code, DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).longProgressWrapper'></a>

`longProgressWrapper` [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper')

<a name='DiGi.GIS.WebAPI.Classes.BuildingsPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')

<a name='DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter'></a>

## ColumnsByCategoriesParameter Class

Represents a parameter containing categories for querying columns\.

```csharp
public class ColumnsByCategoriesParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → ColumnsByCategoriesParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter.ColumnsByCategoriesParameter()'></a>

## ColumnsByCategoriesParameter\(\) Constructor

Initializes a new instance of the [ColumnsByCategoriesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter 'DiGi\.GIS\.WebAPI\.Classes\.ColumnsByCategoriesParameter') class\.

```csharp
public ColumnsByCategoriesParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter.ColumnsByCategoriesParameter(System.Collections.Generic.IEnumerable_string_)'></a>

## ColumnsByCategoriesParameter\(IEnumerable\<string\>\) Constructor

Initializes a new instance of the [ColumnsByCategoriesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter 'DiGi\.GIS\.WebAPI\.Classes\.ColumnsByCategoriesParameter') class with the specified categories\.

```csharp
public ColumnsByCategoriesParameter(System.Collections.Generic.IEnumerable<string> categories);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter.ColumnsByCategoriesParameter(System.Collections.Generic.IEnumerable_string_).categories'></a>

`categories` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of categories for querying columns\.

<a name='DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter.ColumnsByCategoriesParameter(System.Text.Json.Nodes.JsonObject)'></a>

## ColumnsByCategoriesParameter\(JsonObject\) Constructor

Initializes a new instance of the [ColumnsByCategoriesParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter 'DiGi\.GIS\.WebAPI\.Classes\.ColumnsByCategoriesParameter') class using an [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public ColumnsByCategoriesParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter.ColumnsByCategoriesParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.ColumnsByCategoriesParameter.Categories'></a>

## ColumnsByCategoriesParameter\.Categories Property

Gets or sets the categories for querying columns\. All columns will be returned if the collection is null or empty\.

```csharp
public System.Collections.Generic.IEnumerable<string> Categories { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter'></a>

## CountByAdministrativeAreal2DIdsParameter Class

Represents a parameter object used to perform counting operations based on a collection of administrative areal 2D identifiers\.

```csharp
public class CountByAdministrativeAreal2DIdsParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → CountByAdministrativeAreal2DIdsParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter.CountByAdministrativeAreal2DIdsParameter()'></a>

## CountByAdministrativeAreal2DIdsParameter\(\) Constructor

Initializes a new instance of the [CountByAdministrativeAreal2DIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.CountByAdministrativeAreal2DIdsParameter') class\.

```csharp
public CountByAdministrativeAreal2DIdsParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter.CountByAdministrativeAreal2DIdsParameter(DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter)'></a>

## CountByAdministrativeAreal2DIdsParameter\(CountByAdministrativeAreal2DIdsParameter\) Constructor

Initializes a new instance of the [CountByAdministrativeAreal2DIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.CountByAdministrativeAreal2DIdsParameter') class by copying the values from an existing instance\.

```csharp
public CountByAdministrativeAreal2DIdsParameter(DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter countByAdministrativeAreal2DIdsParameter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter.CountByAdministrativeAreal2DIdsParameter(DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter).countByAdministrativeAreal2DIdsParameter'></a>

`countByAdministrativeAreal2DIdsParameter` [CountByAdministrativeAreal2DIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.CountByAdministrativeAreal2DIdsParameter')

The source instance from which to copy the administrative areal 2D identifiers\.

<a name='DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter.CountByAdministrativeAreal2DIdsParameter(System.Text.Json.Nodes.JsonObject)'></a>

## CountByAdministrativeAreal2DIdsParameter\(JsonObject\) Constructor

Initializes a new instance of the [CountByAdministrativeAreal2DIdsParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter 'DiGi\.GIS\.WebAPI\.Classes\.CountByAdministrativeAreal2DIdsParameter') class using the provided JSON object\.

```csharp
public CountByAdministrativeAreal2DIdsParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter.CountByAdministrativeAreal2DIdsParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.CountByAdministrativeAreal2DIdsParameter.AdministrativeAreal2DIds'></a>

## CountByAdministrativeAreal2DIdsParameter\.AdministrativeAreal2DIds Property

Gets or sets the collection of administrative areal 2D identifiers used for counting operations\.

```csharp
public System.Collections.Generic.IEnumerable<int> AdministrativeAreal2DIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController'></a>

## EPWFileController Class

Controller responsible for handling API requests related to EPW files retrieved from or updated in a PostgreSQL database\.

```csharp
public class EPWFileController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → EPWFileController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.EPWFileController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.EPWFilePostgreSQLConverter)'></a>

## EPWFileController\(GISWebAPIConfigurationFileWatcher, EPWFilePostgreSQLConverter\) Constructor

Initializes a new instance of the [EPWFileController](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.EPWFileController 'DiGi\.GIS\.WebAPI\.Classes\.EPWFileController') class\.

```csharp
public EPWFileController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.EPWFilePostgreSQLConverter ePWFilePostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.EPWFileController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.EPWFilePostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher used to monitor changes to the GIS PostgreSQL Web API settings\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.EPWFileController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.EPWFilePostgreSQLConverter).ePWFilePostgreSQLConverter'></a>

`ePWFilePostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.EPWFilePostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.epwfilepostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.EPWFilePostgreSQLConverter')

The converter used for handling EPW file operations within the PostgreSQL database\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.GetEPWFileAsync(double,double,System.Threading.CancellationToken)'></a>

## EPWFileController\.GetEPWFileAsync\(double, double, CancellationToken\) Method

Asynchronously retrieves the closest EPWFile to the given coordinate \(x, y\)\.

The response header `DiGi-EPWFile-Id` ([EPWFileId](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Header.EPWFileId 'DiGi\.GIS\.WebAPI\.Constants\.Header\.EPWFileId')) carries the id of the `epw_file` row the body holds, as an invariant integer, so a client can name the weather source of a stored solar radiation summary. The body is unchanged.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetEPWFileAsync(double x, double y, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.GetEPWFileAsync(double,double,System.Threading.CancellationToken).x'></a>

`x` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate \(longitude\)\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.GetEPWFileAsync(double,double,System.Threading.CancellationToken).y'></a>

`y` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate \(latitude\)\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.GetEPWFileAsync(double,double,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken)'></a>

## EPWFileController\.UpdateItemsAsync\(JsonArray, string, CancellationToken\) Method

Asynchronously updates or inserts a collection of EPW files\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the EPW files to update or insert\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFileController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask'></a>

## EPWFilesPostTask Class

Provides functionality to handle the asynchronous posting of multiple [DiGi\.EPW\.Classes\.EPWFile](https://learn.microsoft.com/en-us/dotnet/api/digi.epw.classes.epwfile 'DiGi\.EPW\.Classes\.EPWFile') objects to the GIS PostgreSQL database\.

```csharp
public class EPWFilesPostTask : DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask<DiGi.EPW.Classes.EPWFile>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.EPW\.Classes\.EPWFile](https://learn.microsoft.com/en-us/dotnet/api/digi.epw.classes.epwfile 'DiGi\.EPW\.Classes\.EPWFile')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → EPWFilesPostTask
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask.EPWFilesPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## EPWFilesPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the [EPWFilesPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.EPWFilesPostTask 'DiGi\.GIS\.WebAPI\.Classes\.EPWFilesPostTask') class\.

```csharp
public EPWFilesPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask.EPWFilesPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') used to manage PostgreSQL GIS operations\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.EPW.Classes.EPWFile_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken)'></a>

## EPWFilesPostTask\.ExecuteAsync\(IEnumerable\<EPWFile\>, LongProgressWrapper, CancellationToken\) Method

Asynchronously executes the posting of EPW files\.

```csharp
protected System.Threading.Tasks.Task<bool> ExecuteAsync(System.Collections.Generic.IEnumerable<DiGi.EPW.Classes.EPWFile>? ePWFiles, DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.EPW.Classes.EPWFile_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).ePWFiles'></a>

`ePWFiles` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.EPW\.Classes\.EPWFile](https://learn.microsoft.com/en-us/dotnet/api/digi.epw.classes.epwfile 'DiGi\.EPW\.Classes\.EPWFile')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of EPW files to post\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.EPW.Classes.EPWFile_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).longProgressWrapper'></a>

`longProgressWrapper` [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper')

The progress wrapper to track the number of posted files\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.EPW.Classes.EPWFile_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation, returning [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') if the post succeeded; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken)'></a>

## EPWFilesPostTask\.ExecuteAsync\(IProgress\<long\>, CancellationToken\) Method

Asynchronously executes the background task, reporting progress\.

```csharp
protected override System.Threading.Tasks.Task<bool> ExecuteAsync(System.IProgress<long> progress, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The progress reporter for reporting progress updates\.

<a name='DiGi.GIS.WebAPI.Classes.EPWFilesPostTask.ExecuteAsync(System.IProgress_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to cancel the operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation, returning [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') if the operation succeeded; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher'></a>

## GISWebAPIConfigurationFileWatcher Class

Provides functionality to watch and retrieve configuration settings for the GIS PostgreSQL Web API from a specified configuration file\.

```csharp
public class GISWebAPIConfigurationFileWatcher : DiGi.Core.IO.FileWatcher.Classes.ConfigurationFileWatcher
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.IO\.FileWatcher\.Classes\.FileWatcher](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.filewatcher.classes.filewatcher 'DiGi\.Core\.IO\.FileWatcher\.Classes\.FileWatcher') → [DiGi\.Core\.IO\.FileWatcher\.Classes\.ConfigurationFileWatcher](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.filewatcher.classes.configurationfilewatcher 'DiGi\.Core\.IO\.FileWatcher\.Classes\.ConfigurationFileWatcher') → GISWebAPIConfigurationFileWatcher
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.GISWebAPIConfigurationFileWatcher(string,double)'></a>

## GISWebAPIConfigurationFileWatcher\(string, double\) Constructor

Initializes a new instance of the GISWebAPIConfigurationFileWatcher class\.

```csharp
public GISWebAPIConfigurationFileWatcher(string path, double interval=5000.0);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.GISWebAPIConfigurationFileWatcher(string,double).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The path to the configuration file to be watched\.

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.GISWebAPIConfigurationFileWatcher(string,double).interval'></a>

`interval` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The time interval in milliseconds between checks for changes to the configuration file\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData'></a>

## GISWebAPIConfigurationFileWatcher\.AllowDeleteYearBuiltData Property

Gets a value indicating whether stored year built data may be deleted \- whole objects, one prediction run, or user entries withdrawn by moderation\.

Kept apart from [AllowUpdateYearBuiltData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateYearBuiltData') because a delete has no undo: a host can accept new predictions and user entries while refusing removals. A missing value denies.

```csharp
public bool AllowDeleteYearBuiltData { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateAddressPoint'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateAddressPoint Property

Gets a value indicating whether updates to PRG address points are permitted according to the configuration file\.

```csharp
public bool AllowUpdateAddressPoint { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateAdministrativeAreal2D'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateAdministrativeAreal2D Property

Gets a value indicating whether updates to administrative areal 2D data are permitted according to the configuration file\.

```csharp
public bool AllowUpdateAdministrativeAreal2D { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateBuilding'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateBuilding Property

Gets a value indicating whether updates to buildings are permitted based on the configuration file settings\.

```csharp
public bool AllowUpdateBuilding { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateBuilding2D'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateBuilding2D Property

Gets a value indicating whether updates to 2D buildings are permitted based on the configuration file settings\.

```csharp
public bool AllowUpdateBuilding2D { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateBuildingData Property

Gets a value indicating whether updates to building data are permitted based on the configuration file settings\.

```csharp
public bool AllowUpdateBuildingData { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingModel'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateBuildingModel Property

Gets a value indicating whether updates to building models are permitted based on the configuration file settings\.

```csharp
public bool AllowUpdateBuildingModel { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateEPWFile'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateEPWFile Property

Gets a value indicating whether updating EPW file data is enabled in the configuration\.

```csharp
public bool AllowUpdateEPWFile { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateOccupancyData'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateOccupancyData Property

Gets a value indicating whether updates to occupancy data are permitted according to the configuration file\.

```csharp
public bool AllowUpdateOccupancyData { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateOrtoDatas'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateOrtoDatas Property

Gets a value indicating whether updates to orthophoto data are permitted according to the configuration file\.

```csharp
public bool AllowUpdateOrtoDatas { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateSolarRadiationData'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateSolarRadiationData Property

Gets a value indicating whether signed\-in users may store the solar radiation of a building \(`gis/buildingsolarradiation/setsolarradiation`\)\.

The write is authorized by the user's bearer token, not by the access key, so this flag is the host's only switch for it. A missing value denies.

```csharp
public bool AllowUpdateSolarRadiationData { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateUnit'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateUnit Property

Gets a value indicating whether updates to territorial unit data are permitted according to the configuration file\.

```csharp
public bool AllowUpdateUnit { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData'></a>

## GISWebAPIConfigurationFileWatcher\.AllowUpdateYearBuiltData Property

Gets a value indicating whether updating year built data is enabled in the configuration\.

```csharp
public bool AllowUpdateYearBuiltData { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.Enabled'></a>

## GISWebAPIConfigurationFileWatcher\.Enabled Property

Gets a value indicating whether write authorization enforcement is enabled\.

False denies every write request; it does not open them.

```csharp
public bool Enabled { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.Key'></a>

## GISWebAPIConfigurationFileWatcher\.Key Property

Gets the secret access token for authorizing write operations, or the value of the DIGI\_GIS\_WEBAPI\_KEY environment variable if not configured in the file\.

```csharp
public string? Key { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.Open'></a>

## GISWebAPIConfigurationFileWatcher\.Open Property

Gets a value indicating whether the key check is explicitly waived, leaving write endpoints reachable without an access key\.

Intended for local development only. This is the sole setting that grants unauthenticated access to write endpoints.

```csharp
public bool Open { get; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager'></a>

## GISWebAPIManager Class

Manages the creation and lifecycle of [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') instances used to interact with the GIS PostgreSQL Web API\.

```csharp
public class GISWebAPIManager
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → GISWebAPIManager
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.GISWebAPIManager(System.Net.Http.IHttpClientFactory,string)'></a>

## GISWebAPIManager\(IHttpClientFactory, string\) Constructor

Initializes a new instance of the [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') class\.

```csharp
public GISWebAPIManager(System.Net.Http.IHttpClientFactory? httpClientFactory, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.GISWebAPIManager(System.Net.Http.IHttpClientFactory,string).httpClientFactory'></a>

`httpClientFactory` [System\.Net\.Http\.IHttpClientFactory](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.ihttpclientfactory 'System\.Net\.Http\.IHttpClientFactory')

The HTTP client factory used to create and manage [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') instances\.

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.GISWebAPIManager(System.Net.Http.IHttpClientFactory,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.Key'></a>

## GISWebAPIManager\.Key Property

Gets or sets the API authorization key used for authenticating requests to protected endpoints\.

```csharp
public string? Key { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.CreateHttpClient(string)'></a>

## GISWebAPIManager\.CreateHttpClient\(string\) Method

Creates an HttpClient instance with the specified name\.

```csharp
public System.Net.Http.HttpClient? CreateHttpClient(string name);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.CreateHttpClient(string).name'></a>

`name` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique identifier or name of the HTTP client to be created\.

#### Returns
[System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient')  
An [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') instance configured with the specified client name and authorization key header if configured\.

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.CreateHttpClient_TControllerBase_(string)'></a>

## GISWebAPIManager\.CreateHttpClient\<TControllerBase\>\(string\) Method

Creates an [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') instance configured for the Web API, resolving the route associated with the specified controller type\.

```csharp
public System.Net.Http.HttpClient? CreateHttpClient<TControllerBase>(out string? route)
    where TControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase;
```
#### Type parameters

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.CreateHttpClient_TControllerBase_(string).TControllerBase'></a>

`TControllerBase`

The TControllerBase type parameter\.
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.CreateHttpClient_TControllerBase_(string).route'></a>

`route` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The route\.

#### Returns
[System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.CreateHttpClient_TControllerBase_(string,string)'></a>

## GISWebAPIManager\.CreateHttpClient\<TControllerBase\>\(string, string\) Method

Creates an [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') instance configured for the specified controller's method and retrieves the corresponding API path\.

```csharp
public System.Net.Http.HttpClient? CreateHttpClient<TControllerBase>(string methodName, out string? path)
    where TControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase;
```
#### Type parameters

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.CreateHttpClient_TControllerBase_(string,string).TControllerBase'></a>

`TControllerBase`

The type of the base controller used to resolve the endpoint path\.
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.CreateHttpClient_TControllerBase_(string,string).methodName'></a>

`methodName` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the method within the controller to resolve\.

<a name='DiGi.GIS.WebAPI.Classes.GISWebAPIManager.CreateHttpClient_TControllerBase_(string,string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The path\.

#### Returns
[System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.HeatTransferCoefficientController'></a>

## HeatTransferCoefficientController Class

Controller responsible for handling API requests related to heat transfer coefficients, providing access to regulated heat transfer coefficient data\.

```csharp
public class HeatTransferCoefficientController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → HeatTransferCoefficientController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.HeatTransferCoefficientController.HeatTransferCoefficientController()'></a>

## HeatTransferCoefficientController\(\) Constructor

Initializes a new instance of the HeatTransferCoefficientController class\.

```csharp
public HeatTransferCoefficientController();
```
### Methods

<a name='DiGi.GIS.WebAPI.Classes.HeatTransferCoefficientController.GetRegulatedHeatTransferCoefficientsByYearAsync(short,System.Threading.CancellationToken)'></a>

## HeatTransferCoefficientController\.GetRegulatedHeatTransferCoefficientsByYearAsync\(short, CancellationToken\) Method

Retrieves the regulated heat transfer coefficients for a specified year\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetRegulatedHeatTransferCoefficientsByYearAsync(short year, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.HeatTransferCoefficientController.GetRegulatedHeatTransferCoefficientsByYearAsync(short,System.Threading.CancellationToken).year'></a>

`year` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

The year used to filter the regulated heat transfer coefficients\.

<a name='DiGi.GIS.WebAPI.Classes.HeatTransferCoefficientController.GetRegulatedHeatTransferCoefficientsByYearAsync(short,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') representing the result of the request, containing the retrieved coefficients or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.HistogramRequestParameter'></a>

## HistogramRequestParameter Class

Parameter class containing options for generating histograms\.

```csharp
public class HistogramRequestParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → HistogramRequestParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.HistogramRequestParameter.HistogramRequestParameter()'></a>

## HistogramRequestParameter\(\) Constructor

Initializes a new instance of the [HistogramRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.HistogramRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.HistogramRequestParameter') class\.

```csharp
public HistogramRequestParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.HistogramRequestParameter.HistogramRequestParameter(System.Text.Json.Nodes.JsonObject)'></a>

## HistogramRequestParameter\(JsonObject\) Constructor

Initializes a new instance of the [HistogramRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.HistogramRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.HistogramRequestParameter') class using an [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public HistogramRequestParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.HistogramRequestParameter.HistogramRequestParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.HistogramRequestParameter.BucketCount'></a>

## HistogramRequestParameter\.BucketCount Property

Gets or sets the total number of histogram buckets\. Defaults to 10\.

Capped because the bucket count is width_bucket's divisor rather than a page size: the server builds and returns one row per bucket whatever the table holds, so an unbounded value turns a cheap aggregate into an arbitrarily large response.

```csharp
public int BucketCount { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

### Example
20

<a name='DiGi.GIS.WebAPI.Classes.HistogramRequestParameter.ColumnUniqueId'></a>

## HistogramRequestParameter\.ColumnUniqueId Property

Gets or sets the column unique identifier to calculate value distributions for\.

```csharp
public string ColumnUniqueId { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
"column\_unique\_id\_123"

<a name='DiGi.GIS.WebAPI.Classes.HistogramRequestParameter.CountyId'></a>

## HistogramRequestParameter\.CountyId Property

Gets or sets the target partition identifier \(County ID\)\. If null, histogram is generated across all partitions\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

### Example
10365

<a name='DiGi.GIS.WebAPI.Classes.HistogramRequestParameter.FilterGroup'></a>

## HistogramRequestParameter\.FilterGroup Property

Gets or sets the optional dynamic hierarchical filters to apply prior to generating the histogram\.

```csharp
public DiGi.PostgreSQL.Table.Classes.FilterGroup? FilterGroup { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

<a name='DiGi.GIS.WebAPI.Classes.HistogramRequestParameter.HistogramBucketing'></a>

## HistogramRequestParameter\.HistogramBucketing Property

Gets or sets the bucketing rule\. Defaults to [DiGi\.PostgreSQL\.Table\.Enums\.HistogramBucketing\.EqualWidth](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.enums.histogrambucketing.equalwidth 'DiGi\.PostgreSQL\.Table\.Enums\.HistogramBucketing\.EqualWidth') \(`width_bucket` over the scope's \[min, max\], the maximum in the overflow bucket [BucketCount](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.HistogramRequestParameter.BucketCount 'DiGi\.GIS\.WebAPI\.Classes\.HistogramRequestParameter\.BucketCount') \+ 1\); [DiGi\.PostgreSQL\.Table\.Enums\.HistogramBucketing\.EqualCount](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.enums.histogrambucketing.equalcount 'DiGi\.PostgreSQL\.Table\.Enums\.HistogramBucketing\.EqualCount') \(`ntile`\) gives every bucket the same number of rows, so a skewed column keeps its resolution where its rows are \- what a consumer placing quantiles needs \(DiGi\-Organization/DiGi\.PostgreSQL\#7\)\.

Sent as the integer value; an omitted property keeps equal width, so clients written against the earlier contract are unaffected.

```csharp
public DiGi.PostgreSQL.Table.Enums.HistogramBucketing HistogramBucketing { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Enums\.HistogramBucketing](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.enums.histogrambucketing 'DiGi\.PostgreSQL\.Table\.Enums\.HistogramBucketing')

### Example
1

<a name='DiGi.GIS.WebAPI.Classes.MinimumAttribute'></a>

## MinimumAttribute Class

Declares the smallest value a numeric action parameter may carry, without the ceiling a [System\.ComponentModel\.DataAnnotations\.RangeAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.rangeattribute 'System\.ComponentModel\.DataAnnotations\.RangeAttribute') would force into the served document\.

Applied to a query parameter it is enforced by MVC model validation, so `[ApiController]` answers a violation with HTTP 400 before the action runs - the same rejection the action's own guard already produced, now at binding (DiGi-Organization/DiGi.GIS.WebAPI#47). A null value passes, so an optional parameter (an `int?` county filter) keeps its "omitted means no filter" meaning; a mandatory one is refused by `[BindRequired]`, not here.

A floor states the guard it mirrors exactly: 0 for `commandTimeout < 0`, 1 for `countyId <= 0`, so the attribute never rejects a value the action accepts nor accepts one it rejects. A non-numeric or [System\.Double\.NaN](https://learn.microsoft.com/en-us/dotnet/api/system.double.nan 'System\.Double\.NaN') value is refused, matching the actions' own finiteness guards.

[ParameterMinimumSchemaFilter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.ParameterMinimumSchemaFilter 'DiGi\.GIS\.WebAPI\.Classes\.ParameterMinimumSchemaFilter') carries the same floor into the parameter's OpenAPI schema as `minimum` - and as `exclusiveMinimum` when [Exclusive](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MinimumAttribute.Exclusive 'DiGi\.GIS\.WebAPI\.Classes\.MinimumAttribute\.Exclusive') is set - so the document states the domain without the absurd `maximum` a `[Range(min, double.MaxValue)]` would emit.

```csharp
public sealed class MinimumAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute 'System\.Attribute') → [System\.ComponentModel\.DataAnnotations\.ValidationAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationattribute 'System\.ComponentModel\.DataAnnotations\.ValidationAttribute') → MinimumAttribute
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.MinimumAttribute.MinimumAttribute(double)'></a>

## MinimumAttribute\(double\) Constructor

Initializes a new instance of the [MinimumAttribute](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MinimumAttribute 'DiGi\.GIS\.WebAPI\.Classes\.MinimumAttribute') class with an inclusive floor\.

```csharp
public MinimumAttribute(double minimum);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.MinimumAttribute.MinimumAttribute(double).minimum'></a>

`minimum` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The smallest value the parameter may carry\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.MinimumAttribute.Exclusive'></a>

## MinimumAttribute\.Exclusive Property

Whether the floor itself is excluded, for a domain that is strictly greater than the floor rather than at least it\.

```csharp
public bool Exclusive { get; set; }
```

#### Property Value
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='DiGi.GIS.WebAPI.Classes.MinimumAttribute.Minimum'></a>

## MinimumAttribute\.Minimum Property

The smallest value the parameter may carry, inclusive unless [Exclusive](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MinimumAttribute.Exclusive 'DiGi\.GIS\.WebAPI\.Classes\.MinimumAttribute\.Exclusive') is set\.

```csharp
public double Minimum { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.MinimumAttribute.IsValid(object,System.ComponentModel.DataAnnotations.ValidationContext)'></a>

## MinimumAttribute\.IsValid\(object, ValidationContext\) Method

Determines whether the bound value lies at or above the floor, or strictly above it when [Exclusive](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MinimumAttribute.Exclusive 'DiGi\.GIS\.WebAPI\.Classes\.MinimumAttribute\.Exclusive') is set\.

```csharp
protected override System.ComponentModel.DataAnnotations.ValidationResult? IsValid(object? value, System.ComponentModel.DataAnnotations.ValidationContext validationContext);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.MinimumAttribute.IsValid(object,System.ComponentModel.DataAnnotations.ValidationContext).value'></a>

`value` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

The bound value; null \(an omitted optional parameter\) is always valid\.

<a name='DiGi.GIS.WebAPI.Classes.MinimumAttribute.IsValid(object,System.ComponentModel.DataAnnotations.ValidationContext).validationContext'></a>

`validationContext` [System\.ComponentModel\.DataAnnotations\.ValidationContext](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationcontext 'System\.ComponentModel\.DataAnnotations\.ValidationContext')

The context describing the parameter being validated\.

#### Returns
[System\.ComponentModel\.DataAnnotations\.ValidationResult](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationresult 'System\.ComponentModel\.DataAnnotations\.ValidationResult')  
[System\.ComponentModel\.DataAnnotations\.ValidationResult\.Success](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.validationresult.success 'System\.ComponentModel\.DataAnnotations\.ValidationResult\.Success') when the value lies inside the domain; otherwise a result naming the floor\.

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter'></a>

## MultivalueAggregateRequestParameter Class

Parameter class containing options for multi\-value database aggregation queries\.

```csharp
public class MultivalueAggregateRequestParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → MultivalueAggregateRequestParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter.MultivalueAggregateRequestParameter()'></a>

## MultivalueAggregateRequestParameter\(\) Constructor

Initializes a new instance of the [MultivalueAggregateRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.MultivalueAggregateRequestParameter') class\.

```csharp
public MultivalueAggregateRequestParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter.MultivalueAggregateRequestParameter(System.Text.Json.Nodes.JsonObject)'></a>

## MultivalueAggregateRequestParameter\(JsonObject\) Constructor

Initializes a new instance of the [MultivalueAggregateRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.MultivalueAggregateRequestParameter') class using a [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public MultivalueAggregateRequestParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter.MultivalueAggregateRequestParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter.ColumnUniqueId'></a>

## MultivalueAggregateRequestParameter\.ColumnUniqueId Property

Gets or sets the column unique identifier to calculate statistics for\.

```csharp
public string ColumnUniqueId { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
"col\_unique\_id\_123"

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter.CountyId'></a>

## MultivalueAggregateRequestParameter\.CountyId Property

Gets or sets the target partition identifier \(County ID\)\. If null, aggregation is performed across all partitions\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

### Example
10365

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter.FilterGroup'></a>

## MultivalueAggregateRequestParameter\.FilterGroup Property

Gets or sets the optional dynamic hierarchical filters to apply prior to aggregation\.

```csharp
public DiGi.PostgreSQL.Table.Classes.FilterGroup? FilterGroup { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter.MaxGroups'></a>

## MultivalueAggregateRequestParameter\.MaxGroups Property

Gets or sets the maximum number of distinct\-value groups a [DiGi\.PostgreSQL\.Table\.Enums\.MultivalueAggregateFunction\.SplitValueDistribution](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.enums.multivalueaggregatefunction.splitvaluedistribution 'DiGi\.PostgreSQL\.Table\.Enums\.MultivalueAggregateFunction\.SplitValueDistribution') may return\. Defaults to 1000\.

For [DiGi\.PostgreSQL\.Table\.Enums\.MultivalueAggregateFunction\.SplitValueDistribution](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.enums.multivalueaggregatefunction.splitvaluedistribution 'DiGi\.PostgreSQL\.Table\.Enums\.MultivalueAggregateFunction\.SplitValueDistribution') only: the result carries at most this many groups plus one trailing truncation marker, so a result longer than this means the distribution was cut and the caller drops the last element. Ignored by [DiGi\.PostgreSQL\.Table\.Enums\.MultivalueAggregateFunction\.SplitDistinctCount](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.enums.multivalueaggregatefunction.splitdistinctcount 'DiGi\.PostgreSQL\.Table\.Enums\.MultivalueAggregateFunction\.SplitDistinctCount').

Capped because the group count is a `GROUP BY` divisor rather than a page size: the server builds and returns one row per distinct value whatever the table holds, so an unbounded value turns a cheap aggregate into an arbitrarily large response.

```csharp
public int MaxGroups { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

### Example
20

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter.MultivalueAggregateFunction'></a>

## MultivalueAggregateRequestParameter\.MultivalueAggregateFunction Property

Gets or sets the multi\-value aggregation function to perform\.

```csharp
public DiGi.PostgreSQL.Table.Enums.MultivalueAggregateFunction MultivalueAggregateFunction { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Enums\.MultivalueAggregateFunction](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.enums.multivalueaggregatefunction 'DiGi\.PostgreSQL\.Table\.Enums\.MultivalueAggregateFunction')

### Example
SplitDistinctCount

<a name='DiGi.GIS.WebAPI.Classes.MultivalueAggregateRequestParameter.Separator'></a>

## MultivalueAggregateRequestParameter\.Separator Property

Gets or sets the optional string separator; defaults to null triggering dynamic auto\-detection\.

```csharp
public string? Separator { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
","

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController'></a>

## OccupancyDataController Class

Controller responsible for handling requests related to occupancy data within the GIS PostgreSQL Web API\.

```csharp
public class OccupancyDataController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → OccupancyDataController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.OccupancyDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## OccupancyDataController\(GISWebAPIConfigurationFileWatcher, Building2DOccupancyDataPostgreSQLConverter, AdministrativeAreal2DOccupancyDataPostgreSQLConverter, Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the OccupancyDataController class\.

```csharp
public OccupancyDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.Building2DOccupancyDataPostgreSQLConverter building2DOccupancyDataPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DOccupancyDataPostgreSQLConverter administrativeAreal2DOccupancyDataPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.OccupancyDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher used to monitor settings for the GIS PostgreSQL Web API\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.OccupancyDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).building2DOccupancyDataPostgreSQLConverter'></a>

`building2DOccupancyDataPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DOccupancyDataPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2doccupancydatapostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DOccupancyDataPostgreSQLConverter')

The converter used for building 2D occupancy data operations in the PostgreSQL database\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.OccupancyDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DOccupancyDataPostgreSQLConverter'></a>

`administrativeAreal2DOccupancyDataPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DOccupancyDataPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2doccupancydatapostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DOccupancyDataPostgreSQLConverter')

The converter used for administrative areal 2D occupancy data operations in the PostgreSQL database\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.OccupancyDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The converter for Building2D objects, used to read which county row a reference is already filed under\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.OccupancyDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.Building2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DOccupancyDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter used for administrative areal 2D data operations in the PostgreSQL database\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.AdministrativeAreal2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken)'></a>

## OccupancyDataController\.AdministrativeAreal2DUpdateItemsAsync\(JsonArray, string, CancellationToken\) Method

Asynchronously updates occupancy data items for administrative areal 2D entities\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> AdministrativeAreal2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.AdministrativeAreal2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray') containing the occupancy data items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.AdministrativeAreal2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.AdministrativeAreal2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') representing the result of the update operation, returning a bad request if updates are disabled or no content if the input array is null or empty\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken)'></a>

## OccupancyDataController\.Building2DUpdateItemsAsync\(JsonArray, string, string, CancellationToken\) Method

Asynchronously updates building 2D items based on the provided JSON data and identification code\.

A county code does not identify a single county row: BDOT10k stores a county whose territory is disconnected as one feature per polygon part, and every part becomes its own row. Every part the code names is passed on, and each datum is filed under the part it actually belongs to - see [Building2DUpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.OccupancyDataController\.Building2DUpdateItemsByCountyIdsAsync\(System\.Text\.Json\.Nodes\.JsonArray, int\[\], string, System\.Threading\.CancellationToken\)') for how that is decided.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Building2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string code, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray') containing the item data to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The identification code used to validate or categorize the update request\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken)'></a>

## OccupancyDataController\.Building2DUpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\) Method

Asynchronously updates building 2D occupancy items in the database for the given county rows\.

The unambiguous counterpart of [Building2DUpdateItemsAsync\(JsonArray, string, string, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.OccupancyDataController\.Building2DUpdateItemsAsync\(System\.Text\.Json\.Nodes\.JsonArray, string, string, System\.Threading\.CancellationToken\)'): it takes county identifiers rather than a code, so the caller states which rows are in play instead of leaving the server to derive them.

The identifiers are the parts of one county in play, and each datum is filed under the part already holding the `building_2d` row its reference names, probed lowest part first - whether one identifier arrived or several, since naming one part is not evidence the county has one. That row was filed by geometry when it was imported, so reusing its answer keeps both tables keyed by the same `(county_id, reference)` pair.

A datum no named part holds is widened to every part of the county its parts name, and only a datum no part of the county holds is left unwritten - it carries no geometry of its own, so nothing states where it belongs, and storing it under a guessed part is the state this replaced.

The county-part lookup not running is answered as a distinct 500 naming it, and a transient database failure as 503 with `Retry-After`, so an unreachable database is never mistaken for a quiet no-op.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Building2DUpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, int[]? countyIds, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray') containing the item data to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The identifiers of the county rows the occupancy data belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.Building2DUpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetAdministrativeAreal2DItemsByReferenceAsync(string,System.Threading.CancellationToken)'></a>

## OccupancyDataController\.GetAdministrativeAreal2DItemsByReferenceAsync\(string, CancellationToken\) Method

Retrieves administrative areal 2D items based on the provided reference identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAdministrativeAreal2DItemsByReferenceAsync(string reference, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetAdministrativeAreal2DItemsByReferenceAsync(string,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string used to identify the administrative areal 2D items\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetAdministrativeAreal2DItemsByReferenceAsync(string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') representing the result of the operation, containing the requested items or an error response\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DDuplicateReferencesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken)'></a>

## OccupancyDataController\.GetBuilding2DDuplicateReferencesAsync\(Nullable\<int\>, int, int, CancellationToken\) Method

Asynchronously retrieves the building references that hold more than one occupancy data record, optionally filtered by county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetBuilding2DDuplicateReferencesAsync(System.Nullable<int> countyId=null, int limit=100, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DDuplicateReferencesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional integer identifier of the county to filter by; if null, searches across all counties\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DDuplicateReferencesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of duplicate references to return\. Defaults to 100\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DDuplicateReferencesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DDuplicateReferencesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token used to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing the list of duplicate references \- `200 []` when there are none \- or 500 when the query could not run, so "clean" and "failed" are never the same answer\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DDuplicatesCountAsync(System.Nullable_int_,int,System.Threading.CancellationToken)'></a>

## OccupancyDataController\.GetBuilding2DDuplicatesCountAsync\(Nullable\<int\>, int, CancellationToken\) Method

Asynchronously retrieves the total count of building references that hold more than one occupancy data record, optionally filtered by county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetBuilding2DDuplicatesCountAsync(System.Nullable<int> countyId=null, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DDuplicatesCountAsync(System.Nullable_int_,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional integer identifier of the county to filter by; if null, counts across all counties\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DDuplicatesCountAsync(System.Nullable_int_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DDuplicatesCountAsync(System.Nullable_int_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token used to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the duplicates count, or 404 if the partition does not exist or count is negative\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## OccupancyDataController\.GetBuilding2DItemsByReferenceAsync\(string, Nullable\<int\>, CancellationToken\) Method

Retrieves Building 2D occupancy data items based on a specified reference and an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetBuilding2DItemsByReferenceAsync(string reference, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string used to identify the building 2D items\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional identifier of the county associated with the building data\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDataController.GetBuilding2DItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing the requested building 2D items, or a [Microsoft\.AspNetCore\.Mvc\.BadRequestResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.badrequestresult 'Microsoft\.AspNetCore\.Mvc\.BadRequestResult') if the reference is null or whitespace\.

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask'></a>

## OccupancyDatasPostTask Class

Represents a task for posting occupancy data to the GIS PostgreSQL Web API\.

```csharp
public class OccupancyDatasPostTask : DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask<DiGi.GIS.Classes.OccupancyData>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → OccupancyDatasPostTask
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask.OccupancyDatasPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## OccupancyDatasPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the OccupancyDatasPostTask class\.

```csharp
public OccupancyDatasPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask.OccupancyDatasPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to handle GIS PostgreSQL Web API operations\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask.Code'></a>

## OccupancyDatasPostTask\.Code Property

Gets or sets the code associated with the occupancy data post task\.

A code does not identify a single county row - a multi-part county holds one row per polygon part - so set [CountyIds](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask.CountyIds 'DiGi\.GIS\.WebAPI\.Classes\.OccupancyDatasPostTask\.CountyIds') instead wherever the identifiers are already known. [CountyIds](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask.CountyIds 'DiGi\.GIS\.WebAPI\.Classes\.OccupancyDatasPostTask\.CountyIds') takes precedence when both are set.

```csharp
public string? Code { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask.CountyIds'></a>

## OccupancyDatasPostTask\.CountyIds Property

Gets or sets the identifiers of the county rows the building 2D occupancy data belong to \- normally every polygon part of one county, since a single id is not evidence the code has one part\. When set it is used in preference to [Code](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask.Code 'DiGi\.GIS\.WebAPI\.Classes\.OccupancyDatasPostTask\.Code'), which lets the server resolve the code to every part\. It does not affect [Values\_AdministrativeAreal2D](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask.Values_AdministrativeAreal2D 'DiGi\.GIS\.WebAPI\.Classes\.OccupancyDatasPostTask\.Values\_AdministrativeAreal2D'), which is not county\-keyed\.

```csharp
public System.Collections.Generic.HashSet<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.HashSet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')

<a name='DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask.Values_AdministrativeAreal2D'></a>

## OccupancyDatasPostTask\.Values\_AdministrativeAreal2D Property

Gets or sets the collection of [DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData') values for administrative areal 2D\.

```csharp
public System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.OccupancyData>? Values_AdministrativeAreal2D { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController'></a>

## OrtoDatasController Class

Controller providing API endpoints for managing and accessing orthophoto data and related GIS spatial information via a PostgreSQL database\.

```csharp
public class OrtoDatasController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → OrtoDatasController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.OrtoDatasController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.OrtoDatasPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter)'></a>

## OrtoDatasController\(GISWebAPIConfigurationFileWatcher, OrtoDatasPostgreSQLConverter, Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter, SecurityKeyManager, TokenRevocationStore, YearBuiltDataPostgreSQLConverter\) Constructor

Initializes a new instance of the OrtoDatasController class\.

```csharp
public OrtoDatasController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.OrtoDatasPostgreSQLConverter ortoDatasPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter, DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager=null, DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore=null, DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter? yearBuiltDataPostgreSQLConverter=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.OrtoDatasController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.OrtoDatasPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher used to monitor changes to the GIS PostgreSQL Web API settings\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.OrtoDatasController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.OrtoDatasPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter).ortoDatasPostgreSQLConverter'></a>

`ortoDatasPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.OrtoDatasPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.ortodataspostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.OrtoDatasPostgreSQLConverter')

The converter used for handling OrtoDatas data operations within the PostgreSQL database\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.OrtoDatasController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.OrtoDatasPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The converter used for handling Building 2D data operations within the PostgreSQL database\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.OrtoDatasController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.OrtoDatasPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter used for handling Administrative Areal 2D data operations within the PostgreSQL database\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.OrtoDatasController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.OrtoDatasPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter).securityKeyManager'></a>

`securityKeyManager` [DiGi\.WebAPI\.Classes\.SecurityKeyManager](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.securitykeymanager 'DiGi\.WebAPI\.Classes\.SecurityKeyManager')

The user extension's security key manager; `null` when the user extension is not loaded, in which case every user\-token check denies\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.OrtoDatasController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.OrtoDatasPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter).tokenRevocationStore'></a>

`tokenRevocationStore` [DiGi\.WebAPI\.Classes\.TokenRevocationStore](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.tokenrevocationstore 'DiGi\.WebAPI\.Classes\.TokenRevocationStore')

The user extension's token revocation store; `null` when the user extension is not loaded, in which case every user\-token check denies\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.OrtoDatasController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.OrtoDatasPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter).yearBuiltDataPostgreSQLConverter'></a>

`yearBuiltDataPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.YearBuiltDataPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.yearbuiltdatapostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.YearBuiltDataPostgreSQLConverter')

The converter reading the buildings and their year\-built rows in the main database \- the other half of the random unverified\-building draw, whose orthophoto half is the storage database; `null` when the main store is not configured, in which case the draw answers 503\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.AcknowledgeBuilding2DReferencesAsync(System.Collections.Generic.IEnumerable_long_,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.AcknowledgeBuilding2DReferencesAsync\(IEnumerable\<long\>, CancellationToken\) Method

Acknowledges and deletes completed building 2D reference objects from the update queue\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> AcknowledgeBuilding2DReferencesAsync(System.Collections.Generic.IEnumerable<long>? ids, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.AcknowledgeBuilding2DReferencesAsync(System.Collections.Generic.IEnumerable_long_,System.Threading.CancellationToken).ids'></a>

`ids` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of queue entry identifiers to acknowledge and remove from the queue\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.AcknowledgeBuilding2DReferencesAsync(System.Collections.Generic.IEnumerable_long_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.ContainsByReferencesAsync\(List\<string\>, Nullable\<int\>, Nullable\<bool\>, CancellationToken\) Method

Asynchronously checks for the existence of a collection of references, optionally filtered by a county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> ContainsByReferencesAsync(System.Collections.Generic.List<string>? references, System.Nullable<int> countyId, System.Nullable<bool> inverted, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

A list of strings representing the references to be checked\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The identifier of the county partition to confine the check to\. Omit to search every partition\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).inverted'></a>

`inverted` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Returns the references that are absent rather than the ones present\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.ContainsByReferencesAsync(System.Collections.Generic.List_string_,System.Nullable_int_,System.Nullable_bool_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetCountByCountyIdAsync\(int, bool, bool, int, CancellationToken\) Method

Asynchronously retrieves the number of orthophoto rows stored for one county partition\.

The cheapest question that can be asked of the store, and the one that separates a county nothing was ever downloaded for from one that was downloaded and holds nothing.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountByCountyIdAsync(int countyId, bool estimated=false, bool analyze=false, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county partition to count\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).estimated'></a>

`estimated` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Reads the planner's row estimate instead of counting the rows\. Far faster on a large partition and accurate to a few percent, but it reflects the last time the partition was analysed rather than this moment\. An unanalysed partition returns 204 NoContent\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).analyze'></a>

`analyze` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to perform an ANALYZE operation before reading the estimate to ensure statistics are current\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the count, 204 NoContent when the partition exists but is unanalysed, or 404 NotFound when the county has no partition\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetEstimatedCoverageFactorAsync(int,int,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetEstimatedCoverageFactorAsync\(int, int, CancellationToken\) Method

Retrieves the orthophoto coverage factor for a specified administrative area 2D identifier\.

Below county level the figure is counted rather than estimated. A subdivision and a municipality have no partition of their own - both tables are partitioned by `county_id` - so the coverage is measured over the buildings whose centre lies inside the area's <b>polygon</b>, by reading its county once per side and matching the references in memory. Membership is geometric, not the stored `subdivision_id`: where the subdivision layer nests, a district holds nothing by that column while its polygon holds thousands, and a municipality is measured over its own polygon rather than as a sum of its subdivisions, which would count a nested city once per level (DiGi.GIS.PostgreSQL#77). County and above keep the planner's row estimate, which is what makes a voivodeship or a country affordable at all, so an exact sub-county figure and its county's estimate can differ by a few percent and both be right.

Where building data is measured but no orthophoto partition has been created for a county, the county has zero orthophotos stored and yields a coverage factor of `0.0`. A coverage that cannot be measured (missing or unanalysed building data, or an unanalysed orthophoto partition) answers 204 NoContent; `countbycountyid?estimated=true` reads the state of one county, answering 200 when it is analysed, 204 when it is unanalysed and 404 when it has no partition.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetEstimatedCoverageFactorAsync(int administrativeAreal2DId, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetEstimatedCoverageFactorAsync(int,int,System.Threading.CancellationToken).administrativeAreal2DId'></a>

`administrativeAreal2DId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the administrative area 2D\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetEstimatedCoverageFactorAsync(int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of each command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetEstimatedCoverageFactorAsync(int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the coverage factor, 204 NoContent when it could not be measured, or an error status code\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetEstimatedCoverageFactorsAsync(System.Collections.Generic.IEnumerable_int_,System.Nullable_bool_,int,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetEstimatedCoverageFactorsAsync\(IEnumerable\<int\>, Nullable\<bool\>, int, CancellationToken\) Method

Retrieves the orthophoto coverage factors for the specified administrative area identifiers\.

The values come back in the order the identifiers were given, one per identifier, so a caller can update one row per value without matching anything up. A value is `null` where the coverage could not be measured (missing or unanalysed building data, or unanalysed orthophoto partition). Where building data is measured but no orthophoto partition has been created for a county, its orthophoto count is 0, yielding a coverage factor of `0.0`.

A county, a voivodeship and a country are answered from the two tables row estimates - every identifier is resolved to the counties it stands for and both estimates are read for the whole set in one query per table. A subdivision and a municipality have no partition of their own and are instead counted, over the buildings whose centre lies inside their own polygon, by reading their county once per side; every subdivision and municipality of one county is served from that single pass. A municipality is one polygon, not a sum of its subdivisions - where the subdivision layer nests that sum counts a city once per level (DiGi.GIS.PostgreSQL#77).

Because counting reads a whole county, at most [MaximumCoverageCountyCount](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.OrtoDatas.MaximumCoverageCountyCount 'DiGi\.GIS\.WebAPI\.Constants\.OrtoDatas\.MaximumCoverageCountyCount') distinct counties are counted per request, taken in the order the identifiers were given. Identifiers sitting in counties beyond that are answered `null` rather than given their county figure or failing the request.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetEstimatedCoverageFactorsAsync(System.Collections.Generic.IEnumerable<int> administrativeAreal2DIds, System.Nullable<bool> analyze, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetEstimatedCoverageFactorsAsync(System.Collections.Generic.IEnumerable_int_,System.Nullable_bool_,int,System.Threading.CancellationToken).administrativeAreal2DIds'></a>

`administrativeAreal2DIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of administrative area 2D identifiers to be processed\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetEstimatedCoverageFactorsAsync(System.Collections.Generic.IEnumerable_int_,System.Nullable_bool_,int,System.Threading.CancellationToken).analyze'></a>

`analyze` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

Refreshes the statistics before reading them\. This applies only to the estimated county\-and\-above path and does nothing for a subdivision or a municipality, which are counted\. It costs one `VACUUM ANALYZE` per resolved county partition on each of the two tables \- for a country identifier that is several hundred maintenance statements against live partitions, so raise `commandtimeout` to match or leave the flag off\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetEstimatedCoverageFactorsAsync(System.Collections.Generic.IEnumerable_int_,System.Nullable_bool_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of each command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetEstimatedCoverageFactorsAsync(System.Collections.Generic.IEnumerable_int_,System.Nullable_bool_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetImageByReferenceAsync(string,short,System.Nullable_int_,bool,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetImageByReferenceAsync\(string, short, Nullable\<int\>, bool, CancellationToken\) Method

Retrieves the orthophoto image a building holds for one exact year\.

Exact-year: the answer is the photo taken in that year, or a 404 when the building holds none for it - a year without a photo is not served by the nearest earlier one, which is what the previous floor lookup did. Send a year from `yearsbyreference`, which lists only the years that have a card.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetImageByReferenceAsync(string reference, short year, System.Nullable<int> countyId=null, bool fallbackByReference=false, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetImageByReferenceAsync(string,short,System.Nullable_int_,bool,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string of the orthophoto image\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetImageByReferenceAsync(string,short,System.Nullable_int_,bool,System.Threading.CancellationToken).year'></a>

`year` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

The exact year the orthophoto is requested for\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetImageByReferenceAsync(string,short,System.Nullable_int_,bool,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional identifier of the county associated with the orthophoto data\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetImageByReferenceAsync(string,short,System.Nullable_int_,bool,System.Threading.CancellationToken).fallbackByReference'></a>

`fallbackByReference` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to re\-run the read by reference alone when nothing is held under the named county\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetImageByReferenceAsync(string,short,System.Nullable_int_,bool,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the photo bytes, 404 when the building holds no photo for that year, 400 when the reference is missing, or 500 when the read failed\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetItemByReferenceAsync\(string, Nullable\<int\>, CancellationToken\) Method

Asynchronously retrieves an orthodata item based on the specified reference and optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemByReferenceAsync(string reference, System.Nullable<int> countyId=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string used to locate the orthodata item\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional identifier of the county associated with the orthodata item\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetItemByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferenceByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetOrtoDatasReferenceByReferenceAsync\(string, Nullable\<int\>, bool, CancellationToken\) Method

Retrieves an orthodata reference by its unique reference code and an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetOrtoDatasReferenceByReferenceAsync(string reference, System.Nullable<int> countyId=null, bool fallbackByReference=false, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferenceByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string of the building to retrieve orthodata metadata for\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferenceByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional integer identifier of the county used to filter the search\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferenceByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken).fallbackByReference'></a>

`fallbackByReference` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to perform a fallback search across all partitions if not matched by county\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferenceByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByBuilding2DReferencesAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DReference_,bool,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetOrtoDatasReferencesByBuilding2DReferencesAsync\(IEnumerable\<Building2DReference\>, bool, CancellationToken\) Method

Retrieves a list of orthodata references for the specified building 2D reference objects\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetOrtoDatasReferencesByBuilding2DReferencesAsync(System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.Building2DReference> building2DReferences, bool fallbackByReference=false, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByBuilding2DReferencesAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DReference_,bool,System.Threading.CancellationToken).building2DReferences'></a>

`building2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference') objects to query\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByBuilding2DReferencesAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DReference_,bool,System.Threading.CancellationToken).fallbackByReference'></a>

`fallbackByReference` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to perform a fallback search across all partitions if not matched by county\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByBuilding2DReferencesAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DReference_,bool,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByCountyIdAsync(int,int[],System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetOrtoDatasReferencesByCountyIdAsync\(int, int\[\], CancellationToken\) Method

Retrieves a list of orthodata references for a specified county, with optional subdivision filtering\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetOrtoDatasReferencesByCountyIdAsync(int countyId, int[]? subdivisionIds=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByCountyIdAsync(int,int[],System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The integer identifier of the county\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByCountyIdAsync(int,int[],System.Threading.CancellationToken).subdivisionIds'></a>

`subdivisionIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

An optional array of subdivision identifiers to filter by\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByCountyIdAsync(int,int[],System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetOrtoDatasReferencesByReferencesAsync\(IEnumerable\<string\>, Nullable\<int\>, bool, CancellationToken\) Method

Retrieves a list of orthodata references for the specified building reference codes and optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetOrtoDatasReferencesByReferencesAsync(System.Collections.Generic.IEnumerable<string> references, System.Nullable<int> countyId=null, bool fallbackByReference=false, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of unique reference strings to query\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional integer identifier of the county used to filter the search\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,System.Threading.CancellationToken).fallbackByReference'></a>

`fallbackByReference` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to perform a fallback search across all partitions if not matched by county\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetOrtoDatasReferencesByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetPolygonalFace2DsByIdAsync(System.Collections.Generic.IEnumerable_int_,int,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetPolygonalFace2DsByIdAsync\(IEnumerable\<int\>, int, CancellationToken\) Method

Resolves administrative areas to their polygons, keyed by identifier \- the shape a coverage is measured over\.

Any level is accepted, and each area is measured as one polygon: a municipality over its own outline rather than as the sum of its subdivisions, which double-counts wherever the subdivision layer nests (DiGi.GIS.PostgreSQL#77).

```csharp
private System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int,DiGi.Geometry.Planar.Classes.PolygonalFace2D>?> GetPolygonalFace2DsByIdAsync(System.Collections.Generic.IEnumerable<int> administrativeAreal2DIds, int commandTimeout, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetPolygonalFace2DsByIdAsync(System.Collections.Generic.IEnumerable_int_,int,System.Threading.CancellationToken).administrativeAreal2DIds'></a>

`administrativeAreal2DIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers to resolve\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetPolygonalFace2DsByIdAsync(System.Collections.Generic.IEnumerable_int_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of each command\. A value of 0 disables the timeout\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetPolygonalFace2DsByIdAsync(System.Collections.Generic.IEnumerable_int_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[DiGi\.Geometry\.Planar\.Classes\.PolygonalFace2D](https://learn.microsoft.com/en-us/dotnet/api/digi.geometry.planar.classes.polygonalface2d 'DiGi\.Geometry\.Planar\.Classes\.PolygonalFace2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The polygon of every identifier that has one, or null when the converter is missing or the rows could not be read\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetQueueSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetQueueSummariesByCountyIdsAsync\(List\<int\>, int, CancellationToken\) Method

Asynchronously reports what each of the named counties still has waiting in the orthophoto download queue\.

Reads the queue without claiming anything from it, unlike [NextBuilding2DReferencesAsync\(int, int, int, int, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasController.NextBuilding2DReferencesAsync(int,int,int,int,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasController\.NextBuilding2DReferencesAsync\(int, int, int, int, System\.Threading\.CancellationToken\)'), which claims the rows it returns. It is the only way to see what a refresh queued, and the way to watch the refresh and the download move against each other.

Naming no county reports every one. Counties with nothing waiting are absent from the result rather than present with a zero, so an empty result means the queue is drained.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetQueueSummariesByCountyIdsAsync(System.Collections.Generic.List<int>? countyIds, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetQueueSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

The identifiers of the counties to report on, repeated once per county\. Omit to report every one\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetQueueSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetQueueSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the queue depths as JSON, or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetRandomBuilding2DReferenceAsync(int[],int,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetRandomBuilding2DReferenceAsync\(int\[\], int, CancellationToken\) Method

Draws one building that has orthophoto coverage and no user\-provided year built yet \- the next candidate for a reviewer\.

The drawn [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference') carries the `building_2d` part it is filed under; that part is what the caller must send back on the write and on every read, since a county code can name several parts.

An optional repeated `countyids` confines the draw to those `building_2d` parts (`?countyids=73482&countyids=73485`, one per polygon part - never a county code); omitted or empty draws from every covered part.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetRandomBuilding2DReferenceAsync(int[]? countyIds=null, int commandTimeout=30, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetRandomBuilding2DReferenceAsync(int[],int,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

Optional `building_2d` part ids that confine the draw; omitted or empty draws from every covered part\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetRandomBuilding2DReferenceAsync(int[],int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 30 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetRandomBuilding2DReferenceAsync(int[],int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the drawn building, 404 when no unverified covered building remains \(in the requested parts, when given\), 401 without a valid user token, 400 for an invalid timeout, or 503 when the main \(year built\) store is not configured\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetSubdivisionLinksByCountyIdAsync(int,int,int,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetSubdivisionLinksByCountyIdAsync\(int, int, int, CancellationToken\) Method

Asynchronously compares, for one county, the subdivision each building is filed under against the one its orthophoto row carries\.

The two tables live in different databases, so nothing keeps them in step on its own and no query can join them - each side is read once and matched in memory. This is the only place the two can be seen together.

Read the result across a run rather than on its own. `OrtoDatasOnlyCount` counts rows whose orthophoto knows a subdivision the building no longer does, and nothing legitimate removes one, so a refresh that lowers it is doing damage. `Building2DOnlyCount` counts what a refresh exists to fix: it should fall to near zero and stay there, and climbing again once the download drains the queue is issue #36.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSubdivisionLinksByCountyIdAsync(int countyId, int sampleCount=20, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetSubdivisionLinksByCountyIdAsync(int,int,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county to compare\. One polygon part, not a code \- a multi\-part county is compared a part at a time\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetSubdivisionLinksByCountyIdAsync(int,int,int,System.Threading.CancellationToken).sampleCount'></a>

`sampleCount` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

How many references to name back per disagreeing category\. The counts are exact whatever this is; the samples are what make a disagreement actionable\. Between 0 and [MaximumSampleCount](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.OrtoDatas.MaximumSampleCount 'DiGi\.GIS\.WebAPI\.Constants\.OrtoDatas\.MaximumSampleCount'), inclusive\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetSubdivisionLinksByCountyIdAsync(int,int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of each command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetSubdivisionLinksByCountyIdAsync(int,int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the comparison as JSON, or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetSummariesByCountyIdsAsync\(List\<int\>, int, CancellationToken\) Method

Asynchronously summarises what each of the named county partitions holds: how many rows, how many name a subdivision, how many distinct subdivisions they are spread across, and when they were written\.

The measurement to take either side of a refresh. A building's subdivision is resolved in another database and pushed across, so [DiGi\.GIS\.PostgreSQL\.Classes\.OrtoDatasCountyResult\.WithSubdivisionIdCount](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.ortodatascountyresult.withsubdivisionidcount 'DiGi\.GIS\.PostgreSQL\.Classes\.OrtoDatasCountyResult\.WithSubdivisionIdCount') can only ever be gained - a run that lowers it is clearing subdivisions rather than filling them in, which is the defect of issues #23, #31 and #36.

Naming no county summarises every partition, in one grouped statement. Counties holding no row are absent from the result rather than present with a zero.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSummariesByCountyIdsAsync(System.Collections.Generic.List<int>? countyIds, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

The identifiers of the county partitions to summarise, repeated once per county\. Omit to summarise every one\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the summaries as JSON, or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetYearsByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.GetYearsByReferenceAsync\(string, Nullable\<int\>, bool, CancellationToken\) Method

Lists the years that hold a photo for one building \- only the years that have imagery, not every year of the `[min, max]` range\.

The read is projected on the server: it answers the years that have a card and never reads the photo bytes, so listing a building's years stays cheap no matter how much imagery it carries. A building with no orthophotos answers 204, so an empty answer is a result the caller can act on rather than a failure to special-case.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetYearsByReferenceAsync(string reference, System.Nullable<int> countyId=null, bool fallbackByReference=false, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetYearsByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The reference of the building to list the years of\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetYearsByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional identifier of the county part the building is filed under\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetYearsByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken).fallbackByReference'></a>

`fallbackByReference` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Re\-run the read by reference alone when nothing is held under the named county\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.GetYearsByReferenceAsync(string,System.Nullable_int_,bool,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the sorted years that hold a photo, 204 when the building holds none, 401 without a valid user token, 400 when the reference is missing, or 500 when the read failed\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.NextBuilding2DReferencesAsync(int,int,int,int,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.NextBuilding2DReferencesAsync\(int, int, int, int, CancellationToken\) Method

Retrieves and claims the next batch of building 2D reference objects from the update queue\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> NextBuilding2DReferencesAsync(int count=100, int claimTimeoutMinutes=30, int maxAttempts=5, int commandTimeout=60, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.NextBuilding2DReferencesAsync(int,int,int,int,System.Threading.CancellationToken).count'></a>

`count` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of building 2D reference objects to retrieve\. Must be at least 1\. Defaults to 100\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.NextBuilding2DReferencesAsync(int,int,int,int,System.Threading.CancellationToken).claimTimeoutMinutes'></a>

`claimTimeoutMinutes` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The duration in minutes before an unacknowledged claim expires and returns to the queue\. Must be at least 1\. Defaults to 30\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.NextBuilding2DReferencesAsync(int,int,int,int,System.Threading.CancellationToken).maxAttempts'></a>

`maxAttempts` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of claim attempts before a reference is retired as a poison row\. Must be at least 1\. Defaults to 5\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.NextBuilding2DReferencesAsync(int,int,int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 60 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.NextBuilding2DReferencesAsync(int,int,int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCodeAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.UpdateItemsByCodeAsync\(JsonArray, string, string, CancellationToken\) Method

Updates multiple orthophoto data items based on the provided JSON array and code\.

A county code does not identify a single county row: BDOT10k stores a county whose territory is disconnected as one feature per polygon part, and every part becomes its own row. Every part the code names is passed on, and each entry is filed under the part it actually belongs to - see [UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasController\.UpdateItemsByCountyIdsAsync\(System\.Text\.Json\.Nodes\.JsonArray, int\[\], string, System\.Threading\.CancellationToken\)') for how that is decided.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsByCodeAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string code, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCodeAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the orthophoto data items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCodeAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique identifier or code used to identify the items for update\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCodeAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCodeAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken)'></a>

## OrtoDatasController\.UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\) Method

Updates orthodata items associated with the given county rows\.

A single identifier is taken as stated and every entry is filed under it. Several identifiers are the polygon parts of one multi-part county, and each entry is then filed under the part it belongs to, decided in two steps:

1. the part already holding the entry's `building_2d` row, probed lowest part first. That row was filed by geometry when it was imported, and reusing its answer keeps both tables keyed by the same `(county_id, reference)` pair - orthodata filed under a part its building is not stored in reads back as missing.

2. geometry, for an entry no part holds a 2D row for: the part containing its bounding box, else the nearest part, else the part it overlaps most. Done by the converter, which drops an entry it cannot place rather than filing it under a guess.

The county-part lookup (step 1) not running is answered as a distinct 500 naming it, before any geometry fallback runs, and a transient database failure as 503 with `Retry-After` - an unreachable database must not look like a decision by geometry.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, int[]? countyIds, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the orthodata items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The identifiers of the county rows the entries belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasFromDatabasePostTask'></a>

## OrtoDatasFromDatabasePostTask Class

Handles the process of posting orthodata retrieved from the database\.

```csharp
public class OrtoDatasFromDatabasePostTask : DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → [OrtoDatasPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasPostTask') → OrtoDatasFromDatabasePostTask
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasFromDatabasePostTask.OrtoDatasFromDatabasePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## OrtoDatasFromDatabasePostTask\(GISWebAPIManager\) Constructor

Handles the process of posting orthodata retrieved from the database\.

```csharp
public OrtoDatasFromDatabasePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasFromDatabasePostTask.OrtoDatasFromDatabasePostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager responsible for handling GIS PostgreSQL Web API communications\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasFromDatabasePostTask.Count'></a>

## OrtoDatasFromDatabasePostTask\.Count Property

Gets or sets the number of items to claim per batch from the update queue\. Defaults to 5\.

```csharp
public int Count { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasFromDatabasePostTask.MaxConsecutiveFailureCount'></a>

## OrtoDatasFromDatabasePostTask\.MaxConsecutiveFailureCount Property

Gets or sets how many failed batches in a row stop the task\. Defaults to 3; values below 1 are treated as 1\.

A batch fails when a request times out or the connection drops, when Building2Ds cannot be fetched, or when the upload or the acknowledge is not accepted. Its references stay claimed and return to the queue when their lease expires, so the run moves on to the next batch. A batch that stores OrtoDatas resets the count. Reaching the limit ends the task with a [System\.TimeoutException](https://learn.microsoft.com/en-us/dotnet/api/system.timeoutexception 'System\.TimeoutException') naming the county and the last failure, so a dead server still stops the run.

No host exposes this setting, so the default is the production value.

```csharp
public int MaxConsecutiveFailureCount { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasFromDatabasePostTask.OrtoDatasBuilding2DOptions'></a>

## OrtoDatasFromDatabasePostTask\.OrtoDatasBuilding2DOptions Property

Gets or sets the options used for retrieving 2D building orthophoto data\.

```csharp
public DiGi.GIS.Classes.OrtoDatasBuilding2DOptions? OrtoDatasBuilding2DOptions { get; set; }
```

#### Property Value
[DiGi\.GIS\.Classes\.OrtoDatasBuilding2DOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatasbuilding2doptions 'DiGi\.GIS\.Classes\.OrtoDatasBuilding2DOptions')

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask'></a>

## OrtoDatasPostTask Class

Handles the posting of OrtoDatas objects to the PostgreSQL web API\.

```csharp
public class OrtoDatasPostTask : DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask<DiGi.GIS.Classes.OrtoDatas>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → OrtoDatasPostTask

Derived  
↳ [OrtoDatasFromDatabasePostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasFromDatabasePostTask 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasFromDatabasePostTask')
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.OrtoDatasPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## OrtoDatasPostTask\(GISWebAPIManager\) Constructor

Handles the posting of OrtoDatas objects to the PostgreSQL web API\.

OrtoDatas carry imagery, so [SerializableObjectsPostOptions](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_.SerializableObjectsPostOptions 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>\.SerializableObjectsPostOptions') defaults to [DefaultDelay](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.DefaultDelay 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasPostTask\.DefaultDelay') per attempt and [DefaultBatchMemorySize](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.DefaultBatchMemorySize 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasPostTask\.DefaultBatchMemorySize') per request instead of the general 20 s and 3 MB. No host exposes these settings, so these defaults are the production values.

```csharp
public OrtoDatasPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.OrtoDatasPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to handle PostgreSQL web API operations\.
### Fields

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.DefaultBatchMemorySize'></a>

## OrtoDatasPostTask\.DefaultBatchMemorySize Field

Default uncompressed batch size in bytes for OrtoDatas uploads \(1 MB\), so a slow write fails less often and a retry resends less\.

```csharp
public const int DefaultBatchMemorySize = 1048576;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.DefaultDelay'></a>

## OrtoDatasPostTask\.DefaultDelay Field

Default per\-attempt upload timeout for OrtoDatas\. Kept below the 60 s [System\.Net\.Http\.HttpClient\.Timeout](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.timeout 'System\.Net\.Http\.HttpClient\.Timeout') set in [ServiceProvider\(\)](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Create.ServiceProvider() 'DiGi\.GIS\.WebAPI\.Create\.ServiceProvider\(\)'), which would otherwise cut the attempt first\.

```csharp
public static readonly TimeSpan DefaultDelay;
```

#### Field Value
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')
### Properties

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.Code'></a>

## OrtoDatasPostTask\.Code Property

Gets or sets the code associated with the OrtoDatas post task\.

A code does not identify a single county row - a multi-part county holds one row per polygon part - so set [CountyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.CountyId 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasPostTask\.CountyId') instead wherever the identifier is already known. [CountyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.CountyId 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasPostTask\.CountyId') takes precedence when both are set.

```csharp
public string? Code { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.CountyId'></a>

## OrtoDatasPostTask\.CountyId Property

Gets or sets the identifier of the county row the OrtoDatas belong to\. When set it is used in preference to [Code](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.Code 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasPostTask\.Code'), which leaves the server to choose between the rows of a multi\-part county\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken)'></a>

## OrtoDatasPostTask\.ExecuteAsync\(IEnumerable\<OrtoDatas\>, IEnumerable\<int\>, LongProgressWrapper, CancellationToken\) Method

Asynchronously posts orthodata in memory\-sized batches, naming the county rows they belong to\.

Several identifiers are the polygon parts of one county, and the server files each entry under the part holding its `building_2d` row; a single identifier is written verbatim.

```csharp
protected System.Threading.Tasks.Task<bool> ExecuteAsync(System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.OrtoDatas>? values, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.Core.Classes.LongProgressWrapper? longProgressWrapper, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The orthodata to post\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the orthodata belongs to \- normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).longProgressWrapper'></a>

`longProgressWrapper` [DiGi\.Core\.Classes\.LongProgressWrapper](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.longprogresswrapper 'DiGi\.Core\.Classes\.LongProgressWrapper')

Tracks the number of entries posted\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask.ExecuteAsync(System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.Core.Classes.LongProgressWrapper,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. True when every batch was accepted; false when there was nothing to post or a batch was refused\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasTask'></a>

## OrtoDatasTask Class

Handles the background processing of orthophoto data within the GIS PostgreSQL context\.

```csharp
public class OrtoDatasTask : DiGi.Core.Classes.ReportableBackgroundTask<long>, DiGi.GIS.PostgreSQL.Interfaces.IGISPostgreSQLObject, DiGi.Core.Interfaces.IObject
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → OrtoDatasTask

Implements [DiGi\.GIS\.PostgreSQL\.Interfaces\.IGISPostgreSQLObject](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.interfaces.igispostgresqlobject 'DiGi\.GIS\.PostgreSQL\.Interfaces\.IGISPostgreSQLObject'), [DiGi\.Core\.Interfaces\.IObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.interfaces.iobject 'DiGi\.Core\.Interfaces\.IObject')
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasTask.OrtoDatasTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager)'></a>

## OrtoDatasTask\(GISWebAPIManager, GISPostgreSQLConverterManager\) Constructor

Initializes a new instance of the OrtoDatasTask class\.

```csharp
public OrtoDatasTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager? gISPostgreSQLConverterManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasTask.OrtoDatasTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager responsible for handling GIS PostgreSQL Web API operations\.

<a name='DiGi.GIS.WebAPI.Classes.OrtoDatasTask.OrtoDatasTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.PostgreSQL.Classes.GISPostgreSQLConverterManager).gISPostgreSQLConverterManager'></a>

`gISPostgreSQLConverterManager` [DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.gispostgresqlconvertermanager 'DiGi\.GIS\.PostgreSQL\.Classes\.GISPostgreSQLConverterManager')

The manager that handles conversion processes for GIS data within a PostgreSQL database context\.

<a name='DiGi.GIS.WebAPI.Classes.ParameterMinimumSchemaFilter'></a>

## ParameterMinimumSchemaFilter Class

Carries the floor a [MinimumAttribute](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MinimumAttribute 'DiGi\.GIS\.WebAPI\.Classes\.MinimumAttribute') declares on an action parameter into that parameter's OpenAPI schema, so the served document states the domain with a `minimum` and no `maximum` \(DiGi\-Organization/DiGi\.GIS\.WebAPI\#47\)\.

Swashbuckle maps only the DataAnnotations it knows, so a custom attribute is invisible to it; but the pass that generates a query parameter's schema runs every registered schema filter with the parameter in the context, which is where this one reads the floor from. Registered through the host's `IWebAPISchemaFilter` hook, the same way as [TableWireFormatSchemaFilter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TableWireFormatSchemaFilter 'DiGi\.GIS\.WebAPI\.Classes\.TableWireFormatSchemaFilter').

The floor is rendered through [System\.Globalization\.CultureInfo\.InvariantCulture](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.invariantculture 'System\.Globalization\.CultureInfo\.InvariantCulture') because an OpenAPI number keyword is an identity, not prose: a decimal separator following the serving machine's culture would describe a different bound to every reader.

```csharp
public class ParameterMinimumSchemaFilter : DiGi.WebAPI.Interfaces.IWebAPISchemaFilter, Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ParameterMinimumSchemaFilter

Implements [DiGi\.WebAPI\.Interfaces\.IWebAPISchemaFilter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.interfaces.iwebapischemafilter 'DiGi\.WebAPI\.Interfaces\.IWebAPISchemaFilter'), [Swashbuckle\.AspNetCore\.SwaggerGen\.ISchemaFilter](https://learn.microsoft.com/en-us/dotnet/api/swashbuckle.aspnetcore.swaggergen.ischemafilter 'Swashbuckle\.AspNetCore\.SwaggerGen\.ISchemaFilter')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.ParameterMinimumSchemaFilter.Apply(Microsoft.OpenApi.IOpenApiSchema,Swashbuckle.AspNetCore.SwaggerGen.SchemaFilterContext)'></a>

## ParameterMinimumSchemaFilter\.Apply\(IOpenApiSchema, SchemaFilterContext\) Method

Sets `minimum` on the schema of a parameter carrying [MinimumAttribute](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MinimumAttribute 'DiGi\.GIS\.WebAPI\.Classes\.MinimumAttribute') \- or `exclusiveMinimum` alone for an exclusive floor, which is how `Microsoft.OpenApi` carries a strict bound: setting `ExclusiveMinimum` clears `Minimum`, so the two never compete over the same edge\.

```csharp
public void Apply(Microsoft.OpenApi.IOpenApiSchema schema, Swashbuckle.AspNetCore.SwaggerGen.SchemaFilterContext context);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.ParameterMinimumSchemaFilter.Apply(Microsoft.OpenApi.IOpenApiSchema,Swashbuckle.AspNetCore.SwaggerGen.SchemaFilterContext).schema'></a>

`schema` [Microsoft\.OpenApi\.IOpenApiSchema](https://learn.microsoft.com/en-us/dotnet/api/microsoft.openapi.iopenapischema 'Microsoft\.OpenApi\.IOpenApiSchema')

The OpenAPI schema to be modified\.

<a name='DiGi.GIS.WebAPI.Classes.ParameterMinimumSchemaFilter.Apply(Microsoft.OpenApi.IOpenApiSchema,Swashbuckle.AspNetCore.SwaggerGen.SchemaFilterContext).context'></a>

`context` [Swashbuckle\.AspNetCore\.SwaggerGen\.SchemaFilterContext](https://learn.microsoft.com/en-us/dotnet/api/swashbuckle.aspnetcore.swaggergen.schemafiltercontext 'Swashbuckle\.AspNetCore\.SwaggerGen\.SchemaFilterContext')

The context containing information about the schema being filtered\.

Implements [Apply\(IOpenApiSchema, SchemaFilterContext\)](https://learn.microsoft.com/en-us/dotnet/api/swashbuckle.aspnetcore.swaggergen.ischemafilter.apply#swashbuckle-aspnetcore-swaggergen-ischemafilter-apply(microsoft-openapi-iopenapischema-swashbuckle-aspnetcore-swaggergen-schemafiltercontext) 'Swashbuckle\.AspNetCore\.SwaggerGen\.ISchemaFilter\.Apply\(Microsoft\.OpenApi\.IOpenApiSchema,Swashbuckle\.AspNetCore\.SwaggerGen\.SchemaFilterContext\)')

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions'></a>

## SerializableObjectsPostOptions Class

Represents the configuration options for posting serializable objects, extending the base post options functionality\.

```csharp
public class SerializableObjectsPostOptions : DiGi.WebAPI.Classes.PostOptions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.Core\.Classes\.SerializableOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableoptions 'DiGi\.Core\.Classes\.SerializableOptions') → [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions') → SerializableObjectsPostOptions
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions.SerializableObjectsPostOptions()'></a>

## SerializableObjectsPostOptions\(\) Constructor

Initializes a new instance of the SerializableObjectsPostOptions class\.

```csharp
public SerializableObjectsPostOptions();
```

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions.SerializableObjectsPostOptions(DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions)'></a>

## SerializableObjectsPostOptions\(SerializableObjectsPostOptions\) Constructor

Initializes a new instance of the SerializableObjectsPostOptions class using values from an existing instance\.

```csharp
public SerializableObjectsPostOptions(DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions? serializableObjectsPostOptions);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions.SerializableObjectsPostOptions(DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions).serializableObjectsPostOptions'></a>

`serializableObjectsPostOptions` [SerializableObjectsPostOptions](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostOptions')

The source options instance to copy values from, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') to use default settings\.

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions.SerializableObjectsPostOptions(System.Text.Json.Nodes.JsonObject)'></a>

## SerializableObjectsPostOptions\(JsonObject\) Constructor

Initializes a new instance of the SerializableObjectsPostOptions class using the specified JSON object\.

```csharp
public SerializableObjectsPostOptions(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions.SerializableObjectsPostOptions(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data used to initialize the options\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions.BatchMemorySize'></a>

## SerializableObjectsPostOptions\.BatchMemorySize Property

Gets or sets the memory size threshold in bytes used for processing batches of serializable objects\.

This bounds the uncompressed JSON of one request. The receiving controller materializes a batch several times over - request DOM, domain objects with full geometry, database rows each re-serialized to JSONB, then one batch command per row - so the server's peak allocation is a large multiple of this value. Kept small deliberately; raising it trades server memory for fewer requests.

```csharp
public int BatchMemorySize { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions.Key'></a>

## SerializableObjectsPostOptions\.Key Property

Gets or sets the API authorization key used for authenticating requests to protected write endpoints\.

```csharp
public string? Key { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_'></a>

## SerializableObjectsPostTask\<T\> Class

Represents a base class for background tasks that handle the posting of serializable GIS objects to the PostgreSQL database\.

```csharp
public abstract class SerializableObjectsPostTask<T> : DiGi.Core.Classes.ReportableBackgroundTask<long>, DiGi.GIS.WebAPI.Interfaces.IGISWebAPIObject, DiGi.Core.Interfaces.IObject
    where T : DiGi.Core.Interfaces.ISerializableObject
```
#### Type parameters

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_.T'></a>

`T`

The type of serializable object being posted, which must implement [DiGi\.Core\.Interfaces\.ISerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.interfaces.iserializableobject 'DiGi\.Core\.Interfaces\.ISerializableObject')\.

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → SerializableObjectsPostTask\<T\>

Derived  
↳ [AddressPointsPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AddressPointsPostTask 'DiGi\.GIS\.WebAPI\.Classes\.AddressPointsPostTask')  
↳ [AdministrativeAreal2DsPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.AdministrativeAreal2DsPostTask 'DiGi\.GIS\.WebAPI\.Classes\.AdministrativeAreal2DsPostTask')  
↳ [Building2DsPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.Building2DsPostTask 'DiGi\.GIS\.WebAPI\.Classes\.Building2DsPostTask')  
↳ [BuildingModelsPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingModelsPostTask 'DiGi\.GIS\.WebAPI\.Classes\.BuildingModelsPostTask')  
↳ [BuildingsPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingsPostTask 'DiGi\.GIS\.WebAPI\.Classes\.BuildingsPostTask')  
↳ [EPWFilesPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.EPWFilesPostTask 'DiGi\.GIS\.WebAPI\.Classes\.EPWFilesPostTask')  
↳ [OccupancyDatasPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OccupancyDatasPostTask 'DiGi\.GIS\.WebAPI\.Classes\.OccupancyDatasPostTask')  
↳ [OrtoDatasPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.OrtoDatasPostTask 'DiGi\.GIS\.WebAPI\.Classes\.OrtoDatasPostTask')  
↳ [YearBuiltDatasPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDatasPostTask')

Implements [DiGi\.GIS\.WebAPI\.Interfaces\.IGISWebAPIObject](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.webapi.interfaces.igiswebapiobject 'DiGi\.GIS\.WebAPI\.Interfaces\.IGISWebAPIObject'), [DiGi\.Core\.Interfaces\.IObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.interfaces.iobject 'DiGi\.Core\.Interfaces\.IObject')
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_.SerializableObjectsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## SerializableObjectsPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the SerializableObjectsPostTask class\.

```csharp
public SerializableObjectsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager gISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_.SerializableObjectsPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).gISWebAPIManager'></a>

`gISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The GIS PostgreSQL Web API manager used to perform database operations\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_.Key'></a>

## SerializableObjectsPostTask\<T\>\.Key Property

Gets or sets the API authorization key used for authenticating requests to protected write endpoints\.

```csharp
public string? Key { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_.SerializableObjectsPostOptions'></a>

## SerializableObjectsPostTask\<T\>\.SerializableObjectsPostOptions Property

Gets or sets the options used for posting serializable objects\.

```csharp
public DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions SerializableObjectsPostOptions { get; set; }
```

#### Property Value
[SerializableObjectsPostOptions](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostOptions')

<a name='DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_.Values'></a>

## SerializableObjectsPostTask\<T\>\.Values Property

Gets or sets the collection of serializable objects to be posted\.

```csharp
public System.Collections.Generic.IEnumerable<T>? Values { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_.T 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter'></a>

## SinglevalueAggregateRequestParameter Class

Parameter class containing options for single\-value database aggregation queries\.

```csharp
public class SinglevalueAggregateRequestParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → SinglevalueAggregateRequestParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter.SinglevalueAggregateRequestParameter()'></a>

## SinglevalueAggregateRequestParameter\(\) Constructor

Initializes a new instance of the [SinglevalueAggregateRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.SinglevalueAggregateRequestParameter') class\.

```csharp
public SinglevalueAggregateRequestParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter.SinglevalueAggregateRequestParameter(System.Text.Json.Nodes.JsonObject)'></a>

## SinglevalueAggregateRequestParameter\(JsonObject\) Constructor

Initializes a new instance of the [SinglevalueAggregateRequestParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter 'DiGi\.GIS\.WebAPI\.Classes\.SinglevalueAggregateRequestParameter') class using a [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public SinglevalueAggregateRequestParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter.SinglevalueAggregateRequestParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter.ColumnUniqueId'></a>

## SinglevalueAggregateRequestParameter\.ColumnUniqueId Property

Gets or sets the column unique identifier to calculate statistics for\.

```csharp
public string ColumnUniqueId { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
"col\_unique\_id\_123"

<a name='DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter.CountyId'></a>

## SinglevalueAggregateRequestParameter\.CountyId Property

Gets or sets the target partition identifier \(County ID\)\. If null, aggregation is performed across all partitions\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

### Example
10365

<a name='DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter.FilterGroup'></a>

## SinglevalueAggregateRequestParameter\.FilterGroup Property

Gets or sets the optional dynamic hierarchical filters to apply prior to aggregation\.

```csharp
public DiGi.PostgreSQL.Table.Classes.FilterGroup? FilterGroup { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

<a name='DiGi.GIS.WebAPI.Classes.SinglevalueAggregateRequestParameter.SinglevalueAggregateFunction'></a>

## SinglevalueAggregateRequestParameter\.SinglevalueAggregateFunction Property

Gets or sets the single\-value aggregation function to perform\.

```csharp
public DiGi.PostgreSQL.Table.Enums.SinglevalueAggregateFunction SinglevalueAggregateFunction { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Enums\.SinglevalueAggregateFunction](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.enums.singlevalueaggregatefunction 'DiGi\.PostgreSQL\.Table\.Enums\.SinglevalueAggregateFunction')

### Example
Sum

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatDocumentFilter'></a>

## TableWireFormatDocumentFilter Class

Removes the Core\.IO [DiGi\.Core\.IO\.Table\.Classes\.Row](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.table.classes.row 'DiGi\.Core\.IO\.Table\.Classes\.Row') component a generated document no longer references\.

Swashbuckle registers that component while it generates a table's schema - a Core.IO table is an `IEnumerable<Row>`, so the array items reference it - and [TableWireFormatSchemaFilter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TableWireFormatSchemaFilter 'DiGi\.GIS\.WebAPI\.Classes\.TableWireFormatSchemaFilter') then replaces those items with the `TableConverter` format, whose rows are positional value arrays (DiGi-Organization/DiGi.GIS.WebAPI#44). Nothing else can reference the Core.IO `Row`, so without this filter the served document would keep advertising a row object schema the wire never carries.

Registered through the host's `IWebAPIDocumentFilter` hook, after the host's own document filters, mirroring how `DiGi.WebAPI.WindowsService` removes its unreferenced enum components.

```csharp
public class TableWireFormatDocumentFilter : DiGi.WebAPI.Interfaces.IWebAPIDocumentFilter, Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TableWireFormatDocumentFilter

Implements [DiGi\.WebAPI\.Interfaces\.IWebAPIDocumentFilter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.interfaces.iwebapidocumentfilter 'DiGi\.WebAPI\.Interfaces\.IWebAPIDocumentFilter'), [Swashbuckle\.AspNetCore\.SwaggerGen\.IDocumentFilter](https://learn.microsoft.com/en-us/dotnet/api/swashbuckle.aspnetcore.swaggergen.idocumentfilter 'Swashbuckle\.AspNetCore\.SwaggerGen\.IDocumentFilter')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatDocumentFilter.Apply(Microsoft.OpenApi.OpenApiDocument,Swashbuckle.AspNetCore.SwaggerGen.DocumentFilterContext)'></a>

## TableWireFormatDocumentFilter\.Apply\(OpenApiDocument, DocumentFilterContext\) Method

Removes the document's Core\.IO `Row` component when nothing in the document references it\.

```csharp
public void Apply(Microsoft.OpenApi.OpenApiDocument swaggerDoc, Swashbuckle.AspNetCore.SwaggerGen.DocumentFilterContext context);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatDocumentFilter.Apply(Microsoft.OpenApi.OpenApiDocument,Swashbuckle.AspNetCore.SwaggerGen.DocumentFilterContext).swaggerDoc'></a>

`swaggerDoc` [Microsoft\.OpenApi\.OpenApiDocument](https://learn.microsoft.com/en-us/dotnet/api/microsoft.openapi.openapidocument 'Microsoft\.OpenApi\.OpenApiDocument')

The OpenAPI document to be modified\.

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatDocumentFilter.Apply(Microsoft.OpenApi.OpenApiDocument,Swashbuckle.AspNetCore.SwaggerGen.DocumentFilterContext).context'></a>

`context` [Swashbuckle\.AspNetCore\.SwaggerGen\.DocumentFilterContext](https://learn.microsoft.com/en-us/dotnet/api/swashbuckle.aspnetcore.swaggergen.documentfiltercontext 'Swashbuckle\.AspNetCore\.SwaggerGen\.DocumentFilterContext')

The context of the document being generated\.

Implements [Apply\(OpenApiDocument, DocumentFilterContext\)](https://learn.microsoft.com/en-us/dotnet/api/swashbuckle.aspnetcore.swaggergen.idocumentfilter.apply#swashbuckle-aspnetcore-swaggergen-idocumentfilter-apply(microsoft-openapi-openapidocument-swashbuckle-aspnetcore-swaggergen-documentfiltercontext) 'Swashbuckle\.AspNetCore\.SwaggerGen\.IDocumentFilter\.Apply\(Microsoft\.OpenApi\.OpenApiDocument,Swashbuckle\.AspNetCore\.SwaggerGen\.DocumentFilterContext\)')

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatDocumentFilter.SchemaReferenceVisitor'></a>

## TableWireFormatDocumentFilter\.SchemaReferenceVisitor Class

Collects the id of every schema component referenced in a walked OpenAPI document, the way `DiGi.WebAPI.WindowsService`'s own visitor does\.

Overrides `Visit(IOpenApiReferenceHolder)`: the walker reports a `$ref` there, while `Visit(IOpenApiSchema)` never sees one.

```csharp
private sealed class TableWireFormatDocumentFilter.SchemaReferenceVisitor : Microsoft.OpenApi.OpenApiVisitorBase
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.OpenApi\.OpenApiVisitorBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.openapi.openapivisitorbase 'Microsoft\.OpenApi\.OpenApiVisitorBase') → SchemaReferenceVisitor
### Properties

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatDocumentFilter.SchemaReferenceVisitor.Ids'></a>

## TableWireFormatDocumentFilter\.SchemaReferenceVisitor\.Ids Property

Gets the ids of the referenced schema components collected so far\.

```csharp
public System.Collections.Generic.HashSet<string> Ids { get; }
```

#### Property Value
[System\.Collections\.Generic\.HashSet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatDocumentFilter.SchemaReferenceVisitor.Visit(Microsoft.OpenApi.IOpenApiReferenceHolder)'></a>

## TableWireFormatDocumentFilter\.SchemaReferenceVisitor\.Visit\(IOpenApiReferenceHolder\) Method

Records the component id of a schema reference\.

```csharp
public override void Visit(Microsoft.OpenApi.IOpenApiReferenceHolder referenceHolder);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatDocumentFilter.SchemaReferenceVisitor.Visit(Microsoft.OpenApi.IOpenApiReferenceHolder).referenceHolder'></a>

`referenceHolder` [Microsoft\.OpenApi\.IOpenApiReferenceHolder](https://learn.microsoft.com/en-us/dotnet/api/microsoft.openapi.iopenapireferenceholder 'Microsoft\.OpenApi\.IOpenApiReferenceHolder')

The reference being visited\.

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatSchemaFilter'></a>

## TableWireFormatSchemaFilter Class

Rewrites the schema of a Core\.IO table payload so that it describes the format the DiGi `TableConverter` writes, not what the MVC JSON options would\.

Every `buildingdata/table*` operation answers with `Core.IO.Table.Convert.ToSystem_String<Table, Column, Row>`: a root object carrying exactly `Columns`, an array of DiGi-serialized column objects, and `Rows`, an array of positional value arrays - one value per column, in column order - with no root `_type` (DiGi-Organization/DiGi.GIS.WebAPI#44).

Registered through the host's `IWebAPISchemaFilter` hook, so it runs after `DiGi.WebAPI.WindowsService`'s `WireFormatSchemaFilter` and fully replaces whatever that filter produced for the type. A Core.IO table is not an `ISerializableObject`, so without this filter the host would describe the type's public members in camelCase, which matches neither writer.

The column items are an open schema requiring only `_type` instead of referencing a component: the document's `Column` schema id is already taken by `DiGi.PostgreSQL.Table.Classes.Column` (the `buildingdata/columns*` operations serve it), and a second component registration would conflict on Swashbuckle's default id.

```csharp
public class TableWireFormatSchemaFilter : DiGi.WebAPI.Interfaces.IWebAPISchemaFilter, Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TableWireFormatSchemaFilter

Implements [DiGi\.WebAPI\.Interfaces\.IWebAPISchemaFilter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.interfaces.iwebapischemafilter 'DiGi\.WebAPI\.Interfaces\.IWebAPISchemaFilter'), [Swashbuckle\.AspNetCore\.SwaggerGen\.ISchemaFilter](https://learn.microsoft.com/en-us/dotnet/api/swashbuckle.aspnetcore.swaggergen.ischemafilter 'Swashbuckle\.AspNetCore\.SwaggerGen\.ISchemaFilter')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatSchemaFilter.Apply(Microsoft.OpenApi.IOpenApiSchema,Swashbuckle.AspNetCore.SwaggerGen.SchemaFilterContext)'></a>

## TableWireFormatSchemaFilter\.Apply\(IOpenApiSchema, SchemaFilterContext\) Method

Replaces the schema of a Core\.IO table with the `TableConverter` wire format: an object requiring exactly `Columns` and `Rows`\.

```csharp
public void Apply(Microsoft.OpenApi.IOpenApiSchema schema, Swashbuckle.AspNetCore.SwaggerGen.SchemaFilterContext context);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatSchemaFilter.Apply(Microsoft.OpenApi.IOpenApiSchema,Swashbuckle.AspNetCore.SwaggerGen.SchemaFilterContext).schema'></a>

`schema` [Microsoft\.OpenApi\.IOpenApiSchema](https://learn.microsoft.com/en-us/dotnet/api/microsoft.openapi.iopenapischema 'Microsoft\.OpenApi\.IOpenApiSchema')

The OpenAPI schema to be modified\.

<a name='DiGi.GIS.WebAPI.Classes.TableWireFormatSchemaFilter.Apply(Microsoft.OpenApi.IOpenApiSchema,Swashbuckle.AspNetCore.SwaggerGen.SchemaFilterContext).context'></a>

`context` [Swashbuckle\.AspNetCore\.SwaggerGen\.SchemaFilterContext](https://learn.microsoft.com/en-us/dotnet/api/swashbuckle.aspnetcore.swaggergen.schemafiltercontext 'Swashbuckle\.AspNetCore\.SwaggerGen\.SchemaFilterContext')

The context containing information about the schema being filtered\.

Implements [Apply\(IOpenApiSchema, SchemaFilterContext\)](https://learn.microsoft.com/en-us/dotnet/api/swashbuckle.aspnetcore.swaggergen.ischemafilter.apply#swashbuckle-aspnetcore-swaggergen-ischemafilter-apply(microsoft-openapi-iopenapischema-swashbuckle-aspnetcore-swaggergen-schemafiltercontext) 'Swashbuckle\.AspNetCore\.SwaggerGen\.ISchemaFilter\.Apply\(Microsoft\.OpenApi\.IOpenApiSchema,Swashbuckle\.AspNetCore\.SwaggerGen\.SchemaFilterContext\)')

<a name='DiGi.GIS.WebAPI.Classes.TerrainController'></a>

## TerrainController Class

Controller responsible for handling API requests related to terrain, reconstructing a ground surface mesh from the stored elevation points of the counties a request covers\.

Every mesh returned here is a two-and-a-half dimensional height field: exactly one elevation per plan position. It models ground, and cannot express a vertical face, an overhang or a canopy.

```csharp
public class TerrainController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → TerrainController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TerrainController(DiGi.GIS.PostgreSQL.Classes.TerrainPointPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## TerrainController\(TerrainPointPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the [TerrainController](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TerrainController 'DiGi\.GIS\.WebAPI\.Classes\.TerrainController') class\.

```csharp
public TerrainController(DiGi.GIS.PostgreSQL.Classes.TerrainPointPostgreSQLConverter terrainPointPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TerrainController(DiGi.GIS.PostgreSQL.Classes.TerrainPointPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).terrainPointPostgreSQLConverter'></a>

`terrainPointPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.terrainpointpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointPostgreSQLConverter')

The converter used for reading terrain points from the PostgreSQL database\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TerrainController(DiGi.GIS.PostgreSQL.Classes.TerrainPointPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter used for resolving which counties an area covers\.
### Fields

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.tileSize'></a>

## TerrainController\.tileSize Field

The edge of one work tile of a coverage walk, counted in lattice steps\.

The default the sampling task writes with, so a coverage tile lines up with a sampled tile and a shortfall is reported against the same batches the run worked in.

```csharp
private const int tileSize = 128;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.CountyIdsAsync(DiGi.Geometry.Planar.Classes.BoundingBox2D,double,System.Threading.CancellationToken)'></a>

## TerrainController\.CountyIdsAsync\(BoundingBox2D, double, CancellationToken\) Method

Resolves which county partitions an area covers\.

This has to happen here rather than inside the terrain converter. The terrain points live in the Storage database and the administrative geometry in Main, and a PostgreSQL connection cannot reach across databases - so the only place both are available is the host, where each converter carries its own connection. The write side has always worked this way; see `PostgreSQLTerrainPointCreateTableTask`.

```csharp
private System.Threading.Tasks.Task<System.Collections.Generic.HashSet<int>?> CountyIdsAsync(DiGi.Geometry.Planar.Classes.BoundingBox2D boundingBox2D, double tolerance, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.CountyIdsAsync(DiGi.Geometry.Planar.Classes.BoundingBox2D,double,System.Threading.CancellationToken).boundingBox2D'></a>

`boundingBox2D` [DiGi\.Geometry\.Planar\.Classes\.BoundingBox2D](https://learn.microsoft.com/en-us/dotnet/api/digi.geometry.planar.classes.boundingbox2d 'DiGi\.Geometry\.Planar\.Classes\.BoundingBox2D')

The area to resolve\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.CountyIdsAsync(DiGi.Geometry.Planar.Classes.BoundingBox2D,double,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The distance the search area is expanded by, in metres\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.CountyIdsAsync(DiGi.Geometry.Planar.Classes.BoundingBox2D,double,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.HashSet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The identifiers of the counties the area meets, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when it meets none\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken)'></a>

## TerrainController\.GetCountByCountyIdAsync\(int, bool, bool, int, CancellationToken\) Method

Asynchronously retrieves the number of terrain points stored for one county partition\.

The cheapest question that can be asked of the store, and the one that separates a county that was never sampled from one that was sampled and holds nothing.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountByCountyIdAsync(int countyId, bool estimated=false, bool analyze=false, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county partition to count\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).estimated'></a>

`estimated` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Reads the planner's row estimate instead of counting the rows\. Far faster on a partition of millions and accurate to a few percent, but it reflects the last time the partition was analysed rather than this moment\. An unanalysed partition returns 204 NoContent\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).analyze'></a>

`analyze` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to perform an ANALYZE operation before reading the estimate to ensure statistics are current\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the count, 204 NoContent when the partition exists but is unanalysed, or 404 NotFound when the county has no partition\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken)'></a>

## TerrainController\.GetCoverageByCountyIdAsync\(int, double, double, double, Nullable\<double\>, int, int, CancellationToken\) Method

Asynchronously compares what one county partition holds against what a sampling run on the given lattice should have put there\.

The question a density cannot answer. A density says how much of a county is missing; this says which nodes, so a run that stepped over a batch can be sent back for exactly those.

The expected nodes are derived from the same subdivision outlines and the same lattice the sampling run itself decides against, so the two agree by construction. Nodes of the county's bounding rectangle that fall outside its land are not expected and not counted.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCoverageByCountyIdAsync(int countyId, double gridSize, double originX, double originY, System.Nullable<double> tolerance, int limit=1000, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county partition to measure\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).gridSize'></a>

`gridSize` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The lattice spacing, in metres\. Not finer than [MinimumGridSize](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MinimumGridSize 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MinimumGridSize')\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).originX'></a>

`originX` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate the lattice is anchored at\. Leave at zero unless a run used something else\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).originY'></a>

`originY` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate the lattice is anchored at\. Leave at zero unless a run used something else\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The distance a stored point may lie from a node and still be counted as that node, in metres\. Capped at half a step\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The largest number of missing coordinates returned\. The count itself is reported in full regardless\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the [DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointCoverageResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.terrainpointcoverageresult 'DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointCoverageResult') as JSON, or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetDensitiesByCountyIdsAsync(System.Collections.Generic.List_int_,System.Nullable_double_,int,System.Threading.CancellationToken)'></a>

## TerrainController\.GetDensitiesByCountyIdsAsync\(List\<int\>, Nullable\<double\>, int, CancellationToken\) Method

Asynchronously reports how densely each of the named county partitions is sampled: the points it holds divided by the area of its subdivisions\.

The cheap sweep. It costs one aggregate per partition and the outlines of the counties named, where deciding the same question node by node costs the generating and the looking up of the whole lattice - so this is what narrows a country down to the few counties worth [GetCoverageByCountyIdAsync\(int, double, double, double, Nullable&lt;double&gt;, int, int, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.TerrainController\.GetCoverageByCountyIdAsync\(int, double, double, double, System\.Nullable\<double\>, int, int, System\.Threading\.CancellationToken\)').

Supplying [gridSize](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TerrainController.GetDensitiesByCountyIdsAsync(System.Collections.Generic.List_int_,System.Nullable_double_,int,System.Threading.CancellationToken).gridSize 'DiGi\.GIS\.WebAPI\.Classes\.TerrainController\.GetDensitiesByCountyIdsAsync\(System\.Collections\.Generic\.List\<int\>, System\.Nullable\<double\>, int, System\.Threading\.CancellationToken\)\.gridSize') is what turns the density into a completeness. Without it the figure to read is the spacing, which needs no knowledge of what a run was asked for.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetDensitiesByCountyIdsAsync(System.Collections.Generic.List<int>? countyIds, System.Nullable<double> gridSize, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetDensitiesByCountyIdsAsync(System.Collections.Generic.List_int_,System.Nullable_double_,int,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

The identifiers of the county partitions to measure, repeated once per county\. At least one and at most [MaximumDensityCountyCount](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MaximumDensityCountyCount 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MaximumDensityCountyCount')\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetDensitiesByCountyIdsAsync(System.Collections.Generic.List_int_,System.Nullable_double_,int,System.Threading.CancellationToken).gridSize'></a>

`gridSize` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The lattice spacing a sampling run used, in metres, when it is known\. Strictly greater than zero when supplied\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetDensitiesByCountyIdsAsync(System.Collections.Generic.List_int_,System.Nullable_double_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetDensitiesByCountyIdsAsync(System.Collections.Generic.List_int_,System.Nullable_double_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the densities as JSON, or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken)'></a>

## TerrainController\.GetGapsByBoundingBoxAsync\(double, double, double, double, double, double, double, Nullable\<double\>, int, int, CancellationToken\) Method

Asynchronously retrieves the lattice nodes inside a rectangle that lie on a county's land and that the terrain point table holds no point for\.

Where [GetCoverageByCountyIdAsync\(int, double, double, double, Nullable&lt;double&gt;, int, int, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TerrainController.GetCoverageByCountyIdAsync(int,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.TerrainController\.GetCoverageByCountyIdAsync\(int, double, double, double, System\.Nullable\<double\>, int, int, System\.Threading\.CancellationToken\)') answers for a whole county, this answers for an area - which is what a coverage reporting a shortfall is followed by. Every county the rectangle meets is measured, so a hole spanning a county boundary is reported once and whole.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetGapsByBoundingBoxAsync(double x_1, double y_1, double x_2, double y_2, double gridSize, double originX, double originY, System.Nullable<double> tolerance, int limit=1000, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).x_1'></a>

`x_1` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate of the first corner, in PL\-1992 \(EPSG:2180\) metres\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).y_1'></a>

`y_1` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate of the first corner, in PL\-1992 \(EPSG:2180\) metres\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).x_2'></a>

`x_2` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate of the second corner, in PL\-1992 \(EPSG:2180\) metres\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).y_2'></a>

`y_2` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate of the second corner, in PL\-1992 \(EPSG:2180\) metres\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).gridSize'></a>

`gridSize` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The lattice spacing, in metres\. Not finer than [MinimumGridSize](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MinimumGridSize 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MinimumGridSize')\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).originX'></a>

`originX` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate the lattice is anchored at\. Leave at zero unless a run used something else\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).originY'></a>

`originY` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate the lattice is anchored at\. Leave at zero unless a run used something else\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The distance a stored point may lie from a node and still be counted as that node, in metres\. Capped at half a step\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The largest number of missing coordinates returned\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetGapsByBoundingBoxAsync(double,double,double,double,double,double,double,System.Nullable_double_,int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the missing coordinates as JSON, or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken)'></a>

## TerrainController\.GetMesh3DByBoundingBoxAsync\(double, double, double, double, Nullable\<double\>, CancellationToken\) Method

Asynchronously retrieves the terrain surface inside an axis aligned bounding box given by two opposite corners\.

Corner order does not matter. Each side of the box is capped at twice [MaximumRadius](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MaximumRadius 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MaximumRadius'), so this endpoint and the circle admit the same largest area.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetMesh3DByBoundingBoxAsync(double x_1, double y_1, double x_2, double y_2, System.Nullable<double> tolerance, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).x_1'></a>

`x_1` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate of the first corner, in PL\-1992 \(EPSG:2180\) metres\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).y_1'></a>

`y_1` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate of the first corner, in PL\-1992 \(EPSG:2180\) metres\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).x_2'></a>

`x_2` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate of the second corner, in PL\-1992 \(EPSG:2180\) metres\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).y_2'></a>

`y_2` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate of the second corner, in PL\-1992 \(EPSG:2180\) metres\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional tolerance for the spatial query, in metres\. If not provided or NaN, a default macro distance is used\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByBoundingBoxAsync(double,double,double,double,System.Nullable_double_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the [DiGi\.Geometry\.Spatial\.Classes\.Mesh3D](https://learn.microsoft.com/en-us/dotnet/api/digi.geometry.spatial.classes.mesh3d 'DiGi\.Geometry\.Spatial\.Classes\.Mesh3D') as JSON, or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken)'></a>

## TerrainController\.GetMesh3DByCircleAsync\(double, double, Nullable\<double\>, Nullable\<double\>, Nullable\<double\>, CancellationToken\) Method

Asynchronously retrieves the terrain surface inside a circle centred on the given plan coordinate\.

The circle is honoured: the points outside it are excluded by the database, not trimmed afterwards, so no part of the returned mesh lies further from the centre than the radius.

Either [radius](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).radius 'DiGi\.GIS\.WebAPI\.Classes\.TerrainController\.GetMesh3DByCircleAsync\(double, double, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Threading\.CancellationToken\)\.radius') or [diameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).diameter 'DiGi\.GIS\.WebAPI\.Classes\.TerrainController\.GetMesh3DByCircleAsync\(double, double, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Threading\.CancellationToken\)\.diameter') must be supplied; [radius](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).radius 'DiGi\.GIS\.WebAPI\.Classes\.TerrainController\.GetMesh3DByCircleAsync\(double, double, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Threading\.CancellationToken\)\.radius') wins when both are. The radius is capped by [MaximumRadius](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MaximumRadius 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MaximumRadius').

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetMesh3DByCircleAsync(double x, double y, System.Nullable<double> radius, System.Nullable<double> diameter, System.Nullable<double> tolerance, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).x'></a>

`x` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate of the centre, in PL\-1992 \(EPSG:2180\) metres, matching the coordinates the terrain points are stored in\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).y'></a>

`y` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate of the centre, in PL\-1992 \(EPSG:2180\) metres, matching the coordinates the terrain points are stored in\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).radius'></a>

`radius` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The search radius in metres\. Optional when [diameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).diameter 'DiGi\.GIS\.WebAPI\.Classes\.TerrainController\.GetMesh3DByCircleAsync\(double, double, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Threading\.CancellationToken\)\.diameter') is supplied\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).diameter'></a>

`diameter` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The search diameter in metres, used only when [radius](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).radius 'DiGi\.GIS\.WebAPI\.Classes\.TerrainController\.GetMesh3DByCircleAsync\(double, double, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Nullable\<double\>, System\.Threading\.CancellationToken\)\.radius') is absent\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional tolerance for the spatial query, in metres\. If not provided or NaN, a default macro distance is used\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetMesh3DByCircleAsync(double,double,System.Nullable_double_,System.Nullable_double_,System.Nullable_double_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the [DiGi\.Geometry\.Spatial\.Classes\.Mesh3D](https://learn.microsoft.com/en-us/dotnet/api/digi.geometry.spatial.classes.mesh3d 'DiGi\.Geometry\.Spatial\.Classes\.Mesh3D') as JSON, or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken)'></a>

## TerrainController\.GetSummariesByCountyIdsAsync\(List\<int\>, int, CancellationToken\) Method

Asynchronously summarises what each of the named county partitions holds: how many points, over what extent, at what elevations, filed under how many subdivisions, and when they were written\.

The account a sampling run leaves behind. The run keeps its tallies in memory and discards them when it ends, so this is what remains to read afterwards - and ordering the result by [DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointCountyResult\.CreatedAt\_First](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.terrainpointcountyresult.createdat_first 'DiGi\.GIS\.PostgreSQL\.Classes\.TerrainPointCountyResult\.CreatedAt\_First') reconstructs how far a run got, because a run walks the counties in ascending identifier order.

Naming no county summarises every partition. Counties holding no point are absent from the result rather than present with a zero.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetSummariesByCountyIdsAsync(System.Collections.Generic.List<int>? countyIds, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

The identifiers of the county partitions to summarise, repeated once per county\. Omit to summarise every one\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.GetSummariesByCountyIdsAsync(System.Collections.Generic.List_int_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the summaries as JSON, or an error status\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.IsFinite(double)'></a>

## TerrainController\.IsFinite\(double\) Method

Determines whether a bound query string value is a usable coordinate or distance\.

Model binding accepts the literals NaN and Infinity for a double, so neither is a value the caller could only have reached by mistake.

```csharp
private static bool IsFinite(double value);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.IsFinite(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The value to test\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the value is neither NaN nor infinite; otherwise [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.Mesh3DResult(DiGi.Geometry.PointCloud.Spatial.Classes.PointCloud3D)'></a>

## TerrainController\.Mesh3DResult\(PointCloud3D\) Method

Reconstructs the ground surface from the gathered terrain points and renders it as the JSON response body\.

The three failure paths are reported separately, because "nothing stored here" and "too little stored here to triangulate" are answered by different fixes and both used to arrive as a bare 404.

```csharp
private Microsoft.AspNetCore.Mvc.IActionResult Mesh3DResult(DiGi.Geometry.PointCloud.Spatial.Classes.PointCloud3D? pointCloud3D);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.Mesh3DResult(DiGi.Geometry.PointCloud.Spatial.Classes.PointCloud3D).pointCloud3D'></a>

`pointCloud3D` [DiGi\.Geometry\.PointCloud\.Spatial\.Classes\.PointCloud3D](https://learn.microsoft.com/en-us/dotnet/api/digi.geometry.pointcloud.spatial.classes.pointcloud3d 'DiGi\.Geometry\.PointCloud\.Spatial\.Classes\.PointCloud3D')

The terrain points gathered for the requested area\.

#### Returns
[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the [DiGi\.Geometry\.Spatial\.Classes\.Mesh3D](https://learn.microsoft.com/en-us/dotnet/api/digi.geometry.spatial.classes.mesh3d 'DiGi\.Geometry\.Spatial\.Classes\.Mesh3D') as JSON, or a not found status\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.PolygonalFace2DsByIdAsync(int,System.Threading.CancellationToken)'></a>

## TerrainController\.PolygonalFace2DsByIdAsync\(int, CancellationToken\) Method

Reads the outlines of a county's subdivisions, keyed by subdivision identifier\.

Read from the administrative database rather than the terrain one, and derived through the same helper the sampling task uses, so an area measured here is the area a run would have sampled.

```csharp
private System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int,DiGi.Geometry.Planar.Classes.PolygonalFace2D>?> PolygonalFace2DsByIdAsync(int countyId, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.PolygonalFace2DsByIdAsync(int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county whose subdivisions are wanted\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.PolygonalFace2DsByIdAsync(int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A cancellation token that can be used by the caller to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[DiGi\.Geometry\.Planar\.Classes\.PolygonalFace2D](https://learn.microsoft.com/en-us/dotnet/api/digi.geometry.planar.classes.polygonalface2d 'DiGi\.Geometry\.Planar\.Classes\.PolygonalFace2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The outlines keyed by subdivision identifier, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when the county has none\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TerrainPointTableExistsAsync()'></a>

## TerrainController\.TerrainPointTableExistsAsync\(\) Method

Reports whether anything has ever been written to the terrain point store\.

Asked once per request, before a walk that would otherwise send a query per tile against a table
            that does not exist. An undefined relation reaches a caller as a server fault, where the plain fact is
            that nothing has been sampled yet - which is an answer, and one a fresh deployment gives.

```csharp
private System.Threading.Tasks.Task<bool> TerrainPointTableExistsAsync();
```

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the terrain point table exists; otherwise [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetLatticeParameters(double,double,double,System.Nullable_double_,int,DiGi.Geometry.Planar.Classes.Point2D,double)'></a>

## TerrainController\.TryGetLatticeParameters\(double, double, double, Nullable\<double\>, int, Point2D, double\) Method

Resolves and checks the lattice a coverage or gap request asked to be measured against\.

```csharp
private static bool TryGetLatticeParameters(double gridSize, double originX, double originY, System.Nullable<double> tolerance, int limit, out DiGi.Geometry.Planar.Classes.Point2D? origin, out double tolerance_Temp);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetLatticeParameters(double,double,double,System.Nullable_double_,int,DiGi.Geometry.Planar.Classes.Point2D,double).gridSize'></a>

`gridSize` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The lattice spacing as bound from the query string\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetLatticeParameters(double,double,double,System.Nullable_double_,int,DiGi.Geometry.Planar.Classes.Point2D,double).originX'></a>

`originX` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The X coordinate the lattice is anchored at\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetLatticeParameters(double,double,double,System.Nullable_double_,int,DiGi.Geometry.Planar.Classes.Point2D,double).originY'></a>

`originY` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The Y coordinate the lattice is anchored at\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetLatticeParameters(double,double,double,System.Nullable_double_,int,DiGi.Geometry.Planar.Classes.Point2D,double).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The tolerance as bound from the query string\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetLatticeParameters(double,double,double,System.Nullable_double_,int,DiGi.Geometry.Planar.Classes.Point2D,double).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The largest number of coordinates the request asked to be returned\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetLatticeParameters(double,double,double,System.Nullable_double_,int,DiGi.Geometry.Planar.Classes.Point2D,double).origin'></a>

`origin` [DiGi\.Geometry\.Planar\.Classes\.Point2D](https://learn.microsoft.com/en-us/dotnet/api/digi.geometry.planar.classes.point2d 'DiGi\.Geometry\.Planar\.Classes\.Point2D')

The resolved anchor of the lattice\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetLatticeParameters(double,double,double,System.Nullable_double_,int,DiGi.Geometry.Planar.Classes.Point2D,double).tolerance_Temp'></a>

`tolerance_Temp` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The resolved tolerance, capped at half a step so that a point can never be taken for a node of the neighbouring cell\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the lattice is usable; otherwise [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetTolerance(System.Nullable_double_,double)'></a>

## TerrainController\.TryGetTolerance\(Nullable\<double\>, double\) Method

Resolves the tolerance a request asked for, falling back to the default when it was not supplied\.

```csharp
private static bool TryGetTolerance(System.Nullable<double> tolerance, out double tolerance_Temp);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetTolerance(System.Nullable_double_,double).tolerance'></a>

`tolerance` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The tolerance as bound from the query string\.

<a name='DiGi.GIS.WebAPI.Classes.TerrainController.TryGetTolerance(System.Nullable_double_,double).tolerance_Temp'></a>

`tolerance_Temp` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The resolved tolerance, in metres\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the tolerance is usable; otherwise [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

<a name='DiGi.GIS.WebAPI.Classes.TransientDatabaseFailureAttribute'></a>

## TransientDatabaseFailureAttribute Class

Answers a transient database failure that escapes an action with HTTP 503 and `Retry-After: 30` \- the "retry can succeed" response the hand\-written `catch (NpgsqlException exception) when (exception.IsTransient)` blocks give \- instead of the host's uniform 500 \(DiGi\-Organization/DiGi\.GIS\.WebAPI\#71\)\.

Applied at class level to every GIS controller, so an action that has no catch of its own, and every action added later, keeps the contract. An action with a generic `catch (Exception)` must let the transient failure through (`when (exception is not NpgsqlException { IsTransient: true })`) for this filter to see it. Any other exception is left unhandled, so the host still answers 500.

A failure counts as transient when the exception or one of its inner exceptions is an [Npgsql\.NpgsqlException](https://learn.microsoft.com/en-us/dotnet/api/npgsql.npgsqlexception 'Npgsql\.NpgsqlException') with [Npgsql\.NpgsqlException\.IsTransient](https://learn.microsoft.com/en-us/dotnet/api/npgsql.npgsqlexception.istransient 'Npgsql\.NpgsqlException\.IsTransient') set - a refused or dropped connection, a timeout, or a server that is starting up, in recovery or out of connection slots.

```csharp
public sealed class TransientDatabaseFailureAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Attribute](https://learn.microsoft.com/en-us/dotnet/api/system.attribute 'System\.Attribute') → [Microsoft\.AspNetCore\.Mvc\.Filters\.ExceptionFilterAttribute](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.filters.exceptionfilterattribute 'Microsoft\.AspNetCore\.Mvc\.Filters\.ExceptionFilterAttribute') → TransientDatabaseFailureAttribute
### Fields

<a name='DiGi.GIS.WebAPI.Classes.TransientDatabaseFailureAttribute.Message'></a>

## TransientDatabaseFailureAttribute\.Message Field

The response body, matching the hand\-written blocks\.

```csharp
public const string Message = "Database temporarily unavailable; retry shortly";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.TransientDatabaseFailureAttribute.RetryAfter'></a>

## TransientDatabaseFailureAttribute\.RetryAfter Field

The seconds a client is told to wait before retrying, matching the hand\-written blocks\.

```csharp
public const string RetryAfter = "30";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Methods

<a name='DiGi.GIS.WebAPI.Classes.TransientDatabaseFailureAttribute.IsTransient(System.Exception)'></a>

## TransientDatabaseFailureAttribute\.IsTransient\(Exception\) Method

Checks whether an exception, or one of its inner exceptions, is a transient [Npgsql\.NpgsqlException](https://learn.microsoft.com/en-us/dotnet/api/npgsql.npgsqlexception 'Npgsql\.NpgsqlException')\.

```csharp
public static bool IsTransient(System.Exception? exception);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.TransientDatabaseFailureAttribute.IsTransient(System.Exception).exception'></a>

`exception` [System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')

The exception to check\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when a retry can succeed; otherwise [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter'></a>

## UniqueValuesByColumnUniqueIdParameter Class

Represents a parameter containing column unique id for querying unique values\.

```csharp
public class UniqueValuesByColumnUniqueIdParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → UniqueValuesByColumnUniqueIdParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter.UniqueValuesByColumnUniqueIdParameter()'></a>

## UniqueValuesByColumnUniqueIdParameter\(\) Constructor

Initializes a new instance of the [UniqueValuesByColumnUniqueIdParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter 'DiGi\.GIS\.WebAPI\.Classes\.UniqueValuesByColumnUniqueIdParameter') class\.

```csharp
public UniqueValuesByColumnUniqueIdParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter.UniqueValuesByColumnUniqueIdParameter(string,System.Nullable_int_)'></a>

## UniqueValuesByColumnUniqueIdParameter\(string, Nullable\<int\>\) Constructor

Initializes a new instance of the [UniqueValuesByColumnUniqueIdParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter 'DiGi\.GIS\.WebAPI\.Classes\.UniqueValuesByColumnUniqueIdParameter') class with the specified column unique id and county id\.

```csharp
public UniqueValuesByColumnUniqueIdParameter(string columnUniqueId, System.Nullable<int> countyId);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter.UniqueValuesByColumnUniqueIdParameter(string,System.Nullable_int_).columnUniqueId'></a>

`columnUniqueId` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique identifier of the column\.

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter.UniqueValuesByColumnUniqueIdParameter(string,System.Nullable_int_).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional unique identifier of the county\.

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter.UniqueValuesByColumnUniqueIdParameter(System.Text.Json.Nodes.JsonObject)'></a>

## UniqueValuesByColumnUniqueIdParameter\(JsonObject\) Constructor

Initializes a new instance of the [UniqueValuesByColumnUniqueIdParameter](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter 'DiGi\.GIS\.WebAPI\.Classes\.UniqueValuesByColumnUniqueIdParameter') class using an [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') object\.

```csharp
public UniqueValuesByColumnUniqueIdParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter.UniqueValuesByColumnUniqueIdParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter.ColumnUniqueId'></a>

## UniqueValuesByColumnUniqueIdParameter\.ColumnUniqueId Property

Gets or sets the unique identifier of the column \(Column\.UniqueId\)\.

```csharp
public string ColumnUniqueId { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter.CountyId'></a>

## UniqueValuesByColumnUniqueIdParameter\.CountyId Property

Gets or sets the county identifier\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.UniqueValuesByColumnUniqueIdParameter.FilterGroup'></a>

## UniqueValuesByColumnUniqueIdParameter\.FilterGroup Property

Gets or sets the optional dynamic hierarchical filters to apply prior to retrieving unique values\.

```csharp
public DiGi.PostgreSQL.Table.Classes.FilterGroup? FilterGroup { get; set; }
```

#### Property Value
[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

<a name='DiGi.GIS.WebAPI.Classes.UnitController'></a>

## UnitController Class

Controller responsible for handling API requests related to BDL territorial units, statistical units, matching, and data compliance in PostgreSQL\.

```csharp
public class UnitController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → UnitController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.UnitController.UnitController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.UnitPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter)'></a>

## UnitController\(GISWebAPIConfigurationFileWatcher, UnitPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter\) Constructor

Initializes a new instance of the [UnitController](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UnitController 'DiGi\.GIS\.WebAPI\.Classes\.UnitController') class\.

```csharp
public UnitController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.UnitPostgreSQLConverter unitPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UnitController.UnitController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.UnitPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher for GIS Web API settings\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.UnitController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.UnitPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).unitPostgreSQLConverter'></a>

`unitPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.UnitPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.unitpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.UnitPostgreSQLConverter')

The converter used for handling territorial unit operations\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.UnitController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.UnitPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter used for administrative area operations\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetComplianceAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken)'></a>

## UnitController\.GetComplianceAsync\(Nullable\<AdministrativeArealType\>, CancellationToken\) Method

Asynchronously evaluates and returns data compliance of administrative areas against BDL statistical units for the specified administrative area type\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetComplianceAsync(System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetComplianceAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The administrative area type to evaluate \(e\.g\. County, Municipality, Voivodeship\)\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetComplianceAsync(System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The data compliance report containing totals, matched counts, compliance percentage, and unmapped entities\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetCountsAsync(System.Threading.CancellationToken)'></a>

## UnitController\.GetCountsAsync\(CancellationToken\) Method

Asynchronously retrieves the distribution of territorial units grouped by level\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountsAsync(System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetCountsAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A dictionary mapping hierarchy level to unit count\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetItemByIdAsync(string,System.Threading.CancellationToken)'></a>

## UnitController\.GetItemByIdAsync\(string, CancellationToken\) Method

Asynchronously retrieves a territorial unit by its unique 12\-digit identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemByIdAsync(string? id, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetItemByIdAsync(string,System.Threading.CancellationToken).id'></a>

`id` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique identifier of the unit\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetItemByIdAsync(string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The territorial unit if found; otherwise, 404 Not Found\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetItemsAsync(System.Nullable_short_,System.Threading.CancellationToken)'></a>

## UnitController\.GetItemsAsync\(Nullable\<short\>, CancellationToken\) Method

Asynchronously retrieves territorial units, optionally filtered by level\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsAsync(System.Nullable<short> level=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetItemsAsync(System.Nullable_short_,System.Threading.CancellationToken).level'></a>

`level` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional level filter \(0=country, 1=macroregion, 2=voivodeship, etc\.\)\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetItemsAsync(System.Nullable_short_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The collection of territorial units\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetMatchAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## UnitController\.GetMatchAsync\(string, Nullable\<AdministrativeArealType\>, Nullable\<int\>, CancellationToken\) Method

Asynchronously finds the matching [DiGi\.GIS\.Classes\.StatisticalUnit](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.statisticalunit 'DiGi\.GIS\.Classes\.StatisticalUnit') for an administrative area by its code and type, or by its integer identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetMatchAsync(string? code=null, System.Nullable<DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType> administrativeArealType=null, System.Nullable<int> id=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetMatchAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique administrative code\. Either \(code and administrativearealtype\) or id must be supplied\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetMatchAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Threading.CancellationToken).administrativeArealType'></a>

`administrativeArealType` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.administrativearealtype 'DiGi\.GIS\.PostgreSQL\.Enums\.AdministrativeArealType')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The administrative area type\. Either \(code and administrativearealtype\) or id must be supplied\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetMatchAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Threading.CancellationToken).id'></a>

`id` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The integer identifier of the administrative area in PostgreSQL\. Either \(code and administrativearealtype\) or id must be supplied\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetMatchAsync(string,System.Nullable_DiGi.GIS.PostgreSQL.Enums.AdministrativeArealType_,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The matched statistical unit if found; otherwise, 404 Not Found\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetStatisticalUnitAsync(System.Threading.CancellationToken)'></a>

## UnitController\.GetStatisticalUnitAsync\(CancellationToken\) Method

Asynchronously constructs and retrieves the root [DiGi\.GIS\.Classes\.StatisticalUnit](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.statisticalunit 'DiGi\.GIS\.Classes\.StatisticalUnit') hierarchy from all stored units\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetStatisticalUnitAsync(System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UnitController.GetStatisticalUnitAsync(System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
The root statistical unit tree\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken)'></a>

## UnitController\.UpdateItemsAsync\(JsonArray, string, CancellationToken\) Method

Asynchronously updates or inserts a collection of territorial units\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.UnitController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the territorial units to update or insert\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.UnitController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A list of updated or inserted unit identifiers\.

<a name='DiGi.GIS.WebAPI.Classes.UpdateItemsResult'></a>

## UpdateItemsResult Class

The outcome of a write endpoint: how much was stored, and which rows were not\.

A partial write still answers 200 - the rows that resolved really were stored, and failing the whole batch would discard them - but it is no longer silent. [Rejected](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejected 'DiGi\.GIS\.WebAPI\.Classes\.UpdateItemsResult\.Rejected') names what did not reach the database so the caller can correct and repost it, or report it onwards.

```csharp
public class UpdateItemsResult
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → UpdateItemsResult
### Properties

<a name='DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejected'></a>

## UpdateItemsResult\.Rejected Property

Gets or sets the rows dropped before the database, each named with the reason it was dropped\.

```csharp
public System.Collections.Generic.List<DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection> Rejected { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[Rejection](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection 'DiGi\.GIS\.WebAPI\.Classes\.UpdateItemsResult\.Rejection')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

<a name='DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Sent'></a>

## UpdateItemsResult\.Sent Property

Gets or sets the number of rows handed to the database\.

```csharp
public int Sent { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

### Example
5000

<a name='DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Updated'></a>

## UpdateItemsResult\.Updated Property

Gets or sets the number of distinct identifiers the database returned\.

Not a row count. Identifiers arrive as a set, and rows of one batch colliding on the conflict key return the same identifier, so this can be lower than the number stored. [Rejected](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejected 'DiGi\.GIS\.WebAPI\.Classes\.UpdateItemsResult\.Rejected') is the exact account of what was lost.

```csharp
public int Updated { get; set; }
```

#### Property Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

### Example
4987

<a name='DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection'></a>

## UpdateItemsResult\.Rejection Class

One row that was dropped before the database, and why\.

```csharp
public class UpdateItemsResult.Rejection
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Rejection
### Properties

<a name='DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection.Reason'></a>

## UpdateItemsResult\.Rejection\.Reason Property

Gets or sets the reason the row was dropped\. It decides whether reposting is worth anything: a payload defect is worth correcting, a footprint outside every candidate county part is not\.

```csharp
public DiGi.GIS.PostgreSQL.Enums.UpdateRejectionReason Reason { get; set; }
```

#### Property Value
[DiGi\.GIS\.PostgreSQL\.Enums\.UpdateRejectionReason](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.enums.updaterejectionreason 'DiGi\.GIS\.PostgreSQL\.Enums\.UpdateRejectionReason')

### Example
CountyUnresolved

<a name='DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection.Reference'></a>

## UpdateItemsResult\.Rejection\.Reference Property

Gets or sets the reference of the dropped row\. Null when the row carried none\.

The host omits null properties, so a rejection with nothing to name arrives as `{"reason":"Undefined"}` - an absent `reference` is the null, not a serialization fault.

```csharp
public string? Reference { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
1234\.5678\.AB\_12

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController'></a>

## YearBuiltDataController Class

Provides API endpoints for managing and updating year built data stored in a PostgreSQL database\.

```csharp
public class YearBuiltDataController : DiGi.WebAPI.Classes.WebAPIController
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [Microsoft\.AspNetCore\.Mvc\.ControllerBase](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase 'Microsoft\.AspNetCore\.Mvc\.ControllerBase') → [DiGi\.WebAPI\.Classes\.WebAPIController](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.webapicontroller 'DiGi\.WebAPI\.Classes\.WebAPIController') → YearBuiltDataController
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.YearBuiltDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore)'></a>

## YearBuiltDataController\(GISWebAPIConfigurationFileWatcher, YearBuiltDataPostgreSQLConverter, Building2DPostgreSQLConverter, AdministrativeAreal2DPostgreSQLConverter, BuildingDataPostgreSQLConverter, SecurityKeyManager, TokenRevocationStore\) Constructor

Initializes a new instance of the YearBuiltDataController class\.

```csharp
public YearBuiltDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher, DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter yearBuiltDataPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter building2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter administrativeAreal2DPostgreSQLConverter, DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter buildingDataPostgreSQLConverter, DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager=null, DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.YearBuiltDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).GISWebAPIConfigurationFileWatcher'></a>

`GISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The configuration file watcher used to monitor changes to the PostgreSQL Web API configuration\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.YearBuiltDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).yearBuiltDataPostgreSQLConverter'></a>

`yearBuiltDataPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.YearBuiltDataPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.yearbuiltdatapostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.YearBuiltDataPostgreSQLConverter')

The converter for YearBuiltData objects when interacting with a PostgreSQL database\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.YearBuiltDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).building2DPostgreSQLConverter'></a>

`building2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DPostgreSQLConverter')

The converter for Building2D objects, used to read which county row a reference is already filed under\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.YearBuiltDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).administrativeAreal2DPostgreSQLConverter'></a>

`administrativeAreal2DPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dpostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DPostgreSQLConverter')

The converter for administrative areal 2D data when interacting with a PostgreSQL database\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.YearBuiltDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).buildingDataPostgreSQLConverter'></a>

`buildingDataPostgreSQLConverter` [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataPostgreSQLConverter](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingdatapostgresqlconverter 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataPostgreSQLConverter')

The converter of the building data table, whose derived year built columns are recomputed after the stored history changes\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.YearBuiltDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).securityKeyManager'></a>

`securityKeyManager` [DiGi\.WebAPI\.Classes\.SecurityKeyManager](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.securitykeymanager 'DiGi\.WebAPI\.Classes\.SecurityKeyManager')

The user extension's security key manager; `null` when the user extension is not loaded, in which case every user\-token check denies\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.YearBuiltDataController(DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,DiGi.GIS.PostgreSQL.Classes.YearBuiltDataPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.Building2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DPostgreSQLConverter,DiGi.GIS.PostgreSQL.Classes.BuildingDataPostgreSQLConverter,DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore).tokenRevocationStore'></a>

`tokenRevocationStore` [DiGi\.WebAPI\.Classes\.TokenRevocationStore](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.tokenrevocationstore 'DiGi\.WebAPI\.Classes\.TokenRevocationStore')

The user extension's token revocation store; `null` when the user extension is not loaded, in which case every user\-token check denies\.
### Methods

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.GetCountByCountyIdAsync\(int, bool, bool, int, CancellationToken\) Method

Asynchronously retrieves the number of year built data items stored for a specified county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountByCountyIdAsync(int countyId, bool estimated=false, bool analyze=false, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the county\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).estimated'></a>

`estimated` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to return an estimated count from table statistics rather than an exact count\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).analyze'></a>

`analyze` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to perform an ANALYZE operation before reading the estimate\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountByCountyIdAsync(int,bool,bool,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying the count, 204 NoContent when the partition exists but is unanalysed, or 404 NotFound when the county has no partition\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountyPartMismatchesAsync(string,int,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.GetCountyPartMismatchesAsync\(string, int, CancellationToken\) Method

Asynchronously reports, for every polygon part of a multi\-part county, the year built data rows it holds whose reference `building_2d` does not hold under the same part\.

A county code names one `administrative_areal_2d` row per polygon part, and a row here is filed under one of them. A row mismatches when `building_2d` holds its reference under a different part, or under none at all. The mismatches split into [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReferencedObjectCountyPartMismatchResult\.CountHeldElsewhere](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreferencedobjectcountypartmismatchresult.countheldelsewhere 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReferencedObjectCountyPartMismatchResult\.CountHeldElsewhere') - a reference `building_2d` holds under another part, so the repair has somewhere to put the row - and [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReferencedObjectCountyPartMismatchResult\.CountOrphan](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreferencedobjectcountypartmismatchresult.countorphan 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReferencedObjectCountyPartMismatchResult\.CountOrphan') - a reference `building_2d` holds under no part, so there is no destination and the gap is a missing building row.

Only parts holding at least one mismatched row are returned, so a clean measurement is an empty list, and single-part codes are left out entirely because with one part there is nothing to be filed under by mistake.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetCountyPartMismatchesAsync(string? code=null, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountyPartMismatchesAsync(string,int,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional county code to restrict the measurement to\. When omitted every multi\-part code is measured\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountyPartMismatchesAsync(string,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetCountyPartMismatchesAsync(string,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token used to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') carrying one entry per part holding a mismatched row \- `200 []` when no measured part holds one \- or 500 when the measurement could not run\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.GetItemsByReferenceAsync\(string, Nullable\<int\>, CancellationToken\) Method

Asynchronously retrieves items based on a provided reference and an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByReferenceAsync(string reference, System.Nullable<int> countyId, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).reference'></a>

`reference` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The unique reference string used to identify the year built data items\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional integer representing the county ID to filter the results\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferenceAsync(string,System.Nullable_int_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,int,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.GetItemsByReferencesAsync\(IEnumerable\<string\>, Nullable\<int\>, bool, int, CancellationToken\) Method

Asynchronously retrieves year built data items for each of the provided references and an optional county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable<string>? references, System.Nullable<int> countyId=null, bool fallbackByReference=false, int commandTimeout=30, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,int,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of unique reference strings used to identify the year built data items\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional integer representing the county identifier used to filter the search\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,int,System.Threading.CancellationToken).fallbackByReference'></a>

`fallbackByReference` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether a reference not found under [countyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,int,System.Threading.CancellationToken).countyId 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.GetItemsByReferencesAsync\(System\.Collections\.Generic\.IEnumerable\<string\>, System\.Nullable\<int\>, bool, int, System\.Threading\.CancellationToken\)\.countyId') is also searched under the other polygon parts of the same county, where a row filed under a sibling part sits\. The search is limited to those parts' partitions; a single\-part county needs none\. Without [countyId](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,int,System.Threading.CancellationToken).countyId 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.GetItemsByReferencesAsync\(System\.Collections\.Generic\.IEnumerable\<string\>, System\.Nullable\<int\>, bool, int, System\.Threading\.CancellationToken\)\.countyId') it does nothing\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 30 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetItemsByReferencesAsync(System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task representing the asynchronous operation, returning a list of year built data items or no content if none were found\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetPredictedYearBuiltRunsAsync(int[],int,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.GetPredictedYearBuiltRunsAsync\(int\[\], int, CancellationToken\) Method

Lists the prediction runs stored under the given county parts: one entry per part, stamp and model identifier, with the number of objects carrying it\.

The stamp is reported as [System\.DateTime\.Ticks](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks 'System\.DateTime\.Ticks'), the value `removepredictedyearbuiltsbycountyids` takes, and the model identifier is null for entries written before it was recorded. A part holding no prediction answers `200 []`; a query that could not run answers 500, so "none" and "failed" are never confused.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetPredictedYearBuiltRunsAsync(int[]? countyIds, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetPredictedYearBuiltRunsAsync(int[],int,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The county parts to list, as a repeated `countyids` parameter\. Required\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetPredictedYearBuiltRunsAsync(int[],int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the query\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetPredictedYearBuiltRunsAsync(int[],int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with a list of [DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRunResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.predictedyearbuiltrunresult 'DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRunResult'), 400 for invalid parameters, 503 on a transient database failure, or 500 when the query failed\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetReferenceDuplicatesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.GetReferenceDuplicatesAsync\(Nullable\<int\>, int, int, CancellationToken\) Method

Asynchronously retrieves the building references that hold more than one year built data record, optionally filtered by county identifier, ordered by count descending\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetReferenceDuplicatesAsync(System.Nullable<int> countyId=null, int limit=100, int commandTimeout=600, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetReferenceDuplicatesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The optional integer identifier of the county to filter by; if null, searches across all counties\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetReferenceDuplicatesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of duplicate references to return\. Defaults to 100\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetReferenceDuplicatesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetReferenceDuplicatesAsync(System.Nullable_int_,int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token used to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing the list of duplicate references \- `200 []` when there are none \- or 500 when the query could not run, so "clean" and "failed" are never the same answer\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetReferencesByCountyIdAsync(int,int,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.GetReferencesByCountyIdAsync\(int, int, CancellationToken\) Method

Asynchronously retrieves the building references that carry stored year built data for a specified county identifier\.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetReferencesByCountyIdAsync(int countyId, int commandTimeout=30, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetReferencesByCountyIdAsync(int,int,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the county\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetReferencesByCountyIdAsync(int,int,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 30 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.GetReferencesByCountyIdAsync(int,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') containing a list of reference strings if found, or 404 if none are found\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.IsValid(int[],string[],int,int,Microsoft.AspNetCore.Mvc.IActionResult)'></a>

## YearBuiltDataController\.IsValid\(int\[\], string\[\], int, int, IActionResult\) Method

Checks the parameters the year built maintenance actions share: at least one county part, a non\-negative command timeout, a limit from 1 to the reference maximum, and no more references than that maximum\.

```csharp
private bool IsValid(int[]? countyIds, string[]? references, int limit, int commandTimeout, out Microsoft.AspNetCore.Mvc.IActionResult? actionResult);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.IsValid(int[],string[],int,int,Microsoft.AspNetCore.Mvc.IActionResult).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The county parts of the request\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.IsValid(int[],string[],int,int,Microsoft.AspNetCore.Mvc.IActionResult).references'></a>

`references` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The references of the request, or null when none was sent\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.IsValid(int[],string[],int,int,Microsoft.AspNetCore.Mvc.IActionResult).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The limit of the request\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.IsValid(int[],string[],int,int,Microsoft.AspNetCore.Mvc.IActionResult).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The command timeout of the request\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.IsValid(int[],string[],int,int,Microsoft.AspNetCore.Mvc.IActionResult).actionResult'></a>

`actionResult` [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')

When the parameters are refused, the 400 to answer with; otherwise, null\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True when the parameters are accepted; otherwise, false\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RefreshBuildingDataYearBuiltAsync(int,System.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.RefreshBuildingDataYearBuiltAsync\(int, IEnumerable\<string\>, CancellationToken\) Method

Recomputes the derived `building_data` year built columns of the given buildings after a committed user write, widening the given part to every polygon part of its county first\. Best\-effort \- see [RefreshBuildingDataYearBuiltAsync\(IEnumerable&lt;int&gt;, IEnumerable&lt;string&gt;, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RefreshBuildingDataYearBuiltAsync(System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.RefreshBuildingDataYearBuiltAsync\(System\.Collections\.Generic\.IEnumerable\<int\>, System\.Collections\.Generic\.IEnumerable\<string\>, System\.Threading\.CancellationToken\)')\.

```csharp
private System.Threading.Tasks.Task RefreshBuildingDataYearBuiltAsync(int countyId, System.Collections.Generic.IEnumerable<string> references, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RefreshBuildingDataYearBuiltAsync(int,System.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The county part the request named\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RefreshBuildingDataYearBuiltAsync(int,System.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings to recompute\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RefreshBuildingDataYearBuiltAsync(int,System.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RefreshBuildingDataYearBuiltAsync(System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.RefreshBuildingDataYearBuiltAsync\(IEnumerable\<int\>, IEnumerable\<string\>, CancellationToken\) Method

Recomputes the derived `building_data` year built columns of the given buildings after a committed write to their history, so `Calculated year built` follows a user entry at once rather than at the next building data run\.

Best-effort: the history write has already committed and stands, so a recompute that is not allowed ([AllowUpdateBuildingData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateBuildingData') disabled) or fails is logged and swallowed, and the next recompute or building data run catches the columns up.

```csharp
private System.Threading.Tasks.Task RefreshBuildingDataYearBuiltAsync(System.Collections.Generic.IEnumerable<int> countyIds, System.Collections.Generic.IEnumerable<string> references, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RefreshBuildingDataYearBuiltAsync(System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts the buildings are stored under\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RefreshBuildingDataYearBuiltAsync(System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings to recompute\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RefreshBuildingDataYearBuiltAsync(System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.RemoveItemsByCountyIdsAsync\(IEnumerable\<string\>, int\[\], bool, bool, int, int, string, CancellationToken\) Method

Deletes stored year built data objects of the given county parts \- by default only the objects holding no entry at all\.

Gated by the access key and by [AllowDeleteYearBuiltData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowDeleteYearBuiltData'), a flag of its own because a delete has no undo; either gate failing answers 401. The scope is always the named county parts - normally every polygon part of one county - narrowed to the references in the body when one is sent. Without a body only `emptyonly=true` is accepted, so the widest request this action takes deletes the empty objects of a county.

`dryrun` defaults to true: the rows are selected and counted, nothing is deleted. A request matching more rows than `limit` deletes nothing and answers 413 carrying the counts, so a mistaken scope is refused whole rather than cut short. The result is a [DiGi\.GIS\.PostgreSQL\.Classes\.YearBuiltDataRemoveResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.yearbuiltdataremoveresult 'DiGi\.GIS\.PostgreSQL\.Classes\.YearBuiltDataRemoveResult').

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable<string>? references, int[]? countyIds, bool emptyOnly=true, bool dryRun=true, int limit=10000, int commandTimeout=600, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings whose objects are deleted, or none for every building of the parts \(only with [emptyOnly](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).emptyOnly 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.RemoveItemsByCountyIdsAsync\(System\.Collections\.Generic\.IEnumerable\<string\>, int\[\], bool, bool, int, int, string, System\.Threading\.CancellationToken\)\.emptyOnly')\)\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The county parts to delete from, as a repeated `countyids` parameter\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).emptyOnly'></a>

`emptyOnly` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether only objects holding no entry are deleted\. Defaults to true\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).dryRun'></a>

`dryRun` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether the rows are only counted\. Defaults to true\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The largest number of rows the request may delete, from 1 to 10000\. Defaults to 10000\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for each database command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the counts, 413 with the counts when the scope exceeds [limit](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveItemsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,bool,int,int,string,System.Threading.CancellationToken).limit 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.RemoveItemsByCountyIdsAsync\(System\.Collections\.Generic\.IEnumerable\<string\>, int\[\], bool, bool, int, int, string, System\.Threading\.CancellationToken\)\.limit'), 401 without a valid key or with the flag disabled, 400 for invalid parameters, 503 on a transient database failure, or 500 when the delete failed\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.RemovePredictedYearBuiltsByCountyIdsAsync\(IEnumerable\<string\>, int\[\], Nullable\<long\>, bool, int, bool, int, string, CancellationToken\) Method

Removes one prediction run \- the predicted year built entries stamped [ticks](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).ticks 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.RemovePredictedYearBuiltsByCountyIdsAsync\(System\.Collections\.Generic\.IEnumerable\<string\>, int\[\], System\.Nullable\<long\>, bool, int, bool, int, string, System\.Threading\.CancellationToken\)\.ticks') \- from the stored year built data objects of the given county parts, in one transaction, and optionally recomputes the derived `building_data` columns of the buildings it changed\.

Gated by the access key and by [AllowDeleteYearBuiltData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowDeleteYearBuiltData'); with `updatebuildingdata=true` also by [AllowUpdateBuildingData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateBuildingData'), checked before anything is written. Any gate failing answers 401.

The stamp is the run's [System\.DateTime\.Ticks](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks 'System\.DateTime\.Ticks') - the key each entry is stored under, as `predictedyearbuiltruns` reports it - never a formatted date. Other stamps and user entries are untouched; an object left with no entry is kept and listed in [DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRemoveResult\.EmptiedReferences](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.predictedyearbuiltremoveresult.emptiedreferences 'DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRemoveResult\.EmptiedReferences') for an explicit `removeitemsbycountyids`. `dryrun` defaults to true, and a run on more objects than `limit` answers 413 and writes nothing. Removing a run already removed matches nothing, so a retry is harmless.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable<string>? references, int[]? countyIds, System.Nullable<long> ticks, bool dryRun=true, int limit=10000, bool updateBuildingData=false, int commandTimeout=600, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings to remove the run from, or none for every building of the parts\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The county parts to remove the run from, as a repeated `countyids` parameter\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).ticks'></a>

`ticks` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The stamp of the run, as [System\.DateTime\.Ticks](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks 'System\.DateTime\.Ticks')\. Required\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).dryRun'></a>

`dryRun` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether the entries are only counted\. Defaults to true\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The largest number of objects the request may rewrite, from 1 to 10000\. Defaults to 10000\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).updateBuildingData'></a>

`updateBuildingData` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether the derived `building_data` columns of the changed buildings are recomputed after the removal commits\. Defaults to false\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for each database command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with a [DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRemoveResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.predictedyearbuiltremoveresult 'DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRemoveResult'), 413 with it when the run exceeds [limit](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemovePredictedYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],System.Nullable_long_,bool,int,bool,int,string,System.Threading.CancellationToken).limit 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.RemovePredictedYearBuiltsByCountyIdsAsync\(System\.Collections\.Generic\.IEnumerable\<string\>, int\[\], System\.Nullable\<long\>, bool, int, bool, int, string, System\.Threading\.CancellationToken\)\.limit'), 401 without a valid key or with a flag disabled, 400 for invalid parameters, 503 on a transient database failure, or 500 when the removal \- or the recompute after it \- failed\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.RemoveUserYearBuiltAsync\(UserYearBuiltParameter, CancellationToken\) Method

Withdraws the signed\-in visitor's own user\-provided year built entry from one building\.

The bearer token identifies the user, exactly as for [SetUserYearBuiltAsync\(UserYearBuiltParameter, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.SetUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.SetUserYearBuiltAsync\(DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter, System\.Threading\.CancellationToken\)'), and the same [AllowUpdateYearBuiltData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateYearBuiltData') flag gates it: withdrawing one's own label is the counterpart of setting it, not a moderation delete. The entry is looked for under every polygon part of the county the given part belongs to, and only an entry recorded by this user is withdrawn - a building whose entry another user recorded answers 403 and is left untouched. The objects themselves are kept.

After a committed withdrawal the building's derived `building_data` columns are recomputed, best-effort: a failure there is logged and the withdrawal still answers 200.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> RemoveUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter? parameter, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter,System.Threading.CancellationToken).parameter'></a>

`parameter` [UserYearBuiltParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter')

The building to withdraw the entry of; [Year](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.Year 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter\.Year') and [Relation](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.Relation 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter\.Relation') are ignored\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 when the entry was withdrawn, 404 when the building holds no user entry, 403 when the entry belongs to another user, 401 without a valid user token, 400 for a disabled flag or an invalid body, or 500 when the withdrawal failed\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,int,string,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.RemoveUserYearBuiltsByCountyIdsAsync\(IEnumerable\<string\>, int\[\], bool, int, string, CancellationToken\) Method

Withdraws the user\-provided year built entry of the given buildings, whoever recorded it \- the moderation counterpart of [RemoveUserYearBuiltAsync\(UserYearBuiltParameter, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.RemoveUserYearBuiltAsync\(DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter, System\.Threading\.CancellationToken\)')\.

Gated by the access key and by [AllowDeleteYearBuiltData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowDeleteYearBuiltData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowDeleteYearBuiltData'); either failing answers 401. The references are required, and are looked for under the named county parts only. `dryrun` defaults to true. The objects are kept. After a committed withdrawal the derived `building_data` columns of the withdrawn buildings are recomputed best-effort when [AllowUpdateBuildingData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateBuildingData') allows it.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> RemoveUserYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable<string>? references, int[]? countyIds, bool dryRun=true, int commandTimeout=600, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,int,string,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings to withdraw the user entry of\. Required\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,int,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The county parts the buildings are stored under, as a repeated `countyids` parameter\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,int,string,System.Threading.CancellationToken).dryRun'></a>

`dryRun` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether the buildings are only classified\. Defaults to true\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,int,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for each database command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,int,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.RemoveUserYearBuiltsByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],bool,int,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with a [DiGi\.GIS\.PostgreSQL\.Classes\.UserYearBuiltRemoveResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.useryearbuiltremoveresult 'DiGi\.GIS\.PostgreSQL\.Classes\.UserYearBuiltRemoveResult'), 401 without a valid key or with the flag disabled, 400 for invalid parameters, 503 on a transient database failure, or 500 when the withdrawal failed\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.Result(DiGi.Core.Interfaces.ISerializableObject,bool)'></a>

## YearBuiltDataController\.Result\(ISerializableObject, bool\) Method

Answers a maintenance result as DiGi JSON: 200, or 413 when the request matched more than its limit and was therefore refused whole\.

```csharp
private Microsoft.AspNetCore.Mvc.ContentResult Result(DiGi.Core.Interfaces.ISerializableObject serializableObject, bool exceeded);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.Result(DiGi.Core.Interfaces.ISerializableObject,bool).serializableObject'></a>

`serializableObject` [DiGi\.Core\.Interfaces\.ISerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.interfaces.iserializableobject 'DiGi\.Core\.Interfaces\.ISerializableObject')

The result to answer with\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.Result(DiGi.Core.Interfaces.ISerializableObject,bool).exceeded'></a>

`exceeded` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether the request exceeded its limit\.

#### Returns
[Microsoft\.AspNetCore\.Mvc\.ContentResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.contentresult 'Microsoft\.AspNetCore\.Mvc\.ContentResult')  
The [Microsoft\.AspNetCore\.Mvc\.ContentResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.contentresult 'Microsoft\.AspNetCore\.Mvc\.ContentResult') carrying the result\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.SetUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.SetUserYearBuiltAsync\(UserYearBuiltParameter, CancellationToken\) Method

Records a single user\-supplied year built entry for one building, on behalf of the signed\-in visitor\.

The bearer token replaces the machine `key` header: the identity is a user, and the entry is stored under that user's email with the time it was recorded. The write is gated by the same [AllowUpdateYearBuiltData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateYearBuiltData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateYearBuiltData') flag as the machine update endpoints, and each check is logged distinctly so the cause of a 4xx is recoverable even where the status is shared.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> SetUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter? parameter, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.SetUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter,System.Threading.CancellationToken).parameter'></a>

`parameter` [UserYearBuiltParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter')

The user year built entry to record\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.SetUserYearBuiltAsync(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 on a committed write, 404 when no building holds the reference under the given part, 401 without a valid user token, 400 for a disabled flag or an invalid body, or 500 when the write failed\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateBuildingDataByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],int,string,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.UpdateBuildingDataByCountyIdsAsync\(IEnumerable\<string\>, int\[\], int, string, CancellationToken\) Method

Recomputes the derived year built columns of `building_data` \- predicted, user and calculated \- from the stored history of the given county parts, writing NULL where the history no longer holds a value\.

Gated by the access key and by [AllowUpdateBuildingData](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.AllowUpdateBuildingData 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.AllowUpdateBuildingData'); either failing answers 401. With references in the body only those buildings are recomputed; without, every building holding a `year_built_data` or a `building_data` row under the parts. A building with neither a building data row nor a value is not written, so the call never adds empty rows. The result is a [DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataYearBuiltUpdateResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingdatayearbuiltupdateresult 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataYearBuiltUpdateResult').

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateBuildingDataByCountyIdsAsync(System.Collections.Generic.IEnumerable<string>? references, int[]? countyIds, int commandTimeout=600, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateBuildingDataByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],int,string,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings to recompute, or none for every building of the parts\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateBuildingDataByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],int,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The county parts to recompute, as a repeated `countyids` parameter\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateBuildingDataByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],int,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for each database command\. A value of 0 disables the timeout; a negative value is refused with HTTP 400\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateBuildingDataByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],int,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateBuildingDataByCountyIdsAsync(System.Collections.Generic.IEnumerable_string_,int[],int,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. 200 with the counts, 401 without a valid key or with the flag disabled, 400 for invalid parameters, 503 on a transient database failure, or 500 when the recompute failed\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.UpdateItemsAsync\(JsonArray, string, string, CancellationToken\) Method

Updates multiple year built data items based on the provided JSON array and identification code\.

A county code does not identify a single county row: BDOT10k stores a county whose territory is disconnected as one feature per polygon part, and every part becomes its own row. Every part the code names is passed on, and each datum is filed under the part it actually belongs to - see [UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.UpdateItemsByCountyIdsAsync\(System\.Text\.Json\.Nodes\.JsonArray, int\[\], string, System\.Threading\.CancellationToken\)') for how that is decided.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, string code, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the data items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The identification code required for the update operation\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') representing the result of the update operation\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken)'></a>

## YearBuiltDataController\.UpdateItemsByCountyIdsAsync\(JsonArray, int\[\], string, CancellationToken\) Method

Updates multiple year built data items in the database for the given county rows\.

The unambiguous counterpart of [UpdateItemsAsync\(JsonArray, string, string, CancellationToken\)](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsAsync(System.Text.Json.Nodes.JsonArray,string,string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDataController\.UpdateItemsAsync\(System\.Text\.Json\.Nodes\.JsonArray, string, string, System\.Threading\.CancellationToken\)'): it takes county identifiers rather than a code, so the caller states which rows are in play instead of leaving the server to derive them.

The identifiers are the parts of one county in play, and each datum is filed under the part already holding the `building_2d` row its reference names, probed lowest part first - whether one identifier arrived or several, since naming one part is not evidence the county has one. That row was filed by geometry when it was imported, so reusing its answer keeps both tables keyed by the same `(county_id, reference)` pair.

A datum no named part holds is widened to every part of the county its parts name, and only a datum no part of the county holds is left unwritten - it carries no geometry of its own, so nothing states where it belongs, and storing it under a guessed part is the state this replaced.

The county-part lookup not running is answered as a distinct 500 naming it, and a transient database failure as 503 with `Retry-After`, so an unreachable database is never mistaken for a quiet no-op.

```csharp
public System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray? jsonArray, int[]? countyIds, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).jsonArray'></a>

`jsonArray` [System\.Text\.Json\.Nodes\.JsonArray](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonarray 'System\.Text\.Json\.Nodes\.JsonArray')

The JSON array containing the data items to be updated\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The identifiers of the county rows the year built data belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The secret access key supplied in the request header\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDataController.UpdateItemsByCountyIdsAsync(System.Text.Json.Nodes.JsonArray,int[],string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellation token to observe\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
An [Microsoft\.AspNetCore\.Mvc\.IActionResult](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.iactionresult 'Microsoft\.AspNetCore\.Mvc\.IActionResult') representing the result of the update operation\.

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask'></a>

## YearBuiltDatasPostTask Class

Provides functionality to handle the asynchronous posting of [DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData') collections to the PostgreSQL database\.

```csharp
public class YearBuiltDatasPostTask : DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask<DiGi.GIS.Classes.YearBuiltData>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.BackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.backgroundtask 'DiGi\.Core\.Classes\.BackgroundTask') → [DiGi\.Core\.Classes\.CancelableBackgroundTask](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.cancelablebackgroundtask 'DiGi\.Core\.Classes\.CancelableBackgroundTask') → [DiGi\.Core\.Classes\.ReportableBackgroundTask&lt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.reportablebackgroundtask-1 'DiGi\.Core\.Classes\.ReportableBackgroundTask\`1') → [DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask&lt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>')[DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData')[&gt;](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostTask_T_ 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostTask\<T\>') → YearBuiltDatasPostTask
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask.YearBuiltDatasPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## YearBuiltDatasPostTask\(GISWebAPIManager\) Constructor

Initializes a new instance of the YearBuiltDatasPostTask class\.

```csharp
public YearBuiltDatasPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask.YearBuiltDatasPostTask(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The GIS PostgreSQL Web API manager used to handle data persistence\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask.Code'></a>

## YearBuiltDatasPostTask\.Code Property

Gets or sets the code associated with the year built data post task\.

A code does not identify a single county row - a multi-part county holds one row per polygon part - so set [CountyIds](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask.CountyIds 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDatasPostTask\.CountyIds') instead wherever the identifiers are already known. [CountyIds](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask.CountyIds 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDatasPostTask\.CountyIds') takes precedence when both are set.

```csharp
public string? Code { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask.CountyIds'></a>

## YearBuiltDatasPostTask\.CountyIds Property

Gets or sets the identifiers of the county rows the year built data belong to \- normally every polygon part of one county, since a single id is not evidence the code has one part\. When set it is used in preference to [Code](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.YearBuiltDatasPostTask.Code 'DiGi\.GIS\.WebAPI\.Classes\.YearBuiltDatasPostTask\.Code'), which lets the server resolve the code to every part\.

```csharp
public System.Collections.Generic.HashSet<int>? CountyIds { get; set; }
```

#### Property Value
[System\.Collections\.Generic\.HashSet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')