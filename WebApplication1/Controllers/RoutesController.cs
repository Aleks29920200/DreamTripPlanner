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
    
    public class RoutesController : Controller
    {
        private readonly ApplicationSystemDbContext _context;

        public RoutesController(ApplicationSystemDbContext context)
        {
            _context = context;
        }

                 public async Task<IActionResult> Index(string sortOrder, string searchQuery, int page = 1, int pageSize = 5)
    {
        ViewBag.SortOrder = sortOrder;
        ViewBag.SearchQuery = searchQuery;
        ViewBag.CurrentPage = page;

        var routes = _context.Routes.Include(r => r.User).
                Include(sd => sd.StartDestination)
                .Include(ed=>ed.EndDestination)
                .AsQueryable();

                if (!string.IsNullOrEmpty(searchQuery))
        {
            routes = routes.Where(r => r.RouteName.Contains(searchQuery) || r.Description.Contains(searchQuery));
        }

                routes = sortOrder switch
        {
            "name_desc" => routes.OrderByDescending(r => r.RouteName),
            "name" => routes.OrderBy(r => r.RouteName),
            "date_desc" => routes.OrderByDescending(r => r.DateCreated),
            "date" => routes.OrderBy(r => r.DateCreated),
            _ => routes.OrderBy(r => r.RouteName)
        };

                var totalCount = await routes.CountAsync();
        ViewBag.TotalPages = (int)System.Math.Ceiling((double)totalCount / pageSize);
        routes = routes.Skip((page - 1) * pageSize).Take(pageSize);

        return View(await routes.ToListAsync());
    }

                public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.User).
                Include(sd=>sd.StartDestination)
                .Include(ed=>ed.EndDestination)
                .FirstOrDefaultAsync(m => m.RouteId == id);

            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

      

        [Authorize(Roles = "Admin")]
        
                public IActionResult Create()
        {

            ViewData["UserId"] = new SelectList(_context.Set<User>(), "UserId", "Email");
            if (_context.Users.IsNullOrEmpty())
            {
                return RedirectToAction("Create", "Users");
            }
            ViewData["StartDestinationId"] = new SelectList(_context.Set<Destination>(), "DestinationId", "Name");
            if (_context.Destinations.IsNullOrEmpty())
            {
                return RedirectToAction("Create", "Destination");
            }
            ViewData["EndDestinationId"] = new SelectList(_context.Set<Destination>(), "DestinationId", "Name");
            if (_context.Destinations.Count()<2)
            {
                return RedirectToAction("Create", "Destination");
            }
            return View();
        }

                                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RouteId,RouteName,UserId,StartDestinationId,EndDestinationId,StartLatitude,StartLongitude,EndLatitude,EndLongitude,Description")] Models.Route route)
        {
            if (ModelState.IsValid)
            {
                _context.Add(route);
                await _context.SaveChangesAsync();



                return RedirectToAction(nameof(Index));

            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "UserId", "Email", route.UserId);
            ViewData["StartDestinationId"] = new SelectList(_context.Set<Destination>(), "StartDestinationId", "Name",route.StartDestinationId);
            ViewData["EndDestinationId"] = new SelectList(_context.Set<Destination>(), "DestinationId", "Name", route.EndDestinationId);
            return View(route);
        }

                [Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "UserId", "Email", route.UserId);
            ViewData["StartDestinationId"] = new SelectList(_context.Set<Destination>(), "DestinationId", "Name", route.StartDestinationId);
            ViewData["EndDestinationId"] = new SelectList(_context.Set<Destination>(), "DestinationId", "Name", route.EndDestinationId);
            return View(route);
        }

                                [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RouteId,RouteName,UserId,StartDestinationId,EndDestinationId,StartLatitude,StartLongitude,EndLatitude,EndLongitude,Description")] Models.Route route)
        {
            if (id != route.RouteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(route);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RouteExists(route.RouteId))
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
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "UserId", "Email", route.UserId);
            ViewData["StartDestinationId"] = new SelectList(_context.Set<Destination>(), "DestinationId", "Name", route.StartDestinationId);
            ViewData["EndDestinationId"] = new SelectList(_context.Set<Destination>(), "DestinationId", "Name", route.EndDestinationId);
            return View(route);
        }

                [Authorize(Roles = "Admin")]
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var route = await _context.Routes
                .Include(r => r.User).
                Include(sd => sd.StartDestination)
                .Include(ed => ed.EndDestination)
                .FirstOrDefaultAsync(m => m.RouteId == id);
            if (route == null)
            {
                return NotFound();
            }

            return View(route);
        }

                [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RouteExists(int id)
        {
            return _context.Routes.Any(e => e.RouteId == id);
        }
    }
}
