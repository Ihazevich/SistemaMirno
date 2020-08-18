namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkUnits", new[] { "RequisitionId" });
            AlterColumn("dbo.WorkUnits", "RequisitionId", c => c.Int());
            CreateIndex("dbo.WorkUnits", "RequisitionId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkUnits", new[] { "RequisitionId" });
            AlterColumn("dbo.WorkUnits", "RequisitionId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkUnits", "RequisitionId");
        }
    }
}
