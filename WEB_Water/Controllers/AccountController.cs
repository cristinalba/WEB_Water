using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;
using WEB_Water.Models;


namespace WEB_Water.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper) //search for the methods in the UserHelper
        {
            _userHelper = userHelper;
        }


        //  Account/Index
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_userHelper.GetAll().OrderBy(x => x.UserName));
        }


        public IActionResult Login() //right button, add razor view(Login, without model, use layout)
        {
            if (User.Identity.IsAuthenticated) //if the user is autherticated, shows Home!
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View(); //If there is not log in, it remains in the same view
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }
                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login!");
            return View(model);
        }
        public IActionResult Register() //Add View+Razor view
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username); //check if the user exists

                if (user == null)
                {
                    user = new User //create user if it doesn't exist
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                    };
                    //METER ROLE AL USER 

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    var loginViewModel = new LoginViewModel //if it creates a new one user, shows Loginviewmodel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.Username
                    };

                    var result2 = await _userHelper.LoginAsync(loginViewModel);
                    if (result2.Succeeded)//if it can log in, shows home
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "The user couldn't be logged.");
                }
                
            }
            return View(model);
        }

        public async Task<IActionResult> ChangeUser() //ADD View
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;            
            }        
            return View(model);
        }

        [HttpPost]//Update data when click over the username
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                if (user != null)
                {

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;                  
                    
                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }
            return View(model);
        }
        public IActionResult ChangePassword() //ADD View
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return this.View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");

        }

        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
