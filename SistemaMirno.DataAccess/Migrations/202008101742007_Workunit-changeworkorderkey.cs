namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Workunitchangeworkorderkey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkUnits", "WorkOrderId", "dbo.WorkOrders");
            DropIndex("dbo.WorkUnits", new[] { "WorkOrderId" });
            DropColumn("dbo.WorkUnits", "WorkOrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkUnits", "WorkOrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkUnits", "WorkOrderId");
            AddForeignKey("dbo.WorkUnits", "WorkOrderId", "dbo.WorkOrders", "Id");
        }
    }
}
