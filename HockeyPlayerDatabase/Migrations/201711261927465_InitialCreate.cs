namespace HockeyPlayerDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        TitleBefore = c.String(),
                        YearOfBirth = c.Int(nullable: false),
                        KrpId = c.Int(nullable: false),
                        AgeCategory = c.Int(),
                        ClubId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.ClubId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "ClubId", "dbo.Clubs");
            DropIndex("dbo.Players", new[] { "ClubId" });
            DropTable("dbo.Players");
            DropTable("dbo.Clubs");
        }
    }
}
