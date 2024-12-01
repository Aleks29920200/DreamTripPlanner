using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using WebApplication1.Identity.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationSystemDbContext _context;
       

        public HomeController(ILogger<HomeController> logger, ApplicationSystemDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "User")]
        public  IActionResult GetAllRoutes()
        {

            var routes = _context.Routes.Select(r=>new Models.RouteDto()
            {
                Id=r.RouteId,
                Name = r.RouteName,
                Coordinates = r.StartLatitude != null && r.StartLongitude != null && r.EndLatitude != null && r.EndLongitude != null
               ? new List<double[]>
               {
                    new double[] { r.StartLatitude, r.StartLongitude },
                    new double[] { r.EndLatitude, r.EndLongitude }
               }
               : new List<double[]>()

            }).ToList();


            

            return Json(routes);
        }





        public IActionResult Privacy()
        {
            return View();
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
