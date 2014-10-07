using System;
using System.Threading.Tasks;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.ViewModels.Passive
{
    public class FavoriteItemViewModel
    {
        #region Fields

        internal readonly Series Series;

        #endregion

        #region Properties

        public BannerViewModel Poster { get; private set; }

        public BannerViewModel Banner { get; private set; }

        // TODO: set watched state
        public bool IsWatched { get; private set; }

        #endregion

        #region Constructor

        public FavoriteItemViewModel(
            BannerViewModelFactory bannerViewModelFactory,
            Series series)
        {
            if (bannerViewModelFactory == null) throw new ArgumentNullException("bannerViewModelFactory");
            if (series == null) throw new ArgumentNullException("series");

            Series = series;
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
