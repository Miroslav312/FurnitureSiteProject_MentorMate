using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurnitureSiteProject_MentorMate.Infrastructure;
using FurnitureSiteProject_MentorMate.Models;
using Microsoft.AspNetCore.Authorization;

namespace FurnitureSiteProject_MentorMate.Controllers
{
    /// <summary>Class OrdersController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller"/></summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class OrdersController : Controller
    {
        /// <summary>The context</summary>
        private readonly FurnitureDatabaseContext _context;
        /// <summary>The user manager</summary>
        private readonly IUserManagement _userManager;

        /// <summary>Initializes a new instance of the <see cref="T:FurnitureSiteProject_MentorMate.Controllers.OrdersController"/> class.</summary>
        /// <param name="context">The context.</param>
        /// <param name="userManager">The user manager.</param>
        public OrdersController(FurnitureDatabaseContext context,
            IUserManagement userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>Indexes this instance.</summary>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindUserByPrincipal(HttpContext.User);
            List<Order> orders =await _context.Order.Include(x => x.Items).Where(x => x.UserId == user.Id).ToListAsync();

            return View(orders);
        }

        /// <summary>Detailses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(x => x.Items)
                .FirstOrDefaultAsync(m => m.Id == id);

            foreach (var item in order.Items)
            {
                item.Product = await _context.Item.FirstOrDefaultAsync(x => x.Id == item.ItemId);
            }

                if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>Orders the exists.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
