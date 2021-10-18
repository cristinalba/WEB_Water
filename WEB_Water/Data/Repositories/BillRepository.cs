using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;

namespace WEB_Water.Data.Repositories
{
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public BillRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Bill>> GetBillAsync(string id)
        {
            var user = await _userHelper.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return _context.Bills
               .Include(r => r.Reader)
               .Include(x => x.Reading)
               .Where(u => u.User == user)
               .OrderByDescending(b => b.BillDate);

        }

        public void AddBill(Bill billfromModel)
        {
            _context.Bills.Add(billfromModel);
        }

        public async Task<Bill> GetBillByIdAsync(string id)//It receives the idDesencrypted
        {
            return await _context.Bills
             .Include(u => u.User)
             .Include(r => r.Reader)
             .Include(x => x.Reading)
             .Where(u => u.Id == Convert.ToInt32(id))
             .FirstOrDefaultAsync();
        }
        public bool UpdateStatusBill(Reading updateReading)
        {
            return _context.Entry(updateReading).Property("BillIssued").IsModified= true;
        }

        public async Task<bool>  SaveBillAsync()
        {
            return await _context.SaveChangesAsync()>0;
        }

        public bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.Reading.Id == id);
        }

        public IQueryable GetAllBillsWithUsers()
        {
            return _context.Bills
                .Include(u => u.User);
              
        }
        

    }
}
