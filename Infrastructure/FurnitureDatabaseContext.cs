using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FurnitureSiteProject_MentorMate.Models;

namespace FurnitureSiteProject_MentorMate.Infrastructure
{
    /// <summary>
    /// Class FurnitureDatabaseContext.
    /// Implements the <see cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext{FurnitureSiteProject_MentorMate.Models.ApplicationUser}"/>
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext{FurnitureSiteProject_MentorMate.Models.ApplicationUser}" />
    public class FurnitureDatabaseContext : IdentityDbContext<ApplicationUser> 
    {
        /// <summary>Initializes a new instance of the <see cref="T:FurnitureSiteProject_MentorMate.Infrastructure.FurnitureDatabaseContext"/> class.</summary>
        /// <param name="options">The options.</param>
        public FurnitureDatabaseContext(DbContextOptions<FurnitureDatabaseContext> options) : base(options)
        {
            
        }

        /// <summary>Gets or sets the item.</summary>
        /// <value>The item.</value>
        public DbSet<FurnitureSiteProject_MentorMate.Models.Item> Item { get; set; }

        /// <summary>Gets or sets the category.</summary>
        /// <value>The category.</value>
        public DbSet<FurnitureSiteProject_MentorMate.Models.Category> Category { get; set; }

        /// <summary>Gets or sets the register view model.</summary>
        /// <value>The register view model.</value>
        public DbSet<FurnitureSiteProject_MentorMate.Models.RegisterViewModel> RegisterViewModel { get; set; }

        /// <summary>Gets or sets the shopping cart item.</summary>
        /// <value>The shopping cart item.</value>
        public DbSet<FurnitureSiteProject_MentorMate.Models.ShoppingCartItem> ShoppingCartItem { get; set; }

        /// <summary>Gets or sets the order.</summary>
        /// <value>The order.</value>
        public DbSet<FurnitureSiteProject_MentorMate.Models.Order> Order { get; set; }

        /// <summary>Gets or sets the add quantity view model.</summary>
        /// <value>The add quantity view model.</value>
        public DbSet<FurnitureSiteProject_MentorMate.Models.AddQuantityViewModel> AddQuantityViewModel { get; set; }
    }
}