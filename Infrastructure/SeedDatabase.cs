using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using FurnitureSiteProject_MentorMate.Models;
using FurnitureSiteProject_MentorMate.Services;
using Microsoft.Extensions.Options;
using FurnitureSiteProject_MentorMate.Infrastructure;
using System;

namespace FurnitureSiteProject_MentorMate.Infrastructure
{
    /// <summary>Class initalize database with default user data</summary>
    public class SeedDatabase : ISeedDatabase
    {

        /// <summary>The context</summary>
        private readonly FurnitureDatabaseContext _context;
        /// <summary>Furniture database context</summary>
        private readonly IDatabaseProvider _FurnitureDbContext;
        /// <summary>Manage users data</summary>
        private readonly IUserManagement _userManagement;
        /// <summary>Manage users roles</summary>
        private readonly IRoleManagement _roleManager;
        /// <summary>Default user data</summary>
        private readonly DefaultUserData _defaultUserData;


        /// <summary>Default constructor</summary>
        /// <param name="context"></param>
        /// <param name="FurnitureDbContext">Furniture database context</param>
        /// <param name="userManager">Provides API to manage users</param>
        /// <param name="roleManager">Provides API to handle user roles assignment</param>
        /// <param name="defaultUserData"></param>
        public SeedDatabase(FurnitureDatabaseContext context,
            IDatabaseProvider FurnitureDbContext,
            IUserManagement userManager,
            IRoleManagement roleManager,
            IOptionsSnapshot<DefaultUserData> defaultUserData )
        {
            _context = context;
            _FurnitureDbContext = FurnitureDbContext;
            _userManagement = userManager;
            _roleManager = roleManager;
            _defaultUserData = defaultUserData.Value;
        }

