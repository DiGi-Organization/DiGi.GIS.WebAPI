#### [DiGi\.GIS\.WebAPI](DiGi.GIS.WebAPI.Overview.md 'DiGi\.GIS\.WebAPI\.Overview')

## DiGi\.GIS\.WebAPI\.Classes\.Parameter Namespace
### Classes

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter'></a>

## BuildingShapeLabelParameter Class

The body of the `gis/buildingshape/label` write: one signed\-in annotator's building shape label of one sample item \(DiGi\.GIS\.WebAPI\#80\)\.

Every member is bound nullable so an omitted field is representable and can be rejected explicitly. [Shape](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.Shape 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.BuildingShapeLabelParameter\.Shape') is the integer value of [DiGi\.GIS\.Enums\.BuildingShape](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.enums.buildingshape 'DiGi\.GIS\.Enums\.BuildingShape'), where `0` ([DiGi\.GIS\.Enums\.BuildingShape\.Undefined](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.enums.buildingshape.undefined 'DiGi\.GIS\.Enums\.BuildingShape\.Undefined')) records that the footprint could not be classified. The interpreter and the timestamp are never taken from this body: they are stamped server-side from the bearer token and the clock.

```csharp
public class BuildingShapeLabelParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → BuildingShapeLabelParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.BuildingShapeLabelParameter()'></a>

## BuildingShapeLabelParameter\(\) Constructor

Initializes a new instance of the [BuildingShapeLabelParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.BuildingShapeLabelParameter') class\.

```csharp
public BuildingShapeLabelParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.BuildingShapeLabelParameter(DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter)'></a>

## BuildingShapeLabelParameter\(BuildingShapeLabelParameter\) Constructor

Initializes a new instance of the [BuildingShapeLabelParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.BuildingShapeLabelParameter') class by copying the values from an existing instance\.

```csharp
public BuildingShapeLabelParameter(DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter buildingShapeLabelParameter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.BuildingShapeLabelParameter(DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter).buildingShapeLabelParameter'></a>

`buildingShapeLabelParameter` [BuildingShapeLabelParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.BuildingShapeLabelParameter')

The source instance from which to copy the values\.

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.BuildingShapeLabelParameter(System.Text.Json.Nodes.JsonObject)'></a>

## BuildingShapeLabelParameter\(JsonObject\) Constructor

Initializes a new instance of the [BuildingShapeLabelParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.BuildingShapeLabelParameter') class using the provided JSON object\.

```csharp
public BuildingShapeLabelParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.BuildingShapeLabelParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.CountyId'></a>

## BuildingShapeLabelParameter\.CountyId Property

Gets or sets the county id \(not code\) the item is filed under; `null` \(an omitted field\) is rejected\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.Note'></a>

## BuildingShapeLabelParameter\.Note Property

Gets or sets an optional free\-text note\.

```csharp
public string? Note { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.Reference'></a>

## BuildingShapeLabelParameter\.Reference Property

Gets or sets the building reference of the item; a blank value is rejected\.

```csharp
public string? Reference { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.SampleKey'></a>

## BuildingShapeLabelParameter\.SampleKey Property

Gets or sets the identity of the sample the labelled item belongs to; a blank value is rejected\.

```csharp
public string? SampleKey { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.BuildingShapeLabelParameter.Shape'></a>

## BuildingShapeLabelParameter\.Shape Property

Gets or sets the label as the integer value of [DiGi\.GIS\.Enums\.BuildingShape](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.enums.buildingshape 'DiGi\.GIS\.Enums\.BuildingShape'); `null` and values outside the enumeration are rejected\.

```csharp
public System.Nullable<short> Shape { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter'></a>

## OrtoAreaLabelParameter Class

The body of the `gis/ortoarea/label` write: one interpreter's reading of one epoch of one object in a full\-interpretation test area\.

Every member is bound nullable so an omitted field is representable and can be rejected explicitly - the object identity is exactly one of [Reference](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.Reference 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter\.Reference') (a footprint) or [ObjectId](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.ObjectId 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter\.ObjectId') (a drawn object), with [CountyId](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.CountyId 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter\.CountyId') only an optional narrowing hint, and [State](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.State 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter\.State') is checked against 0/1/2 rather than compared to a sentinel. The interpreter identity and the observation dates are never taken from this body: they are resolved server-side from the bearer token, the stored object geometry and the orthophoto sheet index.

```csharp
public class OrtoAreaLabelParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → OrtoAreaLabelParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.OrtoAreaLabelParameter()'></a>

## OrtoAreaLabelParameter\(\) Constructor

Initializes a new instance of the [OrtoAreaLabelParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter') class\.

```csharp
public OrtoAreaLabelParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.OrtoAreaLabelParameter(DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter)'></a>

## OrtoAreaLabelParameter\(OrtoAreaLabelParameter\) Constructor

Initializes a new instance of the [OrtoAreaLabelParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter') class by copying the values from an existing instance\.

```csharp
public OrtoAreaLabelParameter(DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter ortoAreaLabelParameter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.OrtoAreaLabelParameter(DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter).ortoAreaLabelParameter'></a>

`ortoAreaLabelParameter` [OrtoAreaLabelParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter')

The source instance from which to copy the values\.

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.OrtoAreaLabelParameter(System.Text.Json.Nodes.JsonObject)'></a>

## OrtoAreaLabelParameter\(JsonObject\) Constructor

Initializes a new instance of the [OrtoAreaLabelParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter') class using the provided JSON object\.

```csharp
public OrtoAreaLabelParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.OrtoAreaLabelParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.CountyId'></a>

## OrtoAreaLabelParameter\.CountyId Property

Gets or sets the county part of the footprint identity as an optional narrowing hint; resolved server\-side from [Reference](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.Reference 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter\.Reference') when omitted, and the resolved part is what files the label row\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.EpochYear'></a>

## OrtoAreaLabelParameter\.EpochYear Property

Gets or sets the imagery epoch the label interprets; `null` \(an omitted value\) is rejected\.

```csharp
public System.Nullable<short> EpochYear { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.Note'></a>

## OrtoAreaLabelParameter\.Note Property

Gets or sets the free\-text note of the interpreter, or `null` when unnoted\.

```csharp
public string? Note { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.ObjectId'></a>

## OrtoAreaLabelParameter\.ObjectId Property

Gets or sets the identifier of the drawn object the label was filed under; exactly one of this or [Reference](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.Reference 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter\.Reference') must be given\. The stored polygon of that object resolves the observation dates \- the body carries none\.

```csharp
public string? ObjectId { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.Reference'></a>

## OrtoAreaLabelParameter\.Reference Property

Gets or sets the building reference of the footprint identity; exactly one of this or [ObjectId](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.ObjectId 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaLabelParameter\.ObjectId') must be given, so the reference alone identifies the footprint\.

```csharp
public string? Reference { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.State'></a>

## OrtoAreaLabelParameter\.State Property

Gets or sets what the interpreter saw, as 0 \(present\), 1 \(absent\) or 2 \(indeterminate\); `null` or any other value is rejected\.

```csharp
public System.Nullable<short> State { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.X_1'></a>

## OrtoAreaLabelParameter\.X\_1 Property

Gets or sets the easting of the first corner of the area bounding box, in EPSG:2180 metres; `null` \(an omitted field\) is rejected\.

```csharp
public System.Nullable<double> X_1 { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.X_2'></a>

## OrtoAreaLabelParameter\.X\_2 Property

Gets or sets the easting of the second corner of the area bounding box, in EPSG:2180 metres; `null` \(an omitted field\) is rejected\.

```csharp
public System.Nullable<double> X_2 { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.Y_1'></a>

## OrtoAreaLabelParameter\.Y\_1 Property

Gets or sets the northing of the first corner of the area bounding box, in EPSG:2180 metres; `null` \(an omitted field\) is rejected\.

```csharp
public System.Nullable<double> Y_1 { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaLabelParameter.Y_2'></a>

## OrtoAreaLabelParameter\.Y\_2 Property

Gets or sets the northing of the second corner of the area bounding box, in EPSG:2180 metres; `null` \(an omitted field\) is rejected\.

```csharp
public System.Nullable<double> Y_2 { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter'></a>

## OrtoAreaObjectParameter Class

The body of the `gis/ortoarea/objects` write: one polygon drawn in a full\-interpretation test area\.

Every member is bound nullable so an omitted field is representable and can be rejected explicitly. The bounding box names the area with the same component names the read routes take as query parameters, and [Polygon](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.Polygon 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaObjectParameter\.Polygon') carries the drawn outline as serialized JSON - its shape is resolved server-side (an array of points, or a point array under `point2Ds`/`points`, or a serialized DiGi `Polygon2D`) and must hold at least three vertices.

```csharp
public class OrtoAreaObjectParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → OrtoAreaObjectParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.OrtoAreaObjectParameter()'></a>

## OrtoAreaObjectParameter\(\) Constructor

Initializes a new instance of the [OrtoAreaObjectParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaObjectParameter') class\.

```csharp
public OrtoAreaObjectParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.OrtoAreaObjectParameter(DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter)'></a>

## OrtoAreaObjectParameter\(OrtoAreaObjectParameter\) Constructor

Initializes a new instance of the [OrtoAreaObjectParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaObjectParameter') class by copying the values from an existing instance\.

```csharp
public OrtoAreaObjectParameter(DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter ortoAreaObjectParameter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.OrtoAreaObjectParameter(DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter).ortoAreaObjectParameter'></a>

`ortoAreaObjectParameter` [OrtoAreaObjectParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaObjectParameter')

The source instance from which to copy the values\.

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.OrtoAreaObjectParameter(System.Text.Json.Nodes.JsonObject)'></a>

## OrtoAreaObjectParameter\(JsonObject\) Constructor

Initializes a new instance of the [OrtoAreaObjectParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.OrtoAreaObjectParameter') class using the provided JSON object\.

```csharp
public OrtoAreaObjectParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.OrtoAreaObjectParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.Polygon'></a>

## OrtoAreaObjectParameter\.Polygon Property

Gets or sets the drawn polygon as serialized JSON; missing or unreadable, or holding fewer than three vertices, is rejected\.

```csharp
public System.Text.Json.Nodes.JsonNode? Polygon { get; set; }
```

#### Property Value
[System\.Text\.Json\.Nodes\.JsonNode](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonnode 'System\.Text\.Json\.Nodes\.JsonNode')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.X_1'></a>

## OrtoAreaObjectParameter\.X\_1 Property

Gets or sets the easting of the first corner of the area bounding box, in EPSG:2180 metres; `null` \(an omitted field\) is rejected\.

```csharp
public System.Nullable<double> X_1 { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.X_2'></a>

## OrtoAreaObjectParameter\.X\_2 Property

Gets or sets the easting of the second corner of the area bounding box, in EPSG:2180 metres; `null` \(an omitted field\) is rejected\.

```csharp
public System.Nullable<double> X_2 { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.Y_1'></a>

## OrtoAreaObjectParameter\.Y\_1 Property

Gets or sets the northing of the first corner of the area bounding box, in EPSG:2180 metres; `null` \(an omitted field\) is rejected\.

```csharp
public System.Nullable<double> Y_1 { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.OrtoAreaObjectParameter.Y_2'></a>

## OrtoAreaObjectParameter\.Y\_2 Property

Gets or sets the northing of the second corner of the area bounding box, in EPSG:2180 metres; `null` \(an omitted field\) is rejected\.

```csharp
public System.Nullable<double> Y_2 { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter'></a>

## UserYearBuiltParameter Class

The body of the `setuseryearbuilt` write: one user\-supplied year built entry for a single building\.

Every member is bound nullable so an omitted field is representable and can be rejected explicitly - a non-nullable binding cannot tell an omitted value from a legitimate one, and `Relation` must be checked against the `YearBuiltRelation` members rather than compared to a sentinel.

```csharp
public class UserYearBuiltParameter : DiGi.WebAPI.Classes.Parameter
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [DiGi\.Core\.Classes\.Object](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.object 'DiGi\.Core\.Classes\.Object') → [DiGi\.Core\.Classes\.SerializableObject](https://learn.microsoft.com/en-us/dotnet/api/digi.core.classes.serializableobject 'DiGi\.Core\.Classes\.SerializableObject') → [DiGi\.WebAPI\.Classes\.Parameter](https://learn.microsoft.com/en-us/dotnet/api/digi.webapi.classes.parameter 'DiGi\.WebAPI\.Classes\.Parameter') → UserYearBuiltParameter
### Constructors

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.UserYearBuiltParameter()'></a>

## UserYearBuiltParameter\(\) Constructor

Initializes a new instance of the [UserYearBuiltParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter') class\.

```csharp
public UserYearBuiltParameter();
```

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.UserYearBuiltParameter(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter)'></a>

## UserYearBuiltParameter\(UserYearBuiltParameter\) Constructor

Initializes a new instance of the [UserYearBuiltParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter') class by copying the values from an existing instance\.

```csharp
public UserYearBuiltParameter(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter userYearBuiltParameter);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.UserYearBuiltParameter(DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter).userYearBuiltParameter'></a>

`userYearBuiltParameter` [UserYearBuiltParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter')

The source instance from which to copy the values\.

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.UserYearBuiltParameter(System.Text.Json.Nodes.JsonObject)'></a>

## UserYearBuiltParameter\(JsonObject\) Constructor

Initializes a new instance of the [UserYearBuiltParameter](DiGi.GIS.WebAPI.Classes.Parameter.md#DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter 'DiGi\.GIS\.WebAPI\.Classes\.Parameter\.UserYearBuiltParameter') class using the provided JSON object\.

```csharp
public UserYearBuiltParameter(System.Text.Json.Nodes.JsonObject jsonObject);
```
#### Parameters

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.UserYearBuiltParameter(System.Text.Json.Nodes.JsonObject).jsonObject'></a>

`jsonObject` [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject')

The [System\.Text\.Json\.Nodes\.JsonObject](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.nodes.jsonobject 'System\.Text\.Json\.Nodes\.JsonObject') containing the data used to initialize the parameter\.
### Properties

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.CountyId'></a>

## UserYearBuiltParameter\.CountyId Property

Gets or sets the identifier of the county part the building is filed under\. A hint passed to the converter, which resolves the part itself; `null` is rejected\.

```csharp
public System.Nullable<int> CountyId { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.Reference'></a>

## UserYearBuiltParameter\.Reference Property

Gets or sets the reference of the building the entry belongs to; blank is rejected\.

```csharp
public string? Reference { get; set; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.Relation'></a>

## UserYearBuiltParameter\.Relation Property

Gets or sets how the stored year relates to the true construction year, as a [DiGi\.GIS\.Enums\.YearBuiltRelation](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.enums.yearbuiltrelation 'DiGi\.GIS\.Enums\.YearBuiltRelation') member\. `null` means `Exact`; a value that is not a [DiGi\.GIS\.Enums\.YearBuiltRelation](https://learn.microsoft.com/en-us/dotnet/api/digi.gis.enums.yearbuiltrelation 'DiGi\.GIS\.Enums\.YearBuiltRelation') member is rejected\.

```csharp
public System.Nullable<int> Relation { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

<a name='DiGi.GIS.WebAPI.Classes.Parameter.UserYearBuiltParameter.Year'></a>

## UserYearBuiltParameter\.Year Property

Gets or sets the year built the user is asserting; `null` \(an omitted selection\) is rejected\.

```csharp
public System.Nullable<short> Year { get; set; }
```

#### Property Value
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')