using SistemaMirno.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SistemaMirno.DataAccess
{
    public class MirnoDbContext : DbContext
    {
        public MirnoDbContext() : base("DesktopMirnoDb")
        {
        }

        public DbSet<AreaConnection> AreaConnections { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductionArea> ProductionAreas { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Responsible> Responsibles { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkUnit> WorkUnits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}