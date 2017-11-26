namespace HockeyPlayerDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clubs", "Test", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clubs", "Test");
        }
    }
}