        /// <summary>Initalizes this instance.</summary>
        public async void Initalize()
        {
            _FurnitureDbContext.EnsureCreated();

            if (!_context.Category.Any())
            {
                _context.Category.Add(new Category() { Id = 1, Name = "Chairs" });
                _context.Category.Add(new Category() { Id = 2, Name = "Dressers" });
                _context.Category.Add(new Category() { Id = 3, Name = "Tables" });
                _context.Category.Add(new Category() { Id = 4, Name = "Desks" });
                await _context.SaveChangesAsync();
            }

            if (!_context.Item.Any())
            {
                _context.Item.Add(new Item()
                {
                    Name = "EKEDALEN",
                    ShortName = "EKEDALEN",
                    Description = "The upholstered seat and comfortable angle of the backrest make the chair perfect for long dinners. You can choose from several different seat covers and switch between them when you like. The cover is easy to remove, machine - wash and put back on again. Solid wood chair frame makes the construction durable and very sturdy.",
                    CategoryId = 1,
                    Weight = 7,
                    Price = 50,
                    WholesalePrice = 45,
                    Quantity = 50,
                    ImagePath = "/resources/SeedDataItemImages/ekedalen-chair-brown.jpg",
                    Date = DateTime.Now.ToString(),
                    SoldOut = false,
                    Archived = false
                });
                _context.Item.Add(new Item()
                {
                    Name = "STEFAN",
                    ShortName = "STEFAN",
                    Description = "A sturdy chair with a solid wood construction that can handle the challenges of everyday life! Combines nicely with most styles, and if you’re looking for extra comfort, simply add a chair pad. ",
                    CategoryId = 1,
                    Weight = 5,
                    Price = 40,
                    WholesalePrice = 35,
                    Quantity = 50,
                    ImagePath = "/resources/SeedDataItemImages/stefan-chair.jpg",
                    Date = DateTime.Now.ToString(),
                    SoldOut = false,
                    Archived = false
                });

                _context.Item.Add(new Item()
                {
                    Name = "HEMNES",
                    ShortName = "HEMNES",
                    Description = "Made of solid wood, which is a durable and warm natural material. Smooth running drawers with pull -out stop. Of course your home should be a safe place for the entire family.That’s why hardware is included so that you can attach the chest of drawers to the wall.",
                    CategoryId = 2,
                    Weight = 20,
                    Price = 230,
                    WholesalePrice = 210,
                    Quantity = 50,
                    ImagePath = "/resources/SeedDataItemImages/hemnes-drawer-chest.jpg",
                    Date = DateTime.Now.ToString(),
                    SoldOut = false,
                    Archived = false
                });
                _context.Item.Add(new Item()
                {
                    Name = "MALM",
                    ShortName = "MALM",
                    Description = "Of course your home should be a safe place for the entire family. That’s why hardware is included so that you can attach the chest of drawers to the wall. A wide chest of drawers gives you plenty of storage space as well as room for lamps or other items you want to display on top.",
                    CategoryId = 2,
                    Weight = 25,
                    Price = 200,
                    WholesalePrice = 180,
                    Quantity = 50,
                    ImagePath = "/resources/SeedDataItemImages/malm-drawer-dresser-white.jpg",
                    Date = DateTime.Now.ToString(),
                    SoldOut = false,
                    Archived = false
                });

                _context.Item.Add(new Item()
                {
                    Name = "BJURSTA",
                    ShortName = "BJURSTA",
                    Description = "Extendable dining table with 2 extra leaves seats 4-8; makes it possible to adjust the table size according to need. The hidden lock keeps the extension leaf in place and prevents gaps between the leaves. You can store the extension leaves within easy reach under the table top. The clear-lacquered surface is easy to wipe clean.",
                    CategoryId = 3,
                    Weight = 13,
                    Price = 150,
                    WholesalePrice = 130,
                    Quantity = 50,
                    ImagePath = "/resources/SeedDataItemImages/Bjrusta.jpg",
                    Date = DateTime.Now.ToString(),
                    SoldOut = false,
                    Archived = false
                });
                _context.Item.Add(new Item()
                {
                    Name = "INGATORP",
                    ShortName = "INGATORP",
                    Description = "It’s quick and easy to change the size of the table to suit your different needs. With the extra leaf stored under the table top you can extend the table to seat from 4 to 6 people. Concealed locking function prevents gaps between top and leaf and keeps the extra leaf in place. Can be easily extended by one person.",
                    CategoryId = 3,
                    Weight = 17,
                    Price = 250,
                    WholesalePrice = 225,
                    Quantity = 50,
                    ImagePath = "/resources/SeedDataItemImages/ingratorp.jpg",
                    Date = DateTime.Now.ToString(),
                    SoldOut = false,
                    Archived = false
                });

                _context.Item.Add(new Item()
                {
                    Name = "MICKE",
                    ShortName = "MICKE",
                    Description = "It’s easy to keep cords and cables out of sight but close at hand with the cable outlet at the back. You can mount the storage unit to the right or left, according to your space or preference. Air ventilates effectively around your computer or other equipment because of an opening in the back panel.",
                    CategoryId = 4,
                    Weight = 27,
                    Price = 90,
                    WholesalePrice = 75,
                    Quantity = 50,
                    ImagePath = "/resources/SeedDataItemImages/micke.jpg",
                    Date = DateTime.Now.ToString(),
                    SoldOut = false,
                    Archived = false
                });
                _context.Item.Add(new Item()
                {
                    Name = "LINNMON",
                    ShortName = "LINNMON",
                    Description = "Drawer stops prevent the drawers from being pulled out too far. Can be placed anywhere in the room because the back is finished. Adjustable feet allow you to level the table on uneven floors.",
                    CategoryId = 4,
                    Weight = 7,
                    Price = 50,
                    WholesalePrice = 45,
                    Quantity = 50,
                    ImagePath = "/resources/SeedDataItemImages/linnmon.jpg",
                    Date = DateTime.Now.ToString(),
                    SoldOut = false,
                    Archived = false
                });
                await _context.SaveChangesAsync();
            }

            if (_FurnitureDbContext.Roles.Any(r => r.Name == "Costumer")) return;

            await _roleManager.CreateAsync(new IdentityRole("Costumer"));

            if (_FurnitureDbContext.Roles.Any(r => r.Name == "Administrator")) return;

            await _roleManager.CreateAsync(new IdentityRole("Administrator"));

            await _userManagement.CreateAsync(new ApplicationUser 
                {
                    UserName = _defaultUserData.Email, 
                    Email = _defaultUserData.Email, 
                    EmailConfirmed = true

                }, _defaultUserData.Password);

            await _userManagement.AddToRoleAsync(await _userManagement.FindByEmailAsync(_defaultUserData.Email), "Administrator");
        }
    }
}