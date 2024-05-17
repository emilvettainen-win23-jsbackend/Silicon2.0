using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Microsoft.Extensions.Logging;


namespace Infrastructure.Repositories.Address;

public class OptionalAddressRepository(ApplicationDbContext context, ILogger<BaseRepository<OptionalAddressEntity, ApplicationDbContext>> logger) : BaseRepository<OptionalAddressEntity, ApplicationDbContext>(context, logger)
{
}