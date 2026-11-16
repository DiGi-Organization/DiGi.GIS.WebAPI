#### [DiGi\.GIS\.WebAPI](DiGi.GIS.WebAPI.Overview.md 'DiGi\.GIS\.WebAPI\.Overview')

## DiGi\.GIS\.WebAPI\.Enums Namespace
### Enums

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus'></a>

## AdministrativeAreal2DCountStatus Enum

Describes how resolving an administrative area to its building data scopes, and counting them, ended\.

Not on the wire: each value maps to an HTTP status in the controller. New values are appended so existing ones keep their numbers.

```csharp
public enum AdministrativeAreal2DCountStatus
```
### Fields

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.Counted'></a>

`Counted` 0

The area was resolved to its county partitions and, for a count, every partition was counted and the counts summed\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.AreaNotFound'></a>

`AreaNotFound` 1

The administrative area does not exist\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.NoSubdivisions'></a>

`NoSubdivisions` 2

An area above the Subdivision level resolved to no subdivision to filter by\. Not evidence that the area holds no buildings\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.TooManyCounties'></a>

`TooManyCounties` 3

The area resolves to more counties than one request may iterate; nothing was counted\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.NotResolved'></a>

`NotResolved` 4

A read or count could not be made: no connection could be built, or a filter column could not be resolved\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.NoBuildings'></a>

`NoBuildings` 5

A Subdivision's polygon holds no building centre, or the Subdivision has no polygon\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.TooManyReferences'></a>

`TooManyReferences` 6

A Subdivision's polygon holds more buildings than one request may confine a read to; nothing was counted\.