namespace SQLappTheSequel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Exercises", "CreatedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Exercises", "ModifiedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Exercises", "ModifiedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Exercises", "CreatedAt", c => c.DateTime(nullable: false));
        }
    }
}
