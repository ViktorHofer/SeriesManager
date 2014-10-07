using System;
using MetroLog;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using SeriesManager.UILogic.Repositories;
using SeriesManager.UILogic.Services;
using SeriesManager.UILogic.ViewModels.Passive;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheTVDBSharp.Models;
using TheTVDBSharp.Services;
using UniRock.Services.Interfaces;
using Windows.UI.Xaml.Navigation;

namespace SeriesManager.UILogic.ViewModels
{
    public class SearchPageViewModel : ViewModel
    {
        #region Fields

        private const string SelectedItemKey = "SelectedItem";
        private readonly ISeriesRepository _seriesRepository;
        private readonly SearchItemViewModelFactory _searchItemViewModelFactory;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private readonly ISettingsService _settingsService;
        private readonly ILogger _logger;
        private string _searchQuery;
        private IReadOnlyCollection<SearchItemViewModel> _searchResult;
        private bool _isLoading;
        private SearchItemViewModel _selectedItem;

        #endregion

        #region Properties

        public SearchItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set 
            { 
                base.SetProperty(ref _selectedItem, value);
                FavoriteCommand.RaiseCanExecuteChanged();
            }
        }

        public IReadOnlyCollection<SearchItemViewModel> SearchResult
        {
            get { return _searchResult; }
            private set { base.SetProperty(ref _searchResult, value); }
        }

