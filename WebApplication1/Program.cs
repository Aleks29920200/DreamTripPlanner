using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace WebApplication1
{
    
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddControllersWithViews();

                builder.Services.AddRazorPages();

                builder.Services.AddDbContext<ApplicationSystemDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                builder.Services.AddIdentity<IdentityUser, IdentityRole>
                    (options => options.SignIn.RequireConfirmedAccount = true).
                    AddDefaultTokenProviders()
                    .AddDefaultUI().
                    AddEntityFrameworkStores<ApplicationSystemDbContext>();



                var app = builder.Build();

                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                }
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();



                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.MapRazorPages();
                app.Run();
            }
        
    }
}
