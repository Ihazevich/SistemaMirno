namespace SistemaMirno.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Colors",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 20),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Materials",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 20),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProductCategories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 20),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProductionAreas",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 20),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Products",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100),
                    ProductCategoryId = c.Int(nullable: false),
                    Price = c.Int(nullable: false),
                    WholesalePrice = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Responsibles",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false, maxLength: 30),
                    LastName = c.String(nullable: false, maxLength: 30),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Supervisors",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false, maxLength: 30),
                    LastName = c.String(nullable: false, maxLength: 30),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.WorkUnits",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ProductId = c.Int(nullable: false),
                    MaterialId = c.Int(nullable: false),
                    ColorId = c.Int(nullable: false),
                    ProductionAreaId = c.Int(nullable: false),
                    ResponsibleId = c.Int(nullable: false),
                    SupervisorId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.WorkUnits");
            DropTable("dbo.Supervisors");
            DropTable("dbo.Responsibles");
            DropTable("dbo.Products");
            DropTable("dbo.ProductionAreas");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Materials");
            DropTable("dbo.Colors");
        }
    }
}