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
    public sealed partial class App
    {
        private readonly UnityContainer _unityContainer = new UnityContainer();

        public App()
        {
            InitializeComponent();

            LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new MetroLog.Targets.FileStreamingTarget());
            GlobalCrashHandler.Configure();

            ExtendedSplashScreenFactory = splash => new SplashPage(splash);
        }

        protected override object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

#if WINDOWS_APP
        protected override IList<SettingsCommand> GetSettingsCommands()
        {
            var settingsCommands = new List<SettingsCommand>();
            var resourceLoader = _unityContainer.Resolve<IResourceLoader>();

            // TODO: Fill list
            settingsCommands.Add(new SettingsCommand(Guid.NewGuid(), resourceLoader.GetString("SettingsCharm"), h => new Views.SettingsPage().ShowIndependent()));

            return settingsCommands;
        }
#endif

        protected override async Task OnInitializeAsync(IActivatedEventArgs args, Frame rootFrame)
        {
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "SeriesManager.UILogic.ViewModels.{0}ViewModel, SeriesManager.UILogic, Version=1.0.0.0, Culture=neutral", viewType.Name);
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });

            // Register core services
            _unityContainer.RegisterInstance<INavigationService>(NavigationService, new ContainerControlledLifetimeManager())
                .RegisterInstance<ISessionStateService>(SessionStateService, new ContainerControlledLifetimeManager())
                .RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager())
                .RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()), new ContainerControlledLifetimeManager())
                .RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager())
                .RegisterType<IAlertMessageService, AlertMessageService>(new ContainerControlledLifetimeManager())
                .RegisterInstance<ILogManager>(LogManagerFactory.DefaultLogManager, new ContainerControlledLifetimeManager())
                .RegisterType<ISeriesRepository, SeriesRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IBannerRepository, BannerRepository>(new ContainerControlledLifetimeManager())
                .RegisterInstance<ITheTvdbManager>(new TheTvdbManager("<< ENTER API KEY >>"))
                .RegisterType<SearchPageViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<MainPageViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<SettingsPageViewModel>(new ContainerControlledLifetimeManager())
                .RegisterType<BannerViewModelFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<SearchItemViewModelFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<FavoriteItemViewModelFactory>(new ContainerControlledLifetimeManager())
                .RegisterInstance<IStorageService>(await StorageService.Create());

            await _unityContainer.Resolve<ISeriesRepository>().LoadFavoritesAsync();

            var frameVm = _unityContainer.Resolve<FrameViewModel>();
            rootFrame.DataContext = frameVm;
            rootFrame.Template = Resources["FrameControlTemplate"] as ControlTemplate;
            rootFrame.RequestedTheme = Windows.UI.Xaml.ElementTheme.Dark;
            rootFrame.Navigated += (s, e) => frameVm.NavigationBarIsOpen = false;
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Main", null);
            return Task.FromResult<object>(null);
        }
    }
}