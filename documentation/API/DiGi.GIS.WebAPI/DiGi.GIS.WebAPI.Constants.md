#### [DiGi\.GIS\.WebAPI](DiGi.GIS.WebAPI.Overview.md 'DiGi\.GIS\.WebAPI\.Overview')

## DiGi\.GIS\.WebAPI\.Constants Namespace
### Classes

<a name='DiGi.GIS.WebAPI.Constants.BuildingData'></a>

## BuildingData Class

Provides the limits applied to building data queries served by the GIS PostgreSQL Web API\.

```csharp
public static class BuildingData
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → BuildingData
### Fields

<a name='DiGi.GIS.WebAPI.Constants.BuildingData.MaximumAdministrativeAreal2DCountyCount'></a>

## BuildingData\.MaximumAdministrativeAreal2DCountyCount Field

The largest number of distinct counties one area count may iterate\.

An area above the subdivision level is counted one county partition at a time, sequentially, so the cost follows the counties its subdivisions sit in. The ceiling of 500 allows a whole-country count across all 406 county parts, while protecting the database connection pool against unbounded input.

```csharp
public static readonly int MaximumAdministrativeAreal2DCountyCount;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Constants.BuildingData.MaximumAggregateSummariesMeasureCount'></a>

## BuildingData\.MaximumAggregateSummariesMeasureCount Field

The largest number of single\-value aggregate measures one `aggregatesummary/batch` request may carry\.

