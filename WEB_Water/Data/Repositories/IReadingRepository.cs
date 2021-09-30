using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Models;

namespace WEB_Water.Data
{
    public interface IReadingRepository : IGenericRepository<Reading>
    {
        Task<IQueryable<Reading>> GetReadingAsync(string username);

        Task AddReadingToCustomerAsync(AddReadingViewModel model, string username);

        Task<Reading> GetReadingByIdAsync(int id);

        public bool BillExists(int id);

    }
}
