using System.ComponentModel.DataAnnotations;

namespace Intuit.Api.Validations
{
    /// <summary>
    /// Validation attribute to ensure a birth date is in the past.
    /// </summary>
    public sealed class BirthDateInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext _)
        {
            if (value is not DateOnly d) return ValidationResult.Success; //por si se usa erroneamente en otro tipo
            return d < DateOnly.FromDateTime(DateTime.UtcNow.Date)
                ? ValidationResult.Success
                : new("La fecha de nacimiento debe ser menor a la fecha actual.");
        }
    }
}
