using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureSiteProject_MentorMate.Models
{
    /// <summary>
    /// A base class for change password model
    /// </summary>
    public class ChangePasswordModel
    {
        [Required]
        [Display(Name = "Current password")]
        /// <summary>
        /// User current password
        /// </summary>
        /// <returns>User current password</returns>
        public string CurrentPassword {get; set;}

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
    }
}