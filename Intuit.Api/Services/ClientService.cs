using Intuit.Api.Domain;
using Intuit.Api.Dtos;
using Intuit.Api.Exceptions;
using Intuit.Api.Interfaces;
using Intuit.Api.Logging;
using Intuit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Intuit.Api.Services
{
    public class ClientService : AppService, IClientService
    {
        public ClientService(IClientRepository clientRepository, ILogService<LogService> logService)
        {
            ClientRepository = clientRepository;
            LogService = logService;
        }

        public async Task<ServiceResult> GetAllClients()
        {
            var list = await ClientRepository.GetAllAsync();
            var dto = list.Select(c => new ClientReadDto(
                          c.ClientId, c.FirstName, c.LastName, c.BirthDate, c.Cuit, c.Address, c.Mobile, c.Email))
                           .ToList();

            LogService.LogInfo($"[Service] GetAll -> {dto.Count} items");
            return ServiceResult.Ok(dto);
        }

        public async Task<ServiceResult> GetByClientId(int id)
        {
            var c = await ClientRepository.GetByIdAsync(id);
            if (c is null)
            {
                LogService.LogWarning($"[Service] GetById -> 404 (id={id})");
                return ServiceResult.NotFound($"El cliente con id={id} no existe.");
            }

            var dto = new ClientReadDto(c.ClientId, c.FirstName, c.LastName, c.BirthDate, c.Cuit, c.Address, c.Mobile, c.Email);
            LogService.LogInfo($"[Service] GetById -> 200 (id={id})");
            return ServiceResult.Ok(dto);
        }

        public async Task<ServiceResult> SearchClientByNameAsync(string nameOrQuery)
        {
            var list = await ClientRepository.SearchByNameAsync(nameOrQuery);
            var dto = list.Select(c => new ClientReadDto(
                          c.ClientId, c.FirstName, c.LastName, c.BirthDate, c.Cuit, c.Address, c.Mobile, c.Email))
                           .ToList();

            LogService.LogInfo($"[Service] Search '{nameOrQuery}' -> {dto.Count} items");
            return ServiceResult.Ok(dto);
        }

        public async Task<ServiceResult> AddNewClients(ClientCreateDto dto)
        {
            if (await ClientRepository.GetAnyAsync(dto.Cuit))
            {
                LogService.LogWarning($"[Service] Create -> 409 (CUIT={dto.Cuit})");
                return ServiceResult.Conflict($"CUIT {dto.Cuit} ya existe.");
            }

            var c = new Client
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                BirthDate = dto.BirthDate,
                Cuit = dto.Cuit,
                Address = dto.Address?.Trim(),
                Mobile = dto.Mobile.Trim(),
                Email = dto.Email.Trim().ToLowerInvariant()
            };

            var ok = await ClientRepository.AddAsync(c);
            if (!ok)
            {
                throw new AppException($"[DB] Insert returned 0 rows: {ToJson(dto)}", LogService);
            }

            var read = new ClientReadDto(c.ClientId, c.FirstName, c.LastName, c.BirthDate, c.Cuit, c.Address, c.Mobile, c.Email);
            LogService.LogInfo($"[Service] Create -> 201 (id={c.ClientId})");
            return ServiceResult.Ok(read, "Cliente creado correctamente.");
        }

        public async Task<ServiceResult> UpdateClient(ClientUpdateDto dto)
        {
            var c = await ClientRepository.GetByIdAsync(dto.ClientId);
            if (c is null)
            {
                LogService.LogWarning($"[Service] Update -> 404 (id={dto.ClientId})");
                return ServiceResult.NotFound($"Id {dto.ClientId} no existe.");
            }

            // Evitar falso duplicado si el CUIT no cambió
            var cuitDuplicado = await ClientRepository.GetAnyAsync(dto.Cuit)
                                 && !string.Equals(c.Cuit, dto.Cuit, StringComparison.OrdinalIgnoreCase);
            if (cuitDuplicado)
            {
                LogService.LogWarning($"[Service] Update -> 409 (CUIT={dto.Cuit})");
                return ServiceResult.Conflict($"CUIT {dto.Cuit} pertenece a otro cliente.");
            }

            c.FirstName = dto.FirstName.Trim();
            c.LastName = dto.LastName.Trim();
            c.BirthDate = dto.BirthDate;
            c.Cuit = dto.Cuit;
            c.Address = dto.Address?.Trim();
            c.Mobile = dto.Mobile.Trim();
            c.Email = dto.Email.Trim().ToLowerInvariant();

            try
            {
                var ok = await ClientRepository.UpdateAsync(c);
                if (!ok)
                    throw new AppException($"[DB] Update returned 0 rows: {ToJson(dto)}", LogService);

                var read = new ClientReadDto(c.ClientId, c.FirstName, c.LastName, c.BirthDate, c.Cuit, c.Address, c.Mobile, c.Email);
                LogService.LogInfo($"[Service] Update -> 200 (id={c.ClientId})");
                return ServiceResult.Ok(read, "Cliente actualizado.");
            }
            catch (DbUpdateConcurrencyException)
            {
                LogService.LogWarning($"[Service] Update -> 409 (concurrencia)");
                return ServiceResult.Conflict("Conflicto de concurrencia.");
            }
        }

        public async Task<ServiceResult> DeleteClient(int id)
        {
            var ok = await ClientRepository.DeleteAsync(id);
            if (!ok)
            {
                LogService.LogWarning($"[Service] Delete -> 404 (id={id})");
                return ServiceResult.NotFound($"El cliente con Id {id} no existe.");
            }

            LogService.LogInfo($"[Service] Delete -> 204 (id={id})");
            return new ServiceResult { Result = true, Message = "Cliente eliminado.", Status = StatusCodes.Status200OK };
        }
    }
}
