using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureSiteProject_MentorMate.Models
{
    /// <summary>
    /// A base class for user 
    /// model
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// User email address
        /// </summary>
        /// <returns>User email address</returns>
        [Required]
        public string Email {get; set;}

        /// <summary>
        /// User password
        /// </summary>
        /// <returns>User password</returns>
        [Required]
        public string Password {get; set;}

        /// <summary>
        /// Indicates whenever user login session should be saved or not
        /// </summary>
        /// <returns>True if user want to be remembered, false in ohter case</returns>
        public bool Remember {get; set;}
    }
}