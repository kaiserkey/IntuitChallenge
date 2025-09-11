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
            catch (DbUpdateException ex) 
            {
                throw new AppException($"[DB] Add failed: {ToJson(entity)}", LogService, ex);
            }
        }

        public async Task<bool> UpdateAsync(Client entity)
        {
            try
            {
                var existing = await Context.Client.FindAsync(entity.ClientId);
                if (existing is null) return false;

                Context.Entry(existing).CurrentValues.SetValues(entity);
                var result = await Context.SaveChangesAsync();
                return result > 0;
            }
            catch (DbUpdateConcurrencyException) 
            {
                throw; 
            }
            catch (DbUpdateException ex) 
            {
                throw new AppException($"[DB] Update failed: {ToJson(entity)}", LogService, ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var client = await Context.Client.FindAsync(id);
                if (client is null) return false;

                Context.Client.Remove(client);
                var result = await Context.SaveChangesAsync();
                return result > 0;
            }
            catch (DbUpdateException ex) 
            {
                throw new AppException($"[DB] Delete failed id={id}", LogService, ex);
            }
        }

        public async Task<List<Client>?> GetAllAsync()
        {
            try
            {
                return await Context.Client.OrderBy(c => c.ClientId).ToListAsync();
            }
            catch (DbException ex) 
            {
                throw new AppException("[DB] GetAll failed", LogService, ex);
            }
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            try
            {
                return await Context.Client.FindAsync(id);
            }
            catch (DbException ex) 
            {
                throw new AppException($"[DB] GetById failed id={id}", LogService, ex);
            }
        }

        public async Task<bool> GetAnyAsync(string cuit)
        {
            try
            {
                return await Context.Client.AnyAsync(c => c.Cuit == cuit);
            }
            catch (DbException ex)
            {
                throw new AppException($"[DB] Exists by CUIT failed cuit={cuit}", LogService, ex);
            }
        }

        public async Task<List<Client>?> SearchByNameAsync(string name)
        {
            try
            {
                return await Context.Client
                    .Where(c => EF.Functions.ILike(c.FirstName + " " + c.LastName, $"%{name}%"))
                    .OrderBy(c => c.ClientId)
                    .ToListAsync();
            }
            catch (DbException ex) 
            {
                throw new AppException($"[DB] Search failed name='{name}'", LogService, ex);
            }
        }
    }
}
