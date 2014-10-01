using System;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.ViewModels.Passive
{
    public class SearchItemViewModelFactory
    {
        private readonly BannerViewModelFactory _bannerViewModelFactory;

        public SearchItemViewModelFactory(BannerViewModelFactory bannerViewModelFactory)
        {
            if (bannerViewModelFactory == null) throw new ArgumentNullException("bannerViewModelFactory");

            _bannerViewModelFactory = bannerViewModelFactory;
        }

        public SearchItemViewModel Create(Series model, bool isFavorite)
        {
            if (model == null) throw new ArgumentNullException("model");

            return new SearchItemViewModel(_bannerViewModelFactory,
                model,
                isFavorite);
        }
    }
}
