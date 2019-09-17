namespace UndderControlService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FarmType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FarmType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Farm", "FarmType_ID", c => c.Int(nullable: false));
            CreateIndex("dbo.Farm", "FarmType_ID");
            AddForeignKey("dbo.Farm", "FarmType_ID", "dbo.FarmType", "ID");
            DropColumn("dbo.Farm", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Farm", "Type", c => c.String());
            DropForeignKey("dbo.Farm", "FarmType_ID", "dbo.FarmType");
            DropIndex("dbo.Farm", new[] { "FarmType_ID" });
            DropColumn("dbo.Farm", "FarmType_ID");
            DropTable("dbo.FarmType");
        }
    }
}
