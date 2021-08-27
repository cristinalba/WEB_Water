using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class,
                                        IEntity
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll() //search all
        {
            return _context.Set<T>().AsNoTracking(); //It searchs for it in the table(T) and then it doesn't leave a track
        }

        public async Task<T> GetByIdAsync(int id) //search by Id, just one
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await SaveAllAsync();//After creating, it is saved with this method
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity); //Update doesn't exist async
            await SaveAllAsync();//After updating, it is saved 

        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity); //Remove doesn't exist async
            await SaveAllAsync();//After creating, it is saved with this method
        }

        public async Task<bool> ExistAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
