using Intuit.Api.Domain;
using Intuit.Api.Dtos;
using Intuit.Api.Exceptions;
using Intuit.Api.Interfaces;
using Intuit.Api.Logging;

namespace Intuit.Api.Services
{
    public class ClientService : AppService, IClientService
    {
        public ClientService(IClientRepository clientRepository, ILogService<LogService> logService)
        {
            ClientRepository = clientRepository;
            LogService = logService;
        }

        public async Task<bool> AddNewClients(ClientCreateDto dto)
        {
            if(await ClientRepository.GetAnyAsync(dto.Cuit))
            {
                throw new AppException($"[ApplicationException] - Client with Cuit: {dto.Cuit} already exists", LogService);
            }
            else
            {
                try
                {
                    var client = new Client
                    {
                        FirstName = dto.FirstName.Trim(),
                        LastName = dto.LastName.Trim(),
                        BirthDate = dto.BirthDate,
                        Cuit = dto.Cuit,
                        Address = dto.Address?.Trim(),
                        Mobile = dto.Mobile.Trim(),
                        Email = dto.Email.Trim().ToLowerInvariant()
                    };

                    var result = await ClientRepository.AddAsync(client);

                    if (!result)
                    {
                        throw new AppException($"[ApplicationException] - Failed to add a new client: {ToJson(dto)}", LogService);
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw new AppException($"[ApplicationException] - An error occurred while adding a new client: {ToJson(dto)}", LogService, ex);
                }
            }

        }

        public async Task<bool> DeleteClient(int id)
        {
            try
            {
                var result = await ClientRepository.DeleteAsync(id);

                if (!result)
                {
                    throw new AppException($"[ApplicationException] - Failed to delete client with id: {id}", LogService);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new AppException($"[ApplicationException] - An error occurred while deleting a client with id: {id}", LogService, ex);
            }
        }

        public async Task<List<ClientReadDto?>> GetAllClients()
        {
            List<ClientReadDto?> clientsDto = new List<ClientReadDto?>();

            var clients = await ClientRepository.GetAllAsync();

            if (clients != null)
            {
                foreach (var client in clients)
                {
                    clientsDto.Add(new ClientReadDto(
                        client.ClientId,
                        client.FirstName,
                        client.LastName,
                        client.BirthDate,
                        client.Cuit,
                        client.Address,
                        client.Mobile,
                        client.Email
                    ));
                }
            }

            return clientsDto;
        }

        public async Task<ClientReadDto?> GetByClientId(int id)
        {
            var client = await ClientRepository.GetByIdAsync(id);

            if (client == null)
            {
                throw new AppException($"[ApplicationException] - Client with id: {id} not found", LogService);
            }

            return new ClientReadDto(
                client.ClientId,
                client.FirstName,
                client.LastName,
                client.BirthDate,
                client.Cuit,
                client.Address,
                client.Mobile,
                client.Email
            );
        }

        public async Task<List<ClientReadDto?>> SearchClientByNameAsync(string name)
        {
            List<ClientReadDto?> clientsDto = new List<ClientReadDto?>();

            var clients = await ClientRepository.SearchByNameAsync(name);

            if (clients != null)
            {
                foreach (var client in clients)
                {
                    clientsDto.Add(new ClientReadDto(
                        client.ClientId,
                        client.FirstName,
                        client.LastName,
                        client.BirthDate,
                        client.Cuit,
                        client.Address,
                        client.Mobile,
                        client.Email
                    ));
                }
            }

            return clientsDto;
        }

        public async Task<bool> UpdateClient(ClientUpdateDto dto)
        {
            try
            {
                var client = await ClientRepository.GetByIdAsync(dto.ClientId);

                if (client == null)
                {
                    throw new AppException($"[ApplicationException] - Client with id: {dto.ClientId} not found", LogService);
                }

                var valid = await ClientRepository.GetAnyAsync(dto.Cuit);

                if (valid)
                {
                    throw new AppException($"[ApplicationException] - Client with Cuit: {dto.Cuit} already exists", LogService);
                }

                client.FirstName = dto.FirstName.Trim();
                client.LastName = dto.LastName.Trim();
                client.BirthDate = dto.BirthDate;
                client.Cuit = dto.Cuit;
                client.Address = dto.Address?.Trim();
                client.Mobile = dto.Mobile.Trim();
                client.Email = dto.Email.Trim().ToLowerInvariant();

                var result = await ClientRepository.UpdateAsync(client);

                if (!result)
                {
                    throw new AppException($"[ApplicationException] - Failed to update client: {ToJson(dto)}", LogService);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new AppException($"[ApplicationException] - An error occurred while updating a client: {ToJson(dto)}", LogService, ex);
            }
        }
    }
}
