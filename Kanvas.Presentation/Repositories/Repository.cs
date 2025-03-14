using Microsoft.EntityFrameworkCore;

namespace Presentation.Repositories;

public abstract class Repository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;

    protected Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public virtual void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }
    
    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<TEntity>()
            .FindAsync(id);
    }

    public virtual async Task<bool> Exists()
    {
        return await _context.Set<TEntity>().AnyAsync();
    }
}