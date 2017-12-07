namespace HockeyPlayerDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameClubRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clubs", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clubs", "Name", c => c.String());
        }
    }
}
