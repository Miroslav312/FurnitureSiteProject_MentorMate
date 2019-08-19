using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureSiteProject_MentorMate.Models
{
    /// <summary>
    /// A base class for forgot password model
    /// </summary>
    public class ForgotPasswordModel
    {
        [Required]
        /// <summary>
        /// User e-mail address
        /// </summary>
        /// <returns>User e-mail address</returns>
        public string Email {get; set;}
    }
}