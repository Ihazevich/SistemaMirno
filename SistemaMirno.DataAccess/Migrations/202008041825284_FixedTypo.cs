namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedTypo : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkOrders", new[] { "SupervisorEmployeeID" });
            CreateIndex("dbo.WorkOrders", "SupervisorEmployeeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkOrders", new[] { "SupervisorEmployeeId" });
            CreateIndex("dbo.WorkOrders", "SupervisorEmployeeID");
        }
    }
}
