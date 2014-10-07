using System;
using TheTVDBSharp.Models;

namespace SeriesManager.DesignViewModels
{
    class SearchItemDesignViewModel
    {
        public Series Model { get; private set; }

        public BannerDesignViewModel Banner { get; private set; }

        public string Title
        {
            get { return Model.Title; }
        }

        public bool IsFavorite
        {
            get { return new Random().Next(0, 1) == 0; }
        }

        public SearchItemDesignViewModel(Series model)
        {
            if (model == null) throw new ArgumentNullException("model");

            Model = model;
            Banner = new BannerDesignViewModel();
        }
    }
}
