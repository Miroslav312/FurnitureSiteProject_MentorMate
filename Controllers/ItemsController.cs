using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FurnitureSiteProject_MentorMate.Models;
using FurnitureSiteProject_MentorMate.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace FurnitureSiteProject_MentorMate.Controllers
{
    /// <summary>Class ItemsController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller"/></summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class ItemsController : Controller
    {
        /// <summary>The context</summary>
        private readonly FurnitureDatabaseContext _context;
        /// <summary>The file provider</summary>
        private readonly IFileProvider _fileProvider;
        /// <summary>The hosting environment</summary>
        private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>The user manager</summary>
        private readonly IUserManagement _userManager;

        /// <summary>Initializes a new instance of the <see cref="T:FurnitureSiteProject_MentorMate.Controllers.ItemsController"/> class.</summary>
        /// <param name="context">The context.</param>
        /// <param name="fileprovider">The fileprovider.</param>
        /// <param name="env">The env.</param>
        /// <param name="userManagemer">The user managemer.</param>
        public ItemsController(FurnitureDatabaseContext context,
            IFileProvider fileprovider,
            IHostingEnvironment env,
            IUserManagement userManagemer)
        {
            _context = context;
            _fileProvider = fileprovider;
            _hostingEnvironment = env;
            _userManager = userManagemer;
        }

        /// <summary>Indexes the specified search string.</summary>
        /// <param name="searchString">The search string.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <param name="ArchivedValue">if set to <c>true</c> [archived value].</param>
        /// <param name="soldOutValue">if set to <c>true</c> [sold out value].</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString,string categoryName,bool ArchivedValue, bool soldOutValue)
        {
            ItemIndexViewModel viewModel = new ItemIndexViewModel();
            viewModel.Items = await _context.Item.Include(i => i.Category).ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                viewModel.Items = viewModel.Items.Where(s => s.Name.Contains(searchString)).ToList();
            }

            if (!String.IsNullOrEmpty(categoryName))
            {
                viewModel.Items = viewModel.Items.Where(s => s.Category.Name == categoryName).ToList();
            }

            if (ArchivedValue)
            {
                viewModel.Items = viewModel.Items.Where(s => s.Archived == true).ToList();
            }

            if (soldOutValue)
            {
                viewModel.Items = viewModel.Items.Where(s => s.SoldOut == true).ToList();
            }

            viewModel.Categories = new SelectList(await _context.Category.Select(x => x.Name).Distinct().ToListAsync());
            return View(viewModel);
        }

        /// <summary>Detailses the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        /// <summary>Adds to cart.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindUserByPrincipal(HttpContext.User);

            if (_context.ShoppingCartItem.Any(x => x.UserId == user.Id && x.ItemId == item.Id))
            {
                var cartItem = await _context.ShoppingCartItem.FirstOrDefaultAsync(x => x.UserId == user.Id && x.ItemId == item.Id);
                cartItem.Quantity++;

                _context.Update(cartItem);
            }
            else
            {
                ShoppingCartItem cartItem = new ShoppingCartItem()
                {
                    UserId = user.Id,
                    ItemId = item.Id,
                    Product = item,
                    Quantity = 1,
                    Ordered = false
                };

                _context.ShoppingCartItem.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        /// <summary>Creates this instance.</summary>
        /// <returns>IActionResult.</returns>
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }

        /// <summary>Creates the specified item.</summary>
        /// <param name="item">The item.</param>
        /// <param name="file">The file.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Weight,Price,WholesalePrice,Image,CategoryId,Archived,SoldOut,Quantity,Date")] Item item, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                item.Date = DateTime.Now.ToString();
                if (item.Name.Length > 19)
                {
                    item.ShortName = item.Name.Substring(19);
                }
                else
                {
                    item.ShortName = item.Name;
                }
                _context.Add(item);
                await _context.SaveChangesAsync();

                if (file != null || file.Length != 0)
                {
                    // Create a File Info 
                    FileInfo fi = new FileInfo(file.FileName);

                    // This code creates a unique file name to prevent duplications 
                    // stored at the file location
                    var newFilename = item.Id + "_" + String.Format("{0:d}",
                                      (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
                    var webPath = _hostingEnvironment.WebRootPath;
                    var path = Path.Combine("", webPath + @"\ImageFiles\" + newFilename);

                    // IMPORTANT: The pathToSave variable will be save on the column in the database
                    var pathToSave = @"/ImageFiles/" + newFilename;

                    // This stream the physical file to the allocate wwwroot/ImageFiles folder
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // This save the path to the record
                    item.ImagePath = pathToSave;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", item.CategoryId);
            return View(item);
        }

        /// <summary>Adds the quantity.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddQuantity(int? id)
        {
            var Item = await _context.Item
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(Item);
        }

        /// <summary>Adds the quantity.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="quantity">The quantity.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddQuantity(int id,int quantity)
        {
            var item = await _context.Item
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            item.Quantity += quantity;
            _context.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>Archives the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Archive(int? id)
        {
            var item = await _context.Item
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            item.Archived = true;
            _context.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>Unarchives the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Unarchive(int? id)
        {
            var item = await _context.Item
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            item.Archived = false;
            _context.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        /// <summary>Edits the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            var item = await _context.Item.FindAsync(id);
            item.Categories = _context.Category.ToList();
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        /// <summary>Edits the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="item">The item.</param>
        /// <param name="file">The file.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Weight,Price,WholesalePrice,ImagePath,CategoryId,Archived,SoldOut,Quantity")] Item item, IFormFile file)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    if(item.Image != file && file != null)
                    {
                        if (file != null || file.Length != 0)
                        {
                            // Create a File Info 
                            FileInfo fi = new FileInfo(file.FileName);

                            // This code creates a unique file name to prevent duplications 
                            // stored at the file location
                            var newFilename = item.Id + "_" + String.Format("{0:d}",
                                              (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
                            var webPath = _hostingEnvironment.WebRootPath;
                            var path = Path.Combine("", webPath + @"\ImageFiles\" + newFilename);

                            // IMPORTANT: The pathToSave variable will be save on the column in the database
                            var pathToSave = @"/ImageFiles/" + newFilename;

                            // This stream the physical file to the allocate wwwroot/ImageFiles folder
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            // This save the path to the record
                            item.ImagePath = pathToSave;
                            _context.Update(item);
                            await _context.SaveChangesAsync();
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", item.CategoryId);
            return View(item);
        }

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        /// <summary>Deletes the confirmed.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>Items the exists.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
