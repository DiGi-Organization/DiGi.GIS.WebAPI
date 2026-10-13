#### [DiGi\.GIS\.WebAPI](DiGi.GIS.WebAPI.Overview.md 'DiGi\.GIS\.WebAPI\.Overview')

## DiGi\.GIS\.WebAPI Namespace
### Classes

<a name='DiGi.GIS.WebAPI.Convert'></a>

## Convert Class

```csharp
public static class Convert
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Convert
### Methods

<a name='DiGi.GIS.WebAPI.Convert.ToSystem_JsonObject(thisSystem.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DCentroid_)'></a>

## Convert\.ToSystem\_JsonObject\(this IEnumerable\<Building2DCentroid\>\) Method

Converts building centroids to the compact, columnar answer of `gis/Building2D/centroidsbyadministrativeareal2Did`: `{"References":[…],"CountyIds":[…],"X":[…],"Y":[…]}`, the i\-th entry of every array describing the same building\.

The full [DiGi\.GIS\.PostgreSQL\.Classes\.Building2DCentroid](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dcentroid 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DCentroid') answer of `point2dsbyadministrativeareal2Did` repeats a `_type` discriminator and four property names per building. For county 1465 (155 307 buildings) that relays as 15.9 MB and takes 2.1-2.6 s, and the client rebuilds every row through `Core.Convert.ToDiGi` only to keep four values (DiGi.GIS.WebAPI.UI#29). This shape carries the values alone.

Coordinates are rounded to 0.01 m (away from zero on a midpoint), which also drops the binary noise of values such as `474823.57999999996`. A centroid without a reference or a county identifier cannot be joined by the client and is skipped, as the full answer's consumers skip it; the arrays therefore always share one length. The input order is kept.

```csharp
public static System.Text.Json.Nodes.JsonObject? ToSystem_JsonObject(this System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.Building2DCentroid>? building2DCentroids);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Convert.ToSystem_JsonObject(thisSystem.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DCentroid_).building2DCentroids'></a>

