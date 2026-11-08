using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Represents a parameter containing categories for querying columns.
    /// </summary>
    public class ColumnsByCategoriesParameter : DiGi.WebAPI.Classes.Parameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnsByCategoriesParameter"/> class.
        /// </summary>
        public ColumnsByCategoriesParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnsByCategoriesParameter"/> class with the specified categories.
        /// </summary>
        /// <param name="categories">The collection of categories for querying columns.</param>
        public ColumnsByCategoriesParameter(IEnumerable<string> categories)
        {
            Categories = categories is null ? [] : [.. categories];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnsByCategoriesParameter"/> class using an <see cref="JsonObject"/> object.
        /// </summary>
        /// <param name="jsonObject">The <see cref="System.Text.Json.Nodes.JsonObject" /> containing the data used to initialize the parameter.</param>
        public ColumnsByCategoriesParameter(JsonObject jsonObject)
        : base(jsonObject)
        {
        }

        /// <summary>
        /// Gets or sets the categories for querying columns. All columns will be returned if the collection is null or empty.
        /// </summary>
        public IEnumerable<string> Categories { get; set; } = [];
    }
}