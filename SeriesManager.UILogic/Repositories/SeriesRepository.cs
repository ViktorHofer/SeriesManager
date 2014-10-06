using SeriesManager.UILogic.Models;
using SeriesManager.UILogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheTVDBSharp;
using TheTVDBSharp.Models;
using TheTVDBSharp.Services;
using UniRock;

namespace SeriesManager.UILogic.Repositories
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly IStorageService _storageService;
        private readonly ITheTvdbManager _theTvdbManager;
        private readonly ISettingsService _settingsService;
        private HashSet<Series> _favorites;

        #region Constructor

        public SeriesRepository(IStorageService storageService, 
            ITheTvdbManager theTvdbManager,
            ISettingsService settingsService)
        {
            if (storageService == null) throw new ArgumentNullException("storageService", "StorageService cannot be null");
            if (theTvdbManager == null) throw new ArgumentNullException("theTvdbManager", "TheTvdbManager cannot be null");
            if (settingsService == null) throw new ArgumentNullException("settingsService", "SettingsService cannot be null");

            _storageService = storageService;
            _theTvdbManager = theTvdbManager;
            _settingsService = settingsService;
        }

        #endregion

        #region ISeriesRepository

        public event EventHandler<FavoriteEventArgs> FavoriteCollectionChanged;

        public IEnumerable<Series> Favorites
        {
            get { return _favorites.AsEnumerable(); }
        }

        public async Task<IReadOnlyCollection<Series>> SearchAsync(string searchQuery)
        {
            // Search web request to api; result can be null
            var searchResult = await _theTvdbManager.SearchSeries(searchQuery, _settingsService.SelectedLanguage);

            // Save search result in database
            await _storageService.SaveSearchHistoryAsync(searchQuery);

            return searchResult
                .Where(series => series.Language == _settingsService.SelectedLanguage)
                .ToArray();
        }

        public async Task<Series> GetSeriesAsync(uint seriesId)
        {
            if (_favorites != null)
            {
                var favorite = _favorites.FirstOrDefault(fav => fav.Id == seriesId);
                if (favorite != null) return favorite;
            }

            var series = await _storageService.GetSeriesAsync(seriesId);
            if (series == null)
            {
                series = await _theTvdbManager.GetSeries(seriesId, _settingsService.SelectedLanguage);
                await _storageService.SaveSeriesAsync(series);
            }

            return series;
        }

        public async Task LoadFavoritesAsync()
        {
            var favoriteIds = await _storageService.GetFavoritesAsync();

            var tasks = favoriteIds
                .Select(favoriteId => Task.Run(async () => await GetSeriesAsync(favoriteId)))
                .ToArray();

            await Task.WhenAll(tasks);

            var array = tasks
                .Select(task => task.Result)
                .ToArray();

            _favorites = new HashSet<Series>(array);
        }

        public async Task ChangeFavoriteAsync(Series series, bool isFavorite)
        {
            if (Favorites == null)
            {
                throw new InvalidOperationException("Favorite collection cannot be null. Call LoadFavorites method first.");
            }

            if (isFavorite)
            {
                await _storageService.SaveSeriesFavoriteAsync(series.Id);
                var fullSeries = await GetSeriesAsync(series.Id);
                if (_favorites.Add(fullSeries))
                {
                    FavoriteCollectionChanged.Raise(this, new FavoriteEventArgs(new[] { fullSeries }, null));
                }
            }
            else
            {
                await _storageService.RemoveSeriesFavoriteAsync(series.Id);
                if (_favorites.RemoveWhere(favorite => favorite.Id == series.Id) == 1)
                {
                    FavoriteCollectionChanged.Raise(this, new FavoriteEventArgs(null, new[] { series }));
                }
            }
        }

        public async Task<bool> IsFavoriteAsync(uint seriesId)
        {
            return await _storageService.IsSeriesFavoriteAsync(seriesId);
        }

        public async Task UpdateFavoriteLanguagesAsync(Language newLanguage)
        {
            var tasks = _favorites
                .Select(series => Task.Run(async () =>
                {
                    Series updatedSeries;

                    try
                    {
                        updatedSeries = await _theTvdbManager.GetSeries(series.Id, newLanguage, false);
                    }
                    catch (BadResponseException)
                    {
                        return;
                    }
                    catch (ServerNotAvailableException)
                    {
                        return;
                    }

                    // If updated series could not be parsed (is null) return
                    if (updatedSeries == null) return;

                    series.Populate(updatedSeries);
                    await _storageService.UpdateSeriesAsync(series);
                }))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        #endregion
    }
}
