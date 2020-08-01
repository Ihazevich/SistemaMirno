namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialEmployeeRework : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreaConnections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromAreaId = c.Int(nullable: false),
                        ToAreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Colors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        MaterialId = c.Int(nullable: false),
                        ColorId = c.Int(nullable: false),
                        WorkOrderId = c.Int(nullable: false),
                        ProductionAreaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Colors", t => t.ColorId)
                .ForeignKey("dbo.Materials", t => t.MaterialId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId)
                .ForeignKey("dbo.WorkAreas", t => t.ProductionAreaId)
                .Index(t => t.ProductId)
                .Index(t => t.MaterialId)
                .Index(t => t.ColorId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.ProductionAreaId);
            
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
                "dbo.WorkAreas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Order = c.Int(nullable: false),
                        WorkAreaResponsibleRoleId = c.Int(nullable: false),
                        WorkAreaSupervisorRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmployeeRoles", t => t.WorkAreaResponsibleRoleId)
                .ForeignKey("dbo.EmployeeRoles", t => t.WorkAreaSupervisorRoleId)
                .Index(t => t.WorkAreaResponsibleRoleId)
                .Index(t => t.WorkAreaSupervisorRoleId);
            
            CreateTable(
                "dbo.EmployeeRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmployeeRoles", t => t.EmployeeRoleId)
                .Index(t => t.EmployeeRoleId);
            
            CreateTable(
                "dbo.WorkOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        FinishTime = c.DateTime(nullable: false),
                        ProductionAreaId = c.Int(nullable: false),
                        ResponsibleEmployeeId = c.Int(nullable: false),
                        SupervisorEmployeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkAreas", t => t.ProductionAreaId)
                .ForeignKey("dbo.Employees", t => t.ResponsibleEmployeeId)
                .ForeignKey("dbo.Employees", t => t.SupervisorEmployeeID)
                .Index(t => t.ProductionAreaId)
                .Index(t => t.ResponsibleEmployeeId)
                .Index(t => t.SupervisorEmployeeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkUnits", "ProductionAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkAreas", "WorkAreaSupervisorRoleId", "dbo.EmployeeRoles");
            DropForeignKey("dbo.WorkAreas", "WorkAreaResponsibleRoleId", "dbo.EmployeeRoles");
            DropForeignKey("dbo.Employees", "EmployeeRoleId", "dbo.EmployeeRoles");
            DropForeignKey("dbo.WorkOrders", "SupervisorEmployeeID", "dbo.Employees");
            DropForeignKey("dbo.WorkOrders", "ResponsibleEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.WorkUnits", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkOrders", "ProductionAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkUnits", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.WorkUnits", "MaterialId", "dbo.Materials");
            DropForeignKey("dbo.WorkUnits", "ColorId", "dbo.Colors");
            DropIndex("dbo.WorkOrders", new[] { "SupervisorEmployeeID" });
            DropIndex("dbo.WorkOrders", new[] { "ResponsibleEmployeeId" });
            DropIndex("dbo.WorkOrders", new[] { "ProductionAreaId" });
            DropIndex("dbo.Employees", new[] { "EmployeeRoleId" });
            DropIndex("dbo.WorkAreas", new[] { "WorkAreaSupervisorRoleId" });
            DropIndex("dbo.WorkAreas", new[] { "WorkAreaResponsibleRoleId" });
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropIndex("dbo.WorkUnits", new[] { "ProductionAreaId" });
            DropIndex("dbo.WorkUnits", new[] { "WorkOrderId" });
            DropIndex("dbo.WorkUnits", new[] { "ColorId" });
            DropIndex("dbo.WorkUnits", new[] { "MaterialId" });
            DropIndex("dbo.WorkUnits", new[] { "ProductId" });
            DropTable("dbo.WorkOrders");
            DropTable("dbo.Employees");
            DropTable("dbo.EmployeeRoles");
            DropTable("dbo.WorkAreas");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.Materials");
            DropTable("dbo.WorkUnits");
            DropTable("dbo.Colors");
            DropTable("dbo.AreaConnections");
        }
    }
}
