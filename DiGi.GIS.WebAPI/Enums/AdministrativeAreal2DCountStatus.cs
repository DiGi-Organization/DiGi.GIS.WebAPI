namespace DiGi.GIS.WebAPI.Enums
{
    /// <summary>
    /// Describes how resolving an administrative area to its building data scopes, and counting them, ended.
    /// <para>Not on the wire: each value maps to an HTTP status in the controller. New values are appended so existing ones keep their numbers.</para>
    /// </summary>
    public enum AdministrativeAreal2DCountStatus
    {
        /// <summary>
        /// The area was resolved to its county partitions and, for a count, every partition was counted and the counts summed.
        /// </summary>
        Counted,

        /// <summary>
        /// The administrative area does not exist.
        /// </summary>
        AreaNotFound,

        /// <summary>
        /// An area above the Subdivision level resolved to no subdivision to filter by. Not evidence that the area holds no buildings.
        /// </summary>
        NoSubdivisions,

        /// <summary>
        /// The area resolves to more counties than one request may iterate; nothing was counted.
        /// </summary>
        TooManyCounties,

        /// <summary>
        /// A read or count could not be made: no connection could be built, or a filter column could not be resolved.
        /// </summary>
        NotResolved,

        /// <summary>
        /// A Subdivision's polygon holds no building centre, or the Subdivision has no polygon.
        /// </summary>
        NoBuildings,

        /// <summary>
        /// A Subdivision's polygon holds more buildings than one request may confine a read to; nothing was counted.
        /// </summary>
        TooManyReferences
    }
}
