using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data;
using WEB_Water.Data.Repositories;
using WEB_Water.Helpers;
using WEB_Water.Models;

namespace WEB_Water.Controllers
{
    public class ReadingsController : Controller
    {
        
        private readonly IUserHelper _userHelper;
        private readonly IReadingRepository _readingRepository; 
        private readonly IReaderRepository _readerRepository;

        public ReadingsController(IUserHelper userHelper, IReaderRepository readerRepository, IReadingRepository readingRepository)
        {
           
            _userHelper = userHelper;
            _readerRepository = readerRepository;
            _readingRepository = readingRepository;

        }
        public async Task<IActionResult> Index() //List of Readings from the customer (NameCustomer/ReaderName/RegistrationDateOfNewReading/Period/m3)
        {
            var reading = await _readingRepository.GetReadingAsync(this.User.Identity.Name);
            return View(reading);
        }

        //[Authorize(Roles = "Admin")] //Admin can see all customers(List) and can register(create) new readings of a customer in particular (+) 
        public IActionResult ListOfAllTheCustomers()
        {
            //TODO:FullName??
            return View(_userHelper.GetAll().OrderBy(x => x.UserName));
        }

        
        // GET: Readings/Create
        public async Task<IActionResult> Create(string id)
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                if (id == null)
                {
                    return RedirectToAction("ListOfAllTheCustomers");
                }

                user = await _userHelper.GetUserByIdAsync(id);
                var model = new AddReadingViewModel
                {
                    Users = _userHelper.GetComboUsers(user.UserName),
                    Readers = _readerRepository.GetComboReaders(user.UserName)
                    
                };
                return View(model);
            }
            else
            {
                var model = new AddReadingViewModel
                {
                    Readers = _readerRepository.GetComboReaders(this.User.Identity.Name)

                };
                return View(model);
            }
        }

        //[Authorize(Roles = "Admin, Worker, Customer")]
        [HttpPost]
        public async Task<IActionResult> Create(AddReadingViewModel model)
        {
            if (model.UserId == "0")
            {
                this.ModelState.AddModelError(string.Empty, "You must select a customer");
                ViewBag.Message = string.Format("You must select a customer");
                return this.RedirectToAction("Create");
            }
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                user = await _userHelper.GetUserByIdAsync(model.UserId.ToString());
                await _readingRepository.AddReadingToCustomerAsync(model, user.UserName);
            }
            else
            {
                await _readingRepository.AddReadingToCustomerAsync(model, this.User.Identity.Name);
            }
            return this.RedirectToAction("Index");

        }

    }
}
