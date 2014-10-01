using MetroLog;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UniRock.Services;
using UniRock.Services.Interfaces;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
#endif
using Windows.UI.Xaml.Controls;
using SeriesManager.UILogic.Repositories;
using SeriesManager.UILogic.Services;
using TheTVDBSharp;
using SeriesManager.UILogic.ViewModels;
using SeriesManager.UILogic.ViewModels.Passive;

namespace SeriesManager
{
    public sealed partial class App : SplashOptimizedMvvmAppBase
    {
        private readonly UnityContainer unityContainer = new UnityContainer();
        private readonly ILogger logger;

        public App()
        {
            this.InitializeComponent();

            MetroLog.LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new MetroLog.Targets.FileStreamingTarget());
            GlobalCrashHandler.Configure();
            this.logger = LogManagerFactory.DefaultLogManager.GetLogger<App>();

            this.ExtendedSplashScreenFactory = (splash) => new SplashPage(splash);
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            base.OnRegisterKnownTypesForSerialization();
        }

        protected override object Resolve(Type type)
        {
            return this.unityContainer.Resolve(type);
        }

#if WINDOWS_APP
        protected override IList<SettingsCommand> GetSettingsCommands()
        {
            var settingsCommands = new List<SettingsCommand>();
            var resourceLoader = this.unityContainer.Resolve<IResourceLoader>();

            // TODO: Fill list
            settingsCommands.Add(new SettingsCommand(Guid.NewGuid(), resourceLoader.GetString("SettingsCharm"), (h) => new SeriesManager.Views.SettingsPage().ShowIndependent()));

            return settingsCommands;
        }
#endif

        protected override async Task OnInitializeAsync(IActivatedEventArgs args, Frame rootFrame)
        {
            this.unityContainer.RegisterInstance<INavigationService>(NavigationService, new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterInstance<ISessionStateService>(SessionStateService, new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()), new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterType<IAlertMessageService, AlertMessageService>(new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterInstance<ILogManager>(LogManagerFactory.DefaultLogManager, new ContainerControlledLifetimeManager());

            // Repositories
            this.unityContainer.RegisterType<ISeriesRepository, SeriesRepository>(new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterType<IBannerRepository, BannerRepository>(new ContainerControlledLifetimeManager());

            // Register web service proxies
            this.unityContainer.RegisterInstance<ITheTVDBManager>(new TheTVDBManager("APIKEY"));

            // Register viewmodels
            this.unityContainer.RegisterType<SearchPageViewModel>(new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterType<MainPageViewModel>(new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterType<SettingsPageViewModel>(new ContainerControlledLifetimeManager());

            // Register factories
            this.unityContainer.RegisterType<BannerViewModelFactory>(new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterType<SearchItemViewModelFactory>(new ContainerControlledLifetimeManager());
            this.unityContainer.RegisterType<FavoriteItemViewModelFactory>(new ContainerControlledLifetimeManager());

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "SeriesManager.UILogic.ViewModels.{0}ViewModel, SeriesManager.UILogic, Version=1.0.0.0, Culture=neutral", viewType.Name);
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });

            // Initialize storage requirements
            this.unityContainer.RegisterInstance<IStorageService>(await StorageService.Create());
            await this.unityContainer.Resolve<ISeriesRepository>().LoadFavoritesAsync();

            // Initialize logger
            this.unityContainer.Resolve<ITheTVDBManager>().Logged += (s, e) =>
            {
                if (e.Level == TheTVDBSharp.Services.LogLevel.Error)
                {
                    this.logger.Error(e.Message, e.InnerException);
                }
            };

            var frameVM = this.unityContainer.Resolve<FrameViewModel>();
            rootFrame.DataContext = frameVM;
            rootFrame.Template = Resources["FrameControlTemplate"] as ControlTemplate;
            rootFrame.RequestedTheme = Windows.UI.Xaml.ElementTheme.Dark;
            rootFrame.Navigated += (s, e) => frameVM.NavigationBarIsOpen = false;
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Main", null);
            return Task.FromResult<object>(null);
        }
    }
}