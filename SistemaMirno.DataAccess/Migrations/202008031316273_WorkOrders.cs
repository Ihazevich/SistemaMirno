namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkOrders : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.WorkOrders", name: "ProductionAreaId", newName: "WorkAreaId");
            RenameIndex(table: "dbo.WorkOrders", name: "IX_ProductionAreaId", newName: "IX_WorkAreaId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.WorkOrders", name: "IX_WorkAreaId", newName: "IX_ProductionAreaId");
            RenameColumn(table: "dbo.WorkOrders", name: "WorkAreaId", newName: "ProductionAreaId");
        }
    }
}
