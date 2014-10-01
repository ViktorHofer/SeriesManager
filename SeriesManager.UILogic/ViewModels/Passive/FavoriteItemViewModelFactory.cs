using System;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.ViewModels.Passive
{
    public class FavoriteItemViewModelFactory
    {
        private readonly BannerViewModelFactory _bannerViewModelFactory;

        public FavoriteItemViewModelFactory(BannerViewModelFactory bannerViewModelFactory)
        {
            if (bannerViewModelFactory == null) throw new ArgumentNullException("bannerViewModelFactory");

            _bannerViewModelFactory = bannerViewModelFactory;
        }

        public FavoriteItemViewModel Create(Series model)
        {
            return new FavoriteItemViewModel(_bannerViewModelFactory,
                model);
        }
    }
}
