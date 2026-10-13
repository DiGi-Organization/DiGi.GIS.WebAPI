namespace DiGi.GIS.WebAPI.Enums
{
    /// <summary>
    /// Describes how counting the building data of an administrative area ended.
    /// </summary>
    public enum AdministrativeAreal2DCountStatus
    {
        /// <summary>
        /// Every county partition was counted and the counts summed.
        /// </summary>
        Counted,

        /// <summary>
        /// The administrative area does not exist.
        /// </summary>
        AreaNotFound,

        /// <summary>
        /// Resolution found no subdivision to filter by. Not evidence that the area holds no buildings.
        /// </summary>
        NoSubdivisions,

        /// <summary>
        /// The area resolves to more counties than one request may iterate; nothing was counted.
        /// </summary>
        TooManyCounties,

        /// <summary>
        /// A count could not be made: no connection could be built, or a filter column could not be resolved.
        /// </summary>
        NotResolved
    }
}
