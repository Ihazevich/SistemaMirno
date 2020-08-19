namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saletype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Requisitions", "RequestedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sales", "Total", c => c.Int(nullable: false));
            AddColumn("dbo.Sales", "SaleTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Sales", "SaleTypeId");
            AddForeignKey("dbo.Sales", "SaleTypeId", "dbo.SaleTypes", "Id");
            DropColumn("dbo.Requisitions", "RequestDate");
            DropColumn("dbo.Sales", "SaleType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sales", "SaleType", c => c.Int(nullable: false));
            AddColumn("dbo.Requisitions", "RequestDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Sales", "SaleTypeId", "dbo.SaleTypes");
            DropIndex("dbo.Sales", new[] { "SaleTypeId" });
            DropColumn("dbo.Sales", "SaleTypeId");
            DropColumn("dbo.Sales", "Total");
            DropColumn("dbo.Requisitions", "RequestedDate");
            DropTable("dbo.SaleTypes");
        }
    }
}
