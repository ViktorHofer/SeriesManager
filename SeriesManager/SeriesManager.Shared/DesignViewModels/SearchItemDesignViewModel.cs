using SeriesManager.UILogic.ViewModels.Passive;
using TheTVDBSharp.Models;

namespace SeriesManager.DesignViewModels
{
    class SearchItemDesignViewModel : SearchItemViewModel
    {
        public SearchItemDesignViewModel(Series model)
            : base(model, true)
        {
            Banner = new BannerDesignViewModel();
        }
    }
}
