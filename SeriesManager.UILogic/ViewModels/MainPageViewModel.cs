using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using SeriesManager.UILogic.Repositories;
using SeriesManager.UILogic.ViewModels.Passive;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UniRock;

namespace SeriesManager.UILogic.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        #region Fields

        private const string SelectedItemsKey = "selectedItems";
        private readonly INavigationService _navigationService;
        private readonly ISeriesRepository _seriesRepository;
        private readonly FavoriteItemViewModelFactory _favoriteItemViewModelFactory;
        private bool _isCommandBarOpen;

        #endregion

        #region Properties

        public bool IsCommandBarOpen
        {
            get { return _isCommandBarOpen; }
            set { base.SetProperty(ref _isCommandBarOpen, value); }
        }

        public ObservableCollection<FavoriteItemViewModel> Favorites { get; private set; }

        public ObservableCollection<FavoriteItemViewModel> SelectedItems { get; private set; }

        public DelegateCommand<FavoriteItemViewModel> ItemClickCommand { get; private set; }

        public DelegateCommand FavoriteCommand { get; private set; }

        public DelegateCommand ClearSelectionCommand { get; private set; }

        public DelegateCommand SelectAllCommand { get; private set; }

        #endregion

        #region Constructor

        public MainPageViewModel(INavigationService navigationService,
            ISeriesRepository seriesRepository,
            FavoriteItemViewModelFactory favoriteItemViewModelFactory)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");
            if (seriesRepository == null) throw new ArgumentNullException("seriesRepository");
            if (favoriteItemViewModelFactory == null) throw new ArgumentNullException("favoriteItemViewModelFactory");

            _navigationService = navigationService;
            _seriesRepository = seriesRepository;
            _favoriteItemViewModelFactory = favoriteItemViewModelFactory;

            ItemClickCommand = new DelegateCommand<FavoriteItemViewModel>(favoriteVm => _navigationService.Navigate("Series", favoriteVm.Series.Id));
            FavoriteCommand = new DelegateCommand(OnFavoriteExecuted, OnFavoriteCanExecute);
            ClearSelectionCommand = new DelegateCommand(OnClearSelectionExecuted, OnClearSelectionCanExecute);
            SelectAllCommand = new DelegateCommand(OnSelectAllExecuted, OnSelectAllCanExecute);

            SelectedItems = new ObservableCollection<FavoriteItemViewModel>();
            SelectedItems.CollectionChanged += (s, e) =>
            {
                FavoriteCommand.RaiseCanExecuteChanged();
                IsCommandBarOpen = SelectedItems.Count > 0;
                ClearSelectionCommand.RaiseCanExecuteChanged();
                SelectAllCommand.RaiseCanExecuteChanged();
            };

            var favoriteItemViewModels = seriesRepository.Favorites
                .Select(favoriteItemViewModelFactory.Create)
                .ToArray();

            Favorites = new ObservableCollection<FavoriteItemViewModel>(favoriteItemViewModels);

            _seriesRepository.FavoriteCollectionChanged += (s, e) =>
            {
                if (e.RemovedSeriesCollection != null)
                {
                    foreach (var favoriteVm in e.RemovedSeriesCollection.Select(series => Favorites.First(vm => vm.Series.Equals(series))))
                    {
                        Favorites.Remove(favoriteVm);
                    }
                }
                if (e.NewSeriesCollection != null)
                {
                    foreach (var series in e.NewSeriesCollection)
                    {
                        Favorites.Add(_favoriteItemViewModelFactory.Create(series));
                    }
                }
            };
        }

        #endregion

        #region Command Implementation

        private void OnFavoriteExecuted()
        {
            SelectedItems
                .AsParallel()
                .ForEach(async favoriteVm => await _seriesRepository.ChangeFavoriteAsync(favoriteVm.Series, false));
            SelectedItems.Clear();
        }

        private bool OnFavoriteCanExecute()
        {
            return SelectedItems.Any();
        }

        private void OnClearSelectionExecuted()
        {
            SelectedItems.Clear();
        }

        private bool OnClearSelectionCanExecute()
        {
            return SelectedItems.Any();
        }

        private void OnSelectAllExecuted()
        {
            var unselectedVMs = Favorites.Except(SelectedItems);
            unselectedVMs.ForEach(vm => SelectedItems.Add(vm));
        }

        private bool OnSelectAllCanExecute()
        {
            return SelectedItems.Count != Favorites.Count;
        }

        #endregion

        #region ViewModel

        public override async void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, System.Collections.Generic.Dictionary<string, object> viewModelState)
        {
            foreach (var favoriteVm in Favorites)
            {
                await favoriteVm.LoadImage();
            }

            object selectedItemIdsRaw;
            if (viewModelState.TryGetValue(SelectedItemsKey, out selectedItemIdsRaw) && selectedItemIdsRaw is string)
            {
                var selectedItemIds = ((string)selectedItemIdsRaw).FromJson<uint[]>();
                selectedItemIds.ForEach(id =>
                {
                    var favorite = Favorites.FirstOrDefault(fav => fav.Series.Id == id);
                    if (favorite != null)
                    {
                        SelectedItems.Add(favorite);
                    }
                });
            }
            
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public override void OnNavigatedFrom(System.Collections.Generic.Dictionary<string, object> viewModelState, bool suspending)
        {
            IsCommandBarOpen = false;

            if (SelectedItems.Any()) 
            {
                var selectedItemIdsRaw = SelectedItems
                    .Select(selectedItem => selectedItem.Series.Id)
                    .ToArray()
                    .ToJson();

                viewModelState[SelectedItemsKey] = selectedItemIdsRaw;
            }

            base.OnNavigatedFrom(viewModelState, suspending);
        }

        #endregion
    }
}
