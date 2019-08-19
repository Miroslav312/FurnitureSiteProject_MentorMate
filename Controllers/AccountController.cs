using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using FurnitureSiteProject_MentorMate.Models;
using FurnitureSiteProject_MentorMate.Infrastructure;
using FurnitureSiteProject_MentorMate.Services;
using System;

namespace FurnitureSiteProject_MentorMate.Controllers
{
    /// <summary>User account management</summary>
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>Manage user data</summary>
        private readonly IUserManagement _userManager;


        /// <summary>Provides e-mail sending service</summary>
        private readonly IMessageService _messageService;


        /// <summary>Default constructor</summary>
        /// <param name="userManagement">Managing user data</param>
        /// <param name="messageService">Provides e-mail sending service</param>
        public AccountController(IUserManagement userManagement,
            IMessageService messageService)
        {
            _userManager = userManagement;
            _messageService = messageService;
        }

        /// <summary>Registers this instance.</summary>
        /// <returns>IActionResult.</returns>
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>Registers the specified model.</summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Password.Equals(model.ConfirmPassword))
                {
                    ApplicationUser applicationUser = new ApplicationUser()
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var result = _userManager.CreateAsync(applicationUser, model.Password).Result;

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(applicationUser, "Costumer");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something dosen´t work.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Passwords dosen´t are the same.");
                }
                return Redirect("~/");
            }

            return View(model);
        }

        /// <summary>Displays login form</summary>
        /// <returns>View with user login form</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // clear existing cookie to make sure we have clean login
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        /// <summary>Autheticate user</summary>
        /// <param name="model">User login view model</param>
        /// <returns>Returns Furniture home view in case of success, login view in case of invalid model state or redirects to password change form in case when default password was not changed yet</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                OperationResultStatus login = await _userManager.Login(model);

                if(login.Result != OperationResult.Success)
                {
                    ModelState.AddModelError(string.Empty, login.Message);
                    return View();
                }
                else
                {
                    return Redirect("~/");
                }
            }

            return View();
        }

        /// <summary>Logout user</summary>
        /// <returns>Redirects to home controller</returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userManager.Logout();

            return Redirect("~/");
        }

        /// <summary>Displays form with password change form</summary>
        /// <returns>View with change password form</returns>
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>Dsuplays view with user profile edit form</summary>
        /// <returns>Returns view with user profile edit form</returns>
        [Authorize]
        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.FindUserByPrincipal(HttpContext.User);

            UserProfileModel userProfile = new UserProfileModel(){

                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(userProfile);
        }

        /// <summary>Updates user profile</summary>
        /// <param name="model">User profile model</param>
        /// <returns>Redirects to the account operation summary view on sucess, AccountView with model validation result on failure</returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserProfileModel model)
        {
            string accountOperationSummaryMessage = string.Empty; 

            if(model == null)
            {
                return NotFound();
            }

            OperationResultStatus updateProfile =  await _userManager.UpdateUserProfile(model);

            if(updateProfile.Result != OperationResult.Success)
            {
                ModelState.AddModelError(string.Empty, updateProfile.Message);

                return View();
            }
            else
            {
                accountOperationSummaryMessage = updateProfile.Message;
            }

            return View("AccountOperationSummary", (object)accountOperationSummaryMessage);
        }


        /// <summary>Change user password</summary>
        /// <param name="model">Change password model</param>
        /// <returns>View with password change summary or change password view in case of invalid model state</returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {           
            if(ModelState.IsValid)
            {
                // summary message to be send to the summary view
                string accountOperationSummaryMessage = string.Empty;

                OperationResultStatus changePassword = await _userManager.ChangePassword(model, HttpContext);

                if(changePassword.Result != OperationResult.Success)
                {
                    ModelState.AddModelError(string.Empty, changePassword.Message);

                    return View();
                }
                else
                {
                    accountOperationSummaryMessage = changePassword.Message;
                }

                return View("AccountOperationSummary", (object)accountOperationSummaryMessage);
            }
            else
            {
                return View();
            }
        }

        /// <summary>Displays password reset form</summary>
        /// <param name="token">Password reset token</param>
        /// <returns>View with password reset form and password reset token hidden input value</returns>
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token = null)
        {
            if (token == null)
            {
                throw new ApplicationException("A token must be supplied for password reset.");
            }

            var model = new ResetPasswordModel {Token = token};

            return View(model);
        }


        /// <summary>Resets user password</summary>
        /// <param name="model">Reset password model</param>
        /// <returns>Redirects to password summary on success or password reset view with validation summary in case of invalid model state</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {           
            if(ModelState.IsValid)
            {
                string accountOperationSummaryMessage = string.Empty;

                OperationResultStatus resetPassword = await _userManager.ResetPassword(model);

                if(resetPassword.Result != OperationResult.Success)
                {
                    ModelState.AddModelError(string.Empty, resetPassword.Message);
                    return View();
                }
                else
                {
                    accountOperationSummaryMessage = "Password has been changed successfully.";

                    return View("AccountOperationSummary", (object)accountOperationSummaryMessage);
                }
            }
            else
            {
                return View();
            }
        }

        /// <summary>Displays view with forgot password view</summary>
        /// <returns>Returns view with forgot password form</returns>
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        /// <summary>Sends e-mail to user with password reset link</summary>
        /// <param name="model">Forgot password model</param>
        /// <returns>Redirects to password summary on success or forgot password view with validation summary in case of invalid model state</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            string accountOperationSummaryMessage = string.Empty;

            if(ModelState.IsValid)
            {
                OperationResultStatus forgotPassword = await _userManager.ForgotPassword(model, Url, Request);

                accountOperationSummaryMessage = forgotPassword.Message;
                
                return View("AccountOperationSummary", (object)accountOperationSummaryMessage);
            }
            else
            {
                return View();
            }
        }

        /// <summary>Metod creates view with password operations summary</summary>
        /// <param name="accountOperationSummaryMessage"></param>
        /// <returns>Returns view with password operations summary</returns>
        [AllowAnonymous]
        public IActionResult PasswordSummary(string accountOperationSummaryMessage)
        {
            return View();
        }

    }
}