using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data;
using WEB_Water.Data.Entities;
using WEB_Water.Data.Repositories;
using WEB_Water.Helpers;
using WEB_Water.Models;

namespace WEB_Water.Controllers
{
    public class BillsController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBillRepository _billRepository;
        private readonly IReadingRepository _readingRepository;

        public BillsController(DataContext context, IUserHelper userHelper, IBillRepository billRepository, IReadingRepository readingRepository)
        {
            _context = context;
            _userHelper = userHelper;
            _billRepository = billRepository;
            _readingRepository = readingRepository;
        }
        public async Task<IActionResult> Index() //Show all the customers with a button to go to the bills
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))//Worker
            {
                return View(_userHelper.GetAll().OrderBy(u => u.UserName));//fullname
            }

            var customers = _userHelper.GetAll()
            .Where(u => u.UserName == user.UserName)
            .OrderBy(u => u.FullName);//Username

            return View(customers);
        }
        public async Task<IActionResult> BillSummary(string id) //All  the bills of a customer
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))//Worker
            {
                var model = await _billRepository.GetBillAsync(id);
                return View(model);
            }
            else
            {
                var model = await _billRepository.GetBillAsync(user.Id);
                return View(model);
            }

        }

        
        public async Task<IActionResult> CalculateBill(int? id)//Receives the id of the selected Reading to calculate the value to pay and show it
        {
            if (id == null)
            {
                return NotFound();
            }

            if (BillExists(id.Value) == true)
            {
                return RedirectToAction("Index");
            }

            var model = await _context.Readings
                .Include(u => u.User)
                .ThenInclude(r => r.Readers)
                .Where(u => u.Id == id).FirstOrDefaultAsync();

            if (model == null)
            {
                return NotFound();
            }

            double TotalValue = 0;

            for (int i = 1; i <= model.ValueOfConsume; i++)
            {
                if (i <= 5)
                {
                    TotalValue += 0.30;
                }
                else if (i > 5 && i <= 15)
                {
                    TotalValue += 0.80;
                }
                else if (i > 15 && i <= 25)
                {
                    TotalValue += 1.20;
                }
                else
                {
                    TotalValue += 1.60;
                }
            }

            var billfromModel = new Bill
            {
                Reader = model.Reader,
                Reading = model,
                BillDate = DateTime.UtcNow,
                ValueToPay = TotalValue,
                User = model.User
            };

            _context.Bills.Add(billfromModel);

            await _context.SaveChangesAsync();
            //check if the bill has been produced

            //var readingUpdate = await _context.Readings.FirstOrDefaultAsync(x => x.Id == model.Id);
            //var readingBill = new Reading { Id = model.Id };
            //readingBill.BillIssued = true;
            //_context.Entry(readingBill).Property("BillIssued").IsModified = true;

            //await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        private bool BillExists(int id)
        {
            return _context.Bills.Any(r => r.Reading.Id == id);

        }
        public async Task<IActionResult> ShowBill(string id)
        {
            id = id.Replace(id.Substring(id.IndexOf(";")), "");

            var idDesencrypted = EncryptationFunctions.DecryptString(id);

            var model = await _context.Bills
                .Include(u => u.User)
                .Include(r => r.Reader)
                .Include(x => x.Reading)
                .Where(u => u.Id == Convert.ToInt32(idDesencrypted))
                .FirstOrDefaultAsync();

            return View(model);
        }
    }
}
