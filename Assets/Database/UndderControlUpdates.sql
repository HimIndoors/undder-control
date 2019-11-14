USE [UndderControl]
GO

/* SurveyStage text Updates */
UPDATE [dbo].[SurveyStage]
   SET [StageTitle] = 'Checkpoint 1'
 WHERE ID = 2
UPDATE [dbo].[SurveyStage]
   SET [StageTitle] = 'Checkpoint 2'
 WHERE ID = 3
UPDATE [dbo].[SurveyStage]
   SET [StageTitle] = 'Checkpoint 3'
 WHERE ID = 4
UPDATE [dbo].[SurveyStage]
   SET [StageTitle] = 'Checkpoint 4'
 WHERE ID = 5
UPDATE [dbo].[SurveyStage]
   SET [StageTitle] = 'Checkpoint 5'
 WHERE ID = 6
GO

/* SurveyQuestion text updates */
UPDATE [dbo].[SurveyQuestion]
   SET QuestionText = 'Is the bulk milk SCC (BMSCC) in the farm lower than 250,000 cells/ml'
 WHERE ID = 2

UPDATE [dbo].[SurveyQuestion]
   SET QuestionText = 'Do you have a dry-off strategy?'
 WHERE ID = 3

UPDATE [dbo].[SurveyQuestion]
   SET QuestionStatement = 'Poor teat-end condition should be present in less than 15% of cows at dry-off.'
 WHERE ID = 4

UPDATE [dbo].[SurveyQuestion]
   SET QuestionText = 'Are cows getting the appropriate energy, protein, dry matter intake and minerals in the last weeks before dry-off?',
	   QuestionStatement = 'The cows should be getting the appropriate energy, protein, dry matter intake and minerals in the last weeks before dry-off.'
 WHERE ID = 7

UPDATE [dbo].[SurveyQuestion]
   SET QuestionStatement = 'Dry-off should take place in a clean, comfortable environment and with the correct sequence of events being followed.'
 WHERE ID = 8

UPDATE [dbo].[SurveyQuestion]
   SET QuestionText = 'Are you using cow somatic cell counts or another reilable test to diagnose infection?'
 WHERE ID = 10

UPDATE [dbo].[SurveyQuestion]
   SET QuestionText = 'Are the cows'' udders and thighs clean?'
 WHERE ID = 13

UPDATE [dbo].[SurveyQuestion]
   SET QuestionText = 'Are the cows'' udders and thighs clean?'
 WHERE ID = 18

UPDATE [dbo].[SurveyQuestion]
   SET QuestionText = 'Does the calving pen provide sufficient space; clean, soft and dry bedding; seclusion and available nutrition?'
 WHERE ID = 23
 GO

