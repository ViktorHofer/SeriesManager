using System;
using System.Threading.Tasks;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.ViewModels.Passive
{
    public class FavoriteItemViewModel
    {
        internal readonly Series Series;

        public BannerViewModel Poster { get; protected set; }

        public BannerViewModel Banner { get; protected set; }

        public bool IsWatched { get; protected set; }

        #region Constructor

        protected FavoriteItemViewModel(Series series)
        {
            if (series == null) throw new ArgumentNullException("series");

            Series = series;
        }

        public FavoriteItemViewModel(
            BannerViewModelFactory bannerViewModelFactory,
            Series series)
            : this(series)
        {
            if (bannerViewModelFactory == null) throw new ArgumentNullException("bannerViewModelFactory");

            Poster = bannerViewModelFactory.Create(series.PosterRemotePath);
            Banner = bannerViewModelFactory.Create(series.BannerRemotePath);
        }

        #endregion

        internal async Task LoadImage()
        {
            await Task.WhenAll(Banner.LoadImage(), 
                Poster.LoadImage());
        }
    }
}
