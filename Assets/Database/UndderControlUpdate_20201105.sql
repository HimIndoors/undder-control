  USE UndderControl;
  GO
  BEGIN TRANSACTION;

  BEGIN TRY
	DELETE FROM [SurveyQuestion] WHERE ID = 1;
	UPDATE [SurveyQuestion] SET QuestionNum = 1 WHERE ID = 2;
	UPDATE [Survey] SET Version = 30, LastUpdated = GETDATE() WHERE [Name] = 'SDCT';
	UPDATE [SurveyQuestion] 
		SET 
			QuestionText = 'Is the body condition score of the cows between 3.0 and 3.5? Are cows getting the appropriate energy, protein, dry matter intake and minerals in the last week before dry-off?',
			QuestionHelpText = 'If all five elements are appropriate, select yes. If fewer than five are appropriate, select no.'
		WHERE Survey_ID = 1 and Stage_ID = 2 and QuestionNum = 5;
  END TRY
  BEGIN CATCH
	SELECT
		ERROR_NUMBER() as ErrorNumber,
		ERROR_SEVERITY() as ErrorSeverity,
		ERROR_STATE() as ErrorState,
		ERROR_PROCEDURE() as ErrorProcedure,
		ERROR_LINE() as ErrorLine,
		ERROR_MESSAGE() as ErrorMessage

		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
  END CATCH;

  IF @@TRANCOUNT > 0
	COMMIT TRANSACTION;
  GO
  