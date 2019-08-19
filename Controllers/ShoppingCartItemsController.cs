using System;
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
    /// <summary>Class ShoppingCartItemsController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller"/></summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class ShoppingCartItemsController : Controller
    {
        /// <summary>The context</summary>
        private readonly FurnitureDatabaseContext _context;
        /// <summary>The user manager</summary>
        private readonly IUserManagement _userManager;

        /// <summary>Initializes a new instance of the <see cref="T:FurnitureSiteProject_MentorMate.Controllers.ShoppingCartItemsController"/> class.</summary>
        /// <param name="context">The context.</param>
        /// <param name="userManager">The user manager.</param>
        public ShoppingCartItemsController(FurnitureDatabaseContext context,
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
            bool AreThereItems = await _context.ShoppingCartItem.AnyAsync(x => x.UserId == user.Id && x.Ordered == false);
            if (AreThereItems == false)
            {
                return RedirectToAction(nameof(CartEmpty));
            }
            else
            {
                ShoppingCartItemViewModel items = new ShoppingCartItemViewModel();
                items.Items = await _context.ShoppingCartItem.Where(x => x.UserId == user.Id && x.Ordered == false).ToListAsync();
                foreach (var item in items.Items)
                {
                    if (item.Quantity > 20)
                    {
                        items.Sum += item.Quantity * _context.Item.FirstOrDefault(x => x.Id == item.ItemId).WholesalePrice;
                    }
                    else
                    {
                        items.Sum += item.Quantity * _context.Item.FirstOrDefault(x => x.Id == item.ItemId).Price;
                    }
                }
                return View(items);
            }
        }

        /// <summary>Shows empty cart.</summary>
        /// <returns>IActionResult.</returns>
        public IActionResult CartEmpty()
        {
            return View();
        }

        /// <summary>Buys all items.</summary>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyAllItems()
        {
            var user = await _userManager.FindUserByPrincipal(HttpContext.User);
            var items = _context.ShoppingCartItem.Where(x => x.UserId == user.Id && x.Ordered == false);
            Order order = new Order()
            {
                UserId = user.Id,
                Date = DateTime.Now.ToString(),
                Items = new List<ShoppingCartItem>()
            };
            
            foreach (var i in items)
            {
                Item item = await _context.Item.FindAsync(i.ItemId);
                order.Items.Add(i);
                item.Quantity = item.Quantity - i.Quantity;
                if (item.Quantity <= 0)
                {
                    item.SoldOut = true;
                }
                _context.Update(item);
            }

            foreach (var item in order.Items)
            {
                if (item.Quantity >= 20)
                {
                    order.Sum += item.Quantity * item.Product.WholesalePrice;
                }
                else
                {
                    order.Sum += item.Quantity * item.Product.Price;
                }
            }

            await _context.Order.AddAsync(order);

            foreach (var i in order.Items)
            {
                i.Ordered = true;
                _context.Update(i);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        /// <summary>Removes all items.</summary>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveAllItems()
        {
            var user = await _userManager.FindUserByPrincipal(HttpContext.User);
            var items = _context.ShoppingCartItem.Where(x => x.UserId == user.Id && x.Ordered == false);

            foreach (var i in items)
            {
                _context.ShoppingCartItem.Remove(i);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>Decreases the quantity.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseQuantity(int? id)
        {
            var item = _context.ShoppingCartItem.FirstOrDefault(x => x.Id == id);

            if (item.Quantity == 1)
            {
            }
            else
            {
                item.Quantity--;
                _context.Update(item);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>Increases the quantity.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseQuantity(int? id)
        {
            var item = _context.ShoppingCartItem.FirstOrDefault(x => x.Id == id);
            var product = await _context.Item.FirstOrDefaultAsync(x => x.Id == item.ItemId);

            if (item.Quantity == product.Quantity)
            {
            }
            else
            {
                item.Quantity++;
                _context.Update(item);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>Decreases the quantity by 10.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseQuantity10(int? id)
        {
            var item = _context.ShoppingCartItem.FirstOrDefault(x => x.Id == id);

            if (item.Quantity <= 10)
            {
                item.Quantity = 1;

                _context.Update(item);

                await _context.SaveChangesAsync();
            }
            else
            {
                item.Quantity = item.Quantity - 10;
                _context.Update(item);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>Increases the quantity by 10.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IncreaseQuantity10(int? id)
        {
            var item = await _context.ShoppingCartItem.FirstOrDefaultAsync(x => x.Id == id);
            var product = await _context.Item.FirstOrDefaultAsync(x => x.Id == item.ItemId);

            if (item.Quantity >= product.Quantity - 10)
            {
                item.Quantity = product.Quantity;

                _context.Update(item);

                await _context.SaveChangesAsync();
            }
            else
            {
                item.Quantity = item.Quantity + 10;
                _context.Update(item);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>Detailses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>IActionResult.</returns>
        public IActionResult Details(int? id)
        {
            return RedirectToAction("Details","Items",new { id = id });
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

            var shoppingCartItem = await _context.ShoppingCartItem
                .FirstOrDefaultAsync(m => m.Id == id);

            _context.ShoppingCartItem.Remove(shoppingCartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>Deletes the confirmed.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCartItem = await _context.ShoppingCartItem.FindAsync(id);
            _context.ShoppingCartItem.Remove(shoppingCartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Shoppings the cart item exists.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ShoppingCartItemExists(int id)
        {
            return _context.ShoppingCartItem.Any(e => e.Id == id);
        }
    }
}
