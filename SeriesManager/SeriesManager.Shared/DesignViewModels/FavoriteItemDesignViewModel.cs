using System;

namespace SeriesManager.DesignViewModels
{
    class FavoriteItemDesignViewModel
    {
        public BannerDesignViewModel Poster { get; private set; }

        public BannerDesignViewModel Banner { get; private set; }

        public bool IsWatched
        {
            get { return new Random().Next(0, 1) == 0; }
        }

        public FavoriteItemDesignViewModel()
        {
            Poster = new BannerDesignViewModel();
            Banner = new BannerDesignViewModel();
        }
    }
}
