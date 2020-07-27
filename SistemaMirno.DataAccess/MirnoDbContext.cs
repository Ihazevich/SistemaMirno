using SistemaMirno.Model;
using System.Data.Entity;

namespace SistemaMirno.DataAccess
{
    public class MirnoDbContext : DbContext
    {
        public MirnoDbContext() : base("MirnoDbContext")
        {

        }
        public DbSet<ProductionArea> ProductionAreas { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Color> Colors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
