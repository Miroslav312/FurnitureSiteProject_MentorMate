using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FurnitureSiteProject_MentorMate.Models;

namespace FurnitureSiteProject_MentorMate.Infrastructure
{
    /// <summary>
    /// Class defines methods used to handle user authorization operations
    /// </summary>
    public class SignInManagement : ISignInManagement
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInManagement(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Method used to authenticate user in applicaiton
        /// </summary>
        /// <param name="user">Application user model</param>
        /// <param name="password">User password</param>
        /// <param name="isPersistent">Remember user authentication</param>
        /// <param name="lockoutOnFailure">Block user account on login failure</param>
        /// <returns>Authorization operation result</returns>
        public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);
        }


        /// <summary>
        /// Method used to sign out user from the application
        /// </summary>
        /// <returns></returns>
        public async Task<Task> SignOutAsync()
        {
            await _signInManager.SignOutAsync();

            return Task.CompletedTask;
        }
    }
}