using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Address
{
    public class UserAddressRepository(ApplicationDbContext context, ILogger<UserAddressRepository> xlogger, ILogger<BaseRepository<UserAddressEntity, ApplicationDbContext>> logger) : BaseRepository<UserAddressEntity, ApplicationDbContext>(context, logger)
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<UserAddressRepository> _logger = xlogger;


        public async Task<UserAddressEntity> GetUserAddressAsync(string userId, int addressId)
        {
            try
            {
                var entity = await _context.UserAddresses.FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AddressId == addressId);
                if (entity != null)
                {
                    return entity;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : UserAddressRepository.GetUserAddressAsync() :: {ex.Message}");
            }
            return null!;
        }

        public async Task<UserAddressEntity> GetAllAddressesAsync(string userId)
        {
            try
            {
                var entities = await _context.UserAddresses
                    .Where(ua => ua.UserId == userId)
                    .Include(a => a.Address)
                    .Include(o => o.OptionalAddress)
                    .FirstOrDefaultAsync();
                if (entities != null)
                {
                    return entities;
                }
                return null!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR : UserAddressRepository.GetAllAddressesAsync() :: {ex.Message}");
                return null!;
            }
        }


    }
}
