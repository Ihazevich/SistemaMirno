// <copyright file="MirnoDbContext.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

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
        private static readonly DatabaseLogger DatabaseLogger = new DatabaseLogger("LogFile.txt", true);

        /// <summary>
        /// Initializes a new instance of the <see cref="MirnoDbContext"/> class.
        /// </summary>
        public MirnoDbContext()
            : base("DesktopMirnoDb")
        {
            DatabaseLogger.StartLogging();
            DbInterception.Add(DatabaseLogger);
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="User"/> entities in the context.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Employee"/> entities in the context.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Role"/> entities in the context.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Assistance"/> entities in the context.
        /// </summary>
        public DbSet<Assistance> Assistances { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="SalaryPayment"/> entities in the context.
        /// </summary>
        public DbSet<SalaryPayment> SalaryPayments { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="SalaryDiscount"/> entities in the context.
        /// </summary>
        public DbSet<SalaryDiscount> SalaryDiscounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="HistoricalSalary"/> entities in the context.
        /// </summary>
        public DbSet<HistoricalSalary> HistoricalSalaries { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Branch"/> entities in the context.
        /// </summary>
        public DbSet<Branch> Branches { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="WorkArea"/> entities in the context.
        /// </summary>
        public DbSet<WorkArea> WorkAreas { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="WorkAreaConnection"/> entities in the context.
        /// </summary>
        public DbSet<WorkAreaConnection> WorkAreaConnections { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="WorkAreaMovement"/> entities in the context.
        /// </summary>
        public DbSet<WorkAreaMovement> WorkAreaMovements { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="WorkUnit"/> entities in the context.
        /// </summary>
        public DbSet<WorkUnit> WorkUnits { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Color"/> entities in the context.
        /// </summary>
        public DbSet<Color> Colors { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Material"/> entities in the context.
        /// </summary>
        public DbSet<Material> Materials { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Product"/> entities in the context.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="ProductCategory"/> entities in the context.
        /// </summary>
        public DbSet<ProductCategory> ProductCategories { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="ProductPicture"/> entities in the context.
        /// </summary>
        public DbSet<ProductPicture> ProductPictures { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="ProductSupply"/> entities in the context.
        /// </summary>
        public DbSet<ProductSupply> ProductSupplies { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="ProductPart"/> entities in the context.
        /// </summary>
        public DbSet<ProductPart> ProductParts { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Supply"/> entities in the context.
        /// </summary>
        public DbSet<Supply> Supplies { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="SupplyCategory"/> entities in the context.
        /// </summary>
        public DbSet<SupplyCategory> SupplyCategories { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="SupplyMovement"/> entities in the context.
        /// </summary>
        public DbSet<SupplyMovement> SupplyMovements { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Hardware"/> entities in the context.
        /// </summary>
        public DbSet<Hardware> Hardwares { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="HardwareCategory"/> entities in the context.
        /// </summary>
        public DbSet<HardwareCategory> HardwareCategories { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="HardwareMaintenanceOrder"/> entities in the context.
        /// </summary>
        public DbSet<HardwareMaintenanceOrder> HardwareMaintenanceOrders { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Technician"/> entities in the context.
        /// </summary>
        public DbSet<Technician> Technicians { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Provider"/> entities in the context.
        /// </summary>
        public DbSet<Provider> Providers { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Purchase"/> entities in the context.
        /// </summary>
        public DbSet<Purchase> Purchases { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="WorkOrder"/> entities in the context.
        /// </summary>
        public DbSet<WorkOrder> WorkOrders { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="WorkOrderUnit"/> entities in the context.
        /// </summary>
        public DbSet<WorkOrderUnit> WorkOrderUnits { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Requisition"/> entities in the context.
        /// </summary>
        public DbSet<Requisition> Requisitions { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Sale"/> entities in the context.
        /// </summary>
        public DbSet<Sale> Sales { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="SaleCollection"/> entities in the context.
        /// </summary>
        public DbSet<SaleCollection> SaleCollections { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Client"/> entities in the context.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="ClientCommunication"/> entities in the context.
        /// </summary>
        public DbSet<ClientCommunication> ClientCommunications { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="DeliveryOrder"/> entities in the context.
        /// </summary>
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Delivery"/> entities in the context.
        /// </summary>
        public DbSet<Delivery> Deliveries { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="DeliveryUnit"/> entities in the context.
        /// </summary>
        public DbSet<DeliveryUnit> DeliveryUnits { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Vehicle"/> entities in the context.
        /// </summary>
        public DbSet<Vehicle> Vehicles { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="VehicleMaintenanceOrder"/> entities in the context.
        /// </summary>
        public DbSet<VehicleMaintenanceOrder> VehicleMaintenanceOrders { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="IntermediateProduct"/> entities in the context.
        /// </summary>
        public DbSet<IntermediateProduct> IntermediateProducts { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="IntermediateWorkUnit"/> entities in the context.
        /// </summary>
        public DbSet<IntermediateWorkUnit> IntermediateWorkUnits { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="IntermediateWorkOrder"/> entities in the context.
        /// </summary>
        public DbSet<IntermediateWorkOrder> IntermediateWorkOrders { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="IntermediateWorkUnitMovement"/> entities in the context.
        /// </summary>
        public DbSet<IntermediateWorkUnitMovement> IntermediateWorkUnitMovements { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Invoice"/> entities in the context.
        /// </summary>
        public DbSet<Invoice> Invoices { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="InvoiceUnit"/> entities in the context.
        /// </summary>
        public DbSet<InvoiceUnit> InvoiceUnits { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Bank"/> entities in the context.
        /// </summary>
        public DbSet<Bank> Banks { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="BankAccount"/> entities in the context.
        /// </summary>
        public DbSet<BankAccount> BankAccounts { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="BankAccountMovement"/> entities in the context.
        /// </summary>
        public DbSet<BankAccountMovement> BankAccountMovements { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="DatedCheck"/> entities in the context.
        /// </summary>
        public DbSet<DatedCheck> DatedChecks { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="BuyOrder"/> entities in the context.
        /// </summary>
        public DbSet<BuyOrder> BuyOrders { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="BuyOrderUnit"/> entities in the context.
        /// </summary>
        public DbSet<BuyOrderUnit> BuyOrderUnits { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="CreditCard"/> entities in the context.
        /// </summary>
        public DbSet<CreditCard> CreditCards { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="ProviderPayment"/> entities in the context.
        /// </summary>
        public DbSet<ProviderPayment> ProviderPayments { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="CashMovement"/> entities in the context.
        /// </summary>
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