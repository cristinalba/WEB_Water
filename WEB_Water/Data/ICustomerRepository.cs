using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;

namespace WEB_Water.Data
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public IQueryable GetAllWithUsers();
    }
}
