using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WEB_Water.Data.Entities;
using WEB_Water.Helpers;
using WEB_Water.Models;


namespace WEB_Water.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(IUserHelper userHelper,
                                 IMailHelper mailHelper,
                                 UserManager<User> userManager,
                                 IConfiguration configuration) //search for the methods in the UserHelper
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _userManager = userManager;
            _configuration = configuration;
        }


        //Account/Index
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_userHelper.GetAll().Where(x => x.IsCustomer==true).OrderBy(x => x.UserName));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult IndexOthers()
        {
            return View(_userHelper.GetAll().Where(x => x.IsCustomer == false).OrderBy(x => x.UserName));
        }

        //Create
        public IActionResult Register() //Add View+Razor view
        {
            return View();
        }

        //Create
        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.UserName); //check if the user exists
                
                if (user == null)
                {
                    user = new User //create user if it doesn't exist
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.UserName,
                        UserName = model.UserName,
                        PhoneNumber = model.PhoneNumber,
                        Nif = model.Nif,
                        IsCustomer = model.IsCustomer

                    };
                    //METER ROLE AL USER 

                    var result = await _userHelper.AddUserAsync(user, model.Password);

                    if (model.IsCustomer == true)
                        await _userHelper.AddUserToRoleAsync(user, "Customer");
                    if (model.IsCustomer == false)
                        await _userHelper.AddUserToRoleAsync(user, "Worker");


                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken
                    }, protocol: HttpContext.Request.Scheme);

                    Response response = _mailHelper.SendEmail(model.UserName, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    { 
                        ViewBag.Message = "The instructions to allow the user to activate the account have been sent to the email account";
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "The user couldn't be logged.");

                    //if (model.IsCustomer == true)
                    //    return RedirectToAction("Index", "Account");
                    //if (model.IsCustomer == false)
                    //    return RedirectToAction("IndexOthers", "Account");
                }

            }
            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProfileUser(string id)
        {

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        //Edit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id) //ADD View
        {

            var user = await _userHelper.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            //var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new RegisterNewUserViewModel();
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.UserName = user.UserName;
                model.PhoneNumber = user.PhoneNumber;
                model.Nif = user.Nif;
                model.IsCustomer = user.IsCustomer;
            }
            return View(model);
        }
        //Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]//Update data when click over the username
        public async Task<IActionResult> Edit(RegisterNewUserViewModel model)
        {
            //if (ModelState.IsValid)
            //{
                var user = await _userHelper.GetUserByEmailAsync(model.UserName);

                //if (user != null)
                //{

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.UserName = model.UserName;
                    user.Nif = model.Nif;
                    user.IsCustomer = model.IsCustomer;

                    if (user.IsCustomer == true)
                        await _userHelper.AddUserToRoleAsync(user, "Customer");
                    if (user.IsCustomer == false)
                        await _userHelper.AddUserToRoleAsync(user, "Worker");
           

                    var response = await _userHelper.UpdateUserAsync(user);

                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
            //}
            //}
          
            return View(model);
        }

        // GET: Account/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Account/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction(nameof(Index));
        }

        /////////////////////////////////////////////////////////////////////////////
        ///
        /// Functions to do the Log In / Register / Change Password 
        /// 
        /////////////////////////////////////////////////////////////////////////////
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
            var user = await _userHelper.GetUserByEmailAsync(model.Username);

            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if(user.EmailConfirmed==true && user.FirstTimePass==false)
                    {
                        user.FirstTimePass = true;
                        await _userHelper.UpdateUserAsync(user);
                        return this.RedirectToAction("ChangePassword", "Account");
                      
                    }
                    else
                    {
                        if (this.Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(this.Request.Query["ReturnUrl"].First());
                        }
                        return this.RedirectToAction("Index", "Home");
                    }
           
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login!");
            return View(model);
        }

        //Edit
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
        //Edit
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
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
              
            }

            return View();

        }
        //public async Task<IActionResult> ChangePassword(string userId, string token)
        //{
        //    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        //    {
        //        return NotFound();
        //    }

        //    var user = await _userHelper.GetUserByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var result = await _userHelper.ConfirmEmailAsync(user, token);
        //    if (!result.Succeeded)
        //    {

        //    }

        //    return View();

        //}


        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