`building2DCentroids` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.Building2DCentroid](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dcentroid 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DCentroid')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The centroids to convert\. This value can be null\.

#### Returns
[System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')  
The columnar object, with four empty arrays for an empty input, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when [building2DCentroids](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Convert.ToSystem_JsonObject(thisSystem.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DCentroid_).building2DCentroids 'DiGi\.GIS\.WebAPI\.Convert\.ToSystem\_JsonObject\(this System\.Collections\.Generic\.IEnumerable\<DiGi\.GIS\.PostgreSQL\.Classes\.Building2DCentroid\>\)\.building2DCentroids') is null\.

<a name='DiGi.GIS.WebAPI.Create'></a>

## Create Class

```csharp
public static class Create
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Create
### Methods

<a name='DiGi.GIS.WebAPI.Create.GISWebAPIConfigurationFileWatcher()'></a>

## Create\.GISWebAPIConfigurationFileWatcher\(\) Method

Creates a new instance of the GISWebAPIConfigurationFileWatcher class\.

```csharp
public static DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher GISWebAPIConfigurationFileWatcher();
```

#### Returns
[GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Create.GISWebAPIManager(string)'></a>

## Create\.GISWebAPIManager\(string\) Method

Creates a new instance of the [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') class\.

```csharp
public static DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager(string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.GISWebAPIManager(string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')  
A new [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance configured with the registered services and optional authorization key\.

<a name='DiGi.GIS.WebAPI.Create.HttpClient_Geoportal(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## Create\.HttpClient\_Geoportal\(GISWebAPIManager\) Method

Creates and configures an [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') instance for the Geoportal service, including a custom User\-Agent header based on the executing assembly's name and version\.

```csharp
public static System.Net.Http.HttpClient? HttpClient_Geoportal(DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.HttpClient_Geoportal(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to create the HTTP client instance\.

#### Returns
[System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Create.HttpClient_GUGiK(DiGi.GIS.WebAPI.Classes.GISWebAPIManager)'></a>

## Create\.HttpClient\_GUGiK\(GISWebAPIManager\) Method

Creates and configures an [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') instance for the "Główny Urząd Geodezji i Kartografii" service, including a custom User\-Agent header based on the executing assembly's name and version\.

```csharp
public static System.Net.Http.HttpClient? HttpClient_GUGiK(DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.HttpClient_GUGiK(DiGi.GIS.WebAPI.Classes.GISWebAPIManager).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager used to create the HTTP client instance\.

#### Returns
[System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisbyte[],System.IO.Compression.CompressionLevel,System.Threading.CancellationToken)'></a>

## Create\.HttpContent\(this byte\[\], CompressionLevel, CancellationToken\) Method

Asynchronously creates GZip\-compressed HttpContent from the provided byte array using the specified compression level\.

```csharp
public static System.Threading.Tasks.Task<System.Net.Http.HttpContent?> HttpContent(this byte[] bytes, System.IO.Compression.CompressionLevel compressionLevel, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisbyte[],System.IO.Compression.CompressionLevel,System.Threading.CancellationToken).bytes'></a>

`bytes` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The raw byte array to be compressed\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisbyte[],System.IO.Compression.CompressionLevel,System.Threading.CancellationToken).compressionLevel'></a>

`compressionLevel` [System\.IO\.Compression\.CompressionLevel](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.compressionlevel 'System\.IO\.Compression\.CompressionLevel')

The compression level applied to the payload\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisbyte[],System.IO.Compression.CompressionLevel,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A token to monitor for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Net\.Http\.HttpContent](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent 'System\.Net\.Http\.HttpContent')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisbyte[],System.Threading.CancellationToken)'></a>

## Create\.HttpContent\(this byte\[\], CancellationToken\) Method

Asynchronously creates GZip\-compressed HttpContent from the provided byte array\.

```csharp
public static System.Threading.Tasks.Task<System.Net.Http.HttpContent?> HttpContent(this byte[] bytes, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisbyte[],System.Threading.CancellationToken).bytes'></a>

`bytes` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The raw byte array to be compressed\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisbyte[],System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A token to monitor for cancellation requests\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Net\.Http\.HttpContent](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent 'System\.Net\.Http\.HttpContent')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisstring,System.Threading.CancellationToken)'></a>

## Create\.HttpContent\(this string, CancellationToken\) Method

Converts a JSON string into an asynchronous [System\.Net\.Http\.HttpContent](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent 'System\.Net\.Http\.HttpContent') object\.

```csharp
public static System.Threading.Tasks.Task<System.Net.Http.HttpContent?> HttpContent(this string json, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisstring,System.Threading.CancellationToken).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The JSON string to be converted\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisstring,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The cancellationToken\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Net\.Http\.HttpContent](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent 'System\.Net\.Http\.HttpContent')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisSystem.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken)'></a>

## Create\.HttpContent\(this IEnumerable\<string\>, CancellationToken\) Method

Converts a collection of strings into an [System\.Net\.Http\.HttpContent](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent 'System\.Net\.Http\.HttpContent') object holding a JSON array\.

```csharp
public static System.Threading.Tasks.Task<System.Net.Http.HttpContent?> HttpContent(this System.Collections.Generic.IEnumerable<string>? values, System.Threading.CancellationToken cancellationToken);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisSystem.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken).values'></a>

`values` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of strings to be serialized and converted to HTTP content\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent(thisSystem.Collections.Generic.IEnumerable_string_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A token to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Net\.Http\.HttpContent](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent 'System\.Net\.Http\.HttpContent')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent_TSerializableObject_(thisSystem.Collections.Generic.IEnumerable_TSerializableObject_,System.Threading.CancellationToken)'></a>

## Create\.HttpContent\<TSerializableObject\>\(this IEnumerable\<TSerializableObject\>, CancellationToken\) Method

Converts a collection of serializable objects into an [System\.Net\.Http\.HttpContent](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent 'System\.Net\.Http\.HttpContent') object by first serializing them to a JSON string\.

```csharp
public static System.Threading.Tasks.Task<System.Net.Http.HttpContent?> HttpContent<TSerializableObject>(this System.Collections.Generic.IEnumerable<TSerializableObject> serializableObjects, System.Threading.CancellationToken cancellationToken)
    where TSerializableObject : DiGi.Core.Interfaces.ISerializableObject;
```
#### Type parameters

<a name='DiGi.GIS.WebAPI.Create.HttpContent_TSerializableObject_(thisSystem.Collections.Generic.IEnumerable_TSerializableObject_,System.Threading.CancellationToken).TSerializableObject'></a>

`TSerializableObject`

The type of the objects in the collection, which must implement [DiGi\.Core\.Interfaces\.ISerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.interfaces.iserializableobject 'DiGi\.Core\.Interfaces\.ISerializableObject')\.
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.HttpContent_TSerializableObject_(thisSystem.Collections.Generic.IEnumerable_TSerializableObject_,System.Threading.CancellationToken).serializableObjects'></a>

`serializableObjects` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TSerializableObject](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Create.HttpContent_TSerializableObject_(thisSystem.Collections.Generic.IEnumerable_TSerializableObject_,System.Threading.CancellationToken).TSerializableObject 'DiGi\.GIS\.WebAPI\.Create\.HttpContent\<TSerializableObject\>\(this System\.Collections\.Generic\.IEnumerable\<TSerializableObject\>, System\.Threading\.CancellationToken\)\.TSerializableObject')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of objects to be serialized and converted to HTTP content\.

<a name='DiGi.GIS.WebAPI.Create.HttpContent_TSerializableObject_(thisSystem.Collections.Generic.IEnumerable_TSerializableObject_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

A token to cancel the asynchronous operation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Net\.Http\.HttpContent](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent 'System\.Net\.Http\.HttpContent')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Create.ServiceProvider()'></a>

## Create\.ServiceProvider\(\) Method

Creates and configures a service provider with the registered services\.

```csharp
public static System.IServiceProvider ServiceProvider();
```

#### Returns
[System\.IServiceProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iserviceprovider 'System\.IServiceProvider')  
An [System\.IServiceProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iserviceprovider 'System\.IServiceProvider') containing the registered services\.

<a name='DiGi.GIS.WebAPI.Create.SubdivisionFilterGroup(DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Collections.Generic.IEnumerable_int_)'></a>

## Create\.SubdivisionFilterGroup\(FilterGroup, IEnumerable\<int\>\) Method

Creates the filter group that confines a building data count to a set of subdivisions, and applies the caller's own filters on top\.

The result is an `And` group holding one `In` condition on the subdivision column, with the caller's group nested beside it as a whole so that its own logical operator is kept (an `Or` group is not flattened into the subdivision condition). The list reaches the `In` branch as a collection, not as a deserialized JSON array.

```csharp
public static DiGi.PostgreSQL.Table.Classes.FilterGroup? SubdivisionFilterGroup(DiGi.PostgreSQL.Table.Classes.FilterGroup? filterGroup, System.Collections.Generic.IEnumerable<int>? subdivisionIds);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.SubdivisionFilterGroup(DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Collections.Generic.IEnumerable_int_).filterGroup'></a>

`filterGroup` [DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

The optional filters of the caller; `null` or empty adds nothing\.

<a name='DiGi.GIS.WebAPI.Create.SubdivisionFilterGroup(DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Collections.Generic.IEnumerable_int_).subdivisionIds'></a>

`subdivisionIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the subdivisions the count is confined to\.

#### Returns
[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')  
The combined filter group, or null when there is no subdivision identifier to confine the count to\.

<a name='DiGi.GIS.WebAPI.Create.Table(System.Text.Json.Nodes.JsonObject)'></a>

## Create\.Table\(JsonObject\) Method

Creates a [Table\(JsonObject\)](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Create.Table(System.Text.Json.Nodes.JsonObject) 'DiGi\.GIS\.WebAPI\.Create\.Table\(System\.Text\.Json\.Nodes\.JsonObject\)') instance from a JSON object using [DiGi\.Core\.IO\.Table\.Classes\.TableConverter&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.table.classes.tableconverter-3 'DiGi\.Core\.IO\.Table\.Classes\.TableConverter\`3')\.

```csharp
public static DiGi.Core.IO.Table.Classes.Table? Table(System.Text.Json.Nodes.JsonObject? jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.Table(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The JSON object containing the serialized table structure and data\.

#### Returns
[DiGi\.Core\.IO\.Table\.Classes\.Table](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.table.classes.table 'DiGi\.Core\.IO\.Table\.Classes\.Table')  
A new [Table\(JsonObject\)](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Create.Table(System.Text.Json.Nodes.JsonObject) 'DiGi\.GIS\.WebAPI\.Create\.Table\(System\.Text\.Json\.Nodes\.JsonObject\)') instance, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') if the JSON object is null or cannot be deserialized\.

<a name='DiGi.GIS.WebAPI.Create.UpdateItemsResult(thisDiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateResult,int)'></a>

## Create\.UpdateItemsResult\(this PostgreSQLUpdateResult, int\) Method

Creates the response payload of a write endpoint from what the database converter reported\.

```csharp
public static DiGi.GIS.WebAPI.Classes.UpdateItemsResult? UpdateItemsResult(this DiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateResult? postgreSQLUpdateResult, int sent);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Create.UpdateItemsResult(thisDiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateResult,int).postgreSQLUpdateResult'></a>

`postgreSQLUpdateResult` [DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.postgresqlupdateresult 'DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateResult')

The outcome returned by the converter's update\.

<a name='DiGi.GIS.WebAPI.Create.UpdateItemsResult(thisDiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateResult,int).sent'></a>

`sent` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of rows handed to the converter\.

#### Returns
[UpdateItemsResult](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UpdateItemsResult 'DiGi\.GIS\.WebAPI\.Classes\.UpdateItemsResult')  
A new [UpdateItemsResult](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UpdateItemsResult 'DiGi\.GIS\.WebAPI\.Classes\.UpdateItemsResult'), or null if [postgreSQLUpdateResult](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Create.UpdateItemsResult(thisDiGi.GIS.PostgreSQL.Classes.PostgreSQLUpdateResult,int).postgreSQLUpdateResult 'DiGi\.GIS\.WebAPI\.Create\.UpdateItemsResult\(this DiGi\.GIS\.PostgreSQL\.Classes\.PostgreSQLUpdateResult, int\)\.postgreSQLUpdateResult') is null\.

<a name='DiGi.GIS.WebAPI.Modify'></a>

## Modify Class

```csharp
public static class Modify
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Modify
### Methods

<a name='DiGi.GIS.WebAPI.Modify.InitializeAsync(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection)'></a>

## Modify\.InitializeAsync\(this IServiceCollection\) Method

Initializes the GIS PostgreSQL Web API services, including the configuration file watcher and converter manager\.

```csharp
public static System.Threading.Tasks.Task InitializeAsync(this Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.InitializeAsync(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection).serviceCollection'></a>

`serviceCollection` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') to add services to\.

#### Returns
[System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task')  
A [System\.Threading\.Tasks\.Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task 'System\.Threading\.Tasks\.Task') representing the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken)'></a>

## Modify\.PostReferencesAsync\<TSerializableObject\>\(this HttpClient, string, IEnumerable\<string\>, int, PostOptions, string, CancellationToken\) Method

Asynchronously posts a list of building references to a protected maintenance endpoint and reads back the DiGi result it answers with\.

The shared plumbing of the year built maintenance clients: the access key travels in the `key` header, never the query string; a null or empty [references](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).references 'DiGi\.GIS\.WebAPI\.Modify\.PostReferencesAsync\<TSerializableObject\>\(this System\.Net\.Http\.HttpClient, string, System\.Collections\.Generic\.IEnumerable\<string\>, int, DiGi\.WebAPI\.Classes\.PostOptions, string, System\.Threading\.CancellationToken\)\.references') sends no body, which those endpoints read as "every building of the parts"; and the per-attempt delay is stretched to the server's command timeout so a long delete is not cut off by the 20 s client default (`Coding - WebAPI Contracts.md` §3). A refused request - 401, 400, or 413 for a scope over its limit - and a failed one both answer null; the server log carries the reason.

```csharp
public static System.Threading.Tasks.Task<TSerializableObject?> PostReferencesAsync<TSerializableObject>(this System.Net.Http.HttpClient? httpClient, string? requestUri, System.Collections.Generic.IEnumerable<string>? references, int commandTimeout, DiGi.WebAPI.Classes.PostOptions? postOptions, string? key, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TSerializableObject : DiGi.Core.Interfaces.ISerializableObject;
```
#### Type parameters

<a name='DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).TSerializableObject'></a>

`TSerializableObject`

The type of the DiGi result the endpoint answers with\.
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).httpClient'></a>

`httpClient` [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient')

The [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') to post with\.

<a name='DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).requestUri'></a>

`requestUri` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The request URI, query string included\.

<a name='DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references to post, or null to post no body\.

<a name='DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The server command timeout the request carries, in seconds; the per\-attempt delay is stretched to it\.

<a name='DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP request\.

<a name='DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The access key of the protected endpoint\.

<a name='DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[TSerializableObject](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Modify.PostReferencesAsync_TSerializableObject_(thisSystem.Net.Http.HttpClient,string,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).TSerializableObject 'DiGi\.GIS\.WebAPI\.Modify\.PostReferencesAsync\<TSerializableObject\>\(this System\.Net\.Http\.HttpClient, string, System\.Collections\.Generic\.IEnumerable\<string\>, int, DiGi\.WebAPI\.Classes\.PostOptions, string, System\.Threading\.CancellationToken\)\.TSerializableObject')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task returning the result, or null when the request was refused or failed\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken)'></a>

## Modify\.RemovePredictedYearBuiltsAsync\(this GISWebAPIManager, IEnumerable\<int\>, long, IEnumerable\<string\>, bool, int, bool, int, PostOptions, string, CancellationToken\) Method

Asynchronously removes one prediction run from the stored year built data objects of the given county parts through `gis/yearbuiltdata/removepredictedyearbuiltsbycountyids`, by default as a dry run\.

The run is named by its stamp as [System\.DateTime\.Ticks](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks 'System\.DateTime\.Ticks') - the value [PredictedYearBuiltRunsAsync\(this GISWebAPIManager, IEnumerable&lt;int&gt;, int, PostOptions, CancellationToken\)](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Query.PredictedYearBuiltRunsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Query\.PredictedYearBuiltRunsAsync\(this DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager, System\.Collections\.Generic\.IEnumerable\<int\>, int, DiGi\.WebAPI\.Classes\.PostOptions, System\.Threading\.CancellationToken\)') reports. Needs the access key and `AllowDeleteYearBuiltData` on the host, and `AllowUpdateBuildingData` as well with [updateBuildingData](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).updateBuildingData 'DiGi\.GIS\.WebAPI\.Modify\.RemovePredictedYearBuiltsAsync\(this DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager, System\.Collections\.Generic\.IEnumerable\<int\>, long, System\.Collections\.Generic\.IEnumerable\<string\>, bool, int, bool, int, DiGi\.WebAPI\.Classes\.PostOptions, string, System\.Threading\.CancellationToken\)\.updateBuildingData'). A run on more objects than [limit](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).limit 'DiGi\.GIS\.WebAPI\.Modify\.RemovePredictedYearBuiltsAsync\(this DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager, System\.Collections\.Generic\.IEnumerable\<int\>, long, System\.Collections\.Generic\.IEnumerable\<string\>, bool, int, bool, int, DiGi\.WebAPI\.Classes\.PostOptions, string, System\.Threading\.CancellationToken\)\.limit') is refused whole (HTTP 413) and answers null here.

```csharp
public static System.Threading.Tasks.Task<DiGi.GIS.PostgreSQL.Classes.PredictedYearBuiltRemoveResult?> RemovePredictedYearBuiltsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? gisWebAPIManager, System.Collections.Generic.IEnumerable<int>? countyIds, long ticks, System.Collections.Generic.IEnumerable<string>? references=null, bool dryRun=true, int limit=10000, bool updateBuildingData=false, int commandTimeout=600, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).gisWebAPIManager'></a>

`gisWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the Web API\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts to remove the run from\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).ticks'></a>

`ticks` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The stamp of the run, as [System\.DateTime\.Ticks](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks 'System\.DateTime\.Ticks')\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings to remove the run from, or null for every building of the parts\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).dryRun'></a>

`dryRun` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether the entries are only counted\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The largest number of objects the request may rewrite, from 1 to 10000\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).updateBuildingData'></a>

`updateBuildingData` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether the derived building data year built columns of the changed buildings are recomputed after the removal\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for each database command on the server\. A value of 0 disables it\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP request\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The access key; defaults to the manager's\.

<a name='DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRemoveResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.predictedyearbuiltremoveresult 'DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRemoveResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task returning the counts, or null when the request was refused or failed\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveUserYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken)'></a>

## Modify\.RemoveUserYearBuiltsAsync\(this GISWebAPIManager, IEnumerable\<int\>, IEnumerable\<string\>, bool, int, PostOptions, string, CancellationToken\) Method

Asynchronously withdraws the user\-provided year built entry of the given buildings, whoever recorded it, through `gis/yearbuiltdata/removeuseryearbuiltsbycountyids` \- the moderation path, by default as a dry run\.

Needs the access key and `AllowDeleteYearBuiltData` on the host. The objects are kept; the derived building data columns of the withdrawn buildings are recomputed on the host when it allows building data updates.

```csharp
public static System.Threading.Tasks.Task<DiGi.GIS.PostgreSQL.Classes.UserYearBuiltRemoveResult?> RemoveUserYearBuiltsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? gisWebAPIManager, System.Collections.Generic.IEnumerable<int>? countyIds, System.Collections.Generic.IEnumerable<string>? references, bool dryRun=true, int commandTimeout=600, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.RemoveUserYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).gisWebAPIManager'></a>

`gisWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the Web API\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveUserYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts the buildings are stored under\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveUserYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings to withdraw the user entry of\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveUserYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).dryRun'></a>

