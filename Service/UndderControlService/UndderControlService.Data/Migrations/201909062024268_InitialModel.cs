namespace UndderControlService.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CowStatus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Farm_ID = c.Int(nullable: false),
                        InfectedAtDryOff = c.Boolean(nullable: false),
                        InfectedAtCalving = c.Boolean(nullable: false),
                        CowIdentifier = c.String(),
                        DateAddedDryOff = c.DateTime(),
                        DateAddedCalving = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Farm", t => t.Farm_ID)
                .Index(t => t.Farm_ID);
            
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
                        QuestionID = c.Int(nullable: false),
                        StageID = c.Int(nullable: false),
                        QuestionResponse = c.Boolean(nullable: false),
                        QuestionStatement = c.String(),
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
                        SurveyVersion = c.Int(nullable: false),
                        SubmittedDate = c.DateTime(nullable: false),
                        Farm_ID = c.Int(nullable: false),
                        User_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Farm", t => t.Farm_ID)
                .ForeignKey("dbo.Survey", t => t.Survey_ID)
                .ForeignKey("dbo.User", t => t.User_ID)
                .Index(t => t.Survey_ID)
                .Index(t => t.Farm_ID)
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
            DropForeignKey("dbo.SurveyResponse", "Farm_ID", "dbo.Farm");
            DropForeignKey("dbo.CowStatus", "Farm_ID", "dbo.Farm");
            DropForeignKey("dbo.Farm", "User_ID", "dbo.User");
            DropIndex("dbo.SurveyStage", new[] { "Survey_ID" });
            DropIndex("dbo.SurveyQuestion", new[] { "Stage_ID" });
            DropIndex("dbo.SurveyQuestion", new[] { "Survey_ID" });
            DropIndex("dbo.SurveyResponse", new[] { "User_ID" });
            DropIndex("dbo.SurveyResponse", new[] { "Farm_ID" });
            DropIndex("dbo.SurveyResponse", new[] { "Survey_ID" });
            DropIndex("dbo.SurveyQuestionResponse", new[] { "SurveyResponse_ID" });
            DropIndex("dbo.Farm", new[] { "User_ID" });
            DropIndex("dbo.CowStatus", new[] { "Farm_ID" });
            DropTable("dbo.SurveyStage");
            DropTable("dbo.SurveyQuestion");
            DropTable("dbo.Survey");
            DropTable("dbo.SurveyResponse");
            DropTable("dbo.SurveyQuestionResponse");
            DropTable("dbo.User");
            DropTable("dbo.Farm");
            DropTable("dbo.CowStatus");
        }
    }
}
