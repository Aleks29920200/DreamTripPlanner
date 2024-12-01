using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Identity.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DestinationController : Controller
    {
        private readonly ApplicationSystemDbContext _context;
        private readonly IWebHostEnvironment _environement;

        public DestinationController(ApplicationSystemDbContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environement = environment;
        }

        [HttpGet]
public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber)
{
    ViewData["CurrentSort"] = sortOrder;
    ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
    ViewData["LocationSortParm"] = sortOrder == "location" ? "location_desc" : "location";
    ViewData["CurrentFilter"] = searchString;

    var destinations = from d in _context.Destinations
                       select d;

    if (!String.IsNullOrEmpty(searchString))
    {
        destinations = destinations.Where(d => 
            d.Name.Contains(searchString) || 
            d.Description.Contains(searchString) || 
            d.Location.Contains(searchString));
    }

    switch (sortOrder)
    {
        case "name_desc":
            destinations = destinations.OrderByDescending(d => d.Name);
            break;
        case "location":
            destinations = destinations.OrderBy(d => d.Location);
            break;
        case "location_desc":
            destinations = destinations.OrderByDescending(d => d.Location);
            break;
        default:
            destinations = destinations.OrderBy(d => d.Name);
            break;
    }

    int pageSize = 10; // Define how many items per page
    return View(await PaginatedList<Destination>.CreateAsync(destinations.AsNoTracking(), pageNumber ?? 1, pageSize));
}





        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations
                .FirstOrDefaultAsync(m => m.DestinationId == id);
            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

                [HttpGet]
        
        public IActionResult Create()
        {
            return View();
        }

                                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DestinationDto destination)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                                if (destination.ImageFile != null)
                {
                                        string uploadsFolder = Path.Combine(_environement.WebRootPath, "uploads");

                                        if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                                        uniqueFileName = Guid.NewGuid().ToString() + "_" + destination.ImageFile.FileName;

                                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await destination.ImageFile.CopyToAsync(fileStream);
                    }
                    Destination destinations1 = new Destination()
                    {
                        Name = destination.Name,
                        Description = destination.Description,
                        Location = destination.Location,
                        PopularSeason = destination.PopularSeason,
                        ImageFileName = "/uploads/"+uniqueFileName,
                    };
                    _context.Add(destinations1);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

             return View(destination);
            
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
            {
                return NotFound();
            }
          
  
            var model = new DestinationDto
            {
                Id = destination.DestinationId,
                Name = destination.Name,
                Description = destination.Description,
                Location = destination.Location,
                PopularSeason = destination.PopularSeason,
            };
          
            return View(model);
 
        }

                                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Location,PopularSeason,ImageFile")] DestinationDto destination)
        {

            Destination? destination1 = await _context.Destinations.FindAsync(id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (destination.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(_environement.WebRootPath, "uploads");

                                                if (!string.IsNullOrEmpty(destination1.ImageFileName))
                        {
                            string oldFilePath = Path.Combine(_environement.WebRootPath, destination1.ImageFileName.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                                                string uniqueFileName = Guid.NewGuid().ToString() + "_" + destination.ImageFile.FileName;
                        string newFilePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await destination.ImageFile.CopyToAsync(fileStream);
                        }

                                                destination1.ImageFileName = "/uploads/" + uniqueFileName;
                    }
                    _context.Update(destination1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinationExists(destination1.DestinationId))
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
            return View(destination);
        }

              
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinations
                .FirstOrDefaultAsync(m => m.DestinationId == id);

            
            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

                [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination != null)
            {
                DbSet<Models.Route> routes = _context.Routes;
                Models.Route[] routes1 = routes.Where(e => e.EndDestinationId == id || e.StartDestinationId == id).ToArray();
                routes.RemoveRange(routes1);
                _context.Destinations.Remove(destination);
                if (!string.IsNullOrEmpty(destination.ImageFileName))
                {
                    string oldFilePath = Path.Combine(_environement.WebRootPath, destination.ImageFileName.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DestinationExists(int id)
        {
            return _context.Destinations.Any(e => e.DestinationId == id);
        }
    }

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }

}
