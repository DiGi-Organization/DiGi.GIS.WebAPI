using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Parameter class containing options for keyset-paginated building 2D reference queries.
    /// </summary>
    public class Building2DReferencesByPagingParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Building2DReferencesByPagingParameter"/> class.
        /// </summary>
        public Building2DReferencesByPagingParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building2DReferencesByPagingParameter"/> class using a <see cref="JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The JSON object containing data used to initialize the parameter.</param>
        public Building2DReferencesByPagingParameter(JsonObject jsonObject)
            : base(jsonObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building2DReferencesByPagingParameter"/> class by copying properties from another instance.
        /// </summary>
        /// <param name="building2DReferencesByPagingParameter">The parameter instance to copy properties from.</param>
        public Building2DReferencesByPagingParameter(Building2DReferencesByPagingParameter building2DReferencesByPagingParameter)
            : base(building2DReferencesByPagingParameter)
        {
            if (building2DReferencesByPagingParameter is not null)
            {
                CountyId = building2DReferencesByPagingParameter.CountyId;
                SubdivisionId = building2DReferencesByPagingParameter.SubdivisionId;
                PageSize = building2DReferencesByPagingParameter.PageSize;
                Cursor = building2DReferencesByPagingParameter.Cursor;
            }
        }

        /// <summary>
        /// Gets or sets the target partition identifier (County ID).
        /// </summary>
        /// <example>10365</example>
        [Required]
        public int CountyId { get; set; }

        /// <summary>
        /// Gets or sets the target subdivision identifier (Subdivision ID). Leave null to return references for the whole county.
        /// </summary>
        /// <example>1035</example>
        public int? SubdivisionId { get; set; }

        /// <summary>
        /// Gets or sets the maximum count of references per page. Defaults to 250.
        /// </summary>
        /// <example>100</example>
        [DefaultValue(250)]
        public int PageSize { get; set; } = 250;

        /// <summary>
        /// Gets or sets the pagination cursor tracking the last processed building reference.
        /// </summary>
        /// <example>BLDG-12345</example>
        public string? Cursor { get; set; }
    }
}