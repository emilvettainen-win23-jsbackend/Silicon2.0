using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;


namespace Infrastructure.Repositories;

public class BaseRepository<TEntity, TContext>(TContext context, ILogger<BaseRepository<TEntity, TContext>> logger) where TEntity : class where TContext : DbContext
{
    private readonly TContext _context = context;
    private readonly ILogger<BaseRepository<TEntity, TContext>> _logger = logger;

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().AnyAsync(predicate);
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.ExistsAsync() :: {ex.Message}");
        }
        return false;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.CreateAsync() :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.GetOneAsync() :: {ex.Message}");
        }
        return null!;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            return entities;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.GetAllAsync() :: {ex.Message}");
        }
        return null!;
    }

    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity updatedEntity)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null && updatedEntity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(updatedEntity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.UpdateAsync() :: {ex.Message}");
        }
        return null!;
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : BaseRepository.DeleteAsync() :: {ex.Message}");
        }
        return false;
    }

}
