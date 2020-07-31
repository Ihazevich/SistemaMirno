namespace SistemaMirno.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Laptop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AreaConnections",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FromAreaId = c.Int(nullable: false),
                    ToAreaId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.WorkOrders",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    StartTime = c.DateTime(nullable: false),
                    FinishTime = c.DateTime(nullable: false),
                    ProductionAreaId = c.Int(nullable: false),
                    ResponsibleId = c.Int(nullable: false),
                    SupervisorID = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.Products", "Code", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Products", "ProductionPrice", c => c.Int(nullable: false));
            AddColumn("dbo.WorkUnits", "WorkOrderId", c => c.Int(nullable: false));
            DropColumn("dbo.WorkUnits", "ResponsibleId");
            DropColumn("dbo.WorkUnits", "SupervisorId");
        }

        public override void Down()
        {
            AddColumn("dbo.WorkUnits", "SupervisorId", c => c.Int(nullable: false));
            AddColumn("dbo.WorkUnits", "ResponsibleId", c => c.Int(nullable: false));
            DropColumn("dbo.WorkUnits", "WorkOrderId");
            DropColumn("dbo.Products", "ProductionPrice");
            DropColumn("dbo.Products", "Code");
            DropTable("dbo.WorkOrders");
            DropTable("dbo.AreaConnections");
        }
    }
}