using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    // To inherit from dbcontext provied by enitity framework
    //enitiy framework produces abstraction as we call functions and then it executes sql commands for us
    // We can mock something easily if it has an interface
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}