using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FurnitureSiteProject_MentorMate.Models;

namespace FurnitureSiteProject_MentorMate.Infrastructure
{
    /// <summary>
    /// Interface defines methods used to handle user authorization operations
    /// </summary>
    public interface ISignInManagement
    {
        /// <summary>
        /// Method used to authenticate user in applicaiton
        /// </summary>
        /// <param name="user">Application user model</param>
        /// <param name="password">User password</param>
        /// <param name="isPersistent">Remember user authentication</param>
        /// <param name="lockoutOnFailure">Block user account on login failure</param>
        /// <returns>Authorization operation result</returns>
        Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);

        Task<SignInResult> PasswordSignInAsync(string Email, string password, bool isPersistent, bool lockoutOnFailure);

        /// <summary>
        /// Method used to sign out user from the application
        /// </summary>
        /// <returns></returns>
        Task<Task> SignOutAsync();
    }
}