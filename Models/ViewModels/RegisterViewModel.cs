using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureSiteProject_MentorMate.Models
{
    public class RegisterViewModel
    {
        /// <summary>
        /// User identifier
        /// </summary>
        /// <returns>User identifier</returns>
        public string Id { get; set; }

        /// <summary>
        /// User email address
        /// </summary>
        /// <returns>User email address</returns>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        /// <returns>User password</returns>
        [Required]
        public string Password { get; set; }


        /// <summary>
        /// User password
        /// </summary>
        /// <returns>User password</returns>
        [Required]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Indicates whenever user login session should be saved or not
        /// </summary>
        /// <returns>True if user want to be remembered, false in ohter case</returns>
        public bool Remember { get; set; }
    }
}
