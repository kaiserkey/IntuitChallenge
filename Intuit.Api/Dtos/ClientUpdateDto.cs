using System.ComponentModel.DataAnnotations;
using Intuit.Api.Validations;

namespace Intuit.Api.Dtos
{
    /// <summary>
    /// Data Transfer Object for updating existing client information.
    /// </summary>
    public record ClientUpdateDto(
        [Required] int ClientId,
        [Required] string FirstName,
        [Required] string LastName,
        [Required, BirthDateInPast] DateOnly BirthDate,
        [Required, Cuit] string Cuit,
        string? Address,
        [Required] string Mobile,
        [Required, EmailAddress] string Email
    );
}
