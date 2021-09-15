using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;
using WEB_Water.Models;

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
        public async Task AddReaderToListAsync(AddReaderViewModel model, string username)
        {
            
            //var userlogIn = await _userHelper.GetUserByEmailAsync(username);
            //if (userlogIn == null)
            //{
            //    return;
            //}

            //if (await _userHelper.IsUserInRoleAsync(userlogIn, "Admin"))
            //{
                var user = await _context.Users.FindAsync(model.UserId);

                var equipment = new Reader
                {
                    ReaderName = model.ReaderName,
                    Installation = model.Installation,
                    AddressName = model.AddressName,
                    User = user
                };

                _context.Readers.Add(equipment);


                await _context.SaveChangesAsync();
            //}

            //return;

        }
    }
}