`dryRun` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether the buildings are only classified\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveUserYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for each database command on the server\. A value of 0 disables it\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveUserYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP request\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveUserYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The access key; defaults to the manager's\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveUserYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.UserYearBuiltRemoveResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.useryearbuiltremoveresult 'DiGi\.GIS\.PostgreSQL\.Classes\.UserYearBuiltRemoveResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task returning the classification of every reference, or null when the request was refused or failed\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken)'></a>

## Modify\.RemoveYearBuiltDatasAsync\(this GISWebAPIManager, IEnumerable\<int\>, IEnumerable\<string\>, bool, bool, int, int, PostOptions, string, CancellationToken\) Method

Asynchronously deletes stored year built data objects of the given county parts through `gis/yearbuiltdata/removeitemsbycountyids` \- by default only the objects holding no entry, and by default as a dry run\.

Needs the access key and `AllowDeleteYearBuiltData` on the host. Without references only [emptyOnly](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).emptyOnly 'DiGi\.GIS\.WebAPI\.Modify\.RemoveYearBuiltDatasAsync\(this DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager, System\.Collections\.Generic\.IEnumerable\<int\>, System\.Collections\.Generic\.IEnumerable\<string\>, bool, bool, int, int, DiGi\.WebAPI\.Classes\.PostOptions, string, System\.Threading\.CancellationToken\)\.emptyOnly') is accepted. A scope matching more rows than [limit](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).limit 'DiGi\.GIS\.WebAPI\.Modify\.RemoveYearBuiltDatasAsync\(this DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager, System\.Collections\.Generic\.IEnumerable\<int\>, System\.Collections\.Generic\.IEnumerable\<string\>, bool, bool, int, int, DiGi\.WebAPI\.Classes\.PostOptions, string, System\.Threading\.CancellationToken\)\.limit') is refused whole (HTTP 413) and answers null here - run it as a dry run first to read the count.

```csharp
public static System.Threading.Tasks.Task<DiGi.GIS.PostgreSQL.Classes.YearBuiltDataRemoveResult?> RemoveYearBuiltDatasAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? gisWebAPIManager, System.Collections.Generic.IEnumerable<int>? countyIds, System.Collections.Generic.IEnumerable<string>? references=null, bool emptyOnly=true, bool dryRun=true, int limit=10000, int commandTimeout=600, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).gisWebAPIManager'></a>

`gisWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the Web API\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts to delete from\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings whose objects are deleted, or null for every building of the parts\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).emptyOnly'></a>

`emptyOnly` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether only objects holding no entry are deleted\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).dryRun'></a>

`dryRun` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A value indicating whether the rows are only counted\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).limit'></a>

`limit` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The largest number of rows the request may delete, from 1 to 10000\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for each database command on the server\. A value of 0 disables it\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP request\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The access key; defaults to the manager's\.

<a name='DiGi.GIS.WebAPI.Modify.RemoveYearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,bool,bool,int,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.YearBuiltDataRemoveResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.yearbuiltdataremoveresult 'DiGi\.GIS\.PostgreSQL\.Classes\.YearBuiltDataRemoveResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task returning the counts, or null when the request was refused or failed\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateBuildingDataYearBuiltAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken)'></a>

## Modify\.UpdateBuildingDataYearBuiltAsync\(this GISWebAPIManager, IEnumerable\<int\>, IEnumerable\<string\>, int, PostOptions, string, CancellationToken\) Method

Asynchronously recomputes the derived building data year built columns \- predicted, user and calculated \- of the given county parts from their stored history, through `gis/yearbuiltdata/updatebuildingdatabycountyids`, writing NULL where the history no longer holds a value\.

