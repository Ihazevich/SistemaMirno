namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class branches : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.WorkAreas", "BranchId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "BranchId", c => c.Int(nullable: false));
            AddColumn("dbo.Sales", "BranchId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkAreas", "BranchId");
            CreateIndex("dbo.Employees", "BranchId");
            CreateIndex("dbo.Sales", "BranchId");
            AddForeignKey("dbo.Employees", "BranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.Sales", "BranchId", "dbo.Branches", "Id");
            AddForeignKey("dbo.WorkAreas", "BranchId", "dbo.Branches", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkAreas", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Sales", "BranchId", "dbo.Branches");
            DropForeignKey("dbo.Employees", "BranchId", "dbo.Branches");
            DropIndex("dbo.Sales", new[] { "BranchId" });
            DropIndex("dbo.Employees", new[] { "BranchId" });
            DropIndex("dbo.WorkAreas", new[] { "BranchId" });
            DropColumn("dbo.Sales", "BranchId");
            DropColumn("dbo.Employees", "BranchId");
            DropColumn("dbo.WorkAreas", "BranchId");
            DropTable("dbo.Branches");
        }
    }
}
