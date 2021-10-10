using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
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

        [Authorize(Roles = "Admin")]
        // GET: Readers/Create
        public async Task<IActionResult> Create(string id)
        {
            var user = await _userHelper.GetByIdAsync(id);
      
            var model = new AddReaderViewModel
            {
                Users = _userHelper.GetComboUsers(user.UserName)
            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
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

        // GET: Readers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _readerRepository.GetByIdAsync(id.Value);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Readers/Edit/5
        [Authorize(Roles = "Worker, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _readerRepository.GetByIdAsync(id.Value);
            if (equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }
        // POST: Readers/Edit/5
        [Authorize(Roles = "Worker, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reader reader)
        {
            if (id != reader.Id)
            {
                return NotFound();
            }
      
            try
            {
                reader.User = await _userHelper.GetByIdAsync(id.ToString());
                await _readerRepository.UpdateAsync(reader);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _readerRepository.ExistAsync(reader.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Readers/Delete/5
        [Authorize(Roles = "Worker, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reader = await _readerRepository.GetByIdAsync(id.Value);
            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }
        // POST: Readers/Delete/5
        [Authorize(Roles = "Worker, Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //if()
            //{
            //    ViewBag.Message = "It is not possible to delete this reader as there are readings associated";
            //    return RedirectToAction(nameof(Index));
            //}
            var reader = await _readerRepository.GetByIdAsync(id);
            try
            {
                //throw new Exception("Excepção de testes");
                await _readerRepository.DeleteAsync(reader);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{reader.ReaderName} might be used";
                    ViewBag.ErrorMessage = $"{reader.ReaderName} can´t be deleted because it has an associated reading to it!</br>" +
                                        "Try to delete first the readings and then come back to delete the reader!";
                }
                return View("Error");
            }
       
        }

    }
}
