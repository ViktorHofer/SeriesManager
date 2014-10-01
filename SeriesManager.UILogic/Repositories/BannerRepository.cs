using SeriesManager.UILogic.Services;
using System;
using System.Threading.Tasks;
using TheTVDBSharp;

namespace SeriesManager.UILogic.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly IStorageService _storageService;
        private readonly ITheTVDBManager _theTvdbManager;

        public BannerRepository(IStorageService storageService, 
            ITheTVDBManager theTvdbManager)
        {
            if (storageService == null) throw new ArgumentNullException("storageService");
            if (theTvdbManager == null) throw new ArgumentNullException("theTvdbManager");

            _storageService = storageService;
            _theTvdbManager = theTvdbManager;
        }

        #region IBannerRepository

        public async Task<byte[]> Get(string remotePath)
        {
            var imageRaw = await _storageService.GetImageAsync(remotePath);
            if (imageRaw != null) return imageRaw;

            var bannerRaw = await _theTvdbManager.GetBanner(remotePath);
            await _storageService.SaveImageAsync(remotePath, bannerRaw);
            return bannerRaw;
        }

        #endregion
    }
}