The measures share one filter, one connection and one SQL command, so the cost follows the measure count. The ceiling of 100 is a guardrail against pathological requests - far above any realistic client - and stays at the converter's own cap so the 400 pre-check and the converter's refusal agree (DiGi-Organization/DiGi.GIS.WebAPI#56).

```csharp
public static readonly int MaximumAggregateSummariesMeasureCount;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Constants.Compression'></a>

## Compression Class

Provides the compression settings applied to request payloads sent to the GIS PostgreSQL Web API\.

```csharp
public static class Compression
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Compression
### Fields

<a name='DiGi.GIS.WebAPI.Constants.Compression.Level'></a>

## Compression\.Level Field

The GZip compression level used for request payloads\.

[System\.IO\.Compression\.CompressionLevel\.Fastest](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.compressionlevel.fastest 'System\.IO\.Compression\.CompressionLevel\.Fastest') rather than [System\.IO\.Compression\.CompressionLevel\.Optimal](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.compressionlevel.optimal 'System\.IO\.Compression\.CompressionLevel\.Optimal'): on the bulk import path the client is CPU bound, and compressing a multi-megabyte JSON batch at [System\.IO\.Compression\.CompressionLevel\.Optimal](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.compressionlevel.optimal 'System\.IO\.Compression\.CompressionLevel\.Optimal') costs several times more than it saves in transfer. Revisit if the link to the host becomes the bottleneck.

```csharp
public static readonly CompressionLevel Level;
```

#### Field Value
[System\.IO\.Compression\.CompressionLevel](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.compressionlevel 'System\.IO\.Compression\.CompressionLevel')

<a name='DiGi.GIS.WebAPI.Constants.FileName'></a>

## FileName Class

Provides constant values for file names used within the GIS PostgreSQL Web API\.

```csharp
public static class FileName
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → FileName
### Fields

<a name='DiGi.GIS.WebAPI.Constants.FileName.GISWebAPIConfigurationFile'></a>

## FileName\.GISWebAPIConfigurationFile Field

Gets the filename of the configuration file for the GIS PostgreSQL Web API\.

```csharp
public const string GISWebAPIConfigurationFile = "GIS_WebAPI.conf";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Constants.Header'></a>

## Header Class

Provides the names of the HTTP headers the GIS Web API sets on its responses\.

```csharp
public static class Header
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Header
### Fields

<a name='DiGi.GIS.WebAPI.Constants.Header.NextCursor'></a>

## Header\.NextCursor Field

The response header carrying the cursor of the next page of a physical\-order building data read\. Absent when the county part is exhausted, and absent when the endpoint answered in reference order instead\.

```csharp
public const string NextCursor = "DiGi-Next-Cursor";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Constants.Name'></a>

## Name Class

Provides a collection of constant name identifiers used within the GIS PostgreSQL Web API\.

```csharp
public static class Name
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Name

<a name='DiGi.GIS.WebAPI.Constants.Name.Client'></a>

## Name\.Client Class

Provides the identifiers for the named HTTP clients created by the Web API\.

```csharp
public static class Name.Client
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Client
### Fields

<a name='DiGi.GIS.WebAPI.Constants.Name.Client.Geoportal'></a>

## Name\.Client\.Geoportal Field

The Geoportal client identifier\.

```csharp
public static string Geoportal;
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Constants.Name.Client.GIS'></a>

## Name\.Client\.GIS Field

The GIS client identifier\.

```csharp
public static string GIS;
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Constants.Name.Client.GUGiK'></a>

## Name\.Client\.GUGiK Field

The GUGiK \(Główny Urząd Geodezji i Kartografii\) client identifier\.

```csharp
public static string GUGiK;
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='DiGi.GIS.WebAPI.Constants.OrtoDatas'></a>

## OrtoDatas Class

Provides the limits applied to orthophoto queries served by the GIS PostgreSQL Web API\.

```csharp
public static class OrtoDatas
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → OrtoDatas
### Fields

<a name='DiGi.GIS.WebAPI.Constants.OrtoDatas.MaximumCoverageCountyCount'></a>

## OrtoDatas\.MaximumCoverageCountyCount Field

The largest number of distinct counties one coverage request may have measured exactly\.

A subdivision or a municipality has no partition of its own, so its coverage is counted rather than estimated - one read per side of the county it sits in, measured at 0.10 s for a county of 33 000 buildings and 0.63 s for the largest in the country. Every subdivision and municipality of one county is served from that single pass, so the cost follows the counties named and not the identifiers.

The ceiling of 500 allows full-country listings covering all 380 Polish counties at once, while protecting the database connection pool against unbounded input.

```csharp
public static readonly int MaximumCoverageCountyCount;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Constants.OrtoDatas.MaximumSampleCount'></a>

## OrtoDatas\.MaximumSampleCount Field

The largest number of references a subdivision comparison may name back per category\.

The comparison itself walks a whole county, and the counts it returns are exact whatever this is set to. The samples exist to make a disagreement actionable without returning a hundred thousand strings, so the ceiling bounds the response rather than the work.

`const` rather than `static readonly` so `subdivisionlinksbycountyid` can carry it as the maximum of a [System\.ComponentModel\.DataAnnotations\.RangeAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.rangeattribute 'System\.ComponentModel\.DataAnnotations\.RangeAttribute'), whose argument has to be a compile-time constant (DiGi-Organization/DiGi.GIS.WebAPI#47).

```csharp
public const int MaximumSampleCount = 1000;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Constants.OrtoDatas.MaximumSummaryCountyCount'></a>

## OrtoDatas\.MaximumSummaryCountyCount Field

The largest number of counties a summary or queue request may name at once\.

Each county costs a grouped aggregate over one partition, which is cheap, but the request is unauthenticated and naming every county in the country in one call is not what these are for. Omitting the identifiers entirely is still allowed and answers every partition in a single grouped statement, which is one query rather than many.

```csharp
public static readonly int MaximumSummaryCountyCount;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Constants.Terrain'></a>

## Terrain Class

Provides the limits applied to terrain queries served by the GIS PostgreSQL Web API\.

```csharp
public static class Terrain
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Terrain
### Fields

<a name='DiGi.GIS.WebAPI.Constants.Terrain.EdgeLengthFactor'></a>

## Terrain\.EdgeLengthFactor Field

How many times its own vertex spacing a terrain mesh triangle's longest edge may reach before it is treated as bridging no data\.

A Delaunay triangulation covers the convex hull of its sites, so without a limit the mesh bridges county edges, no-data gaps and concave outlines with a skirt of long thin triangles that look like terrain and are not.

Expressed as a multiple of local spacing rather than as a distance, because the stored lattice is not the only sampling this has to survive: points added where the ground moves sharply make one area denser than another, and any fixed distance is then simultaneously too tight for the coarse part and too loose for the fine part. A factor measures each triangle against its own vertices and needs no retuning when the sampling changes.

The consequence to know when reading a served mesh: an empty area entirely enclosed by terrain - a lake, or a pocket the elevation service has no data for - is now spanned flat rather than left open, because nothing over it reaches the boundary to be removed. That is acceptable for a surface that is rendered; it is not a licence to treat the mesh as measured ground everywhere it has vertices.

On the present 100 m lattice this behaves like a limit of about 250 m. It only governs the outer edge: [DiGi\.Geometry\.PointCloud\.Core\.Query\.ErodedIndexes\(System\.Double\[\],System\.Double\[\],System\.Collections\.Generic\.List\{System\.Int32\[\]\},System\.Double\)](https://learn.microsoft.com/en-us/dotnet/api/digi.geometry.pointcloud.core.query.erodedindexes#digi-geometry-pointcloud-core-query-erodedindexes(system-double[]-system-double[]-system-collections-generic-list{system-int32[]}-system-double) 'DiGi\.Geometry\.PointCloud\.Core\.Query\.ErodedIndexes\(System\.Double\[\],System\.Double\[\],System\.Collections\.Generic\.List\{System\.Int32\[\]\},System\.Double\)') removes triangles from the boundary inwards, so a point missing from the interior keeps the triangles around it and reads as a flat spot rather than a hole.

```csharp
public static readonly double EdgeLengthFactor;
```

#### Field Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='DiGi.GIS.WebAPI.Constants.Terrain.MaximumDensityCountyCount'></a>

## Terrain\.MaximumDensityCountyCount Field

The largest number of counties a single density request may name\.

A density costs the reading of every subdivision outline of the county it is asked for, so this is what keeps one request from pulling the administrative geometry of the whole country. Sweeping every county means several requests, which is also what makes the sweep interruptible.

```csharp
public static readonly int MaximumDensityCountyCount;
```

#### Field Value
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='DiGi.GIS.WebAPI.Constants.Terrain.MaximumGapExtent'></a>

## Terrain\.MaximumGapExtent Field

The longest side, in model units, a gap request may ask for\.

Larger than the mesh endpoints admit, because a gap request returns coordinates rather than a triangulated surface and is meant for looking over a region at once. It is still bounded by [MaximumNodeCount](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MaximumNodeCount 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MaximumNodeCount') at any given lattice.

```csharp
public static readonly double MaximumGapExtent;
```

#### Field Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='DiGi.GIS.WebAPI.Constants.Terrain.MaximumNodeCount'></a>

## Terrain\.MaximumNodeCount Field

The largest number of lattice nodes a single coverage or gap request may generate\.

Checked against the requested area before any node is built, so an area and a lattice that would together exceed it are refused rather than started. This is the cap that holds when a large county meets a fine lattice, both of which are individually allowed.

```csharp
public static readonly long MaximumNodeCount;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='DiGi.GIS.WebAPI.Constants.Terrain.MaximumRadius'></a>

## Terrain\.MaximumRadius Field

The largest search radius, in model units, a terrain request may ask for\.

The terrain endpoints are unauthenticated reads with no natural ceiling of their own: the radius alone decides how many stored points are gathered, triangulated and serialised, and the store is partitioned by county rather than capped by extent. Without this a single request can ask for the whole country.

The cap is a half-extent, so the largest search area is 4 km by 4 km. Counties are sampled onto a lattice between 10 m and 100 m, which puts the worst case at roughly 160 000 points. Raising this is safe only if the finest lattice is never queried at the new size - at 5 000 m the same lattice yields about a million points.

`const` rather than `static readonly` so the terrain mesh endpoints can carry it as a [System\.ComponentModel\.DataAnnotations\.RangeAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.rangeattribute 'System\.ComponentModel\.DataAnnotations\.RangeAttribute') maximum, which has to be a compile-time constant.

```csharp
public const double MaximumRadius = 2000;
```

#### Field Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='DiGi.GIS.WebAPI.Constants.Terrain.MinimumGridSize'></a>

## Terrain\.MinimumGridSize Field

The finest lattice, in model units, a coverage or gap request may be measured against\.

The work of those endpoints rises with the square of how fine the lattice is: they generate every node of a county and decide each one against its outlines. A county of 1 000 square kilometres is 100 000 nodes at 100 m and 10 million at 10 m, and below that the request stops being a diagnostic and becomes a denial of service that anyone can send.

`const` rather than `static readonly` so the lattice endpoints can carry it as the floor of a [MinimumAttribute](DiGi.GIS.WebAPI.Classes.md#DiGi.GIS.WebAPI.Classes.MinimumAttribute 'DiGi\.GIS\.WebAPI\.Classes\.MinimumAttribute'), whose argument has to be a compile-time constant, the way [MaximumRadius](DiGi.GIS.WebAPI.Constants.md#DiGi.GIS.WebAPI.Constants.Terrain.MaximumRadius 'DiGi\.GIS\.WebAPI\.Constants\.Terrain\.MaximumRadius') became one for [System\.ComponentModel\.DataAnnotations\.RangeAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.rangeattribute 'System\.ComponentModel\.DataAnnotations\.RangeAttribute') (DiGi-Organization/DiGi.GIS.WebAPI#47).

```csharp
public const double MinimumGridSize = 10;
```

#### Field Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='DiGi.GIS.WebAPI.Constants.Uri'></a>

## Uri Class

Provides constant URI values used throughout the PostgreSQL Web API application\.

```csharp
public static class Uri
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Uri
### Fields

<a name='DiGi.GIS.WebAPI.Constants.Uri.BaseAddress'></a>

## Uri\.BaseAddress Field

Gets or sets the base address for the Web API services\.

```csharp
public static Uri BaseAddress;
```

#### Field Value
[System\.Uri](https://learn.microsoft.com/en-us/dotnet/api/system.uri 'System\.Uri')