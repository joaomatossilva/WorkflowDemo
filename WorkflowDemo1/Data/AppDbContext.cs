using System;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WorkflowDemo1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("appConString")
        {
            Database.SetInitializer<AppDbContext>(null);
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Holiday> Holidays { get; set; }
    }

    public class Holiday
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}