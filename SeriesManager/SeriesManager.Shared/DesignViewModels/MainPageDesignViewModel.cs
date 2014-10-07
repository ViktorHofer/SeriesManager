using Microsoft.Practices.Prism.Commands;
using SeriesManager.UILogic.ViewModels.Passive;
using System.Collections.ObjectModel;
using System.Linq;

namespace SeriesManager.DesignViewModels
{
    class MainPageDesignViewModel
    {
        public bool IsCommandBarOpen
        {
            get { return false; }
        }

        public ObservableCollection<FavoriteItemDesignViewModel> Favorites { get; private set; }

        public ObservableCollection<FavoriteItemDesignViewModel> SelectedItems { get; private set; }

        public DelegateCommand<FavoriteItemViewModel> ItemClickCommand { get; private set; }

        public DelegateCommand FavoriteCommand { get; private set; }

        public DelegateCommand ClearSelectionCommand { get; private set; }

        public DelegateCommand SelectAllCommand { get; private set; }

        public MainPageDesignViewModel()
        {
            var favoriteVms = Enumerable.Range(1, 6)
                .Select(number => new FavoriteItemDesignViewModel())
                .ToArray();

            Favorites = new ObservableCollection<FavoriteItemDesignViewModel>(favoriteVms);
            SelectedItems = new ObservableCollection<FavoriteItemDesignViewModel>(new[] { favoriteVms.First() });
            ItemClickCommand = null;
            FavoriteCommand = null;
            ClearSelectionCommand = null;
            SelectAllCommand = null;
        }
    }
}
