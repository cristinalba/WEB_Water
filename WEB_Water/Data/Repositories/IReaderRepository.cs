using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Models;

namespace WEB_Water.Data.Repositories
{
    public interface IReaderRepository : IGenericRepository<Reader>
    {

        Task AddReaderToListAsync(AddReaderViewModel model, string name);
    }
}
