using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Models;
using Route = WebApplication1.Models.Route;

namespace WebApplication1.Identity.Data;

public class ApplicationSystemDbContext(DbContextOptions<ApplicationSystemDbContext> options) : IdentityDbContext<IdentityUser>(options)

{
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Destination> Destinations { get; set; }
    public DbSet<Route> Routes { get; set; }

    


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Group>()
           .HasOne(g => g.User)
           .WithMany(u => u.groups) 
           .HasForeignKey(g => g.UserId);

        builder.Entity<Route>()
          .HasOne(g => g.User)
          .WithMany(u => u.routes) 
          .HasForeignKey(g => g.UserId);
       
        builder.Entity<Route>()
  .HasOne(g => g.StartDestination)
  .WithMany(u => u.routes)
  .HasForeignKey(g => g.StartDestinationId);

        builder.Entity<Route>()
 .HasOne(g => g.StartDestination)
 .WithMany(u => u.routes)
 .HasForeignKey(g => g.EndDestinationId);
        builder.Entity<User>()
          .HasOne(g => g.role)
          .WithMany(u => u.users) 
          .HasForeignKey(g => g.RoleId);
    }
}
