namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectionsRefactor : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AreaConnections", name: "ToWorkAreaId", newName: "ConnectedWorkAreaId");
            RenameColumn(table: "dbo.AreaConnections", name: "FromWorkAreaId", newName: "WorkAreaId");
            RenameIndex(table: "dbo.AreaConnections", name: "IX_FromWorkAreaId", newName: "IX_WorkAreaId");
            RenameIndex(table: "dbo.AreaConnections", name: "IX_ToWorkAreaId", newName: "IX_ConnectedWorkAreaId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AreaConnections", name: "IX_ConnectedWorkAreaId", newName: "IX_ToWorkAreaId");
            RenameIndex(table: "dbo.AreaConnections", name: "IX_WorkAreaId", newName: "IX_FromWorkAreaId");
            RenameColumn(table: "dbo.AreaConnections", name: "WorkAreaId", newName: "FromWorkAreaId");
            RenameColumn(table: "dbo.AreaConnections", name: "ConnectedWorkAreaId", newName: "ToWorkAreaId");
        }
    }
}
