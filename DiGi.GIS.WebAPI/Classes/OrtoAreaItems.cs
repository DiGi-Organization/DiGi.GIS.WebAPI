using DiGi.GIS.PostgreSQL.Classes;
using System.Collections.Generic;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// The answer of <c>gis/ortoarea/areabyboundingbox</c>: one full-interpretation test area with everything drawn and filed under it.
    /// <para><see cref="Labels"/> holds the rows of every interpreter, not only the caller's - reading what someone else marked is part of the interpretation workflow, while only <c>gis/ortoarea/label</c> writes, always under the caller's own bearer-token identity.</para>
    /// </summary>
    public class OrtoAreaItems
    {
        /// <summary>
        /// Gets or sets the area with its sampled cells, created on first sight of the box.
        /// </summary>
        public OrtoArea? Area { get; set; }

        /// <summary>
        /// Gets or sets the polygons drawn in the area, in creation order; empty when none were drawn.
        /// </summary>
        public List<OrtoAreaObject> Objects { get; set; } = [];

        /// <summary>
        /// Gets or sets every label row of the area, all interpreters; empty when none were filed.
        /// </summary>
        public List<OrtoAreaLabel> Labels { get; set; } = [];
    }
}
