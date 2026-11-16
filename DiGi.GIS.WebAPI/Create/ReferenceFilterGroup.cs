using DiGi.PostgreSQL.Table;
using DiGi.PostgreSQL.Table.Classes;
using DiGi.PostgreSQL.Table.Enums;
using System.Collections.Generic;

namespace DiGi.GIS.WebAPI
{
    public static partial class Create
    {
        /// <summary>
        /// Creates the filter group that confines a building data read to a set of building references of one county partition, and applies the caller's own filters on top.
        /// <para>The result is an <c>And</c> group holding one <c>In</c> condition on the reference column, with the caller's group nested beside it as a whole so that its own logical operator is kept (an <c>Or</c> group is not flattened into the reference condition). The list reaches the <c>In</c> branch as a collection and is rendered as one <c>= ANY(@array)</c> parameter, not as a deserialized JSON array. A reference is unique only within its county partition, so the group is meaningful only together with that partition's identifier.</para>
        /// </summary>
        /// <param name="filterGroup">The optional filters of the caller; <c>null</c> or empty adds nothing.</param>
        /// <param name="references">The building references the read is confined to.</param>
        /// <returns>The combined filter group, or null when there is no reference to confine the read to.</returns>
        public static FilterGroup? ReferenceFilterGroup(FilterGroup? filterGroup, IEnumerable<string>? references)
        {
            if (references is null)
            {
                return null;
            }

            List<string> references_List = [];
            foreach (string? reference in references)
            {
                if (!string.IsNullOrWhiteSpace(reference))
                {
                    references_List.Add(reference!);
                }
            }

            if (references_List.Count == 0)
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
                        ColumnUniqueId = IO.Constants.Column.Reference.UniqueId(),
                        FilterOperator = FilterOperator.In,
                        Value = references_List
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
