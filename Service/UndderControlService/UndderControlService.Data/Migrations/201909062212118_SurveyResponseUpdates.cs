namespace UndderControlService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyResponseUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SurveyResponse", "ResponseIdentifier", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SurveyResponse", "ResponseIdentifier");
        }
    }
}
