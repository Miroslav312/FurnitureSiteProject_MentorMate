using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurnitureSiteProject_MentorMate.Models
{
    public class UserProfileModel
    {
        /// <summary>
        /// Gets or sets the user unique id
        /// </summary>
        /// <returns>User unique id</returns>
        public string Id {get; set;}


        /// <summary>
        /// Gets or sets the user e-mail address
        /// </summary>
        /// <returns>User e-mail address</returns>
        [Display(Name = "E-mail")]
        public string Email {get; set;}

        /// <summary>
        /// Gets or sets user first name
        /// </summary>
        /// <returns>User first name</returns>
        [Display(Name = "First name")]
        public string FirstName {get; set;}

        /// <summary>
        /// Gets or sets user last name
        /// </summary>
        /// <returns>User last name</returns>
        [Display(Name = "Last name")]
        public string LastName {get; set;}
    }
}