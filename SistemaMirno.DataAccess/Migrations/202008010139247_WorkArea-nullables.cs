namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkAreanullables : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkAreas", new[] { "WorkAreaResponsibleRoleId" });
            DropIndex("dbo.WorkAreas", new[] { "WorkAreaSupervisorRoleId" });
            AlterColumn("dbo.WorkAreas", "WorkAreaResponsibleRoleId", c => c.Int());
            AlterColumn("dbo.WorkAreas", "WorkAreaSupervisorRoleId", c => c.Int());
            CreateIndex("dbo.WorkAreas", "WorkAreaResponsibleRoleId");
            CreateIndex("dbo.WorkAreas", "WorkAreaSupervisorRoleId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkAreas", new[] { "WorkAreaSupervisorRoleId" });
            DropIndex("dbo.WorkAreas", new[] { "WorkAreaResponsibleRoleId" });
            AlterColumn("dbo.WorkAreas", "WorkAreaSupervisorRoleId", c => c.Int(nullable: false));
            AlterColumn("dbo.WorkAreas", "WorkAreaResponsibleRoleId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkAreas", "WorkAreaSupervisorRoleId");
            CreateIndex("dbo.WorkAreas", "WorkAreaResponsibleRoleId");
        }
    }
}
