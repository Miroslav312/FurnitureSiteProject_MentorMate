using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurnitureSiteProject_MentorMate.Models;
using FurnitureSiteProject_MentorMate.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace FurnitureSiteProject_MentorMate.Controllers
{

    /// <summary>Class CategoriesController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller"/></summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        /// <summary>The context</summary>
        private readonly FurnitureDatabaseContext _context;

        /// <summary>Initializes a new instance of the <see cref="T:FurnitureSiteProject_MentorMate.Controllers.CategoriesController"/> class.</summary>
        /// <param name="context">The context.</param>
        public CategoriesController(FurnitureDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>Indexes this instance.</summary>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }


        /// <summary>Creates this instance.</summary>
        /// <returns>IActionResult.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>Creates the specified category.</summary>
        /// <param name="category">The category.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        /// <summary>Deletes the confirmed.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Categories the exists.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
