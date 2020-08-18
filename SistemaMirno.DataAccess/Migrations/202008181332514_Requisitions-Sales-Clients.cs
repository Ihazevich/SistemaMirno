namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequisitionsSalesClients : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requisitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestDate = c.DateTime(nullable: false),
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
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Ammount = c.Int(nullable: false),
                        PaymentTypeId = c.Int(nullable: false),
                        SaleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentTypes", t => t.PaymentTypeId)
                .ForeignKey("dbo.Sales", t => t.SaleId)
                .Index(t => t.PaymentTypeId)
                .Index(t => t.SaleId);
            
            CreateTable(
                "dbo.PaymentTypes",
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
                        RequisitionId = c.Int(nullable: false),
                        ResponsibleId = c.Int(nullable: false),
                        DeliveryDate = c.DateTime(nullable: false),
                        EstimatedDeliveryDate = c.DateTime(nullable: false),
                        Discount = c.Int(nullable: false),
                        IVA = c.Int(nullable: false),
                        SaleType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requisitions", t => t.RequisitionId)
                .ForeignKey("dbo.Employees", t => t.ResponsibleId)
                .Index(t => t.RequisitionId)
                .Index(t => t.ResponsibleId);
            
            AddColumn("dbo.WorkUnits", "RequisitionId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkUnits", "RequisitionId");
            AddForeignKey("dbo.WorkUnits", "RequisitionId", "dbo.Requisitions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sales", "ResponsibleId", "dbo.Employees");
            DropForeignKey("dbo.Sales", "RequisitionId", "dbo.Requisitions");
            DropForeignKey("dbo.Payments", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.Payments", "PaymentTypeId", "dbo.PaymentTypes");
            DropForeignKey("dbo.WorkUnits", "RequisitionId", "dbo.Requisitions");
            DropForeignKey("dbo.Requisitions", "ClientId", "dbo.Clients");
            DropIndex("dbo.Sales", new[] { "ResponsibleId" });
            DropIndex("dbo.Sales", new[] { "RequisitionId" });
            DropIndex("dbo.Payments", new[] { "SaleId" });
            DropIndex("dbo.Payments", new[] { "PaymentTypeId" });
            DropIndex("dbo.Requisitions", new[] { "ClientId" });
            DropIndex("dbo.WorkUnits", new[] { "RequisitionId" });
            DropColumn("dbo.WorkUnits", "RequisitionId");
            DropTable("dbo.Sales");
            DropTable("dbo.PaymentTypes");
            DropTable("dbo.Payments");
            DropTable("dbo.Clients");
            DropTable("dbo.Requisitions");
        }
    }
}
