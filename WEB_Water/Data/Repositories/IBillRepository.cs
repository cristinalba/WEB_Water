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

        public void AddBill(Bill billfromModel);

        Task<Bill> GetBillByIdAsync(string id);

        public bool UpdateStatusBill(Reading updateReading);

        Task<bool> SaveBillAsync();

        bool BillExists(int id);
    }
}
