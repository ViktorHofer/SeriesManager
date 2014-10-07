using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using SeriesManager.UILogic.Repositories;
using SeriesManager.UILogic.Services;
using System;
using System.Collections.Generic;
using TheTVDBSharp.Models;
using Windows.ApplicationModel;

namespace SeriesManager.UILogic.ViewModels
{
    public class SettingsPageViewModel : BindableBase, IFlyoutViewModel
    {
        #region Fields

        private readonly ISettingsService _settingsService;
        private readonly ISeriesRepository _seriesRepository;
        private bool _isSeriesLanguagesUpdating;

        #endregion

        #region Fields

        public string AppVersion
        {
            get
            {
                var version = Package.Current.Id.Version;
                return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }

        public bool HideNonImageSearchResults
        {
            get { return _settingsService.HideNonImageSearchResults; }
            set { _settingsService.HideNonImageSearchResults = value; }
        }

        public IReadOnlyCollection<Language> Languages
        {
            get { return (Language[])Enum.GetValues(typeof(Language)); }
        }

        public Language SelectedLanguage
        {
            get { return _settingsService.SelectedLanguage; }
            set { _settingsService.SelectedLanguage = value; }
        }

        public bool IsSeriesLanguagesUpdating
        {
            get { return _isSeriesLanguagesUpdating; }
            set { base.SetProperty(ref _isSeriesLanguagesUpdating, value); }
        }

        #endregion

        #region Constructor

        public SettingsPageViewModel(ISettingsService settingsService, ISeriesRepository seriesRepository)
        {
            if (settingsService == null) throw new ArgumentNullException("settingsService", "SettingsService cannot be null");
            if (seriesRepository == null) throw new ArgumentNullException("seriesRepository");

            _settingsService = settingsService;
            _seriesRepository = seriesRepository;
            _settingsService.SelectedLanguageChanged += async (s, e) =>
            {
                OnPropertyChanged(() => SelectedLanguage);
                IsSeriesLanguagesUpdating = true;
                await _seriesRepository.UpdateFavoriteLanguagesAsync(_settingsService.SelectedLanguage);
                IsSeriesLanguagesUpdating = false;
            };
            _settingsService.HideNonImageSearchResultsChanged += (s, e) => OnPropertyChanged(() => HideNonImageSearchResults);
        }

        #endregion

        #region IFlyoutViewModel

        public Action CloseFlyout { get; set; }

        #endregion
    }
}
