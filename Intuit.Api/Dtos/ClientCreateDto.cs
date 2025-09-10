using System.ComponentModel.DataAnnotations;
using Intuit.Api.Validations;

namespace Intuit.Api.Dtos
{
    /// <summary>
    /// Data Transfer Object for creating a new client.
    /// </summary>
    public record ClientCreateDto(
        [Required] string FirstName,
        [Required] string LastName,
        [Required, BirthDateInPast] DateOnly BirthDate,
        [Required, Cuit] string Cuit,
        string? Address,
        [Required] string Mobile,
        [Required, EmailAddress(ErrorMessage = "El email ingresado tiene un formato incorrecto.")] string Email
    );
}
