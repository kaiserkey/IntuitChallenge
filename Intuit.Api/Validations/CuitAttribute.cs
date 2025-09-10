using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Intuit.Api.Validations
{
    /// <summary>
    /// Validation attribute to ensure a CUIT (Clave Única de Identificación Tributaria) is valid.
    /// </summary>
    public sealed partial class CuitAttribute : ValidationAttribute
    {
        [GeneratedRegex("[^0-9]")] // genera una expresión regular para encontrar caracteres no numéricos                 
        private static partial Regex NonDigits(); 

        protected override ValidationResult? IsValid(object? value, ValidationContext _)
        {
            if (value is not string s) return ValidationResult.Success; // por si se usa erroneamente en otro tipo
            var digits = NonDigits().Replace(s, ""); // elimina todo lo que no sea dígito 
            if (digits.Length != 11) return new("El CUIT debe tener 11 dígitos.");

            // Cálculo del dígito verificador
            int[] weights = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2]; 
            int sum = 0;
            for (int i = 0; i < 10; i++) sum += (digits[i] - '0') * weights[i];
            int mod = 11 - (sum % 11);
            int dv = mod == 11 ? 0 : mod == 10 ? 9 : mod;

            return dv == (digits[10] - '0')
                ? ValidationResult.Success
                : new("CUIT inválido.");
        }
    }
}
