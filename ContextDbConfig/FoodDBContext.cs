using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Food_Application.Models;

namespace Food_Application.ContextDbConfig
{
    public class FoodDBContext:IdentityDbContext<ApplicationUser>
    {
        public FoodDBContext(DbContextOptions<FoodDBContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
    }
}
