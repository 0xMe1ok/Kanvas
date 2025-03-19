using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Presentation.Data;
using Presentation.Entities;

namespace Presentation.Repositories;

public abstract class Repository<TEntity> where TEntity : EntityBase<Guid>
{
    protected readonly ApplicationDbContext _context;

    protected Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
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
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
    }
    
    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _context.Set<TEntity>()
            .FindAsync(id);
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Set<TEntity>().AnyAsync(e => e.Id == id);
    }
    
    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().Where(predicate).AnyAsync();
    }


    public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
    }
    
    public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().Where(predicate).ToListAsync();
    }
}