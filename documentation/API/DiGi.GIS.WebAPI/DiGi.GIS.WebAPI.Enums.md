#### [DiGi\.GIS\.WebAPI](DiGi.GIS.WebAPI.Overview.md 'DiGi\.GIS\.WebAPI\.Overview')

## DiGi\.GIS\.WebAPI\.Enums Namespace
### Enums

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus'></a>

## AdministrativeAreal2DCountStatus Enum

Describes how counting the building data of an administrative area ended\.

```csharp
public enum AdministrativeAreal2DCountStatus
```
### Fields

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.Counted'></a>

`Counted` 0

Every county partition was counted and the counts summed\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.AreaNotFound'></a>

`AreaNotFound` 1

The administrative area does not exist\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.NoSubdivisions'></a>

`NoSubdivisions` 2

Resolution found no subdivision to filter by\. Not evidence that the area holds no buildings\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.TooManyCounties'></a>

`TooManyCounties` 3

The area resolves to more counties than one request may iterate; nothing was counted\.

<a name='DiGi.GIS.WebAPI.Enums.AdministrativeAreal2DCountStatus.NotResolved'></a>

`NotResolved` 4

A count could not be made: no connection could be built, or a filter column could not be resolved\.