using DiGi.GIS.PostgreSQL.Classes;
using DiGi.GIS.PostgreSQL.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI
{
    public static partial class Query
    {
        /// <summary>
        /// Asynchronously determines whether an identifier is a county part, that is an <c>administrative_areal_2d</c> row of type <see cref="AdministrativeArealType.County"/>.
        /// <para>Separates a known county part that holds no rows of some kind (a count of zero) from an identifier that names no county part (not found). The lookup is made on every call and is not cached.</para>
        /// </summary>
        /// <param name="administrativeAreal2DPostgreSQLConverter">The converter used to look the identifier up.</param>
        /// <param name="countyId">The identifier to look up.</param>
        /// <param name="commandTimeout">The timeout in seconds for the execution of the command. A value of 0 disables the timeout. Defaults to 30 seconds.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to observe for cancellation requests.</param>
        /// <returns><c>true</c> when the identifier names a county part; <c>false</c> when it names a row of another type, names no row, or the converter is <c>null</c>.</returns>
        public static async Task<bool> IsCountyAsync(this AdministrativeAreal2DPostgreSQLConverter? administrativeAreal2DPostgreSQLConverter, int countyId, int commandTimeout = 30, CancellationToken cancellationToken = default)
        {
            if (administrativeAreal2DPostgreSQLConverter is null)
            {
                return false;
            }

            AdministrativeAreal2DReference? administrativeAreal2DReference = await administrativeAreal2DPostgreSQLConverter.GetAdministrativeAreal2DReferenceByIdAsync(countyId, commandTimeout, cancellationToken);

            return administrativeAreal2DReference is not null && administrativeAreal2DReference.AdministrativeArealType == AdministrativeArealType.County;
        }
    }
}
