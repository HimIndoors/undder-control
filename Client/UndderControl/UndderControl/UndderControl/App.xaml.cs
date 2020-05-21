using MonkeyCache.SQLite;
using Prism;
using Prism.Ioc;
using System;
using System.Collections.Generic;
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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTUxNTY5QDMxMzcyZTMzMmUzMFZnS0V6ZjZKTUF2WFF6Q2F3MjNud0hzVFVvaVJOSThXK0xQbHpNbFVmS0E9");// Version 1.7.3.*

            //if (Config.TestMode) UserSettings.UserId = 1;
            //UserSettings.UserId = 0;

            //Initialize MonkeyCache barrel
            Barrel.ApplicationId = "MSC_Undder_Control";

            VersionTracking.Track();
            InitializeComponent();

            await NavigationService.NavigateAsync("SplashPage");
        }

        public void OnMenuButtonPressed(object sender, EventArgs e)
        {
            (Current.MainPage as MasterDetailPage).IsPresented = true;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
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
            containerRegistry.RegisterForNavigation<NoResultsComparisonPage, NoResultsComparisonPageViewModel>();
            containerRegistry.RegisterForNavigation<SplashPage, SplashPageViewModel>();
            containerRegistry.RegisterForNavigation<ConnectionIssuePage, ConnectionIssuePageViewModel>();
            containerRegistry.RegisterForNavigation<TestWebPage, TestWebPageViewModel>();
        }

        public static void UnhandledException(Exception ex, string v)
        {
            DependencyService.Get<IMetricsManagerService>().TrackException(v, ex);
        }
    }
}
