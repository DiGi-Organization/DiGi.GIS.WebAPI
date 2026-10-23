using DiGi.Analytical.Building.HVAC.Classes;
using DiGi.Analytical.Building.HVAC.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Controller responsible for handling API requests related to heat transfer coefficients, providing access to regulated heat transfer coefficient data.
    /// </summary>
    [ApiController]
    [Route("gis/[controller]")]
    [TransientDatabaseFailure]
    public class HeatTransferCoefficientController : DiGi.WebAPI.Classes.WebAPIController
    {
        private static readonly RegulatedHeatTransferCoefficientsManager regulatedHeatTransferCoefficientsManager = DiGi.Analytical.Building.HVAC.Create.RegulatedHeatTransferCoefficientsManager();

        /// <summary>
        /// Initializes a new instance of the HeatTransferCoefficientController class.
        /// </summary>
        public HeatTransferCoefficientController()
        {
        }

        /// <summary>
        /// Retrieves the regulated heat transfer coefficients for a specified year.
        /// </summary>
        /// <param name="year">The year used to filter the regulated heat transfer coefficients.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by the caller to cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the request, containing the retrieved coefficients or an error status.</returns>
        [HttpGet("regulatedheattransfercoefficientsbyyear")]
        public async Task<IActionResult> GetRegulatedHeatTransferCoefficientsByYearAsync([BindRequired, FromQuery(Name = "year")] short year, CancellationToken cancellationToken = default)
        {
            Serilog.Modify.Log("{Type}:{Name} started", nameof(HeatTransferCoefficientController), nameof(GetRegulatedHeatTransferCoefficientsByYearAsync));

            Serilog.Modify.Log("Year provided: {Year}", year);

            if (regulatedHeatTransferCoefficientsManager is null)
            {
                Serilog.Modify.Log(Serilog.Enums.LogEventLevel.Error, "RegulatedHeatTransferCoefficientsManager is null");
                return BadRequest();
            }

            IRegulatedHeatTransferCoefficients? regulatedHeatTransferCoefficients = regulatedHeatTransferCoefficientsManager.GetRegulatedHeatTransferCoefficients(new DateTime(year, 1, 1));

            if (regulatedHeatTransferCoefficients is null)
            {
                Serilog.Modify.Log("No RegulatedHeatTransferCoefficients found for provided year");
                return NoContent();
            }

            return Content(Core.Convert.ToSystem_String(regulatedHeatTransferCoefficients) ?? string.Empty, "application/json");
        }
    }
}