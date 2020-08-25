using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.ModelConfiguration.Conventions;
using SistemaMirno.Model;

namespace SistemaMirno.DataAccess
{
    /// <summary>
    /// A class representing the database context of the system.
    /// </summary>
    public class MirnoDbContext : DbContext
    {
        static readonly DatabaseLogger DatabaseLogger = new DatabaseLogger("LogFile.txt", true);

        /// <summary>
        /// Initializes a new instance of the <see cref="MirnoDbContext"/> class.
        /// </summary>
        public MirnoDbContext() : base("LaptopMirnoDb")
        {
            DatabaseLogger.StartLogging();
            DbInterception.Add(DatabaseLogger);
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

        public DbSet<Color> Colors { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductPicture> ProductPictures { get; set; }

        public DbSet<ProductSupply> ProductSupplies { get; set; }

        public DbSet<ProductPart> ProductParts { get; set; }

        public DbSet<Supply> Supplies { get; set; }

        public DbSet<SupplyCategory> SupplyCategories { get; set; }

        public DbSet<SupplyMovement> SupplyMovements { get; set; }

        public DbSet<Hardware> Hardwares { get; set; }

        public DbSet<HardwareCategory> HardwareCategories { get; set; }

        public DbSet<HardwareMaintenanceOrder> HardwareMaintenanceOrders { get; set; }

        public DbSet<Technician> Technicians { get; set; }

        public DbSet<Provider> Providers { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<WorkOrder> WorkOrders { get; set; }

        public DbSet<WorkOrderUnit> WorkOrderUnits { get; set; }

        public DbSet<Requisition> Requisitions { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<SaleCollection> SaleCollections { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientCommunication> ClientCommunications { get; set; }

        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<DeliveryUnit> DeliveryUnits { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<VehicleMaintenanceOrder> VehicleMaintenanceOrders { get; set; }

        public DbSet<IntermediateProduct> IntermediateProducts { get; set; }

        public DbSet<IntermediateWorkUnit> IntermediateWorkUnits { get; set; }

        public DbSet<IntermediateWorkOrder> IntermediateWorkOrders { get; set; }

        public DbSet<IntermediateWorkUnitMovement> IntermediateWorkUnitMovements { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceUnit> InvoiceUnits { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<BankAccountMovement> BankAccountMovements { get; set; }

        public DbSet<DatedCheck> DatedChecks { get; set; }

        public DbSet<BuyOrder> BuyOrders { get; set; }

        public DbSet<BuyOrderUnit> BuyOrderUnits { get; set; }

        public DbSet<CreditCard> CreditCards { get; set; }

        public DbSet<ProviderPayment> ProviderPayments { get; set; }

        public DbSet<CashMovement> CashMovements { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        protected override void Dispose(bool disposing)
        {
            DbInterception.Remove(DatabaseLogger);
            DatabaseLogger.StopLogging();
            base.Dispose(disposing);
        }
    }
}