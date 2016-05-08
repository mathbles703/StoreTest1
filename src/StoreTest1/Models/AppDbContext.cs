using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace StoreTest1.Models
{

    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Brand>().ForSqlServerToTable("Brands")
                .Property(c => c.Id).UseSqlServerIdentityColumn();
            builder.Entity<Item>().ForSqlServerToTable("Items");
            builder.Entity<Tray>().ForSqlServerToTable("Trays")
                .Property(t => t.Id).UseSqlServerIdentityColumn();
            builder.Entity<TrayItem>().ForSqlServerToTable("TrayItems")
                .Property(ti => ti.Id).UseSqlServerIdentityColumn();
            builder.Entity<Store>().ForSqlServerToTable("Stores")
                .Property(s => s.Id).UseSqlServerIdentityColumn();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;AttachDbFilename=D:\\College\\Term 4\\INFO 3067\\$Workspace 3067\\StoreTest1DB\\StoreTest1Db.mdf;Integrated Security=True;Connect Timeout=10");
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Tray> Trays { get; set; }
        public virtual DbSet<TrayItem> TrayItems { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
    }
}
