using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FurnitureSiteProject_MentorMate.Models;
using Microsoft.AspNetCore.Http;

namespace FurnitureSiteProject_MentorMate.Infrastructure
{
    /// <summary>
    /// Interface defines methods used to manage user in applicaiton
    /// </summary>
    public interface IUserManagement
    {
        /// <summary>
        /// Method authorize user in application
        /// </summary>
        /// <param name="model">Login view model</param>
        /// <returns>Operation result status and message</returns>
        Task<OperationResultStatus> Login(LoginViewModel model);

        /// <summary>
        /// Method verifies whenever the default password for default user has been changed or not
        /// </summary>
        /// <param name="email">User e-mail address</param>
        /// <returns>True when password has been already changed, false in other case</returns>
        Task<bool> IsDefaultPasswordChanged(string email);

        /// <summary>
        /// Method logout user from the application
        /// </summary>
        /// <returns></returns>
        Task<Task> Logout();

        /// <summary>
        /// Method changes user password
        /// </summary>
        /// <param name="model">Change password user model</param>
        /// <param name="httpContext">User principal</param>
        /// <returns>Operation result status and message</returns>
        Task<OperationResultStatus> ChangePassword(ChangePasswordModel model, HttpContext httpContext);

        /// <summary>
        /// Method resets user password
        /// </summary>
        /// <param name="model">Reset password model</param>
        /// <returns>Operation result status and message</returns>
        Task<OperationResultStatus> ResetPassword(ResetPasswordModel model);

        /// <summary>
        /// Method sends password reset link to the user
        /// </summary>
        /// <param name="model">Forgot password model</param>
        /// <param name="url">Application URL address</param>
        /// <param name="request">Request type</param>
        /// <returns>Operation result status and message</returns>
        Task<OperationResultStatus> ForgotPassword(ForgotPasswordModel model, IUrlHelper url, HttpRequest request);

        /// <summary>
        /// Method finds method by user e-mail address
        /// </summary>
        /// <param name="email">User e-mail address</param>
        /// <returns>Application user model</returns>
        Task<ApplicationUser> FindByEmailAsync(string email);

        /// <summary>
        /// Method creates new user
        /// </summary>
        /// <param name="user">Application user model</param>
        /// <param name="password">User password</param>
        /// <returns>Identity operations result</returns>
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        /// <summary>
        /// Method assing user to role
        /// </summary>
        /// <param name="user">Application user model</param>
        /// <param name="role">User role in application</param>
        /// <returns>Identity operations result</returns>
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);

        /// <summary>
        /// Method finds user by principal
        /// </summary>
        /// <param name="principal">User principal</param>
        /// <returns>Application user model</returns>
        Task<ApplicationUser> FindUserByPrincipal(ClaimsPrincipal principal);

        /// <summary>
        /// Method updates user data in database
        /// </summary>
        /// <param name="model">Application user model</param>
        /// <returns>Operation result status and message</returns>
        Task<OperationResultStatus> UpdateUserProfile(UserProfileModel model);

    }
}
