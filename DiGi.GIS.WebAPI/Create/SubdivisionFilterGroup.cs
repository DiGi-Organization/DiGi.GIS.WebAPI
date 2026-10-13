using DiGi.PostgreSQL.Table;
using DiGi.PostgreSQL.Table.Classes;
using DiGi.PostgreSQL.Table.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DiGi.GIS.WebAPI
{
    public static partial class Create
    {
        /// <summary>
        /// Creates the filter group that confines a building data count to a set of subdivisions, and applies the caller's own filters on top.
        /// <para>The result is an <c>And</c> group holding one <c>In</c> condition on the subdivision column, with the caller's group nested beside it as a whole so that its own logical operator is kept (an <c>Or</c> group is not flattened into the subdivision condition). The list reaches the <c>In</c> branch as a collection, not as a deserialized JSON array.</para>
        /// </summary>
        /// <param name="filterGroup">The optional filters of the caller; <c>null</c> or empty adds nothing.</param>
        /// <param name="subdivisionIds">The identifiers of the subdivisions the count is confined to.</param>
        /// <returns>The combined filter group, or null when there is no subdivision identifier to confine the count to.</returns>
        public static FilterGroup? SubdivisionFilterGroup(FilterGroup? filterGroup, IEnumerable<int>? subdivisionIds)
        {
            if (subdivisionIds is null)
            {
                return null;
            }

            List<int> subdivisionIds_List = [.. subdivisionIds];
            if (subdivisionIds_List.Count == 0)
            {
                return null;
            }

            FilterGroup result = new()
            {
                LogicalOperator = FilterLogicalOperator.And,
                FilterConditions =
                [
                    new FilterCondition()
                    {
                        ColumnUniqueId = IO.Constants.Column.SubdivisionId.UniqueId(),
                        FilterOperator = FilterOperator.In,
                        Value = subdivisionIds_List
                    }
                ]
            };

            if (filterGroup is not null && (filterGroup.FilterConditions.Count != 0 || filterGroup.FilterGroups.Count != 0))
            {
                result.FilterGroups.Add(filterGroup);
            }

            return result;
        }
    }
}
