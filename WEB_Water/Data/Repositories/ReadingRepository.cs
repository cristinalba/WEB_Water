using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;
using WEB_Water.Models;

namespace WEB_Water.Data
{
    public class ReadingRepository : GenericRepository<Reading>, IReadingRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ReadingRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task AddReadingToCustomerAsync(AddReadingViewModel model, string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return;
            }

            var reader = await _context.Readers.FindAsync(model.ReaderId);
            if (reader == null)
            {
                return;
            }

            var reading = new Reading
            {
                User = user,
                Reader = reader,
                Begin = model.Start,
                End = model.End,
                MonthlyConsume = model.MonthlyConsume,
                RegistrationDateNewReading = DateTime.UtcNow
            };

            _context.Readings.Add(reading);

            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<Reading>> GetReadingAsync(string username)
        {
            //throw new NotImplementedException();
            var user = await _userHelper.GetUserByEmailAsync("admin@webwater.com");
            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                //Admin
                return _context.Readings
                    .Include(o => o.User)
                    .Include(c => c.Reader)
                    .OrderByDescending(c => c.RegistrationDateNewReading);
            }

            //Customer
            //if (await _userHelper.IsUserInRoleAsync(user, "Customer"))
            //{ }
            //Worker
            //if (await _userHelper.IsUserInRoleAsync(user, "Worker"))
            //{ }
            return _context.Readings
                .Include(i => i.Reader)
                .Where(c => c.User == user)
                .OrderByDescending(c => c.RegistrationDateNewReading);
        }
    }
}
