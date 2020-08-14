namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reportsbool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkAreas", "ReportsInProcess", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkAreas", "ReportsInProcess");
        }
    }
}
