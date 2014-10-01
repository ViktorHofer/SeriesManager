using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.Repositories
{
    public interface ISeriesRepository
    {
        event EventHandler<FavoriteEventArgs> FavoriteCollectionChanged;

        IEnumerable<Series> Favorites { get; }

        Task<IReadOnlyCollection<Series>> SearchAsync(string searchQuery);

        Task<Series> GetSeriesAsync(uint seriesId);

        Task LoadFavoritesAsync();

        Task ChangeFavoriteAsync(Series series, bool isFavorite);

        Task<bool> IsFavoriteAsync(uint seriesId);

        Task UpdateFavoriteLanguagesAsync(Language newLanguage);
    }
}
