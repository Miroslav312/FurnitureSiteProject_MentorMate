using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FurnitureSiteProject_MentorMate.Models;
using FurnitureSiteProject_MentorMate.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;

namespace FurnitureSiteProject_MentorMate.Services
{
    /// <summary>
    /// Defines operations on database
    /// </summary>
    public class DatabaseProvider : IDatabaseProvider
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly FurnitureDatabaseContext _databaseContext;

        /// <summary>
        /// Gets the roles definned by Identity framework in database
        /// </summary>
        /// <returns>Roles definned by Identity framework in database</returns>
        public DbSet<IdentityRole> Roles{
            get {return _databaseContext.Roles;}
        }

        /// <summary>
        /// Default constructir
        /// </summary>
        /// <param name="databaseContext">Database context</param>
        public DatabaseProvider(FurnitureDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Method ensures that dabase has been creaed
        /// </summary>
        /// <returns>True when created or already exists, false in other case</returns>
        public bool EnsureCreated()
        {
            return _databaseContext.Database.EnsureCreated();
        }
    }
}