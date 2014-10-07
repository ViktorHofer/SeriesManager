using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using Windows.UI.Xaml.Media;

namespace SeriesManager.UILogic.ViewModels
{
    public class FrameViewModel : ViewModel
    {
        #region Fields

        private readonly INavigationService _navigationService;
        private bool _navigationBarIsOpen;
        private ImageSource _background;

        #endregion

        #region Properties

        public bool NavigationBarIsOpen
        {
            get { return _navigationBarIsOpen; }
            set { base.SetProperty(ref _navigationBarIsOpen, value); }
        }

        public ImageSource Background
        {
            get { return _background; }
            private set { base.SetProperty(ref _background, value); }
        }

        public DelegateCommand HomeCommand { get; private set; }

        public DelegateCommand<string> SearchCommand { get; private set; }

        #endregion

        #region Constructor

        public FrameViewModel(INavigationService navigationService)
        {
            if (navigationService == null) throw new ArgumentNullException("navigationService");

            _navigationService = navigationService;

            HomeCommand = new DelegateCommand(() => _navigationService.Navigate("Main", null));
            SearchCommand = new DelegateCommand<string>(searchQuery => _navigationService.Navigate("Search", searchQuery));
        }

        #endregion
    }
}
