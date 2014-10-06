using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SeriesManager
{
    public sealed partial class SplashPage
    {
        private readonly SplashScreen _splashScreen;

        public SplashPage(SplashScreen splashScreen)
        {
            _splashScreen = splashScreen;

            InitializeComponent();

            SizeChanged += ExtendedSplashScreen_SizeChanged;
            splashImage.ImageOpened += splashImage_ImageOpened;
        }

        void splashImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            // The application's window should not become activate until the extended splash screen is ready to be shown 
            // in order to prevent flickering when switching between the real splash screen and this one.
            // In order to do this we need to be sure that the image was opened so we subscribed to
            // this event and activate the window in it.

            Resize();
            Window.Current.Activate();
        }

        // Whenever the size of the application change, the image position and size need to be recalculated.
        void ExtendedSplashScreen_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resize();
        }

        // This method is used to position and resizing the splash screen image correctly.
        private void Resize()
        {
            if (_splashScreen == null) return;

            // The splash image's not always perfectly centered. Therefore we need to set our image's position 
            // to match the original one to obtain a clean transition between both splash screens.

            splashImage.Height = _splashScreen.ImageLocation.Height;
            splashImage.Width = _splashScreen.ImageLocation.Width;

            splashImage.SetValue(Canvas.TopProperty, _splashScreen.ImageLocation.Top);
            splashImage.SetValue(Canvas.LeftProperty, _splashScreen.ImageLocation.Left);

            progressRing.SetValue(Canvas.TopProperty, _splashScreen.ImageLocation.Top + _splashScreen.ImageLocation.Height + 50);
            progressRing.SetValue(Canvas.LeftProperty, _splashScreen.ImageLocation.Left + _splashScreen.ImageLocation.Width / 2 - progressRing.Width / 2);
        }
    }
}
