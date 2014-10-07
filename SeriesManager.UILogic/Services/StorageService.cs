using SeriesManager.UILogic.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheTVDBSharp.Models;
using Windows.Storage;

namespace SeriesManager.UILogic.Services
{
    public class StorageService : IStorageService
    {
        #region Fields

        private readonly Lazy<SQLiteAsyncConnection> _localDb = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection("series.sqlite"));
        private readonly Lazy<SQLiteAsyncConnection> _roamingDb = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(ApplicationData.Current.RoamingFolder.Path + "\\series.sqlite"));

        #endregion

        #region Properties

        private SQLiteAsyncConnection LocalDb 
        { 
            get { return _localDb.Value; } 
        }

        private SQLiteAsyncConnection RoamingDb 
        { 
            get { return _roamingDb.Value; } 
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Hide constructor, only allow creation with asynchronous "constructor"
        /// </summary>
        private StorageService()
        {
        }

        public static async Task<StorageService> Create()
        {
            var storageService = new StorageService();

            await storageService.LocalDb.CreateTablesAsync(
                typeof(SeriesDto),
                typeof(EpisodeDto),
                typeof(ActorDto),
                typeof(BannerDto),
                typeof(ImageDto));

            await storageService.RoamingDb.CreateTablesAsync(
                typeof(FavoriteDto),
                typeof(SearchHistoryDto));

            return storageService;
        }

        #endregion

        #region IStorageService

        public async Task<Series> GetSeriesAsync(uint seriesId)
        {
            var localDb = LocalDb;

            var seriesDto = await localDb
                .FindAsync<SeriesDto>(series => series.Id == seriesId);

            if (seriesDto == null) return null;

            var episodes = await localDb
                .Table<EpisodeDto>()
                .Where(episode => episode.SeriesId == seriesId)
                .ToListAsync();

            var actors = await localDb
                .Table<ActorDto>()
                .Where(actor => actor.SeriesId == seriesId)
                .ToListAsync();

            var banners = await localDb
                .Table<BannerDto>()
                .Where(banner => banner.SeriesId == seriesId)
                .ToListAsync();

            return seriesDto.ToDomain(episodes, actors, banners);
        }

        public async Task SaveSeriesAsync(Series series)
        {
            var localDb = LocalDb;
            var tuple = series.FromDomain();

            await localDb.InsertAsync(tuple.Item1);
            if (tuple.Item2 != null) await localDb.InsertAllAsync(tuple.Item2);
            if (tuple.Item3 != null) await localDb.InsertAllAsync(tuple.Item3);
            if (tuple.Item4 != null) await localDb.InsertAllAsync(tuple.Item4);
        }

        public async Task UpdateSeriesAsync(Series series)
        {
            var localDb = LocalDb;
            var tuple = series.FromDomain();

            await localDb.UpdateAsync(tuple.Item1);
            await localDb.UpdateAllAsync(tuple.Item2);
        }


        public async Task<bool> IsSeriesFavoriteAsync(uint seriesId)
        {
            var roamingDb = RoamingDb;

            var result = await roamingDb
                .FindAsync<FavoriteDto>(favoriteDto => favoriteDto.SeriesId == seriesId);

            return result != null;
        }

        public async Task SaveSearchHistoryAsync(string searchQuery)
        {
            var roamingDb = RoamingDb;

            await roamingDb.InsertAsync(new SearchHistoryDto { SearchQuery = searchQuery });
        }

        public async Task<IReadOnlyCollection<string>> GetSearchHistoryAsync()
        {
            var roamingDb = RoamingDb;

            var searchHistoryDtos = await roamingDb
                .Table<SearchHistoryDto>()
                .ToListAsync();

            return searchHistoryDtos
                .Select(searchHistoryDto => searchHistoryDto.SearchQuery)
                .ToArray();
        }

        public async Task SaveImageAsync(string remotePath, byte[] rawImage)
        {
            var localDb = LocalDb;
            var imageEntry = new ImageDto
            { 
                RemotePath = remotePath, 
                Image = Convert.ToBase64String(rawImage) 
            };

            await localDb.InsertAsync(imageEntry);
        }

        public async Task<byte[]> GetImageAsync(string remotePath)
        {
            var localDb = LocalDb;

            var dto = await localDb.FindAsync<ImageDto>(img => img.RemotePath == remotePath);
            if (dto != null)
            {
                return Convert.FromBase64String(dto.Image);
            }

            return null;
        }

        public async Task<IReadOnlyCollection<uint>> GetFavoritesAsync()
        {
            var roamingDb = RoamingDb;

            var favoriteDtos = await roamingDb
                .Table<FavoriteDto>()
                .ToListAsync();

            return favoriteDtos
                .Select(favoriteDto => favoriteDto.SeriesId)
                .ToArray();
        }


        public async Task SaveSeriesFavoriteAsync(uint seriesId)
        {
            var roamingDb = RoamingDb;

            await roamingDb.InsertAsync(new FavoriteDto { SeriesId = seriesId });
        }

        public async Task RemoveSeriesFavoriteAsync(uint seriesId)
        {
            var roamingDb = RoamingDb;

            await roamingDb.DeleteAsync(new FavoriteDto { SeriesId = seriesId });
        }

        #endregion
    }
}
