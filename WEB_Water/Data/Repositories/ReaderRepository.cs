using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ReaderRepository(DataContext context, IUserHelper userHelper) : base(context)
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

        public IEnumerable<SelectListItem> GetComboReaders()
        {
            var list = _context.Readers
                .Select(r => new SelectListItem
                {
                    Text = r.ReaderName,
                    Value = r.Id.ToString()
                }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a reader...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboReaders(string email)
        {
            var list = _context.Readers.Where(x => x.User.UserName == email)
                .Select(r => new SelectListItem
                {
                    Text = r.ReaderName,
                    Value = r.Id.ToString()
                }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a reader...)",
                Value = "0"
            });

            return list;
        }
    }
}
