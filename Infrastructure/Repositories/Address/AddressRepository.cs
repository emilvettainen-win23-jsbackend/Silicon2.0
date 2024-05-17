using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.Address;

public class AddressRepository(ApplicationDbContext context, ILogger<BaseRepository<AddressEntity, ApplicationDbContext>> logger) : BaseRepository<AddressEntity, ApplicationDbContext>(context, logger)
{
}
