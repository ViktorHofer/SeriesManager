using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Threading.Tasks;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.ViewModels.Passive
{
    public class SearchItemViewModel : ViewModel
    {
        private bool _isFavorite;

        public Series Model { get; private set; }

        public BannerViewModel Banner { get; protected set; }

        public string Title
        {
            get 
            {
                var title = Model.Title;
                if (Model.FirstAired.HasValue)
                {
                    title += string.Format(" ({0})", Model.FirstAired.Value.Year);
                }

                return title;
            }
        }

        public bool IsFavorite
        {
            get { return _isFavorite; }
            internal set { base.SetProperty(ref _isFavorite, value); }
        }

        #region Constructor

        protected SearchItemViewModel(Series model, bool isFavorite)
        {
            if (model == null) throw new ArgumentNullException("model");

            Model = model;
            _isFavorite = isFavorite;
        }

        public SearchItemViewModel(
            BannerViewModelFactory bannerViewModelFactory, 
            Series model, 
            bool isFavorite)
            : this(model, isFavorite)
        {
            if (bannerViewModelFactory == null) throw new ArgumentNullException("bannerViewModelFactory");

            Banner = bannerViewModelFactory.Create(model.BannerRemotePath);
        }

        #endregion

        internal async Task LoadImage()
        {
            await Banner.LoadImage();
        }
    }
}
