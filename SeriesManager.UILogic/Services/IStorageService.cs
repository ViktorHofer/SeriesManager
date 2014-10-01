using System.Collections.Generic;
using System.Threading.Tasks;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.Services
{
    public interface IStorageService
    {
        Task<Series> GetSeriesAsync(uint seriesId);

        Task SaveSeriesAsync(Series series);

        Task UpdateSeriesAsync(Series series);

        Task SaveSeriesFavoriteAsync(uint seriesId);

        Task RemoveSeriesFavoriteAsync(uint seriesId);

        Task<bool> IsSeriesFavoriteAsync(uint seriesId);

        Task SaveSearchHistoryAsync(string searchQuery);

        Task<IReadOnlyCollection<string>> GetSearchHistoryAsync();

        Task SaveImageAsync(string remotePath, byte[] rawImage);

        Task<byte[]> GetImageAsync(string remotePath);

        Task<IReadOnlyCollection<uint>> GetFavoritesAsync();
    }
}