Needs the access key and `AllowUpdateBuildingData` on the host.

```csharp
public static System.Threading.Tasks.Task<DiGi.GIS.PostgreSQL.Classes.BuildingDataYearBuiltUpdateResult?> UpdateBuildingDataYearBuiltAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? gisWebAPIManager, System.Collections.Generic.IEnumerable<int>? countyIds, System.Collections.Generic.IEnumerable<string>? references=null, int commandTimeout=600, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateBuildingDataYearBuiltAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).gisWebAPIManager'></a>

`gisWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the Web API\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateBuildingDataYearBuiltAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts to recompute\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateBuildingDataYearBuiltAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The references of the buildings to recompute, or null for every building of the parts\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateBuildingDataYearBuiltAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for each database command on the server\. A value of 0 disables it\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateBuildingDataYearBuiltAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateBuildingDataYearBuiltAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The access key; defaults to the manager's\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateBuildingDataYearBuiltAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,System.Collections.Generic.IEnumerable_string_,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataYearBuiltUpdateResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.buildingdatayearbuiltupdateresult 'DiGi\.GIS\.PostgreSQL\.Classes\.BuildingDataYearBuiltUpdateResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task returning the counts, or null when the request was refused or failed\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],int,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, byte\[\], int, PostOptions, string\) Method

Asynchronously updates building items for one explicitly identified county row from an already\-serialized UTF\-8 JSON payload via the PostgreSQL Web API\.

For a county stored as several polygon parts, pass every part to the [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') overload instead - naming one part files the whole batch there whether or not the buildings belong to it.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, byte[]? utf8Json, int countyId, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],int,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],int,DiGi.WebAPI.Classes.PostOptions,string).utf8Json'></a>

`utf8Json` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The UTF\-8 encoded JSON array of [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],int,DiGi.WebAPI.Classes.PostOptions,string).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the buildings belong to\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],int,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],int,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],string,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, byte\[\], string, PostOptions, string\) Method

Asynchronously updates building items from an already\-serialized UTF\-8 JSON payload via the PostgreSQL Web API\.

Used by [BuildingsPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingsPostTask 'DiGi\.GIS\.WebAPI\.Classes\.BuildingsPostTask'), where the batch was serialized once while being sized, so it must not be serialized again here.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, byte[]? utf8Json, string? code=null, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],string,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],string,DiGi.WebAPI.Classes.PostOptions,string).utf8Json'></a>

`utf8Json` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The UTF\-8 encoded JSON array of [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],string,DiGi.WebAPI.Classes.PostOptions,string).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional code used for the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],string,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],string,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, byte\[\], IEnumerable\<int\>, PostOptions, string\) Method

Asynchronously updates building items for an explicitly identified county row from an already\-serialized UTF\-8 JSON payload via the PostgreSQL Web API\.

Used by [BuildingsPostTask](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.BuildingsPostTask 'DiGi\.GIS\.WebAPI\.Classes\.BuildingsPostTask'), where the batch was serialized once while being sized, so it must not be serialized again here.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, byte[]? utf8Json, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).utf8Json'></a>

`utf8Json` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The UTF\-8 encoded JSON array of [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the buildings belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,byte[],System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,int,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, Table, int, PostOptions, string\) Method

Asynchronously updates building data items for one explicitly identified county row via the PostgreSQL Web API\.

For a county stored as several polygon parts, pass every part to the [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') overload instead - naming one part files the whole batch there whether or not the buildings belong to it.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, DiGi.Core.IO.Table.Classes.Table? table, int countyId, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,int,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to facilitate the web API communication\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,int,DiGi.WebAPI.Classes.PostOptions,string).table'></a>

`table` [DiGi\.Core\.IO\.Table\.Classes\.Table](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.table.classes.table 'DiGi\.Core\.IO\.Table\.Classes\.Table')

The [DiGi\.Core\.IO\.Table\.Classes\.Table](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.table.classes.table 'DiGi\.Core\.IO\.Table\.Classes\.Table') containing the building data to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,int,DiGi.WebAPI.Classes.PostOptions,string).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the building data belongs to\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,int,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,int,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, Table, IEnumerable\<int\>, PostOptions, string\) Method

Asynchronously updates building data items for candidate county rows via the PostgreSQL Web API\.

A multi-part county holds one `administrative_areal_2d` row per polygon part, so pass every part rather than picking one - the server files each item under the part it belongs to.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, DiGi.Core.IO.Table.Classes.Table? table, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to facilitate the web API communication\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).table'></a>

`table` [DiGi\.Core\.IO\.Table\.Classes\.Table](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.table.classes.table 'DiGi\.Core\.IO\.Table\.Classes\.Table')

The [DiGi\.Core\.IO\.Table\.Classes\.Table](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.table.classes.table 'DiGi\.Core\.IO\.Table\.Classes\.Table') containing the building data to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the building data belongs to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.Core.IO.Table.Classes.Table,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.AdministrativeAreal2D,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, AdministrativeAreal2D, PostOptions, string\) Method

Updates a single administrative area item asynchronously\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, DiGi.GIS.Classes.AdministrativeAreal2D? administrativeAreal2D, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.AdministrativeAreal2D,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The GIS PostgreSQL Web API manager instance\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.AdministrativeAreal2D,DiGi.WebAPI.Classes.PostOptions,string).administrativeAreal2D'></a>

`administrativeAreal2D` [DiGi\.GIS\.Classes\.AdministrativeAreal2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.administrativeareal2d 'DiGi\.GIS\.Classes\.AdministrativeAreal2D')

The administrative area item to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.AdministrativeAreal2D,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.AdministrativeAreal2D,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,int,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, Building2D, int, PostOptions, string\) Method

Asynchronously updates a single 2D building item for an explicitly identified county row via the PostgreSQL Web API\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, DiGi.GIS.Classes.Building2D? building2D, int countyId, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,int,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to facilitate the web API communication\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,int,DiGi.WebAPI.Classes.PostOptions,string).building2D'></a>

`building2D` [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')

The [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') object to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,int,DiGi.WebAPI.Classes.PostOptions,string).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the building belongs to\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,int,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,int,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,string,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, Building2D, string, PostOptions, string\) Method

Asynchronously updates building items in the PostgreSQL GIS database via the web API\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, DiGi.GIS.Classes.Building2D? building2D, string? code=null, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,string,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to handle communication with the PostgreSQL Web API\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,string,DiGi.WebAPI.Classes.PostOptions,string).building2D'></a>

`building2D` [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')

The [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') object containing the data to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,string,DiGi.WebAPI.Classes.PostOptions,string).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional identification code associated with the update request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,string,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration settings for the HTTP POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,DiGi.GIS.Classes.Building2D,string,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, string, bool, bool, bool, SerializableObjectsPostOptions, IProgress\<long\>, Nullable\<CancellationToken\>, string\) Method

Asynchronously updates items from the specified file system path\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, string? path, bool oT_ADJA_A=true, bool oT_ADMS_A=true, bool oT_BUBD_A=true, DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions? serializableObjectsPostOptions=null, System.IProgress<long>? progress=null, System.Nullable<System.Threading.CancellationToken> cancellationToken=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The GIS PostgreSQL Web API manager instance\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string).path'></a>

`path` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The file system path to the source data files\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string).oT_ADJA_A'></a>

`oT_ADJA_A` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether items with the OT\_ADJA\_A suffix should be updated\. Defaults to `true`\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string).oT_ADMS_A'></a>

`oT_ADMS_A` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether items with the OT\_ADMS\_A suffix should be updated\. Defaults to `true`\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string).oT_BUBD_A'></a>

`oT_BUBD_A` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Indicates whether items with the OT\_BUBD\_A suffix should be updated\. Defaults to `true`\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string).serializableObjectsPostOptions'></a>

`serializableObjectsPostOptions` [SerializableObjectsPostOptions](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions 'DiGi\.GIS\.WebAPI\.Classes\.SerializableObjectsPostOptions')

The options used for serializing objects during the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string).progress'></a>

`progress` [System\.IProgress&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iprogress-1 'System\.IProgress\`1')

The progress reporter for reporting progress updates\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string).cancellationToken'></a>

`cancellationToken` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The cancellation token to observe\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,string,bool,bool,bool,DiGi.GIS.WebAPI.Classes.SerializableObjectsPostOptions,System.IProgress_long_,System.Nullable_System.Threading.CancellationToken_,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,int,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<BuildingModel\>, int, PostOptions, string\) Method

Asynchronously updates multiple building models for one explicitly identified county row via the PostgreSQL Web API\.

