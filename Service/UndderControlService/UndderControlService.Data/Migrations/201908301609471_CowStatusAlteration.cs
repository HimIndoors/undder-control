namespace UndderControlService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CowStatusAlteration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cow", "Farm_ID", "dbo.Farm");
            DropForeignKey("dbo.Cow", "Process_ID", "dbo.CowProcess");
            DropIndex("dbo.Cow", new[] { "Farm_ID" });
            DropIndex("dbo.Cow", new[] { "Process_ID" });
            CreateTable(
                "dbo.CowStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Farm_ID = c.Int(nullable: false),
                        InfectedAtDryOff = c.Boolean(nullable: false),
                        InfectedAtCalving = c.Boolean(nullable: false),
                        CowIdentifier = c.String(),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Farm", t => t.Farm_ID)
                .Index(t => t.Farm_ID);
            
            DropTable("dbo.CowProcess");
            DropTable("dbo.Cow");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Cow",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Farm_ID = c.Int(nullable: false),
                        Process_ID = c.Int(nullable: false),
                        Infected = c.Boolean(nullable: false),
                        CowIdentifier = c.String(),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CowProcess",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.CowStatus", "Farm_ID", "dbo.Farm");
            DropIndex("dbo.CowStatus", new[] { "Farm_ID" });
            DropTable("dbo.CowStatus");
            CreateIndex("dbo.Cow", "Process_ID");
            CreateIndex("dbo.Cow", "Farm_ID");
            AddForeignKey("dbo.Cow", "Process_ID", "dbo.CowProcess", "ID");
            AddForeignKey("dbo.Cow", "Farm_ID", "dbo.Farm", "ID");
        }
    }
}
