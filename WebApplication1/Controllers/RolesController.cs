using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Identity.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly ApplicationSystemDbContext _context;

        public RolesController(ApplicationSystemDbContext context)
        {
            _context = context;
        }

                public async Task<IActionResult> Index(string sortOrder, string searchQuery, int page = 1, int pageSize = 5)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.CurrentPage = page;

            var roles = _context.Roles.AsQueryable();

                        if (!string.IsNullOrEmpty(searchQuery))
            {
                roles = roles.Where(r => r.Name.Contains(searchQuery) || r.Description.Contains(searchQuery));
            }

                        roles = sortOrder switch
            {
                "name_desc" => roles.OrderByDescending(r => r.Name),
                "name" => roles.OrderBy(r => r.Name),
                "desc_desc" => roles.OrderByDescending(r => r.Description),
                "desc" => roles.OrderBy(r => r.Description),
                _ => roles.OrderBy(r => r.Name)
            };

                        var totalCount = await roles.CountAsync();
            ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalCount / pageSize);
            roles = roles.Skip((page - 1) * pageSize).Take(pageSize);

            return View(await roles.ToListAsync());
        }

                public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

                
        public IActionResult Create()
        {
            return View();
        }

                                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,Name,Description")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

                
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

                                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoleId,Name,Description")] Role role)
        {
            if (id != role.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.RoleId))
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
            return View(role);
        }

               
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

                [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.RoleId == id);
        }
    }
}
