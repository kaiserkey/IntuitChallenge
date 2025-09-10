using System.Data.Common;
using Intuit.Api.Domain;
using Intuit.Api.Exceptions;
using Intuit.Api.Interfaces;
using Intuit.Api.Logging;
using Microsoft.EntityFrameworkCore;

namespace Intuit.Api.Data.Repository
{
    public class ClientRepository : Repository, IClientRepository
    {
        public ClientRepository(IntuitDBContext context, ILogService<LogService> logService)
        {
            Context = context;
            LogService = logService;
        }

        public async Task<bool> AddAsync(Client entity)
        {
            try
            {
                await Context.Client.AddAsync(entity);

                var result = await Context.SaveChangesAsync();

                return result > 0;
            }
            catch (DbException ex)
            {
                throw new AppException($"[DataBaseException] - An error occurred while adding a new client: {ToJson(entity)}", LogService, ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var client = await Context.Client.FindAsync(id);

                if (client == null || client.ClientId == 0)
                {
                    return false;
                }

                Context.Client.Remove(client);

                var result = await Context.SaveChangesAsync();

                return result > 0;
            }
            catch (DbException ex)
            {
                throw new AppException($"[DataBaseException] - An error occurred while deleting a client with id: {id}", LogService, ex);
            }
        }

        public async Task<List<Client?>> GetAllAsync()
        {
            try
            {
                var clients = await Context.Client
                    .OrderBy(c => c.ClientId)
                    .ToListAsync();

                return clients;
            }
            catch (DbException ex)
            {
                throw new AppException("[DataBaseException] - An error occurred while retrieving all clients", LogService, ex);
            }
        }

        public async Task<bool> GetAnyAsync(string Cuit)
        {
            try
            {
                var exists = await Context.Client.AnyAsync(c => c.Cuit == Cuit);
                return exists;
            }
            catch (DbException ex)
            {
                throw new AppException($"[DataBaseException] - An error occurred while searching for clients by CUIT: {Cuit}", LogService, ex);
            }
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            try
            {
                var client = await Context.Client.FindAsync(id);

                return client;
            }
            catch (DbException ex)
            {
                throw new AppException($"[DataBaseException] - An error occurred while retrieving a client with id: {id}", LogService, ex);
            }
        }

        public async Task<List<Client?>> SearchByNameAsync(string name)
        {
            try
            {
                var clients = await Context.Client
                    .Where(c => EF.Functions.ILike(c.FirstName + " " + c.LastName, $"%{name}%"))
                    .OrderBy(c => c.ClientId)
                    .ToListAsync();

                return clients;
            }
            catch (DbException ex)
            {
                throw new AppException($"[DataBaseException] - An error occurred while searching for clients by name: {name}", LogService, ex);
            }
        }

        public async Task<bool> UpdateAsync(Client entity)
        {
            try
            {
                var existingClient = await Context.Client.FindAsync(entity.ClientId);

                if (existingClient == null || existingClient.ClientId == 0)
                {
                    return false;
                }

                Context.Entry(existingClient).CurrentValues.SetValues(entity);

                var result = await Context.SaveChangesAsync();

                return result > 0;
            }
            catch (DbException ex)
            {
                throw new AppException($"[DataBaseException] - An error occurred while updating a client: {ToJson(entity)}", LogService, ex);
            }
        }
    }
}
