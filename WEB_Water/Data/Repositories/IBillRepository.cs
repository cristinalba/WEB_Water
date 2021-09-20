using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Data.Repositories
{
    public interface IBillRepository : IGenericRepository<Bill>
    {
        Task<IQueryable<Bill>> GetBillAsync(string username);

        //bool BillExists(int id);
    }
}
