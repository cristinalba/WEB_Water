using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Models;

namespace WEB_Water.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);

        Task<string> GenerateEmailConfirmationTokenAsync(User user);

        Task<IdentityResult> ConfirmEmailAsync(User user, string token);


        Task<User> GetUserByIdAsync(string id);

        Task<User> GetByIdAsync(string id);




        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        IEnumerable<SelectListItem> GetComboUsers();//readerscontroller
        IEnumerable<SelectListItem> GetComboUsers(string email);//readingscontroller

        IQueryable<User> GetAll();

    }
}
