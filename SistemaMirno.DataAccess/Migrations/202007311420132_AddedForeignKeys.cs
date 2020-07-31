namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKeys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Responsibles", "ProductionAreaId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkUnits", "ProductId");
            CreateIndex("dbo.WorkUnits", "MaterialId");
            CreateIndex("dbo.WorkUnits", "ColorId");
            CreateIndex("dbo.WorkUnits", "WorkOrderId");
            CreateIndex("dbo.WorkUnits", "ProductionAreaId");
            CreateIndex("dbo.Products", "ProductCategoryId");
            CreateIndex("dbo.Responsibles", "ProductionAreaId");
            CreateIndex("dbo.WorkOrders", "ProductionAreaId");
            CreateIndex("dbo.WorkOrders", "ResponsibleId");
            CreateIndex("dbo.WorkOrders", "SupervisorID");
            AddForeignKey("dbo.WorkUnits", "ColorId", "dbo.Colors", "Id");
            AddForeignKey("dbo.WorkUnits", "MaterialId", "dbo.Materials", "Id");
            AddForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories", "Id");
            AddForeignKey("dbo.WorkUnits", "ProductId", "dbo.Products", "Id");
            AddForeignKey("dbo.Responsibles", "ProductionAreaId", "dbo.ProductionAreas", "Id");
            AddForeignKey("dbo.WorkOrders", "ProductionAreaId", "dbo.ProductionAreas", "Id");
            AddForeignKey("dbo.WorkOrders", "ResponsibleId", "dbo.Responsibles", "Id");
            AddForeignKey("dbo.WorkOrders", "SupervisorID", "dbo.Supervisors", "Id");
            AddForeignKey("dbo.WorkUnits", "ProductionAreaId", "dbo.ProductionAreas", "Id");
            AddForeignKey("dbo.WorkUnits", "WorkOrderId", "dbo.WorkOrders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkUnits", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkUnits", "ProductionAreaId", "dbo.ProductionAreas");
            DropForeignKey("dbo.WorkOrders", "SupervisorID", "dbo.Supervisors");
            DropForeignKey("dbo.WorkOrders", "ResponsibleId", "dbo.Responsibles");
            DropForeignKey("dbo.WorkOrders", "ProductionAreaId", "dbo.ProductionAreas");
            DropForeignKey("dbo.Responsibles", "ProductionAreaId", "dbo.ProductionAreas");
            DropForeignKey("dbo.WorkUnits", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.WorkUnits", "MaterialId", "dbo.Materials");
            DropForeignKey("dbo.WorkUnits", "ColorId", "dbo.Colors");
            DropIndex("dbo.WorkOrders", new[] { "SupervisorID" });
            DropIndex("dbo.WorkOrders", new[] { "ResponsibleId" });
            DropIndex("dbo.WorkOrders", new[] { "ProductionAreaId" });
            DropIndex("dbo.Responsibles", new[] { "ProductionAreaId" });
            DropIndex("dbo.Products", new[] { "ProductCategoryId" });
            DropIndex("dbo.WorkUnits", new[] { "ProductionAreaId" });
            DropIndex("dbo.WorkUnits", new[] { "WorkOrderId" });
            DropIndex("dbo.WorkUnits", new[] { "ColorId" });
            DropIndex("dbo.WorkUnits", new[] { "MaterialId" });
            DropIndex("dbo.WorkUnits", new[] { "ProductId" });
            DropColumn("dbo.Responsibles", "ProductionAreaId");
        }
    }
}
