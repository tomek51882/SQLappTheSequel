namespace SQLappTheSequel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AnswerModelNames", "ModelName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AnswerModelNames", "ModelName", c => c.Int(nullable: false));
        }
    }
}
