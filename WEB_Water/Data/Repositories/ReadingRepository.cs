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
                Begin = model.Begin,
                End = model.End,
                ValueOfConsume = model.ValueOfConsume,
                RegistrationDateNewReading = DateTime.UtcNow
            };

            _context.Readings.Add(reading);

            await _context.SaveChangesAsync();//Guardar
        }

        public async Task<IQueryable<Reading>> GetReadingAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return null;
            }

            //if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            //{
            //    //Admin
            //    return _context.Readings
            //        .Include(o => o.User)
            //        .Include(c => c.Reader) //ThenInclude
            //        .OrderByDescending(c => c.RegistrationDateNewReading);
            //}

            if (await _userHelper.IsUserInRoleAsync(user, "Worker"))
            {
                //Worker can see the readings from the customers
                return _context.Readings
                    .Include(o => o.User)
                    .Include(c => c.Reader) //ThenInclude
                    .OrderByDescending(c => c.RegistrationDateNewReading);
            }

            //Customer

            return _context.Readings
                   .Include(i => i.Reader)
                   .Where(c => c.User == user)
                   .OrderByDescending(c => c.RegistrationDateNewReading);

        }

        public async Task<Reading> GetReadingByIdAsync(int id)
        {
            return await _context.Readings
                 .Include(e => e.Reader)
                 .Where(u => u.Id == id)
                 .FirstOrDefaultAsync();
        }


        //public bool BillExists(int id)
        //{
        //    return _context.Bills.Any(e => e.Reading.Id == id);
        //}

        //public async Task<T> GetByIdAsync(int id) //search by Id, just one
        //{
        //    return await _context.Set<T>()
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(e => e.Id == id);
        //}

        //////   var consumption = await _context.Readings
        //////.Include(e => e.Reader)
        //////.Where(u => u.Id == id)
        //////.FirstOrDefaultAsync();



    }
}
