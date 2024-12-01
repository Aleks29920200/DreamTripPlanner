using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Identity.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationSystemDbContext _context;

        public GroupsController(ApplicationSystemDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchQuery = "", string sortOrder = "", int page = 1, int pageSize = 10)
        {
            var groupsQuery = _context.Groups.Include(u => u.User).AsQueryable();

            
            if (!string.IsNullOrEmpty(searchQuery))
            {
                groupsQuery = groupsQuery.Where(g => g.GroupName.Contains(searchQuery) || g.Description.Contains(searchQuery));
            }

           
            groupsQuery = sortOrder switch
            {
                "name_desc" => groupsQuery.OrderByDescending(g => g.GroupName),
                "date" => groupsQuery.OrderBy(g => g.CreationDate),
                "date_desc" => groupsQuery.OrderByDescending(g => g.CreationDate),
                _ => groupsQuery.OrderBy(g => g.GroupName),             };

                        var totalCount = await groupsQuery.CountAsync();

                        var groups = await groupsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                        ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.SortOrder = sortOrder;
            ViewBag.SearchQuery = searchQuery;

            return View(groups);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

                [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "UserId", "Email");
                if (_context.Users.IsNullOrEmpty())
            {
                return RedirectToAction("Create","Users");
            }
            return View();
        }

                                [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupName,UserId,Description")] GroupDto group)
        {
var groupEntity = new Group()
                {
                    GroupName = group.GroupName,
                    UserId = group.UserId,
                    CreationDate = group.CreationDate,
                    Description = group.Description,
                };
            if (ModelState.IsValid)
            {
                
                _context.Add(groupEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", groupEntity.UserId);
            return View(groupEntity);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "UserId", "Email", @group.UserId);


            return View(@group);
        }

                                [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("GroupId,GroupName,UserId,Description")] Group @group)
        {
            if (id != @group.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.GroupId))
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
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "UserId", "Email", @group.UserId);
            return View(@group);
        }

                public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(@Users => @Users.User)
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

                [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Groups.FindAsync(id);
            if (@group != null)
            {
                _context.Groups.Remove(@group);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }
    }
}
