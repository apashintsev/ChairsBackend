using ChairsBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChairsBackend.DAL
{
    public interface IRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game> GetByIdAsync(int id);
        Task<int> AddAsync(Game entity);
        Task UpdateAsync(Game entity);
        Task DeleteAsync(Game entity);
        Task<Game> SingleOrDefaultAsync(Expression<Func<Game, bool>> predicate);
        Task<IEnumerable<Game>> GetWhere(Expression<Func<Game, bool>> predicate);
        Task<Game> GetWithNewestIdAsync();
    }

    public class Repository : IRepository
    {
        private readonly GameContext _dbContext;
        private readonly DbSet<Game> _dbSet;

        public Repository(GameContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<Game>();
        }

        public async Task<Game> GetWithNewestIdAsync()
        {
            return await _dbSet.AsNoTracking().Include(x=>x.Players).Include(x=>x.Winners).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return  await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<Game> GetByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.Players).Include(x => x.Winners).FirstOrDefaultAsync(x=>x.Id==id);
        }


        public async Task<Game> SingleOrDefaultAsync(Expression<Func<Game, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<Game>> GetWhere(Expression<Func<Game, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<int> AddAsync(Game entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(Game entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Game entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
