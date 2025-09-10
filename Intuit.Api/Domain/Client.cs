namespace Intuit.Api.Domain
{
    /// <summary>
    /// Represents a client entity.
    /// </summary>
    public class Client
    {
        public int ClientId { get; set; } 
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public DateOnly BirthDate { get; set; }
        public string Cuit { get; set; } = default!;
        public string? Address { get; set; }
        public string Mobile { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
