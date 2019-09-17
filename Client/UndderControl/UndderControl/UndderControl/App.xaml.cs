using MonkeyCache.SQLite;
using Prism;
using Prism.Ioc;
using Prism.Navigation.Xaml;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UndderControl.Helpers;
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
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        #region Global Data
        public static SurveyDto LatestSurvey { get; set; }
        public static FarmDto SelectedFarm { get; set; }
        public static List<CowStatusDto> LatestCowStatusData { get; set; }
        public static List<CowStatusDto> PreviousCowStatusData { get; set; }
        public static SurveyResponseDto LatestSurveyResponse { get; set; }
        public static SurveyResponseDto PreviousSurveyResponse { get; set; }
        #endregion Global Data

        protected override async void OnInitialized()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTIwOTUxQDMxMzcyZTMyMmUzMFlvWjZiUENiOVVkTm1CSG04RXRGWEJ0cW4rR0Fuc2ZNK2pjM2p0REZCelk9");
            if (Config.TestMode) UserSettings.UserId = 1;
            //Initialize MonkeyCache barrel
            Barrel.ApplicationId = "PMN_Undder_Control";

            //TODO: Important - remove this line for live
            Barrel.Current.EmptyAll();

            VersionTracking.Track();
            InitializeComponent();
            await NavigateToPage();
        }

        private async Task NavigateToPage()
        {
            if (UserSettings.UserId <= 0)
                await NavigationService.NavigateAsync("LoginPage");
            else
                if (VersionTracking.IsFirstLaunchEver || VersionTracking.IsFirstLaunchForCurrentVersion|| Config.TestMode)
                    await NavigationService.NavigateAsync("TermsPage");
                else
                    await NavigationService.NavigateAsync("/SdctMasterDetailPage/NavigationPage/RootPage");
        }

        public void OnMenuButtonPressed(object sender, EventArgs e)
        {
            (Current.MainPage as MasterDetailPage).IsPresented = true;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<CustomNavigationPage>();
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
            containerRegistry.RegisterForNavigation<FarmDetailPage, FarmDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<CowStatusPage, CowStatusPageViewModel>();
            containerRegistry.RegisterForNavigation<CowStatusResultsPage, CowStatusResultsPageViewModel>();
            containerRegistry.RegisterForNavigation<CowStatusComparisonPage, CowStatusComparisonPageViewModel>();
            containerRegistry.RegisterForNavigation<SurveyComparisonPage, SurveyComparisonPageViewModel>();
            containerRegistry.RegisterForNavigation<CowStatusInputPage, CowStatusInputPageViewModel>();
            containerRegistry.RegisterForNavigation<CowStatusFinishPage, CowStatusFinishPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
            containerRegistry.RegisterForNavigation<NoResultsComparison, NoResultsComparisonViewModel>();
        }
    }
}
