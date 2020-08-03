namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AreaConnections : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AreaConnections", "FromWorkAreaId", c => c.Int(nullable: false));
            AddColumn("dbo.AreaConnections", "ToWorkAreaId", c => c.Int(nullable: false));
            CreateIndex("dbo.AreaConnections", "FromWorkAreaId");
            CreateIndex("dbo.AreaConnections", "ToWorkAreaId");
            AddForeignKey("dbo.AreaConnections", "ToWorkAreaId", "dbo.WorkAreas", "Id");
            AddForeignKey("dbo.AreaConnections", "FromWorkAreaId", "dbo.WorkAreas", "Id");
            DropColumn("dbo.AreaConnections", "FromAreaId");
            DropColumn("dbo.AreaConnections", "ToAreaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AreaConnections", "ToAreaId", c => c.Int(nullable: false));
            AddColumn("dbo.AreaConnections", "FromAreaId", c => c.Int(nullable: false));
            DropForeignKey("dbo.AreaConnections", "FromWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.AreaConnections", "ToWorkAreaId", "dbo.WorkAreas");
            DropIndex("dbo.AreaConnections", new[] { "ToWorkAreaId" });
            DropIndex("dbo.AreaConnections", new[] { "FromWorkAreaId" });
            DropColumn("dbo.AreaConnections", "ToWorkAreaId");
            DropColumn("dbo.AreaConnections", "FromWorkAreaId");
        }
    }
}
