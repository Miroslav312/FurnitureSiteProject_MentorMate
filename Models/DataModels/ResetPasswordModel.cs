using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureSiteProject_MentorMate.Models
{
    /// <summary>
    /// A base class for restet password model
    /// </summary>
    public class ResetPasswordModel
    {
        /// <summary>
        /// User identifier
        /// </summary>
        /// <returns>User identifier</returns>
        public string Id {get; set;}

        [Required]
        [Display(Name = "New password")]
        /// <summary>
        /// User new password
        /// </summary>
        /// <returns>User new password</returns>
        public string NewPassword {get; set;}

        [Required]
        [Display(Name = "Retype password")]
        /// <summary>
        /// User re-typed password
        /// </summary>
        /// <returns>User re-typed password</returns>
        public string RePassword {get; set;}

        /// <summary>
        /// Password reset token to be send by e-mail to user
        /// </summary>
        /// <returns>Password reset token to be send by e-mail to user</returns>
        public string Token {get; set;}
    }
}