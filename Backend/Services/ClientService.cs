using Backend.Common;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
    public interface IClientService
    {
        Task<Result<IEnumerable<Client>>> GetAllAsync();
        Task<Result<Client>> GetByIdAsync(int id);
        Task<Result<Client>> CreateAsync(Client client);
        Task<Result> UpdateAsync(Client client);
        Task<Result> SoftDeleteAsync(int id);
    }

    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Client>>> GetAllAsync()
        {
            var clients = await _context.Clients.ToListAsync();
            return Result<IEnumerable<Client>>.Success(clients);
        }

        public async Task<Result<Client>> GetByIdAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return Result<Client>.Failure("Client not found.");
            return Result<Client>.Success(client);
        }

        public async Task<Result<Client>> CreateAsync(Client client)
        {
            try
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return Result<Client>.Success(client);
            }
            catch (Exception ex)
            {
                return Result<Client>.Failure(ex.Message);
            }
        }

        public async Task<Result> UpdateAsync(Client client)
        {
            try
            {
                _context.Entry(client).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> SoftDeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return Result.Failure("Client not found.");

            client.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
