using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;

namespace WEB_Water.Data.Repositories
{
    public class ReaderRepository : GenericRepository<Reader>, IReaderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ReaderRepository(DataContext context, IUserHelper userHelper): base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }
    }
}