        public string SearchQuery 
        {
            get { return _searchQuery; }
            private set { base.SetProperty(ref _searchQuery, value); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            private set { base.SetProperty(ref _isLoading, value); }
        }

        #endregion

        #region Commands

        public DelegateCommand FavoriteCommand { get; private set; }

        #endregion

        #region Constructor

        public SearchPageViewModel(ISeriesRepository seriesRepository, 
            SearchItemViewModelFactory searchItemViewModelFactory,
            IAlertMessageService alertMessageService,
            IResourceLoader resourceLoader,
            ISettingsService settingsService,
            ILogManager logManager)
        {
            if (seriesRepository == null) throw new ArgumentNullException("seriesRepository");
            if (searchItemViewModelFactory == null) throw new ArgumentNullException("searchItemViewModelFactory");
            if (alertMessageService == null) throw new ArgumentNullException("alertMessageService");
            if (resourceLoader == null) throw new ArgumentNullException("resourceLoader");
            if (settingsService == null) throw new ArgumentNullException("settingsService");

            _seriesRepository = seriesRepository;
            _searchItemViewModelFactory = searchItemViewModelFactory;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
            _settingsService = settingsService;
            _logger = logManager.GetLogger<SearchPageViewModel>();

            FavoriteCommand = DelegateCommand.FromAsyncHandler(OnFavoriteExecuted, 
                () => _selectedItem != null);

            _seriesRepository.FavoriteCollectionChanged += (s, e) =>
            {
                if (e.RemovedSeriesCollection == null) return;

                foreach (var searchItemVm in 
                    e.RemovedSeriesCollection.Select(series => SearchResult.FirstOrDefault(searchVm => searchVm.Model.Equals(series))))
                {
                    if (searchItemVm == null) return;
                    searchItemVm.IsFavorite = false;
                }
            };
        }

        #endregion

        #region Command Implementation

        private async Task OnFavoriteExecuted()
        {
            // Toggle isFavorite state of selected series
            var selectedItem = _selectedItem;
            selectedItem.IsFavorite = !selectedItem.IsFavorite;

            // Clear selection
            SelectedItem = null;

            // Save in local storage
            await _seriesRepository.ChangeFavoriteAsync(selectedItem.Model, selectedItem.IsFavorite);
        }

        private async Task OnSearchExecuted(string searchQuery)
        {
            if (searchQuery == null) throw new ArgumentNullException("searchQuery");

            SearchQuery = searchQuery;
            SearchResult = null;

            IsLoading = true;
            IReadOnlyCollection<Series> seriesCollection;

            try
            {
                seriesCollection = await _seriesRepository.SearchAsync(SearchQuery);
            }
            catch (ParseException exception)
            {
                // ReSharper disable once UnusedVariable
                var x = _alertMessageService.ShowAsync(_resourceLoader.GetString("ParseExceptionMessage"), _resourceLoader.GetString("ParseExceptionTitle"));
                _logger.Error(string.Format("Parse error while searching for {0}", SearchQuery), exception);
                return;
            }
            catch (BadResponseException exception)
            {
                // ReSharper disable once UnusedVariable
                var x = _alertMessageService.ShowAsync(_resourceLoader.GetString("BadResponseMessage"), _resourceLoader.GetString("BadResponseTitle"));
                _logger.Error(string.Format("BadResponse while searching for {0}", SearchQuery), exception);
                return;
            }
            catch (ServerNotAvailableException exception)
            {
                // ReSharper disable once UnusedVariable
                var x = _alertMessageService.ShowAsync(_resourceLoader.GetString("ServerNotAvailableMessage"), _resourceLoader.GetString("ServerNotAvailableTitle"));
                _logger.Error(string.Format("ServerNotAvailable while searching for {0}", SearchQuery), exception);
                return;
            }
            finally
            {
                IsLoading = false;
            }

            var searchResult = new List<SearchItemViewModel>(seriesCollection.Count);
            var hideNonImageSearchResults = _settingsService.HideNonImageSearchResults;
            foreach (var series in seriesCollection)
            {
                // If search results without images should be hidden and the series does not have an image skip it
                if (hideNonImageSearchResults && string.IsNullOrWhiteSpace(series.BannerRemotePath)) continue;

                // Check if series already is an favorite
                var isFavorite = await _seriesRepository.IsFavoriteAsync(series.Id);

                searchResult.Add(_searchItemViewModelFactory.Create(series, isFavorite));
            }
            SearchResult = searchResult;

            // Load search result images
            var tasks = _searchResult.Select(vm => Task.Run(async () => await vm.LoadImage()));
            await Task.WhenAll(tasks);
        }

        #endregion

        #region ViewModel

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // Subscribe to event which invokes when the user changes app settings like the prefered series language or if search results without an image should be shown.
            // If one of these settings change the active search results will be updated with the new settings.
            _settingsService.SelectedLanguageChanged += OnSettingsServiceOnSelectedLanguageChanged;
            _settingsService.HideNonImageSearchResultsChanged += OnSettingsServiceOnHideNonImageSearchResultsChanged;

            var searchQuery = navigationParameter as string;
            if (!string.IsNullOrWhiteSpace(searchQuery) && (!string.Equals(SearchQuery, searchQuery, StringComparison.OrdinalIgnoreCase) || navigationMode == NavigationMode.Refresh))
            {
                await OnSearchExecuted(searchQuery);

                // Restore selected series
                object selectedItemId;
                if (viewModelState.TryGetValue(SelectedItemKey, out selectedItemId) && selectedItemId is uint)
                {
                    var searchVm = SearchResult.FirstOrDefault(vm => vm.Model.Id == (uint)selectedItemId);
                    if (searchVm != null)
                    {
                        SelectedItem = searchVm;
                    }
                }
            }

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            // Unsubscribe user setting changes
            _settingsService.SelectedLanguageChanged -= OnSettingsServiceOnSelectedLanguageChanged;
            _settingsService.HideNonImageSearchResultsChanged -= OnSettingsServiceOnHideNonImageSearchResultsChanged;

            // Save selected series
            if (_selectedItem != null)
            {
                viewModelState[SelectedItemKey] = _selectedItem.Model.Id;
            }

            base.OnNavigatedFrom(viewModelState, suspending);
        }

        #endregion

        private async void OnSettingsServiceOnHideNonImageSearchResultsChanged(object s, EventArgs e)
        {
            await UpdateSearchResults();
        }

        private async void OnSettingsServiceOnSelectedLanguageChanged(object s, EventArgs e)
        {
            await UpdateSearchResults();
        }

        private async Task UpdateSearchResults()
        {
            // If search cannot perform again return
            if (string.IsNullOrWhiteSpace(_searchQuery)) return;

            await OnSearchExecuted(_searchQuery);
        }
    }
}
