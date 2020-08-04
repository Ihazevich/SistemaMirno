namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anothertypo : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.WorkUnits", name: "ProductionAreaId", newName: "WorkAreaId");
            RenameIndex(table: "dbo.WorkUnits", name: "IX_ProductionAreaId", newName: "IX_WorkAreaId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.WorkUnits", name: "IX_WorkAreaId", newName: "IX_ProductionAreaId");
            RenameColumn(table: "dbo.WorkUnits", name: "WorkAreaId", newName: "ProductionAreaId");
        }
    }
}
