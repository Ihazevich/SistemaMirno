namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkOrderUnits : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.WorkOrders", name: "WorkAreaId", newName: "EnteringWorkAreaId");
            RenameIndex(table: "dbo.WorkOrders", name: "IX_WorkAreaId", newName: "IX_EnteringWorkAreaId");
            CreateTable(
                "dbo.WorkOrderUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        WorkUnitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId)
                .ForeignKey("dbo.WorkUnits", t => t.WorkUnitId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.WorkUnitId);
            
            AddColumn("dbo.WorkOrders", "LeavingWorkAreaId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkOrders", "LeavingWorkAreaId");
            AddForeignKey("dbo.WorkOrders", "LeavingWorkAreaId", "dbo.WorkAreas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrders", "LeavingWorkAreaId", "dbo.WorkAreas");
            DropForeignKey("dbo.WorkOrderUnits", "WorkUnitId", "dbo.WorkUnits");
            DropForeignKey("dbo.WorkOrderUnits", "WorkOrderId", "dbo.WorkOrders");
            DropIndex("dbo.WorkOrderUnits", new[] { "WorkUnitId" });
            DropIndex("dbo.WorkOrderUnits", new[] { "WorkOrderId" });
            DropIndex("dbo.WorkOrders", new[] { "LeavingWorkAreaId" });
            DropColumn("dbo.WorkOrders", "LeavingWorkAreaId");
            DropTable("dbo.WorkOrderUnits");
            RenameIndex(table: "dbo.WorkOrders", name: "IX_EnteringWorkAreaId", newName: "IX_WorkAreaId");
            RenameColumn(table: "dbo.WorkOrders", name: "EnteringWorkAreaId", newName: "WorkAreaId");
        }
    }
}
