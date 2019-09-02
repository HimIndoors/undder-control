namespace UndderControlService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CowProcess",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Cow",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Farm_ID = c.Int(nullable: false),
                        Process_ID = c.Int(nullable: false),
                        Infected = c.Boolean(nullable: false),
                        CowIdentifier = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Farm", t => t.Farm_ID)
                .ForeignKey("dbo.CowProcess", t => t.Process_ID)
                .Index(t => t.Farm_ID)
                .Index(t => t.Process_ID);
            
            CreateTable(
                "dbo.Farm",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        ContactName = c.String(),
                        PhoneNumber = c.String(),
                        HerdSize = c.Int(nullable: false),
                        Type = c.String(),
                        User_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.User_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SurveyQuestionResponse",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SurveyResponse_ID = c.Int(nullable: false),
                        Question_ID = c.Int(nullable: false),
                        Stage_ID = c.Int(nullable: false),
                        QuestionResponse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SurveyResponse", t => t.SurveyResponse_ID)
                .Index(t => t.SurveyResponse_ID);
            
            CreateTable(
                "dbo.SurveyResponse",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Survey_ID = c.Int(nullable: false),
                        Survey_Version = c.Int(nullable: false),
                        User_ID = c.Int(nullable: false),
                        StartTime = c.DateTimeOffset(nullable: false, precision: 7),
                        EndTime = c.DateTimeOffset(precision: 7),
                        ResponseIdentifier = c.Guid(nullable: false),
                        Lat = c.Double(),
                        Lon = c.Double(),
                        Accuracy = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Survey", t => t.Survey_ID)
                .ForeignKey("dbo.User", t => t.User_ID)
                .Index(t => t.Survey_ID)
                .Index(t => t.User_ID);
            
            CreateTable(
                "dbo.Survey",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IntroText = c.String(),
                        Version = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        Language = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SurveyQuestion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Survey_ID = c.Int(nullable: false),
                        Stage_ID = c.Int(nullable: false),
                        QuestionNum = c.Int(nullable: false),
                        QuestionText = c.String(),
                        QuestionHelpText = c.String(),
                        QuestionStatement = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SurveyStage", t => t.Stage_ID)
                .ForeignKey("dbo.Survey", t => t.Survey_ID, cascadeDelete: true)
                .Index(t => t.Survey_ID)
                .Index(t => t.Stage_ID);
            
            CreateTable(
                "dbo.SurveyStage",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Survey_ID = c.Int(nullable: false),
                        StageText = c.String(),
                        ShowStageIntro = c.Boolean(nullable: false),
                        StageTitle = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Survey", t => t.Survey_ID, cascadeDelete: true)
                .Index(t => t.Survey_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveyResponse", "User_ID", "dbo.User");
            DropForeignKey("dbo.SurveyResponse", "Survey_ID", "dbo.Survey");
            DropForeignKey("dbo.SurveyQuestion", "Survey_ID", "dbo.Survey");
            DropForeignKey("dbo.SurveyQuestion", "Stage_ID", "dbo.SurveyStage");
            DropForeignKey("dbo.SurveyStage", "Survey_ID", "dbo.Survey");
            DropForeignKey("dbo.SurveyQuestionResponse", "SurveyResponse_ID", "dbo.SurveyResponse");
            DropForeignKey("dbo.Cow", "Process_ID", "dbo.CowProcess");
            DropForeignKey("dbo.Cow", "Farm_ID", "dbo.Farm");
            DropForeignKey("dbo.Farm", "User_ID", "dbo.User");
            DropIndex("dbo.SurveyStage", new[] { "Survey_ID" });
            DropIndex("dbo.SurveyQuestion", new[] { "Stage_ID" });
            DropIndex("dbo.SurveyQuestion", new[] { "Survey_ID" });
            DropIndex("dbo.SurveyResponse", new[] { "User_ID" });
            DropIndex("dbo.SurveyResponse", new[] { "Survey_ID" });
            DropIndex("dbo.SurveyQuestionResponse", new[] { "SurveyResponse_ID" });
            DropIndex("dbo.Farm", new[] { "User_ID" });
            DropIndex("dbo.Cow", new[] { "Process_ID" });
            DropIndex("dbo.Cow", new[] { "Farm_ID" });
            DropTable("dbo.SurveyStage");
            DropTable("dbo.SurveyQuestion");
            DropTable("dbo.Survey");
            DropTable("dbo.SurveyResponse");
            DropTable("dbo.SurveyQuestionResponse");
            DropTable("dbo.User");
            DropTable("dbo.Farm");
            DropTable("dbo.Cow");
            DropTable("dbo.CowProcess");
        }
    }
}
