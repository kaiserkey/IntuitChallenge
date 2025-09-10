using Intuit.Api.Dtos;
using Intuit.Api.Models;

namespace Intuit.Api.Interfaces
{
    public interface IService
    {
        Task<ServiceResult> GetAllClients();
        Task<ServiceResult> GetByClientId(int id);
        Task<ServiceResult> AddNewClients(ClientCreateDto entity);
        Task<ServiceResult> UpdateClient(ClientUpdateDto entity);
        Task<ServiceResult> DeleteClient(int id);
    }
}
