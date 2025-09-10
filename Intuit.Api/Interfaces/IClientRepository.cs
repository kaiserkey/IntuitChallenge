using Intuit.Api.Domain;

namespace Intuit.Api.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<List<Client?>> SearchByNameAsync(string name);
        Task<bool> GetAnyAsync(string Cuit);
    }
}
