using Intuit.Api.Models;

namespace Intuit.Api.Interfaces
{
    public interface IClientService : IService
    {
        Task<ServiceResult> SearchClientByNameAsync(string nameOrQuery);
    }
}
