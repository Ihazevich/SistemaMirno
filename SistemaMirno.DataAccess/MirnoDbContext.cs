using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SistemaMirno.Model;

namespace SistemaMirno.DataAccess
{
    /// <summary>
    /// A class representing the database context of the system.
    /// </summary>
    public class MirnoDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MirnoDbContext"/> class.
        /// </summary>
        public MirnoDbContext() : base("LaptopMirnoDb")
        {
            Database.SetInitializer<MirnoDbContext>(new DropCreateDatabaseIfModelChanges<MirnoDbContext>());
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Assistance> Assistances { get; set; }

        public DbSet<SalaryPayment> SalaryPayments { get; set; }

        public DbSet<SalaryDiscount> SalaryDiscounts { get; set; }

        public DbSet<HistoricalSalary> HistoricalSalaries { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<WorkArea> WorkAreas { get; set; }

        public DbSet<WorkAreaConnection> WorkAreaConnections { get; set; }

        public DbSet<WorkAreaMovement> WorkAreaMovements { get; set; }

        public DbSet<WorkUnit> WorkUnits { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<ProductPicture> ProductPictures { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}