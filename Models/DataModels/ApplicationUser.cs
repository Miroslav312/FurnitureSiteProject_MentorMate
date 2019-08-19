using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FurnitureSiteProject_MentorMate.Models
{
    /// <summary>
    /// A base class for user data for Identity Framework. This and base class is mapped to database by Entity Framework
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets user first name
        /// </summary>
        /// <returns>User first name</returns>
        public string FirstName {get; set;}

        /// <summary>
        /// Gets or sets user last name
        /// </summary>
        /// <returns>User last name</returns>
        public string LastName {get; set;}

        /// <summary>
        /// Gets or sets the status of default password change
        /// </summary>
        /// <returns>1 when default password has been already changed, 0 in other case</returns>
        public int DefaultPasswordChanged {get; set;}
        /// <summary>
        /// Gets or sets the name of the image file with Furniture author photo
        /// </summary>
        /// <returns>Name of the image file with Furniture author photo</returns>
        public string ProfilePhoto {get; set;}
    }
}