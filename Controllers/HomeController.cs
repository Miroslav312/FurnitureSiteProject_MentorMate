using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurnitureSiteProject_MentorMate.Services;
using FurnitureSiteProject_MentorMate.Infrastructure;
using FurnitureSiteProject_MentorMate.Models;
using Microsoft.Extensions.Options;
using System;

namespace FurnitureSiteProject_MentorMate.Controllers
{
    /// <summary>Controller for home page management</summary>
    public class HomeController : Controller
    {

        /// <summary>The context</summary>
        private readonly FurnitureDatabaseContext _context;

        /// <summary>Default constructor</summary>
        /// <param name="context"></param>
        public HomeController(FurnitureDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>Displays view with home page</summary>
        /// <returns>View with home page</returns>
        public async Task<IActionResult> Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Items = await _context.Item.ToListAsync();
            homeViewModel.NewestItem = homeViewModel.Items.FirstOrDefault(x => x.Date == x.Date);

            return View(homeViewModel);
        }

        public IActionResult Contacts()
        {
            return View();
        }
    }
}