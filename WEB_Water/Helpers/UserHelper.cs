using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data;
using WEB_Water.Data.Entities;
using WEB_Water.Models;

namespace WEB_Water.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;

        public UserHelper(UserManager<User> userManager,
                          SignInManager<User> signInManager,
                          RoleManager<IdentityRole> roleManager,
                          DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<SignInResult> LoginAsync(LoginViewModel model)//receives the view model 
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
            //false= to not lock out on failure
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }   

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
        public async Task<IdentityResult> ChangePasswordAsync(User user,
                                                              string oldPassword,
                                                              string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)//If the role doesn't exist, we create it
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public IEnumerable<SelectListItem> GetComboUsers()
        {
            var list = _context.Users.Where(x => x.IsCustomer == true).Select(u => new SelectListItem
            {
                Text = u.UserName,
                Value = u.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Choose a customer...)",
                Value = "0"
            });

            return list;

        }
        public IEnumerable<SelectListItem> GetComboUsers(string email)
        {
            var list = _context.Users.Where(x => x.UserName==email).Select(u => new SelectListItem
            {
                Text = u.UserName,
                Value = u.Id.ToString(),
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Choose a customer...)",
                Value = "0"
            });

            return list;
        }
        public IQueryable<User> GetAll()
        {
            return _context.Set<User>();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.Set<User>().FindAsync(id);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
               
        }

      
    }
}
