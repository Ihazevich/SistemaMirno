namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentMethod : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PaymentTypes", newName: "PaymentMethods");
            DropForeignKey("dbo.Payments", "PaymentTypeId", "dbo.PaymentTypes");
            DropIndex("dbo.Payments", new[] { "PaymentTypeId" });
            AddColumn("dbo.Payments", "PaymentMethod_Id", c => c.Int());
            CreateIndex("dbo.Payments", "PaymentMethod_Id");
            AddForeignKey("dbo.Payments", "PaymentMethod_Id", "dbo.PaymentMethods", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "PaymentMethod_Id", "dbo.PaymentMethods");
            DropIndex("dbo.Payments", new[] { "PaymentMethod_Id" });
            DropColumn("dbo.Payments", "PaymentMethod_Id");
            CreateIndex("dbo.Payments", "PaymentTypeId");
            AddForeignKey("dbo.Payments", "PaymentTypeId", "dbo.PaymentTypes", "Id");
            RenameTable(name: "dbo.PaymentMethods", newName: "PaymentTypes");
        }
    }
}
