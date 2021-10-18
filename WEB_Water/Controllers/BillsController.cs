using Microsoft.AspNetCore.Authorization;
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
        
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IBillRepository _billRepository;
        private readonly IReadingRepository _readingRepository;

        public BillsController(IUserHelper userHelper,
                               IMailHelper mailHelper,
                               IBillRepository billRepository, 
                               IReadingRepository readingRepository)
        {
         
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _billRepository = billRepository;
            _readingRepository = readingRepository;
        }

        [Authorize(Roles = "Worker, Customer")]
        public async Task<IActionResult> Index() //Show all the customers with a button to go to the bills
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            if (await _userHelper.IsUserInRoleAsync(user, "Worker"))//Worker
            {
                return View(_userHelper.GetAll().Where(x => x.IsCustomer==true).OrderBy(u => u.UserName));//fullname
            }

            var customers = _userHelper.GetAll()
            .Where(u => u.UserName == user.UserName)
            .OrderBy(u => u.FullName);//Username

            return View(customers);
        }

        [Authorize(Roles = "Worker, Customer")]
        public async Task<IActionResult> BillSummary(string id) //All the bills of a customer
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            if (await _userHelper.IsUserInRoleAsync(user, "Worker"))//Worker
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

        [Authorize(Roles = "Worker")]
        public async Task<IActionResult> CalculateBill(int? id)//Receives the id of the selected Reading to calculate the value to pay and show it
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_billRepository.BillExists(id.Value) == true)
            {
                return RedirectToAction("Index");
            }

            //select the reading
            
            var model = await _readingRepository.GetReadingForBillByIdAsync(id.Value);


            if (model == null)
            {
                return NotFound();
            }

            double TotalValue = 0;

            for (int i = 1; i <= model.ValueOfConsumption; i++)
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

            //creates a bill with the reading selected
            var billfromModel = new Bill
            {
                Reader = model.Reader,
                Reading = model,
                BillDate = DateTime.UtcNow,
                ValueToPay = Math.Round(TotalValue,2),
                User = model.User,
            };
       
            _billRepository.AddBill(billfromModel);

            //check if the bill has been produced
            var readingUpdate = await _readingRepository.GetReadingByIdAsync(model.Id);
           

            readingUpdate.BillIssued = true;

            _billRepository.UpdateStatusBill(readingUpdate);


            await _billRepository.SaveBillAsync();

            //Send alert to customer of new Bill

            Response response = _mailHelper.SendEmail(model.User.UserName, "New invoice from Tajo Water Company", $"<h3>You can check your last consumption</h3>" +
                   $"Thank you for choosing us");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowBill(string id)
        {
            id = id.Replace(id.Substring(id.IndexOf(";")), "");

            string idDesencrypted = EncryptationFunctions.DecryptString(id);

            var model = await _billRepository.GetBillByIdAsync(idDesencrypted);

            return View(model);
        }


    }
}
