namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableLeavingWorkAreaId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkOrders", new[] { "LeavingWorkAreaId" });
            AlterColumn("dbo.WorkOrders", "LeavingWorkAreaId", c => c.Int());
            CreateIndex("dbo.WorkOrders", "LeavingWorkAreaId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkOrders", new[] { "LeavingWorkAreaId" });
            AlterColumn("dbo.WorkOrders", "LeavingWorkAreaId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkOrders", "LeavingWorkAreaId");
        }
    }
}
