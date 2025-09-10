namespace Intuit.Api.Dtos
{
    /// <summary>
    /// Data Transfer Object for reading client information.
    /// </summary>
    public record ClientReadDto(
        int ClientId, 
        string FirstName, 
        string LastName, 
        DateOnly BirthDate,
        string Cuit, 
        string? Address, 
        string Mobile, 
        string Email
    );
}
