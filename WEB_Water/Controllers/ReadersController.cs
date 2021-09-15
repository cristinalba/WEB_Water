using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Repositories;
using WEB_Water.Helpers;
using WEB_Water.Models;

namespace WEB_Water.Controllers
{
    public class ReadersController : Controller
    {
        private readonly IReaderRepository _readerRepository;
        private readonly IUserHelper _userHelper;

        public ReadersController(IReaderRepository readerRepository, IUserHelper userHelper)
        {
            _readerRepository = readerRepository;
            _userHelper = userHelper;
        }

        //GET: READERS
        public IActionResult Index()
        {
            var readers = _readerRepository.GetAll()
                 .Include(x => x.User)
                 .OrderBy(e => e.ReaderName);

            return View(readers);
        }

        //[Authorize(Roles = "Admin")]
        // GET: Equipments/Create
        public IActionResult Create()
        {
            var model = new AddReaderViewModel
            {
                Users = _userHelper.GetComboUsers()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddReaderViewModel model)
        {
            if (this.ModelState.IsValid)
            {

                await _readerRepository.AddReaderToListAsync(model, this.User.Identity.Name);

                ViewBag.Message = "New reader registered!";

                return RedirectToAction("Index");
            }

            return this.View(model);
        }


    }
}
