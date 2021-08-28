using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEB_Water.Data;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;

namespace WEB_Water.Controllers
{
    public class CustomersController : Controller
    {
        //private readonly DataContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        //private readonly IConverterHelper _converterHelper;

        public CustomersController(ICustomerRepository customerRepository,
                                   IUserHelper userHelper,
                                   IImageHelper imageHelper)
        {
            _customerRepository = customerRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
        }

        // GET: Customers
        public IActionResult Index()
        {
            return View(_customerRepository.GetAll().OrderBy(e => e.FirstName));//Show Customers order by FirstName
            //return View(await _context.Customers.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _customerRepository.GetByIdAsync(id.Value);
            //var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,NIF_customer,Telefone,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                //TODO: change to the user who is logged in
                customer.User = await _userHelper.GetUserByEmailAsync("cristinajular@gmail.com");
                await _customerRepository.CreateAsync(customer);
                //_context.Add(customer);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _customerRepository.GetByIdAsync(id.Value);
            //var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,NIF_customer,Telefone,Email")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _customerRepository.UpdateAsync(customer);
                    //_context.Update(customer);
                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!CustomerExists(customer.Id))
                    if (!await _customerRepository.ExistAsync(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _customerRepository.GetByIdAsync(id.Value);
            //var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            await _customerRepository.DeleteAsync(customer);
            //var customer = await _context.Customers.FindAsync(id);
            //_context.Customers.Remove(customer);
            //await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool CustomerExists(int id)
        //{
        //    return _context.Customers.Any(e => e.Id == id);
        //}
    }
}
