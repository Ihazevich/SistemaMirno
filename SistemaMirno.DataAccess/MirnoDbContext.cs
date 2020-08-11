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
        }

        /// <summary>
        /// Gets or sets the database set representing the users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the work areas' connections.
        /// </summary>
        public DbSet<AreaConnection> AreaConnections { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the product's colors.
        /// </summary>
        public DbSet<Color> Colors { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the product's materials.
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the products.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the work areas.
        /// </summary>
        public DbSet<WorkArea> WorkAreas { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the product categories.
        /// </summary>
        public DbSet<ProductCategory> ProductCategories { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the employees.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the employees' roles.
        /// </summary>
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the work orders.
        /// </summary>
        public DbSet<WorkOrder> WorkOrders { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the work units.
        /// </summary>
        public DbSet<WorkUnit> WorkUnits { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the work order units.
        /// </summary>
        public DbSet<WorkOrderUnit> WorkOrderUnits { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}