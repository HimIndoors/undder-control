using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism;
using Prism.Ioc;
using System.Threading.Tasks;
using UndderControl.Services;
using UndderControl.ViewModels;
using UndderControl.Views;
using UndderControlLib.Dtos;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace UndderControl
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
         //TODO: UserName needs resolving and storing securly 
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }
        public static SurveyDto LatestSurvey { get; set; }
        public static FarmDto SelectedFarm { get; set; }
        private static ISettings AppSettings => CrossSettings.Current;

        protected override async void OnInitialized()
        {
            //TODO: Remove this for release
            if (Config.TestMode) AppSettings.AddOrUpdateValue("UserId", "abcd123xyz");
            VersionTracking.Track();
            InitializeComponent();
            await NavigateToPage();
        }

        private async Task NavigateToPage()
        {
            if (VersionTracking.IsFirstLaunchEver || VersionTracking.IsFirstLaunchForCurrentVersion|| Config.TestMode)
            {
                await NavigationService.NavigateAsync("TermsPage");
            }
            else
            {
                if (string.IsNullOrEmpty(AppSettings.GetValueOrDefault("UserId", null)))
                    await NavigationService.NavigateAsync("LoginPage");
                else
                    await NavigationService.NavigateAsync("NavigationPage/RootPage");
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SdctMasterDetailPage, SdctMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<AssessmentPage, AssessmentPageViewModel>();
            containerRegistry.RegisterForNavigation<MonitorPage, MonitorPageViewModel>();
            containerRegistry.RegisterForNavigation<SurveyPage, SurveyPageViewModel>();
            containerRegistry.RegisterForNavigation<RootPage, RootPageViewModel>();
            containerRegistry.RegisterForNavigation<ManageFarmsPage, ManageFarmsPageViewModel>();
            containerRegistry.RegisterForNavigation<TermsPage, TermsPageViewModel>();
            containerRegistry.RegisterForNavigation<SurveyResultsPage, SurveyResultsPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ResultsPage, ResultsPageViewModel>();
            containerRegistry.RegisterForNavigation<FarmDetailPage, FarmDetailPageViewModel>();
        }
    }
}
