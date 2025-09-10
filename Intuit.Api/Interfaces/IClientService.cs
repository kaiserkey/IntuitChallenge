using Intuit.Api.Dtos;

namespace Intuit.Api.Interfaces
{
    public interface IClientService : IService
    {
        Task<List<ClientReadDto?>> SearchClientByNameAsync(string name);
    }
}
