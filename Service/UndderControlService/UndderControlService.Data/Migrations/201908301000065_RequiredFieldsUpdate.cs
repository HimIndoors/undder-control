namespace UndderControlService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredFieldsUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cow", "DateAdded", c => c.DateTime(nullable: false));
            AddColumn("dbo.SurveyResponse", "Farm_ID", c => c.Int(nullable: false));
            AddForeignKey("dbo.SurveyResponse", "User_ID", "dbo.Farm", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyResponse", "User_ID", "dbo.Farm");
            DropColumn("dbo.SurveyResponse", "Farm_ID");
            DropColumn("dbo.Cow", "DateAdded");
        }
    }
}
