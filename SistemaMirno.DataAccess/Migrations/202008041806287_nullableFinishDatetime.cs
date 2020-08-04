namespace SistemaMirno.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableFinishDatetime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkOrders", "FinishTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkOrders", "FinishTime", c => c.DateTime(nullable: false));
        }
    }
}
