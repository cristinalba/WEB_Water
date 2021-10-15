using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(IUserHelper userHelper,
                                 IMailHelper mailHelper,
                                 IImageHelper imageHelper,
                                 IConverterHelper converterHelper,
                                 UserManager<User> userManager,
                                 IConfiguration configuration) //search for the methods in the UserHelper
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
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
        [Authorize(Roles = "Admin")]
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

                    Response response = _mailHelper.SendEmail(model.UserName, "Email confirmation", $"<h1>Email Confirmation:</h1>" +
                    $"To access your new account, " +
                    $"please click in this link: <br/><br/><a href = \"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    { 
                        ViewBag.Message = "User registered! Add a reader to the user now";
                        return View(model);
                        //return this.RedirectToAction("Create", "Readers");
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

            var user = await _userHelper.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToChangeUserViewModel(user);
            return View(model);
        }
        //Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]//Update data when click over the username
        public async Task<IActionResult> Edit(EditUserWithImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.UserName);
                try
                {
                    if (user != null)
                    {
                        var path = string.Empty;

                        if (model.ImageFile != null && model.ImageFile.Length > 0)
                        {
                            path = await _imageHelper.UploadImageAsync(model.ImageFile, "users");
                        }
                        //before saving in the database

                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.PhoneNumber;
                        user.UserName = model.UserName;
                        user.Nif = model.Nif;
                        user.IsCustomer = model.IsCustomer;
                        user.ImageUrl = path;

                        if (user.IsCustomer == true)
                            await _userHelper.AddUserToRoleAsync(user, "Customer");
                        if (user.IsCustomer == false)
                            await _userHelper.AddUserToRoleAsync(user, "Worker");


                        var response = await _userHelper.UpdateUserAsync(user);

                        if (response.Succeeded)
                        {
                            ViewBag.Message = "User updated!";
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("EDIT"))
                    {
                        ViewBag.ErrorTitle = $"{user.FullName} might be used";
                        ViewBag.ErrorMessage = $"{user.FullName} can´t be deleted because it has information associated!</br>" +
                                            "Try to delete first that info and then come back to edit the user!";
                    }
                    return View("Error");
                }

            }

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

            var user = await _userHelper.GetByIdAsync(id.ToString());
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

            try
            {
                //throw new Exception("Excepção de testes");
                await _userManager.DeleteAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{user.FullName} might be used";
                    ViewBag.ErrorMessage = $"{user.FullName} can´t be deleted because it has a reader associated!</br>" +
                                        "Try to delete first the readers and then come back to delete the user!";
                }
                return View("Error");
            }

        }

        /////////////////////////////////////////////////////////////////////////////
        ///
        /// Functions to do the Log In / Register / Change Password /Logout
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
                    //if(user.EmailConfirmed==true && user.FirstTimePass==false)
                    //{
                    //    user.FirstTimePass = true;
                    //    await _userHelper.UpdateUserAsync(user);
                    //    //return this.RedirectToAction("ChangePassword", "Account");
                    //    ViewBag.Message = "Go to your email to follow instructions";

                    //}
                    //else
                    //{
                        if (this.Request.Query.Keys.Contains("ReturnUrl"))
                        {
                            return Redirect(this.Request.Query["ReturnUrl"].First());
                        }
                        return this.RedirectToAction("Index", "Home");
                    //}
           
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
                        ViewBag.Message = "User updated!";
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

        /////////////////////////////////////////////////////////////////////////////
        ///
        /// Functions with Token: Confirm/Recover/Reset
        /// 
        /////////////////////////////////////////////////////////////////////////////
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

            var user = await _userHelper.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
              
            }


            var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

            var link = this.Url.Action(
                "ResetPassword",
                "Account",
                new { token = myToken }, protocol: HttpContext.Request.Scheme);

            Response response = _mailHelper.SendEmail(user.UserName, "Web_Water Password Reset", $"<h1>Password Reset</h1>" +
            $"To reset the password click in this link:</br></br>" +
            $"<a href = \"{link}\">Reset Password</a>");

            if (response.IsSuccess)
            {
                ViewBag.Message = "The instructions to allow the user to activate the account have been sent to the email account";
                return View();
            }


            return View();

        }

        public IActionResult RecoverPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Shop Password Reset", $"<h1>Shop Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password have been sent to email.";
                }

                return this.View();

            }

            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }
        /////////////////////////////////////////////////////////////////////////////
        ///
        /// Functions to Control the price of Consumption
        /// 
        /////////////////////////////////////////////////////////////////////////////

        //Account/CheckPrices
        [Authorize(Roles = "Admin")]
        public IActionResult PriceControlCheck()
        {
            return View();
        }

        //Account/UpdatePrices
        [Authorize(Roles = "Admin")]
        public IActionResult PriceControlUpdate()
        {
            return View();
        }

        /////////////////////////////////////////////////////////////////////////////
        ///
        /// Functions to Control the price of Consumption
        /// 
        /////////////////////////////////////////////////////////////////////////////


        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
