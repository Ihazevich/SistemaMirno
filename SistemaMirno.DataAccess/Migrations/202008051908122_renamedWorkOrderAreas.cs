namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renamedWorkOrderAreas : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.WorkOrders", name: "EnteringWorkAreaId", newName: "DestinationWorkAreaId");
            RenameColumn(table: "dbo.WorkOrders", name: "LeavingWorkAreaId", newName: "OriginWorkAreaId");
            RenameIndex(table: "dbo.WorkOrders", name: "IX_LeavingWorkAreaId", newName: "IX_OriginWorkAreaId");
            RenameIndex(table: "dbo.WorkOrders", name: "IX_EnteringWorkAreaId", newName: "IX_DestinationWorkAreaId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.WorkOrders", name: "IX_DestinationWorkAreaId", newName: "IX_EnteringWorkAreaId");
            RenameIndex(table: "dbo.WorkOrders", name: "IX_OriginWorkAreaId", newName: "IX_LeavingWorkAreaId");
            RenameColumn(table: "dbo.WorkOrders", name: "OriginWorkAreaId", newName: "LeavingWorkAreaId");
            RenameColumn(table: "dbo.WorkOrders", name: "DestinationWorkAreaId", newName: "EnteringWorkAreaId");
        }
    }
}