For a county stored as several polygon parts, pass every part to the [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') overload instead - naming one part files the whole batch there whether or not the models belong to it.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels, int countyId, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,int,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,int,DiGi.WebAPI.Classes.PostOptions,string).buildingModels'></a>

`buildingModels` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,int,DiGi.WebAPI.Classes.PostOptions,string).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the building models belong to\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,int,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,int,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<BuildingModel\>, string, PostOptions, string\) Method

Asynchronously updates multiple building models via the PostgreSQL Web API\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels, string? code=null, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.WebAPI.Classes.PostOptions,string).buildingModels'></a>

`buildingModels` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.WebAPI.Classes.PostOptions,string).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The administrative area code the building models belong to, resolved server\-side to a county identifier\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,string,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<BuildingModel\>, IEnumerable\<int\>, PostOptions, string\) Method

Asynchronously updates multiple building models for an explicitly identified county row via the PostgreSQL Web API\.

The counterpart of the `code` overload, for a caller that already holds the identifiers: a multi-part county holds one `administrative_areal_2d` row per polygon part, so pass every part rather than picking one - the server files each item under the part it belongs to.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.Analytical.Building.Classes.BuildingModel>? buildingModels, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).buildingModels'></a>

`buildingModels` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.Analytical\.Building\.Classes\.BuildingModel](https://learn.microsoft.com/en-us/dotnet/api/digi.analytical.building.classes.buildingmodel 'DiGi\.Analytical\.Building\.Classes\.BuildingModel') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the building models belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.Analytical.Building.Classes.BuildingModel_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<Building\>, int, PostOptions, string\) Method

Asynchronously updates multiple building items for one explicitly identified county row via the PostgreSQL Web API\.

For a county stored as several polygon parts, pass every part to the [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') overload instead - naming one part files the whole batch there whether or not the buildings belong to it.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.CityGML.Classes.Building>? buildings, int countyId, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.WebAPI.Classes.PostOptions,string).buildings'></a>

`buildings` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.WebAPI.Classes.PostOptions,string).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the buildings belong to\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,int,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<Building\>, string, PostOptions, string\) Method

Asynchronously updates multiple building items via the PostgreSQL Web API\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.CityGML.Classes.Building>? buildings, string? code=null, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.WebAPI.Classes.PostOptions,string).buildings'></a>

`buildings` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.WebAPI.Classes.PostOptions,string).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional code used for the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,string,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<Building\>, IEnumerable\<int\>, PostOptions, string\) Method

Asynchronously updates multiple building items for an explicitly identified county row via the PostgreSQL Web API\.

The counterpart of the `code` overload, for a caller that already holds the identifiers: a multi-part county holds one `administrative_areal_2d` row per polygon part, so pass every part rather than picking one - the server files each item under the part it belongs to.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.CityGML.Classes.Building>? buildings, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).buildings'></a>

`buildings` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.CityGML\.Classes\.Building](https://learn.microsoft.com/en-us/dotnet/api/digi.citygml.classes.building 'DiGi\.CityGML\.Classes\.Building') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the buildings belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.CityGML.Classes.Building_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.EPW.Classes.EPWFile_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<EPWFile\>, PostOptions, string\) Method

Asynchronously updates a collection of EPW files\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.EPW.Classes.EPWFile>? ePWFiles, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.EPW.Classes.EPWFile_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The GIS PostgreSQL Web API manager instance\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.EPW.Classes.EPWFile_,DiGi.WebAPI.Classes.PostOptions,string).ePWFiles'></a>

`ePWFiles` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.EPW\.Classes\.EPWFile](https://learn.microsoft.com/en-us/dotnet/api/digi.epw.classes.epwfile 'DiGi\.EPW\.Classes\.EPWFile')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of EPW files to update\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.EPW.Classes.EPWFile_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.EPW.Classes.EPWFile_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AdministrativeAreal2D_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<AdministrativeAreal2D\>, PostOptions, string\) Method

Asynchronously updates a collection of administrative area items\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.AdministrativeAreal2D>? administrativeAreal2Ds, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AdministrativeAreal2D_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The GIS PostgreSQL Web API manager instance\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AdministrativeAreal2D_,DiGi.WebAPI.Classes.PostOptions,string).administrativeAreal2Ds'></a>

`administrativeAreal2Ds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.AdministrativeAreal2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.administrativeareal2d 'DiGi\.GIS\.Classes\.AdministrativeAreal2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of administrative area items to update\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AdministrativeAreal2D_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.AdministrativeAreal2D_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<Building2D\>, int, PostOptions, string\) Method

Asynchronously updates multiple 2D building items for one explicitly identified county row via the PostgreSQL Web API\.

For a county stored as several polygon parts, pass every part to the [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') overload instead - naming one part files the whole batch there whether or not the buildings belong to it.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.Building2D>? building2Ds, int countyId, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to facilitate the web API communication\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.WebAPI.Classes.PostOptions,string).building2Ds'></a>

`building2Ds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') objects to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.WebAPI.Classes.PostOptions,string).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the buildings belong to\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,int,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<Building2D\>, string, PostOptions, string\) Method

Updates multiple 2D building items asynchronously\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.Building2D>? building2Ds, string? code=null, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The GIS PostgreSQL Web API manager instance\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.WebAPI.Classes.PostOptions,string).building2Ds'></a>

`building2Ds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.WebAPI.Classes.PostOptions,string).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional code used for the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,string,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<Building2D\>, IEnumerable\<int\>, PostOptions, string\) Method

Asynchronously updates multiple 2D building items for an explicitly identified county row via the PostgreSQL Web API\.

The counterpart of the `code` overload, for a caller that already holds the identifiers: a multi-part county holds one `administrative_areal_2d` row per polygon part, so pass every part rather than picking one - the server files each item under the part it belongs to.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.Building2D>? building2Ds, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to facilitate the web API communication\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).building2Ds'></a>

`building2Ds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.Building2D](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.building2d 'DiGi\.GIS\.Classes\.Building2D') objects to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the buildings belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.Building2D_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<OccupancyData\>, PostOptions, string\) Method

Asynchronously updates multiple occupancy data items via the PostgreSQL Web API\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.OccupancyData>? occupancyDatas, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to facilitate the API communication\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,DiGi.WebAPI.Classes.PostOptions,string).occupancyDatas'></a>

`occupancyDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of [DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,int,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<OccupancyData\>, int, PostOptions, string\) Method

Asynchronously updates multiple building 2D occupancy data items for one explicitly identified county row via the Web API\.

For a county stored as several polygon parts, pass every part to the [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') overload instead - naming one part files the whole batch there whether or not the data belong to it.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.OccupancyData>? occupancyDatas, int countyId, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,int,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to create the HTTP client for the request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,int,DiGi.WebAPI.Classes.PostOptions,string).occupancyDatas'></a>

`occupancyDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,int,DiGi.WebAPI.Classes.PostOptions,string).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the occupancy data belong to\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,int,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration settings for the HTTP POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,int,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,string,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<OccupancyData\>, string, PostOptions, string\) Method

Asynchronously updates multiple occupancy data items via the Web API\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.OccupancyData>? occupancyDatas, string? code=null, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,string,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to create the HTTP client for the request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,string,DiGi.WebAPI.Classes.PostOptions,string).occupancyDatas'></a>

`occupancyDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,string,DiGi.WebAPI.Classes.PostOptions,string).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional code associated with the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,string,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration settings for the HTTP POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,string,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<OccupancyData\>, IEnumerable\<int\>, PostOptions, string\) Method

Asynchronously updates multiple building 2D occupancy data items for an explicitly identified county row via the Web API\.

The counterpart of the `code` overload, for a caller that already holds the identifiers: a multi-part county holds one `administrative_areal_2d` row per polygon part, so pass every part rather than picking one - the server files each item under the part it belongs to.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.OccupancyData>? occupancyDatas, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to create the HTTP client for the request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).occupancyDatas'></a>

`occupancyDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.OccupancyData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.occupancydata 'DiGi\.GIS\.Classes\.OccupancyData') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the occupancy data belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration settings for the HTTP POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OccupancyData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,int,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<OrtoDatas\>, int, PostOptions, string\) Method

Updates multiple ortho data items for one explicitly identified county row\.

