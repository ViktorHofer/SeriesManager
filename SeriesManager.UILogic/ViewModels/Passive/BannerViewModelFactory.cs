using SeriesManager.UILogic.Repositories;
using System;

namespace SeriesManager.UILogic.ViewModels.Passive
{
    public class BannerViewModelFactory
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerViewModelFactory(IBannerRepository bannerRepository)
        {
            if (bannerRepository == null) throw new ArgumentNullException("bannerRepository");

            _bannerRepository = bannerRepository;
        }

        public BannerViewModel Create(string remotePath)
        {
            return new BannerViewModel(_bannerRepository, remotePath);
        }
    }
}
