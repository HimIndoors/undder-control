using Prism;
using Prism.Ioc;
using UndderControl.ViewModels;
using UndderControl.Views;
using UndderControlLib.Dtos;
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

        public static SurveyDto LatestSurvey { get; set; }
        public static FarmDto SelectedFarm { get; set; }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            //await NavigationService.NavigateAsync("MainPage");
            await NavigationService.NavigateAsync("NavigationPage/RootPage");
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
        }
    }
}
