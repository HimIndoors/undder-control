﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndderControlService.Data.Entities;
using UndderControlService.Data.Repositories;

namespace UndderControlService.Data.Seed
{
    public class EntitiesDbInitialiser : System.Data.Entity.DropCreateDatabaseIfModelChanges<EntitiesDbContext>
    {
        protected override void Seed(EntitiesDbContext context)
        {
            //Add test user
            var user = new User() { Name = "Vicky the Vet", Token = "MERCK_USER_TOKEN" };
            context.Users.Add(user);
            context.SaveChanges();

            //Add test farms
            var farms = new List<Farm>
            {
                new Farm{Name="Buttercup Farm",Address="9 Laneside", ContactName="Farmer Bob", HerdSize=200, PhoneNumber="99999999", User=user},
                new Farm{Name="Old MacDonalds",Address="123 Nursery Row", ContactName="Farmer Harry", HerdSize=2000,PhoneNumber="1234567890", User=user}
            };

            farms.ForEach(s => context.Farms.Add(s));
            context.SaveChanges();

            //Add Survey
            var survey = new Survey { Description = "Farm Assessment Questionnaire", IntroText = null, LastUpdated = DateTime.Now, Name = "SDCT", Version = 1 };

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
            stages.ForEach(s => context.SurveyStages.Add(s));
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
            questions.ForEach(q => context.SurveyQuestions.Add(q));
            context.SaveChanges();
        }
    }
}
