using Intuit.Api.Dtos;

namespace Intuit.Api.Interfaces
{
    public interface IService
    {
        Task<List<ClientReadDto?>> GetAllClients();
        Task<ClientReadDto?> GetByClientId(int id);
        Task<bool> AddNewClients(ClientCreateDto entity);
        Task<bool> UpdateClient(ClientUpdateDto entity);
        Task<bool> DeleteClient(int id);
    }
}