For a county stored as several polygon parts, pass every part to the [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') overload instead - naming one part files the whole batch there whether or not the data belong to it.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.OrtoDatas>? ortoDatas, int countyId, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,int,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,int,DiGi.WebAPI.Classes.PostOptions,string).ortoDatas'></a>

`ortoDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of [DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,int,DiGi.WebAPI.Classes.PostOptions,string).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the items belong to\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,int,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,int,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,string,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<OrtoDatas\>, string, PostOptions, string\) Method

Asynchronously updates multiple ortho data items via the PostgreSQL Web API\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.OrtoDatas>? ortoDatas, string? code=null, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,string,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,string,DiGi.WebAPI.Classes.PostOptions,string).ortoDatas'></a>

`ortoDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,string,DiGi.WebAPI.Classes.PostOptions,string).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional code used to identify or filter the items for updating\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,string,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the update request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,string,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<OrtoDatas\>, IEnumerable\<int\>, PostOptions, string\) Method

Updates multiple ortho data items for a specific county\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.OrtoDatas>? ortoDatas, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to perform the update operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).ortoDatas'></a>

`ortoDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of [DiGi\.GIS\.Classes\.OrtoDatas](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.ortodatas 'DiGi\.GIS\.Classes\.OrtoDatas') items to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the items belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.OrtoDatas_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,int,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<YearBuiltData\>, int, PostOptions, string\) Method

Asynchronously updates multiple year built data items for one explicitly identified county row via the PostgreSQL Web API\.

For a county stored as several polygon parts, pass every part to the [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') overload instead - naming one part files the whole batch there whether or not the data belong to it.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.YearBuiltData>? yearBuiltDatas, int countyId, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,int,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to facilitate the web API communication\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,int,DiGi.WebAPI.Classes.PostOptions,string).yearBuiltDatas'></a>

`yearBuiltDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData') objects to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,int,DiGi.WebAPI.Classes.PostOptions,string).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the county row the year built data belong to\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,int,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,int,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,string,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<YearBuiltData\>, string, PostOptions, string\) Method

Asynchronously updates multiple year built data items via the PostgreSQL Web API\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.YearBuiltData>? yearBuiltDatas, string? code=null, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,string,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to facilitate the web API communication\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,string,DiGi.WebAPI.Classes.PostOptions,string).yearBuiltDatas'></a>

`yearBuiltDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData') objects to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,string,DiGi.WebAPI.Classes.PostOptions,string).code'></a>

`code` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

An optional code identifier for the update request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,string,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,string,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this GISWebAPIManager, IEnumerable\<YearBuiltData\>, IEnumerable\<int\>, PostOptions, string\) Method

Asynchronously updates multiple year built data items for an explicitly identified county row via the PostgreSQL Web API\.

The counterpart of the `code` overload, for a caller that already holds the identifiers: a multi-part county holds one `administrative_areal_2d` row per polygon part, so pass every part rather than picking one - the server files each item under the part it belongs to.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? GISWebAPIManager, System.Collections.Generic.IEnumerable<DiGi.GIS.Classes.YearBuiltData>? yearBuiltDatas, System.Collections.Generic.IEnumerable<int>? countyIds, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).GISWebAPIManager'></a>

`GISWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The manager instance used to facilitate the web API communication\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).yearBuiltDatas'></a>

`yearBuiltDatas` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

A collection of [DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData') objects to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The identifiers of the county rows the year built data belong to\. Normally every polygon part of one county\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the POST request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_DiGi.GIS.Classes.YearBuiltData_,System.Collections.Generic.IEnumerable_int_,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,byte[],DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this HttpClient, string, byte\[\], PostOptions, string\) Method

Asynchronously updates items by sending an already\-serialized UTF\-8 JSON payload to the specified request URI\.

Preferred over the string overload on bulk paths: the payload never has to be materialized as a UTF-16 string and re-encoded back to UTF-8.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this System.Net.Http.HttpClient httpClient, string? requestUri, byte[]? utf8Json, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,byte[],DiGi.WebAPI.Classes.PostOptions,string).httpClient'></a>

