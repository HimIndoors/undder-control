using HtmlAgilityPack;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UndderControl.Events;
using UndderControl.Extensions;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMetricsManagerService _metricsService;
        private string _html;
        public string Html
        {
            get { return _html; }
            set {
                _html = value;
                RaisePropertyChanged();
                _eventAggregator.GetEvent<HtmlChangedEvent>().Publish();
            }
        }
        private Dictionary<string, string> _userDetails = new Dictionary<string, string>();
        private UserDto User { get; set; }

        public LoginPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IMetricsManagerService metricsManager)
            :base(navigationService)
        {
            _eventAggregator = eventAggregator;
            _metricsService = metricsManager;
            _eventAggregator.GetEvent<HtmlChangedEvent>().Subscribe(CheckLogin);
        }

        private void CheckLogin()
        {
            if (Html.Contains("LOGIN-SUCCESS"))
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(Html);
                var comment = htmlDoc.DocumentNode.SelectSingleNode("//comment()");

                string[] stringSeparators = new string[] { "\\n" };
                string[] lines = comment.InnerHtml.Split(stringSeparators, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    if (line.Contains("="))
                    {
                        var detail = line.Split('=');
                        _userDetails.Add(detail[0], detail[1]);
                    }
                }
                
                if (_userDetails["LOGIN-SUCCESS"] == "true" &&_userDetails.ContainsKey("LFW_user"))
                {
                    LocalLoginAsync(_userDetails["LFW_user"]);
                } 
            }
        }

        private async void LocalLoginAsync(string userToken)
        {
            _metricsService.TrackEvent("UserLogin");
            await RunSafe(GetUser(userToken));
            UserSettings.UserId = User.ID;
            UserSettings.UserToken = User.Token;
        }

        private async Task GetUser(string userToken)
        {
            try
            {
                var apiresponse = await ApiManager.GetUserByToken(userToken);
                if (apiresponse.IsSuccessStatusCode)
                {
                    var response = await apiresponse.Content.ReadAsStringAsync();
                    var json = await Task.Run(() => JsonConvert.DeserializeObject<UserDto>(response));
                    User = json;
                }
                else
                {
                    await PageDialog.AlertAsync("Unable to retrieve cow status data", "Error", "OK");
                }
            }
            catch (Exception ex)
            {
                _metricsService.TrackException("Error getting cowstatus data", ex);
                await PageDialog.AlertAsync("Unable to retrieve cow status data", "Error", "OK");
            }


        }


    }
}
