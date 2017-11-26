namespace HockeyPlayerDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTest : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Clubs", "Test");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clubs", "Test", c => c.String());
        }
    }
}