`httpClient` [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient')

The [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') used to perform the network request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,byte[],DiGi.WebAPI.Classes.PostOptions,string).requestUri'></a>

`requestUri` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The target URI where the update request is sent\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,byte[],DiGi.WebAPI.Classes.PostOptions,string).utf8Json'></a>

`utf8Json` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The UTF\-8 encoded JSON payload containing the data to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,byte[],DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions') used to configure the operation, such as specifying a delay\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,byte[],DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,string,DiGi.WebAPI.Classes.PostOptions,string)'></a>

## Modify\.UpdateItemsAsync\(this HttpClient, string, string, PostOptions, string\) Method

Asynchronously updates items by sending a JSON payload to the specified request URI\.

```csharp
public static System.Threading.Tasks.Task<bool> UpdateItemsAsync(this System.Net.Http.HttpClient httpClient, string? requestUri, string? json, DiGi.WebAPI.Classes.PostOptions? postOptions=null, string? key=null);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,string,DiGi.WebAPI.Classes.PostOptions,string).httpClient'></a>

`httpClient` [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient')

The [System\.Net\.Http\.HttpClient](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient 'System\.Net\.Http\.HttpClient') used to perform the network request\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,string,DiGi.WebAPI.Classes.PostOptions,string).requestUri'></a>

`requestUri` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The target URI where the update request is sent\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,string,DiGi.WebAPI.Classes.PostOptions,string).json'></a>

`json` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The JSON string containing the data to be updated\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,string,DiGi.WebAPI.Classes.PostOptions,string).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions') used to configure the operation, such as specifying a delay\.

<a name='DiGi.GIS.WebAPI.Modify.UpdateItemsAsync(thisSystem.Net.Http.HttpClient,string,string,DiGi.WebAPI.Classes.PostOptions,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The optional API authorization key used for authenticating requests to protected endpoints\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\.

<a name='DiGi.GIS.WebAPI.Modify.Update_Id(thisDiGi.Core.IO.Table.Classes.Table,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DReference_)'></a>

## Modify\.Update\_Id\(this Table, IEnumerable\<Building2DReference\>\) Method

Updates the Id column of the table based on the provided building2DReferences\. If a matching row is found \(based on CountyId and Reference\), it updates the Id value\. If no matching row is found, it adds a new row with the CountyId, Reference, and Id values\.

```csharp
public static void Update_Id(this DiGi.Core.IO.Table.Classes.Table? table, System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.Building2DReference>? building2DReferences);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Modify.Update_Id(thisDiGi.Core.IO.Table.Classes.Table,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DReference_).table'></a>

`table` [DiGi\.Core\.IO\.Table\.Classes\.Table](https://learn.microsoft.com/en-us/dotnet/api/digi.core.io.table.classes.table 'DiGi\.Core\.IO\.Table\.Classes\.Table')

The table to update

<a name='DiGi.GIS.WebAPI.Modify.Update_Id(thisDiGi.Core.IO.Table.Classes.Table,System.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.Building2DReference_).building2DReferences'></a>

`building2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.building2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.Building2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The building2DReferences to use for updating

<a name='DiGi.GIS.WebAPI.Query'></a>

## Query Class

```csharp
public static class Query
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Query
### Methods

<a name='DiGi.GIS.WebAPI.Query.CountByAdministrativeAreal2DIdAsync(int,DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference__,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.List_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference___,System.Func_DiGi.PostgreSQL.Table.Classes.FilterGroup,int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Nullable_long___,int,System.Threading.CancellationToken)'></a>

## Query\.CountByAdministrativeAreal2DIdAsync\(int, FilterGroup, Func\<int,CancellationToken,Task\<AdministrativeAreal2DReference\>\>, Func\<int,CancellationToken,Task\<List\<AdministrativeAreal2DReference\>\>\>, Func\<FilterGroup,int,CancellationToken,Task\<Nullable\<long\>\>\>, int, CancellationToken\) Method

Asynchronously counts the building data rows of an administrative area, optionally narrowed by a filter group\.

An area above the subdivision level is resolved to its Subdivision children, which are grouped by the county partition they sit in; each county is then counted with `subdivision_id IN (that county's subdivisions)` plus the caller's filters, and the counts are summed. A Subdivision is counted by membership of its own id, so a nested subdivision counts only the buildings filed under that exact id (DiGi.GIS.PostgreSQL#75).

Every step is awaited in turn - the reads are never fanned out, because concurrent requests exhaust the Npgsql pool. The three steps are taken as delegates so the orchestration is separate from the converters that perform them.

```csharp
public static System.Threading.Tasks.Task<(DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus Status,long Count)> CountByAdministrativeAreal2DIdAsync(int administrativeAreal2DId, DiGi.PostgreSQL.Table.Classes.FilterGroup? filterGroup, System.Func<int,System.Threading.CancellationToken,System.Threading.Tasks.Task<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference?>> readReferenceAsync, System.Func<int,System.Threading.CancellationToken,System.Threading.Tasks.Task<System.Collections.Generic.List<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>?>> readSubdivisionsAsync, System.Func<DiGi.PostgreSQL.Table.Classes.FilterGroup,int,System.Threading.CancellationToken,System.Threading.Tasks.Task<System.Nullable<long>>> countAsync, int maximumCountyCount, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Query.CountByAdministrativeAreal2DIdAsync(int,DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference__,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.List_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference___,System.Func_DiGi.PostgreSQL.Table.Classes.FilterGroup,int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Nullable_long___,int,System.Threading.CancellationToken).administrativeAreal2DId'></a>

`administrativeAreal2DId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The identifier of the administrative area\.

<a name='DiGi.GIS.WebAPI.Query.CountByAdministrativeAreal2DIdAsync(int,DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference__,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.List_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference___,System.Func_DiGi.PostgreSQL.Table.Classes.FilterGroup,int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Nullable_long___,int,System.Threading.CancellationToken).filterGroup'></a>

`filterGroup` [DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')

The optional filters applied on top of the area\.

<a name='DiGi.GIS.WebAPI.Query.CountByAdministrativeAreal2DIdAsync(int,DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference__,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.List_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference___,System.Func_DiGi.PostgreSQL.Table.Classes.FilterGroup,int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Nullable_long___,int,System.Threading.CancellationToken).readReferenceAsync'></a>

`readReferenceAsync` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

Reads the area reference by identifier; returns null when there is none\.

<a name='DiGi.GIS.WebAPI.Query.CountByAdministrativeAreal2DIdAsync(int,DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference__,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.List_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference___,System.Func_DiGi.PostgreSQL.Table.Classes.FilterGroup,int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Nullable_long___,int,System.Threading.CancellationToken).readSubdivisionsAsync'></a>

`readSubdivisionsAsync` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

Reads the Subdivision references below the area, given the area identifier\.

<a name='DiGi.GIS.WebAPI.Query.CountByAdministrativeAreal2DIdAsync(int,DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference__,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.List_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference___,System.Func_DiGi.PostgreSQL.Table.Classes.FilterGroup,int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Nullable_long___,int,System.Threading.CancellationToken).countAsync'></a>

`countAsync` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-4 'System\.Func\`4')[DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup](https://learn.microsoft.com/en-us/dotnet/api/digi.postgresql.table.classes.filtergroup 'DiGi\.PostgreSQL\.Table\.Classes\.FilterGroup')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-4 'System\.Func\`4')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-4 'System\.Func\`4')[System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-4 'System\.Func\`4')[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-4 'System\.Func\`4')

Counts the rows of one county partition matching a filter group, given the filter group and the county identifier; returns null when the count could not be made\.

<a name='DiGi.GIS.WebAPI.Query.CountByAdministrativeAreal2DIdAsync(int,DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference__,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.List_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference___,System.Func_DiGi.PostgreSQL.Table.Classes.FilterGroup,int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Nullable_long___,int,System.Threading.CancellationToken).maximumCountyCount'></a>

`maximumCountyCount` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The largest number of counties that may be iterated; exceeding it counts nothing\.

<a name='DiGi.GIS.WebAPI.Query.CountByAdministrativeAreal2DIdAsync(int,DiGi.PostgreSQL.Table.Classes.FilterGroup,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference__,System.Func_int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Collections.Generic.List_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference___,System.Func_DiGi.PostgreSQL.Table.Classes.FilterGroup,int,System.Threading.CancellationToken,System.Threading.Tasks.Task_System.Nullable_long___,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple 'System\.ValueTuple')[AdministrativeAreal2DCountStatus](DiGi.GIS.WebAPI.Enums.md#DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus 'DiGi\.GIS\.WebAPI\.Enums\.AdministrativeAreal2DCountStatus')[,](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple 'System\.ValueTuple')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.valuetuple 'System\.ValueTuple')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that represents the asynchronous operation\. The result carries how the count ended and, for [Counted](DiGi.GIS.WebAPI.Enums.md#DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.Counted 'DiGi\.GIS\.WebAPI\.Enums\.AdministrativeAreal2DCountStatus\.Counted'), the summed count\.

<a name='DiGi.GIS.WebAPI.Query.GetUserEmail(DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,string)'></a>

## Query\.GetUserEmail\(SecurityKeyManager, TokenRevocationStore, string\) Method

Validates a bearer token issued by the user extension and returns the identity it carries\.

Denies by default: a missing [DiGi\.WebAPI\.Classes\.SecurityKeyManager](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.securitykeymanager 'DiGi\.WebAPI\.Classes\.SecurityKeyManager') or [DiGi\.WebAPI\.Classes\.TokenRevocationStore](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.tokenrevocationstore 'DiGi\.WebAPI\.Classes\.TokenRevocationStore') (a GIS-only host), a missing or malformed `Authorization` header, a failed signature or lifetime check, or a revoked `jti` all answer `null`. Callers map `null` to HTTP 401.

Validation mirrors the user extension's `TokenValidationParameters` - issuer and audience unchecked, the signature checked against every key the manager holds, the lifetime checked - so both extensions agree on which tokens are valid.

```csharp
public static string? GetUserEmail(DiGi.WebAPI.Classes.SecurityKeyManager? securityKeyManager, DiGi.WebAPI.Classes.TokenRevocationStore? tokenRevocationStore, string? authorizationHeader);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Query.GetUserEmail(DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,string).securityKeyManager'></a>

`securityKeyManager` [DiGi\.WebAPI\.Classes\.SecurityKeyManager](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.securitykeymanager 'DiGi\.WebAPI\.Classes\.SecurityKeyManager')

The user extension's security key manager; `null` on a GIS\-only host\.

<a name='DiGi.GIS.WebAPI.Query.GetUserEmail(DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,string).tokenRevocationStore'></a>

`tokenRevocationStore` [DiGi\.WebAPI\.Classes\.TokenRevocationStore](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.tokenrevocationstore 'DiGi\.WebAPI\.Classes\.TokenRevocationStore')

The user extension's token revocation store; `null` on a GIS\-only host\.

<a name='DiGi.GIS.WebAPI.Query.GetUserEmail(DiGi.WebAPI.Classes.SecurityKeyManager,DiGi.WebAPI.Classes.TokenRevocationStore,string).authorizationHeader'></a>

`authorizationHeader` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The value of the request's `Authorization` header, or `null`\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The [System\.Security\.Claims\.ClaimTypes\.Email](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes.email 'System\.Security\.Claims\.ClaimTypes\.Email') the token carries, or `null` on every denial path\.

<a name='DiGi.GIS.WebAPI.Query.IsAuthorized(thisDiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,string)'></a>

## Query\.IsAuthorized\(this GISWebAPIConfigurationFileWatcher, string\) Method

Determines whether a provided access key is authorized according to the GIS PostgreSQL Web API configuration\.

Denies by default: a missing configuration watcher, disabled enforcement, a blank configured key or a blank supplied key all deny access. The only way to perform write operations without a key is the explicit [Open](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher.Open 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher\.Open') opt-out.

```csharp
public static bool IsAuthorized(this DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher? gISWebAPIConfigurationFileWatcher, string? key);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Query.IsAuthorized(thisDiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,string).gISWebAPIConfigurationFileWatcher'></a>

`gISWebAPIConfigurationFileWatcher` [GISWebAPIConfigurationFileWatcher](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIConfigurationFileWatcher')

The GIS PostgreSQL Web API configuration file watcher to validate against\.

<a name='DiGi.GIS.WebAPI.Query.IsAuthorized(thisDiGi.GIS.WebAPI.Classes.GISWebAPIConfigurationFileWatcher,string).key'></a>

`key` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The access key to validate\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
True if access is authorized; otherwise, false\.

<a name='DiGi.GIS.WebAPI.Query.PredictedYearBuiltRunsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken)'></a>

## Query\.PredictedYearBuiltRunsAsync\(this GISWebAPIManager, IEnumerable\<int\>, int, PostOptions, CancellationToken\) Method

Asynchronously lists the prediction runs stored under the given county parts through `gis/yearbuiltdata/predictedyearbuiltruns`: one entry per part, stamp and model identifier, with the number of objects carrying it\.

The stamp is [System\.DateTime\.Ticks](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.ticks 'System\.DateTime\.Ticks'), the value [RemovePredictedYearBuiltsAsync\(this GISWebAPIManager, IEnumerable&lt;int&gt;, long, IEnumerable&lt;string&gt;, bool, int, bool, int, PostOptions, string, CancellationToken\)](DiGi.GIS.WebAPI.md#DiGi.GIS.WebAPI.Modify.RemovePredictedYearBuiltsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,long,System.Collections.Generic.IEnumerable_string_,bool,int,bool,int,DiGi.WebAPI.Classes.PostOptions,string,System.Threading.CancellationToken) 'DiGi\.GIS\.WebAPI\.Modify\.RemovePredictedYearBuiltsAsync\(this DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager, System\.Collections\.Generic\.IEnumerable\<int\>, long, System\.Collections\.Generic\.IEnumerable\<string\>, bool, int, bool, int, DiGi\.WebAPI\.Classes\.PostOptions, string, System\.Threading\.CancellationToken\)') takes. An empty list means no prediction is stored; null means the request failed.

```csharp
public static System.Threading.Tasks.Task<System.Collections.Generic.List<DiGi.GIS.PostgreSQL.Classes.PredictedYearBuiltRunResult>?> PredictedYearBuiltRunsAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? gisWebAPIManager, System.Collections.Generic.IEnumerable<int>? countyIds, int commandTimeout=600, DiGi.WebAPI.Classes.PostOptions? postOptions=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Query.PredictedYearBuiltRunsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).gisWebAPIManager'></a>

`gisWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the Web API\.

<a name='DiGi.GIS.WebAPI.Query.PredictedYearBuiltRunsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).countyIds'></a>

`countyIds` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The county parts to list\.

<a name='DiGi.GIS.WebAPI.Query.PredictedYearBuiltRunsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the query on the server\. A value of 0 disables it\.

<a name='DiGi.GIS.WebAPI.Query.PredictedYearBuiltRunsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP request\.

<a name='DiGi.GIS.WebAPI.Query.PredictedYearBuiltRunsAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_int_,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRunResult](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.predictedyearbuiltrunresult 'DiGi\.GIS\.PostgreSQL\.Classes\.PredictedYearBuiltRunResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task returning the runs, or null when the request failed\.

<a name='DiGi.GIS.WebAPI.Query.RejectionSample(thisSystem.Collections.Generic.IEnumerable_DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection_,int)'></a>

## Query\.RejectionSample\(this IEnumerable\<Rejection\>, int\) Method

Renders the first rejections of a write as a readable log fragment\.

A count alone does not say what to do about a shortfall - the references identify the rows to repost, and the reason says whether reposting them unchanged would achieve anything.

```csharp
public static string RejectionSample(this System.Collections.Generic.IEnumerable<DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection>? rejections, int count=20);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Query.RejectionSample(thisSystem.Collections.Generic.IEnumerable_DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection_,int).rejections'></a>

`rejections` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[Rejection](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection 'DiGi\.GIS\.WebAPI\.Classes\.UpdateItemsResult\.Rejection')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The rejections to render; may be null\.

<a name='DiGi.GIS.WebAPI.Query.RejectionSample(thisSystem.Collections.Generic.IEnumerable_DiGi.GIS.WebAPI.Classes.UpdateItemsResult.Rejection_,int).count'></a>

`count` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of rejections to include\. Defaults to 20\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A comma\-separated list of `reference (reason)` entries, empty when there is nothing to render\.

<a name='DiGi.GIS.WebAPI.Query.SubdivisionIdsByCountyId(thisSystem.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_)'></a>

## Query\.SubdivisionIdsByCountyId\(this IEnumerable\<AdministrativeAreal2DReference\>\) Method

Groups the identifiers of subdivision references by the county partition they sit in\.

The groups come out in the order a county was first met, so a caller that caps or iterates them does so in a stable, documented order. References without a county identifier, and identifiers already seen, are skipped.

```csharp
public static System.Collections.Generic.Dictionary<int,System.Collections.Generic.List<int>> SubdivisionIdsByCountyId(this System.Collections.Generic.IEnumerable<DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference>? administrativeAreal2DReferences);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Query.SubdivisionIdsByCountyId(thisSystem.Collections.Generic.IEnumerable_DiGi.GIS.PostgreSQL.Classes.AdministrativeAreal2DReference_).administrativeAreal2DReferences'></a>

`administrativeAreal2DReferences` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.postgresql.classes.administrativeareal2dreference 'DiGi\.GIS\.PostgreSQL\.Classes\.AdministrativeAreal2DReference')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The subdivision references to group\.

#### Returns
[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')  
The subdivision identifiers keyed by county identifier; empty when there is nothing to group\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataCountAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,bool,bool,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken)'></a>

## Query\.YearBuiltDataCountAsync\(this GISWebAPIManager, int, bool, bool, int, PostOptions, CancellationToken\) Method

Asynchronously retrieves the number of year built data items stored for a specified county identifier\.

```csharp
public static System.Threading.Tasks.Task<System.Nullable<long>> YearBuiltDataCountAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? gisWebAPIManager, int countyId, bool estimated=false, bool analyze=false, int commandTimeout=600, DiGi.WebAPI.Classes.PostOptions? postOptions=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataCountAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,bool,bool,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).gisWebAPIManager'></a>

`gisWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the Web API\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataCountAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,bool,bool,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the county\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataCountAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,bool,bool,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).estimated'></a>

`estimated` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to return an estimated count from table statistics rather than an exact count\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataCountAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,bool,bool,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).analyze'></a>

`analyze` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to perform an ANALYZE operation before reading the estimate\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataCountAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,bool,bool,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout\. Defaults to 600 seconds\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataCountAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,bool,bool,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP request\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataCountAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,bool,bool,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task returning the record count, or null when the county has no partition or the request failed\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataReferencesAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken)'></a>

## Query\.YearBuiltDataReferencesAsync\(this GISWebAPIManager, int, int, PostOptions, CancellationToken\) Method

Asynchronously retrieves the building references that carry stored year built data for a specified county identifier\.

```csharp
public static System.Threading.Tasks.Task<System.Collections.Generic.HashSet<string>?> YearBuiltDataReferencesAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? gisWebAPIManager, int countyId, int commandTimeout=30, DiGi.WebAPI.Classes.PostOptions? postOptions=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataReferencesAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).gisWebAPIManager'></a>

`gisWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the Web API\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataReferencesAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The unique identifier of the county\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataReferencesAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).commandTimeout'></a>

