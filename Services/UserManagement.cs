using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FurnitureSiteProject_MentorMate.Models;
using Microsoft.AspNetCore.Http;
using FurnitureSiteProject_MentorMate.Services;
using Microsoft.Extensions.Options;
using System.IO;

namespace FurnitureSiteProject_MentorMate.Infrastructure
{
    /// <summary>
    /// Interface defines methods used to manage user in applicaiton
    /// </summary>
    public class UserManagement : IUserManagement
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ISignInManagement _signInManager;

        private readonly IMessageService _messageService;

    

        public UserManagement(UserManager<ApplicationUser> userManager, ISignInManagement signInManager, IMessageService messageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageService = messageService;
           
        }


        /// <summary>
        /// Method authorize user in application
        /// </summary>
        /// <param name="model">Login view model</param>
        /// <returns>Operation result status and message</returns>
        public async Task<OperationResultStatus> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                return new OperationResultStatus() {Result = OperationResult.InvalidUserName, Message = "Invalid user name."};
            }

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: model.Remember, lockoutOnFailure: false);

            if(!passwordSignInResult.Succeeded)
            {
                return new OperationResultStatus() {Result = OperationResult.InvalidUserNamePassword, Message = "Invalid user name or password."};
            }

            return new OperationResultStatus() {Result = OperationResult.Success, Message = "Login success."};   
        }


        /// <summary>
        /// Method verifies whenever the default password for default user has been changed or not
        /// </summary>
        /// <param name="email">User e-mail address</param>
        /// <returns>True when password has been already changed, false in other case</returns>
        public async Task<bool> IsDefaultPasswordChanged(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if(user.DefaultPasswordChanged != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method logout user from the application
        /// </summary>
        /// <returns></returns>
        public async Task<Task> Logout()
        {
            return await _signInManager.SignOutAsync();
        }


        /// <summary>
        /// Method changes user password
        /// </summary>
        /// <param name="model">Change password user model</param>
        /// <param name="httpContext">User principal</param>
        /// <returns>Operation result status and message</returns>
        public async Task<OperationResultStatus> ChangePassword(ChangePasswordModel model, HttpContext httpContext)
        {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(httpContext.User));

            if(user == null)
            {
                return new OperationResultStatus() {Result = OperationResult.InvalidUserId, Message = "Invalid user id."};
            }

            if (model.NewPassword != model.RePassword)
            {
                return new OperationResultStatus() {Result = OperationResult.PasswordsDontMatch, Message = "Passwords do not match."};
            }

            var changePasswordOperation = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!changePasswordOperation.Succeeded)
            {
                return new OperationResultStatus() {Result = OperationResult.Failure, Message = "Change password failure."};             
            }
            else
            {
                user.DefaultPasswordChanged = 1;
                await _userManager.UpdateAsync(user);

                return new OperationResultStatus() {Result = OperationResult.Success, Message = "Password has been changed successfully."};
            }
        }

        /// <summary>
        /// Method resets user password
        /// </summary>
        /// <param name="model">Reset password model</param>
        /// <returns>Operation result status and message</returns>
        public async Task<OperationResultStatus> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return new OperationResultStatus() {Result = OperationResult.InvalidUserId, Message = "Invalid user id."};
            }

            if (model.NewPassword != model.RePassword)
            {
                return new OperationResultStatus() {Result = OperationResult.PasswordsDontMatch, Message = "Passwords do not match"};
            }

            model.Token = model.Token.Replace(" ", "+");

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!resetPasswordResult.Succeeded)
            {
                return new OperationResultStatus() {Result = OperationResult.Failure, Message = "Password reset failure."};        
            }
            else
            {
                user.DefaultPasswordChanged = 1;
                await _userManager.UpdateAsync(user);
            }


            return new OperationResultStatus() {Result = OperationResult.Success, Message = "Password reset success."};
        }


        /// <summary>
        /// Method sends password reset link to the user
        /// </summary>
        /// <param name="model">Forgot password model</param>
        /// <param name="url">Application URL address</param>
        /// <param name="request">Request type</param>
        /// <returns>Operation result status and message</returns>
        public async Task<OperationResultStatus> ForgotPassword(ForgotPasswordModel model, IUrlHelper url, HttpRequest request)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new OperationResultStatus() {Result = OperationResult.InvalidUserId, Message = "Invalid user id."};
            }

            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                return new OperationResultStatus() {Result = OperationResult.EmailNotConfirmed, Message = "Email addres was not yet confirmed."};
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResetUrl = url.Action("ResetPassword", "Account", new {id = user.Id, token = passwordResetToken}, request.Scheme);

            await _messageService.Send(model.Email, "Password reset request", $"Click <a href=\"" + passwordResetUrl + "\">here</a> to reset your password");

            return new OperationResultStatus() {Result = OperationResult.Success, Message = "Password reset link has been sent to your e-mail address : " + model.Email};
        }


        /// <summary>
        /// Method updates user data in database
        /// </summary>
        /// <param name="model">Application user model</param>
        /// <returns>Operation result status and message</returns>

        public async Task<OperationResultStatus> UpdateUserProfile(UserProfileModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;

            string json = File.ReadAllText("customSettings.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            jsonObj["DefaultUserData"]["Firstname"] = model.FirstName;
            jsonObj["DefaultUserData"]["Lastname"] = model.LastName;
            jsonObj["DefaultUserData"]["Email"] = model.Email;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText("customSettings.json", output);

            await _userManager.UpdateAsync(user);

            return new OperationResultStatus() {Result = OperationResult.Success, Message = "Profile has been updated successfully."};
    
        }

        /// <summary>
        /// Method finds method by user e-mail address
        /// </summary>
        /// <param name="email">User e-mail address</param>
        /// <returns>Application user model</returns>
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Method creates new user
        /// </summary>
        /// <param name="user">Application user model</param>
        /// <param name="password">User password</param>
        /// <returns>Identity operations result</returns>
        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Method assing user to role
        /// </summary>
        /// <param name="user">Application user model</param>
        /// <param name="role">User role in application</param>
        /// <returns>Identity operations result</returns>
        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        /// <summary>
        /// Method finds user by principal
        /// </summary>
        /// <param name="principal">User principal</param>
        /// <returns>Application user model</returns>
        public async Task<ApplicationUser> FindUserByPrincipal(ClaimsPrincipal principal)
        {
            return await _userManager.FindByIdAsync(_userManager.GetUserId(principal));
        }
    }
}