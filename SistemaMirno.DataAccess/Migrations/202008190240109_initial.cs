namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreaConnections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkAreaId = c.Int(nullable: false),
                        ConnectedWorkAreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkAreas", t => t.WorkAreaId)
                .ForeignKey("dbo.WorkAreas", t => t.ConnectedWorkAreaId)
                .Index(t => t.WorkAreaId)
                .Index(t => t.ConnectedWorkAreaId);
            
            CreateTable(
                "dbo.WorkAreas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Order = c.Int(nullable: false),
                        WorkAreaResponsibleRoleId = c.Int(),
                        WorkAreaSupervisorRoleId = c.Int(),
                        ReportsInProcess = c.Boolean(nullable: false),
                        BranchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmployeeRoles", t => t.WorkAreaResponsibleRoleId)
                .ForeignKey("dbo.EmployeeRoles", t => t.WorkAreaSupervisorRoleId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.WorkAreaResponsibleRoleId)
                .Index(t => t.WorkAreaSupervisorRoleId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.WorkOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        FinishTime = c.DateTime(),
                        OriginWorkAreaId = c.Int(),
                        DestinationWorkAreaId = c.Int(nullable: false),
                        ResponsibleEmployeeId = c.Int(),
                        SupervisorEmployeeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.ResponsibleEmployeeId)
                .ForeignKey("dbo.Employees", t => t.SupervisorEmployeeId)
                .ForeignKey("dbo.WorkAreas", t => t.DestinationWorkAreaId)
                .ForeignKey("dbo.WorkAreas", t => t.OriginWorkAreaId)
                .Index(t => t.OriginWorkAreaId)
                .Index(t => t.DestinationWorkAreaId)
                .Index(t => t.ResponsibleEmployeeId)
                .Index(t => t.SupervisorEmployeeId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 30),
                        LastName = c.String(nullable: false, maxLength: 30),
                        DocumentNumber = c.Int(nullable: false),
                        HiredDate = c.DateTime(nullable: false),
                        EmployeeRoleId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmployeeRoles", t => t.EmployeeRoleId)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .Index(t => t.EmployeeRoleId)
                .Index(t => t.BranchId);
            
            CreateTable(
                "dbo.WorkOrderUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        WorkUnitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId)
                .ForeignKey("dbo.WorkUnits", t => t.WorkUnitId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.WorkUnitId);
            
            CreateTable(
                "dbo.WorkUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        ColorId = c.Int(nullable: false),
                        WorkAreaId = c.Int(nullable: false),
                        RequisitionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId)
                .ForeignKey("dbo.Materials", t => t.MaterialId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Requisitions", t => t.RequisitionId)
                .ForeignKey("dbo.WorkAreas", t => t.WorkAreaId)
                .Index(t => t.ProductId)
                .Index(t => t.MaterialId)
                .Index(t => t.ColorId)
                .Index(t => t.WorkAreaId)
                .Index(t => t.RequisitionId);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Name = c.String(nullable: false, maxLength: 100),
                        ProductCategoryId = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        WholesalePrice = c.Int(nullable: false),
                        ProductionPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategoryId)
                .Index(t => t.ProductCategoryId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requisitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestedDate = c.DateTime(nullable: false),
                        FullfilledDate = c.DateTime(nullable: false),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        RUC = c.String(),
                        PhoneNumber = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Department = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BranchId = c.Int(nullable: false),
                        RequisitionId = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        EstimatedDeliveryDate = c.DateTime(nullable: false),
                        Total = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                        IVA = c.Int(nullable: false),
                        SaleTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.BranchId)
                .ForeignKey("dbo.Requisitions", t => t.RequisitionId)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .ForeignKey("dbo.SaleTypes", t => t.SaleTypeId)
                .Index(t => t.BranchId)
                .Index(t => t.RequisitionId)
                .Index(t => t.ResponsibleId)
                .Index(t => t.SaleTypeId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Ammount = c.Int(nullable: false),
                        PaymentTypeId = c.Int(nullable: false),
                        SaleId = c.Int(nullable: false),
                        PaymentMethod_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentMethods", t => t.PaymentMethod_Id)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .Index(t => t.SaleId)
                .Index(t => t.PaymentMethod_Id);
            
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AccessLevel = c.Int(nullable: false),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkAreas", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Sales", "SaleTypeId", "dbo.SaleTypes");
            DropForeignKey("dbo.Sales", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.Sales", "RequisitionId", "dbo.Requisitions");
            DropForeignKey("dbo.Payments", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Payments", "PaymentMethod_Id", "dbo.PaymentMethods");
            DropForeignKey("dbo.Sales", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Employees", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.WorkAreas", "WorkAreaSupervisorRoleId", "dbo.EmployeeRoles");
            DropForeignKey("dbo.WorkAreas", "WorkAreaResponsibleRoleId", "dbo.EmployeeRoles");
            DropForeignKey("dbo.Employees", "EmployeeRoleId", "dbo.EmployeeRoles");
            DropForeignKey("dbo.WorkOrders", "OriginWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkOrders", "DestinationWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkOrderUnits", "WorkUnitId", "dbo.WorkUnits");
            DropForeignKey("dbo.WorkUnits", "WorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkUnits", "RequisitionId", "dbo.Requisitions");
            DropForeignKey("dbo.Requisitions", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.WorkUnits", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.WorkUnits", "MaterialId", "dbo.Materials");
            DropForeignKey("dbo.WorkUnits", "ColorId", "dbo.Colors");
            DropForeignKey("dbo.WorkOrderUnits", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkOrders", "SupervisorEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.WorkOrders", "ResponsibleEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.AreaConnections", "ConnectedWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.AreaConnections", "WorkAreaId", "dbo.WorkAreas");
            DropIndex("dbo.Payments", new[] { "PaymentMethod_Id" });
            DropIndex("dbo.Payments", new[] { "SaleId" });
            DropIndex("dbo.Sales", new[] { "SaleTypeId" });
            DropIndex("dbo.Sales", new[] { "ResponsibleId" });
            DropIndex("dbo.Sales", new[] { "RequisitionId" });
            DropIndex("dbo.Sales", new[] { "BranchId" });
            DropIndex("dbo.Requisitions", new[] { "ClientId" });
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropIndex("dbo.WorkUnits", new[] { "RequisitionId" });
            DropIndex("dbo.WorkUnits", new[] { "WorkAreaId" });
            DropIndex("dbo.WorkUnits", new[] { "ColorId" });
            DropIndex("dbo.WorkUnits", new[] { "MaterialId" });
            DropIndex("dbo.WorkUnits", new[] { "ProductId" });
            DropIndex("dbo.WorkOrderUnits", new[] { "WorkUnitId" });
            DropIndex("dbo.WorkOrderUnits", new[] { "WorkOrderId" });
            DropIndex("dbo.Employees", new[] { "BranchId" });
            DropIndex("dbo.Employees", new[] { "EmployeeRoleId" });
            DropIndex("dbo.WorkOrders", new[] { "SupervisorEmployeeId" });
            DropIndex("dbo.WorkOrders", new[] { "ResponsibleEmployeeId" });
            DropIndex("dbo.WorkOrders", new[] { "DestinationWorkAreaId" });
            DropIndex("dbo.WorkOrders", new[] { "OriginWorkAreaId" });
            DropIndex("dbo.WorkAreas", new[] { "BranchId" });
            DropIndex("dbo.WorkAreas", new[] { "WorkAreaSupervisorRoleId" });
            DropIndex("dbo.WorkAreas", new[] { "WorkAreaResponsibleRoleId" });
            DropIndex("dbo.AreaConnections", new[] { "ConnectedWorkAreaId" });
            DropIndex("dbo.AreaConnections", new[] { "WorkAreaId" });
            DropTable("dbo.Users");
            DropTable("dbo.SaleTypes");
            DropTable("dbo.PaymentMethods");
            DropTable("dbo.Payments");
            DropTable("dbo.Sales");
            DropTable("dbo.Branches");
            DropTable("dbo.EmployeeRoles");
            DropTable("dbo.Clients");
            DropTable("dbo.Requisitions");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Materials");
            DropTable("dbo.Colors");
            DropTable("dbo.WorkUnits");
            DropTable("dbo.WorkOrderUnits");
            DropTable("dbo.Employees");
            DropTable("dbo.WorkOrders");
            DropTable("dbo.WorkAreas");
            DropTable("dbo.AreaConnections");
        }
    }
}
