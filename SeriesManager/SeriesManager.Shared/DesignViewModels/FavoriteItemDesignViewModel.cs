using SeriesManager.UILogic.ViewModels.Passive;
using TheTVDBSharp.Models;

namespace SeriesManager.DesignViewModels
{
    class FavoriteItemDesignViewModel : FavoriteItemViewModel
    {
        public FavoriteItemDesignViewModel(Series series)
            : base(series)
        {
            Poster = new BannerDesignViewModel();
            Banner = new BannerDesignViewModel();
            IsWatched = true;
        }
    }
}
