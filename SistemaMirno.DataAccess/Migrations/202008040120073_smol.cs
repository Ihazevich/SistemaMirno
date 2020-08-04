namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smol : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkOrders", new[] { "ResponsibleEmployeeId" });
            DropIndex("dbo.WorkOrders", new[] { "SupervisorEmployeeID" });
            AlterColumn("dbo.WorkOrders", "ResponsibleEmployeeId", c => c.Int());
            AlterColumn("dbo.WorkOrders", "SupervisorEmployeeID", c => c.Int());
            CreateIndex("dbo.WorkOrders", "ResponsibleEmployeeId");
            CreateIndex("dbo.WorkOrders", "SupervisorEmployeeID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkOrders", new[] { "SupervisorEmployeeID" });
            DropIndex("dbo.WorkOrders", new[] { "ResponsibleEmployeeId" });
            AlterColumn("dbo.WorkOrders", "SupervisorEmployeeID", c => c.Int(nullable: false));
            AlterColumn("dbo.WorkOrders", "ResponsibleEmployeeId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkOrders", "SupervisorEmployeeID");
            CreateIndex("dbo.WorkOrders", "ResponsibleEmployeeId");
        }
    }
}
