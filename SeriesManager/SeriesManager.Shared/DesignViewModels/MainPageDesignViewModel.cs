using SeriesManager.UILogic.ViewModels;
using SeriesManager.UILogic.ViewModels.Passive;
using System.Collections.ObjectModel;
using System.Linq;
using TheTVDBSharp.Models;

namespace SeriesManager.DesignViewModels
{
    class MainPageDesignViewModel : MainPageViewModel
    {
        public MainPageDesignViewModel()
            : base()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                var favoriteVMs = new Series[]
                {
                    new Series(0)
                    {

                    },
                    new Series(1)
                    {

                    },
                    new Series(3)
                    {

                    }
                }.Select(series => new FavoriteItemDesignViewModel(series));
                this.Favorites = new ObservableCollection<FavoriteItemViewModel>(favoriteVMs);
            }
        }
    }
}
