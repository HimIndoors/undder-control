using System;
using System.Threading.Tasks;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControl.ViewModels;
using UndderControlLib.Dtos;
using Xamarin.Forms;

namespace UndderControl.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="SurveyResponseDto"/>.
    /// </summary>
    static class SurveyResponseDtoExtensions
    {
        /// <summary>
        /// Saves the survey to local storage asynchronously.
        /// </summary>
        /// <param name="response">The survey response.</param>
        /// <returns>A task to await the save operation.</returns>
        public static Task SaveAsync(this SurveyResponseDto response)
        {
            var fileHelper = new FileHelper();
            return fileHelper.SaveAsync(response.GetFilename(), response);
        }

        /// <summary>
        /// Deletes the survey from local storage asynchronously.
        /// </summary>
        /// <param name="response">The survey response.</param>
        /// <returns>A task to await the delete operation.</returns>
        public static Task DeleteAsync(this SurveyResponseDto response)
        {
            var fileHelper = new FileHelper();
            return fileHelper.DeleteAsync(response.GetFilename());
        }

        /// <summary>
        /// Initializes a new survey response.
        /// </summary>
        /// <returns>The survey response.</returns>
        public static SurveyResponseDto CreateNew()
        {
            return new SurveyResponseDto
            {
                ResponseIdentifier = Guid.NewGuid(),
                SurveyID = App.LatestSurvey.ID,
                Survey_Version = App.LatestSurvey.Version
            };
        }

        /// <summary>
        /// Gets the filename of the survey response, based on the survey response <see cref="Guid"/>.
        /// </summary>
        /// <param name="response">The survey response.</param>
        /// <returns>The filename.</returns>
        private static string GetFilename(this SurveyResponseDto response)
        {
            return $"{response.ResponseIdentifier}.survey.json";
        }
    }
}