`commandTimeout` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The timeout in seconds for the execution of the command\. A value of 0 disables the timeout\. Defaults to 30 seconds\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataReferencesAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP request\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDataReferencesAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,int,int,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.HashSet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task returning the set of reference strings, or null when the request failed or no partition was found\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken)'></a>

## Query\.YearBuiltDatasAsync\(this GISWebAPIManager, IEnumerable\<string\>, Nullable\<int\>, bool, PostOptions, CancellationToken\) Method

Asynchronously retrieves year built data items for the specified references, optionally filtered by county identifier\.

If the number of references exceeds the endpoint's maximum request limit (10,000), requests are automatically partitioned into batches.

```csharp
public static System.Threading.Tasks.Task<System.Collections.Generic.List<DiGi.GIS.Classes.YearBuiltData>?> YearBuiltDatasAsync(this DiGi.GIS.WebAPI.Classes.GISWebAPIManager? gisWebAPIManager, System.Collections.Generic.IEnumerable<string>? references, System.Nullable<int> countyId=null, bool fallbackByReference=false, DiGi.WebAPI.Classes.PostOptions? postOptions=null, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken));
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).gisWebAPIManager'></a>

`gisWebAPIManager` [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager')

The [GISWebAPIManager](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.GISWebAPIManager 'DiGi\.GIS\.WebAPI\.Classes\.GISWebAPIManager') instance used to communicate with the Web API\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).references'></a>

`references` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection of unique reference strings to retrieve\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).countyId'></a>

`countyId` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

An optional integer representing the county identifier to filter the results\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).fallbackByReference'></a>

`fallbackByReference` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

A boolean value indicating whether to perform a fallback search by reference alone if not found under the specified county\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).postOptions'></a>

`postOptions` [DiGi\.WebAPI\.Classes\.PostOptions](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.postoptions 'DiGi\.WebAPI\.Classes\.PostOptions')

Optional configuration options for the HTTP request\.

<a name='DiGi.GIS.WebAPI.Query.YearBuiltDatasAsync(thisDiGi.GIS.WebAPI.Classes.GISWebAPIManager,System.Collections.Generic.IEnumerable_string_,System.Nullable_int_,bool,DiGi.WebAPI.Classes.PostOptions,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

The [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken') to observe while waiting for the task to complete\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[DiGi\.GIS\.Classes\.YearBuiltData](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.classes.yearbuiltdata 'DiGi\.GIS\.Classes\.YearBuiltData')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task returning the list of year built data items, or null when the request failed\.