using HtmlAgilityPack;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UndderControl.Events;
using UndderControl.Helpers;
using UndderControl.Services;
using UndderControlLib.Dtos;

namespace UndderControl.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
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
        public DelegateCommand OnBackCommand { get; private set; }

        public LoginPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IMetricsManagerService metricsManager)
            :base(navigationService, metricsManager)
        {
            Title = "";
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<HtmlChangedEvent>().Subscribe(CheckLogin);

            OnBackCommand = new DelegateCommand(ResetLogin);
        }

        private void ResetLogin()
        {
            _eventAggregator.GetEvent<LoginBackEvent>().Publish();
        }

        private void CheckLogin()
        {
            if (Html.Contains("LOGIN-SUCCESS"))
            {
                //Clear down our dictionary just in case it's not the first time through and the Merck login has a valid user cookie.
                _userDetails.Clear();

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
            
            await RunSafe(GetUser(userToken));

            if (User != null)
            {
                UserSettings.UserId = User.ID;
                UserSettings.UserToken = User.Token;

                MetricsManager.TrackEvent("UserLogin");

                await NavigationService.NavigateAsync("/SdctMasterDetailPage/NavigationPage/RootPage");
            } 
            else
            {
                await NavigationService.NavigateAsync("/ConnectionIssuePage");
            }
            
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
                    await PageDialog.AlertAsync("Unable to retrieve user data", "Error", "OK");
                }
            }
            catch (Exception ex)
            {
                MetricsManager.TrackException("Error getting cowstatus data", ex);
                await PageDialog.AlertAsync("Unable to retrieve user data", "Error", "OK");
            }
        }




    }
}
