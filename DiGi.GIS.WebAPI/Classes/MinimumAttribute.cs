using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DiGi.GIS.WebAPI.Classes
{
    /// <summary>
    /// Declares the smallest value a numeric action parameter may carry, without the ceiling a <see cref="RangeAttribute"/> would force into the served document.
    /// <para>Applied to a query parameter it is enforced by MVC model validation, so <c>[ApiController]</c> answers a violation with HTTP 400 before the action runs - the same rejection the action's own guard already produced, now at binding (DiGi-Organization/DiGi.GIS.WebAPI#47). A null value passes, so an optional parameter (an <c>int?</c> county filter) keeps its "omitted means no filter" meaning; a mandatory one is refused by <c>[BindRequired]</c>, not here.</para>
    /// <para>A floor states the guard it mirrors exactly: 0 for <c>commandTimeout &lt; 0</c>, 1 for <c>countyId &lt;= 0</c>, so the attribute never rejects a value the action accepts nor accepts one it rejects. A non-numeric or <see cref="double.NaN"/> value is refused, matching the actions' own finiteness guards.</para>
    /// <para><see cref="ParameterMinimumSchemaFilter"/> carries the same floor into the parameter's OpenAPI schema as <c>minimum</c> - and as <c>exclusiveMinimum</c> when <see cref="Exclusive"/> is set - so the document states the domain without the absurd <c>maximum</c> a <c>[Range(min, double.MaxValue)]</c> would emit.</para>
    /// </summary>
    public sealed class MinimumAttribute : ValidationAttribute
    {
        /// <summary>
        /// The smallest value the parameter may carry, inclusive unless <see cref="Exclusive"/> is set.
        /// </summary>
        public double Minimum { get; }

        /// <summary>
        /// Whether the floor itself is excluded, for a domain that is strictly greater than the floor rather than at least it.
        /// </summary>
        public bool Exclusive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimumAttribute"/> class with an inclusive floor.
        /// </summary>
        /// <param name="minimum">The smallest value the parameter may carry.</param>
        public MinimumAttribute(double minimum)
        {
            Minimum = minimum;
        }

        /// <summary>
        /// Determines whether the bound value lies at or above the floor, or strictly above it when <see cref="Exclusive"/> is set.
        /// </summary>
        /// <param name="value">The bound value; null (an omitted optional parameter) is always valid.</param>
        /// <param name="validationContext">The context describing the parameter being validated.</param>
        /// <returns><see cref="ValidationResult.Success"/> when the value lies inside the domain; otherwise a result naming the floor.</returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return ValidationResult.Success;
            }

            if (value is not IConvertible)
            {
                return Invalid(validationContext);
            }

            double double_Value = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);

            // NaN compares false against every floor, so a value the actions' own finiteness guards refuse is refused here too.
            if (double.IsNaN(double_Value) || double_Value < Minimum || (Exclusive && double_Value == Minimum))
            {
                return Invalid(validationContext);
            }

            return ValidationResult.Success;
        }

        private ValidationResult Invalid(ValidationContext validationContext)
        {
            string relationship = Exclusive ? "greater than" : "at least";

            return new ValidationResult($"The field {validationContext.DisplayName} must be {relationship} {Minimum.ToString(CultureInfo.InvariantCulture)}.", [validationContext.MemberName ?? validationContext.DisplayName]);
        }
    }
}
