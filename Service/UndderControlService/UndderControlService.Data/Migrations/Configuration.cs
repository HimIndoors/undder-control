namespace UndderControlService.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using UndderControlService.Data.Entities;
    using UndderControlService.Data.Helper;

    internal sealed class Configuration : DbMigrationsConfiguration<UndderControlService.Data.Repositories.EntitiesDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(UndderControlService.Data.Repositories.EntitiesDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            DateTime now = DateTime.Now;
            DateTime lastYear = now.AddYears(-1);
            BoolHelper helper = new BoolHelper();

            //Add test user
            var user = new User() { Name = "Vicky the Vet", Token = "PMN_TEST_TOKEN" };
            context.Users.AddOrUpdate(user);
            context.SaveChanges();

            //Add test farms
            var farms = new List<Farm>
            {
                new Farm{Name="Buttercup Farm",Address="9 Laneside", ContactName="Farmer Bob", HerdSize=200, PhoneNumber="99999999", User=user},
                new Farm{Name="Old MacDonalds",Address="123 Nursery Row", ContactName="Farmer Harry", HerdSize=2000,PhoneNumber="1234567890", User=user}
            };

            farms.ForEach(s => context.Farms.AddOrUpdate(s));
            context.SaveChanges();

            //Add Survey
            var survey = new Survey { Description = "Farm Assessment Questionnaire", IntroText = null, LastUpdated = DateTime.Now, Name = "SDCT", Version = 1, Active = true, Language = "EN" };
            context.Surveys.AddOrUpdate(survey);
            context.SaveChanges();

            //Create stages
            var stages = new List<SurveyStage>
            {
                new SurveyStage{ StageText=null, StageTitle="Farm Suitability", ShowStageIntro=false, Survey = survey},
                new SurveyStage{ StageText="Dry-off Preparation", StageTitle="Stage 1", ShowStageIntro=true, Survey = survey},
                new SurveyStage{ StageText="Dry-off", StageTitle="Stage 2", ShowStageIntro=true, Survey = survey},
                new SurveyStage{ StageText="Far-off", StageTitle="Stage 3", ShowStageIntro=true, Survey = survey},
                new SurveyStage{ StageText="Close up", StageTitle="Stage 4", ShowStageIntro=true, Survey = survey},
                new SurveyStage{ StageText="Calving", StageTitle="Stage 5", ShowStageIntro=true, Survey = survey}
            };
            stages.ForEach(s => context.SurveyStages.AddOrUpdate(s));
            context.SaveChanges();

            //Add Questions
            var questions = new List<SurveyQuestion>
            {
                new SurveyQuestion{ QuestionNum=1, QuestionText="Is the farm willing to implement SDCT?", QuestionHelpText=null, QuestionStatement=null, Stage=stages[0], Survey=survey},
                new SurveyQuestion{ QuestionNum=2, QuestionText="Is the BSCC in the farm lower than 250,000?", QuestionHelpText=null, QuestionStatement=null, Stage=stages[0], Survey=survey},

                new SurveyQuestion{ QuestionNum=1, QuestionText="Do you have a dry-off strategy?", QuestionHelpText="A set dry period length, reliable calving prediction, planning", QuestionStatement="You should work with your vet/MSD Animal Health Respresentative to create a dry-off stategy.", Stage=stages[1], Survey=survey},
                new SurveyQuestion{ QuestionNum=2, QuestionText="Is poor teat-end condition present in less that 15% of cows at dry-off?", QuestionHelpText=null, QuestionStatement="Poor teat-end condition should be present in less than 15% of cows at dry off.", Stage=stages[1], Survey=survey},
                new SurveyQuestion{ QuestionNum=3, QuestionText="Is milk production lower than 15kg/day for more than 90% of the cows?", QuestionHelpText=null, QuestionStatement="Milk production should be lower than 15 kg/day for more than 90% of the cows.", Stage=stages[1], Survey=survey},
                new SurveyQuestion{ QuestionNum=4, QuestionText="Is milk leakage happening in less that 10% of the cows?", QuestionHelpText=null, QuestionStatement="Milk leakage should be happening in less than 10% of the cows.", Stage=stages[1], Survey=survey},
                new SurveyQuestion{ QuestionNum=5, QuestionText="Are cows getting the appropriate energy, protein, dry-matter intake and minerals in the last weeks before dry-off?", QuestionHelpText="If all four elements are appropriate, select yes. If fewer than four are appropriate, select no.", QuestionStatement="The cows should be getting the appropriate energy, protein, dry matter intake and minerals in the last weeks before dry off.", Stage=stages[1], Survey=survey},

                new SurveyQuestion{ QuestionNum=1, QuestionText="Does dry-off take place in a clean, comfortable environment and is the correct sequence of events followed?", QuestionHelpText="If all three elements are being met, select yes. If fewer than three are being met, select no.", QuestionStatement="Dry off should take place in a clean, comfortable environment and with the correct sequence of events being followed.", Stage=stages[2], Survey=survey},
                new SurveyQuestion{ QuestionNum=2, QuestionText="Are you following correct hygiene protocols, such as using rubber gloves, removing dirt from teats, disinfecting for at least 30 seconds twice and partially inserting the antibiotic or teat seal?", QuestionHelpText="If all four criteria are being met, select yes. If fewer than four are being met, select no.", QuestionStatement="You should be following correct hygiene protocols, such as using clean rubber gloves, removing dirt from teats, disinfecting for at least 30 seconds twice and partially inserting the antibiotic or teat seal.", Stage=stages[2], Survey=survey},
                new SurveyQuestion{ QuestionNum=3, QuestionText="Are you using cow somatic cell counts or other reilable test to diagnose infection?", QuestionHelpText=null, QuestionStatement="You should be using cow somatic cell counts or another reliable test to diagnose infection.", Stage=stages[2], Survey=survey},
                new SurveyQuestion{ QuestionNum=4, QuestionText="Is your antibiotic and/or teat seal tube selection based on well-supported data?", QuestionHelpText=null, QuestionStatement="Your antibiotic and/or teat seal tube selection should be based on well-supported data.", Stage=stages[2], Survey=survey},
                new SurveyQuestion{ QuestionNum=5, QuestionText="Are you mitigating potential stressors, such as commingling, ample space per cow, access to feed and water?", QuestionHelpText="If all four criteria are being met, select yes. If fewer than four are being met, select no.", QuestionStatement="You should mitigate potential stressors, such as commingling, ample space per cow, access to feed and access to water.", Stage=stages[2], Survey=survey},

                new SurveyQuestion{ QuestionNum=1, QuestionText="Are the cows udders and thighs clean?", QuestionHelpText=null, QuestionStatement="You must ensure cows' udders and thighs are clean.", Stage=stages[3], Survey=survey},
                new SurveyQuestion{ QuestionNum=2, QuestionText="Have tails been clipped, udders shaven as needed and is bedding refreshed and disinfected regularly?", QuestionHelpText=null, QuestionStatement="Tails must be clipped, udders shaven as needed and bedding refreshed and disinfected regularly.", Stage=stages[3], Survey=survey},
                new SurveyQuestion{ QuestionNum=3, QuestionText="Is the calculated ration, fed ration, eaten ration and dry matter intake the same and meeting the standard nutritional requirements?", QuestionHelpText="If all four are correct, select yes. If fewer than four are correct, select no.", QuestionStatement="You need to calculate ration, fed ration, eaten ration and dry matter intake is the same and meeting standard nutritional requirements.", Stage=stages[3], Survey=survey},
                new SurveyQuestion{ QuestionNum=4, QuestionText="Are commingling and overcrowding being minimised?", QuestionHelpText=null, QuestionStatement="Commingling and overcrowding should be minimised.", Stage=stages[3], Survey=survey},
                new SurveyQuestion{ QuestionNum=5, QuestionText="Does the housing provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation?", QuestionHelpText="If all four are correct, select yes. If fewer than four are correct, select no.", QuestionStatement="The housing should provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation.", Stage=stages[3], Survey=survey},

                new SurveyQuestion{ QuestionNum=1, QuestionText="Are the cows udders and thighs clean?", QuestionHelpText=null, QuestionStatement="You must ensure cows' udders and thighs are clean.", Stage=stages[4], Survey=survey},
                new SurveyQuestion{ QuestionNum=2, QuestionText="Have tails been clipped, udders shaven as needed and is bedding refreshed and disinfected regularly?", QuestionHelpText=null, QuestionStatement="Tails must be clipped, udders shaven as needed and bedding refreshed and disinfected regularly.", Stage=stages[4], Survey=survey},
                new SurveyQuestion{ QuestionNum=3, QuestionText="Is the calculated ration, fed ration, eaten ration and dry matter intake the same and meeting the standard nutritional requirements?", QuestionHelpText="If all four are correct, select yes. If fewer than four are correct, select no.", QuestionStatement="You need to calculate ration, fed ration, eaten ration and dry matter intake is the same and meeting standard nutritional requirements.", Stage=stages[4], Survey=survey},
                new SurveyQuestion{ QuestionNum=4, QuestionText="Are commingling and overcrowding being minimised?", QuestionHelpText=null, QuestionStatement="Commingling and overcrowding should be minimised.", Stage=stages[4], Survey=survey},
                new SurveyQuestion{ QuestionNum=5, QuestionText="Does the housing provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation?", QuestionHelpText="If all four are correct, select yes. If fewer than four are correct, select no.", QuestionStatement="The housing should provide proper cubicle size and comfort, ample walking space, feed bunk space and adequate ventilation.", Stage=stages[4], Survey=survey},

                new SurveyQuestion{ QuestionNum=1, QuestionText="Does the calving pen provide sufficient space; clean soft bedding; seclusion and available nutrition?", QuestionHelpText="If all four are being provided, select yes. If fewer than four are being provided, select no.", QuestionStatement="The calving pen should provide sufficient space; clean, soft and dry bedding; seclusion; and available nutrition.", Stage=stages[5], Survey=survey},
                new SurveyQuestion{ QuestionNum=2, QuestionText="Is the calving pen clean, dry and well ventilated with no sick cows?", QuestionHelpText="If all four are correct, select yes. If fewer than four are correct, select no.", QuestionStatement="The calving pen should be clean, dry and well ventilated with no sick cows.", Stage=stages[5], Survey=survey},
                new SurveyQuestion{ QuestionNum=3, QuestionText="Are less than 5% of cows showing visible milk leakage?", QuestionHelpText=null, QuestionStatement="Less than 5% of cows should be showing visible milk leakage.", Stage=stages[5], Survey=survey},
                new SurveyQuestion{ QuestionNum=4, QuestionText="Do less than 10% of calvings need assistance?", QuestionHelpText=null, QuestionStatement="Less than 10% of calvings should need assistance.", Stage=stages[5], Survey=survey},
                new SurveyQuestion{ QuestionNum=5, QuestionText="Is the milking machine for the first milkings after calving functioning properly and thoroughly cleaned and disinfected before and after milkings?", QuestionHelpText="If the two are correct, select yes. If fewer than two are correct, select no.", QuestionStatement="The milking machine for the first milkings after calving should be functioning properly and be thoroughly cleaned and disinfected before and after milkings.", Stage=stages[5], Survey=survey}

            };
            questions.ForEach(q => context.SurveyQuestions.AddOrUpdate(q));
            context.SaveChanges();

            var cows = new List<CowStatus>
            {
                new CowStatus{ CowIdentifier="001", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="002", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="003", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="004", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="005", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="006", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="007", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="008", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="009", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="010", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="101", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="102", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="103", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="104", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="105", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="106", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="107", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="108", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus{ CowIdentifier="109", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },
                new CowStatus { CowIdentifier="110", Farm = farms[0], InfectedAtDryOff = helper.GetRandomBoolean(), InfectedAtCalving = helper.GetRandomBoolean(), DateAddedDryOff = lastYear.AddDays(-60), DateAddedCalving = lastYear },

                new CowStatus{ CowIdentifier="001", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = true, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="002", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="003", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="004", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = true, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="005", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="006", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="007", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="008", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = true, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="009", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="010", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = true, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="101", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = true, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="102", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="103", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = true, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="104", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="105", Farm = farms[0], InfectedAtDryOff = true, InfectedAtCalving = true, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="106", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="107", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = true, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="108", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus{ CowIdentifier="109", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = true, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now },
                new CowStatus { CowIdentifier="110", Farm = farms[0], InfectedAtDryOff = false, InfectedAtCalving = false, DateAddedDryOff = now.AddDays(-60), DateAddedCalving = now }
            };
            cows.ForEach(c => context.CowStatus.AddOrUpdate(c));
            context.SaveChanges();

            var answersThisYear = new List<SurveyQuestionResponse>();
            var answersLastYear = new List<SurveyQuestionResponse>();

            foreach (SurveyQuestion question in questions)
            {
                answersThisYear.Add(new SurveyQuestionResponse { QuestionID = question.ID, StageID = question.Stage_ID, QuestionResponse = helper.GetRandomBoolean(), QuestionStatement = question.QuestionStatement });
                answersLastYear.Add(new SurveyQuestionResponse { QuestionID = question.ID, StageID = question.Stage_ID, QuestionResponse = helper.GetRandomBoolean(), QuestionStatement = question.QuestionStatement });
            }

            var surveyResponses = new List<SurveyResponse> {
                new SurveyResponse { Survey = survey, Farm = farms[0], User = user, SubmittedDate = now, SurveyVersion = survey.Version, QuestionResponses = answersThisYear, ResponseIdentifier = Guid.NewGuid() },
                new SurveyResponse { Survey = survey, Farm = farms[0], User = user, SubmittedDate = lastYear, SurveyVersion = survey.Version, QuestionResponses = answersLastYear, ResponseIdentifier = Guid.NewGuid() }
            };

            surveyResponses.ForEach(s => context.SurveyResponses.AddOrUpdate(s));
            context.SaveChanges();
        }
    }
}
