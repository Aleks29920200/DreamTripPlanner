using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Identity.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationSystemDbContext _context;

        public UsersController(ApplicationSystemDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1, int pageSize = 5)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["UsernameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "username";
            ViewData["EmailSortParm"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["FirstNameSortParm"] = sortOrder == "firstName" ? "firstName_desc" : "firstName";
            ViewData["LastNameSortParm"] = sortOrder == "lastName" ? "lastName_desc" : "lastName";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentPage"] = page;

            var users = _context.Users.Include(u => u.role).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Username.Contains(searchString)
                                      || u.Email.Contains(searchString)
                                      || u.FirstName.Contains(searchString)
                                      || u.LastName.Contains(searchString));
            }

            users = sortOrder switch
            {
                "username_desc" => users.OrderByDescending(u => u.Username),
                "username" => users.OrderBy(u => u.Username),
                "email_desc" => users.OrderByDescending(u => u.Email),
                "email" => users.OrderBy(u => u.Email),
                "firstName_desc" => users.OrderByDescending(u => u.FirstName),
                "firstName" => users.OrderBy(u => u.FirstName),
                "lastName_desc" => users.OrderByDescending(u => u.LastName),
                "lastName" => users.OrderBy(u => u.LastName),
                _ => users.OrderBy(u => u.Username),
            };

            var totalCount = await users.CountAsync();
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);

            users = users.Skip((page - 1) * pageSize).Take(pageSize);

            return View(await users.AsNoTracking().ToListAsync());
        }




        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

                [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Set<Role>(), "RoleId", "Name");
            if (_context.Roles.IsNullOrEmpty())
            {
                return RedirectToAction("Create", "Roles");
            }
            return View();
        }

                                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Username,PasswordHash,FirstName,LastName,Email,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Set<Role>(), "RoleId", "Name", user.RoleId);
            return View(user);
        }

                [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Set<Role>(), "RoleId", "Name", user.RoleId);
            return View(user);
        }

                                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Username,FirstName,LastName,PasswordHash,Email,RoleId")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            ViewData["RoleId"] = new SelectList(_context.Set<Role>(), "RoleId", "Name", user.RoleId);
            return View(user);
        }

                [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.role)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

                [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);

            DbSet<Models.Route> routes = _context.Routes;
            Models.Route[] routes1 = routes.Where(e => e.UserId==id).ToArray();
            if (routes1.Length != 0)
            {
             routes.RemoveRange(routes1);
            }
            DbSet<Group> groups = _context.Groups;
            Group[] groups1 = groups.Where(e => e.UserId == id).ToArray();
            if (groups1.Length != 0)
            {
                groups.RemoveRange(groups1);
            }
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
