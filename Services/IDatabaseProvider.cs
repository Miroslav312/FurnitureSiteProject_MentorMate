using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FurnitureSiteProject_MentorMate.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FurnitureSiteProject_MentorMate.Services
{
    /// <summary>
    /// Interface defines operations on Furniture database
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Gets the roles definned by Identity framework in database
        /// </summary>
        DbSet<IdentityRole> Roles {get;}

        /// <summary>
        /// Method ensures that dabase has been creaed
        /// </summary>
        /// <returns>True when created or already exists, false in other case</returns>
        bool EnsureCreated();
    }
}