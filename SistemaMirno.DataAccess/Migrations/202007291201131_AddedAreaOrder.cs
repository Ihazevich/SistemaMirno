namespace SistemaMirno.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedAreaOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductionAreas", "Order", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.ProductionAreas", "Order");
        }
    }
}