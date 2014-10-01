using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SeriesManager
{
    public sealed partial class SplashPage : Page
    {
        private readonly SplashScreen splashScreen;

        public SplashPage(SplashScreen splashScreen)
        {
            this.splashScreen = splashScreen;

            this.InitializeComponent();

            this.SizeChanged += ExtendedSplashScreen_SizeChanged;
            this.splashImage.ImageOpened += splashImage_ImageOpened;
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
            this.Resize();
        }

        // This method is used to position and resizing the splash screen image correctly.
        private void Resize()
        {
            if (this.splashScreen == null) return;

            // The splash image's not always perfectly centered. Therefore we need to set our image's position 
            // to match the original one to obtain a clean transition between both splash screens.

            this.splashImage.Height = this.splashScreen.ImageLocation.Height;
            this.splashImage.Width = this.splashScreen.ImageLocation.Width;

            this.splashImage.SetValue(Canvas.TopProperty, this.splashScreen.ImageLocation.Top);
            this.splashImage.SetValue(Canvas.LeftProperty, this.splashScreen.ImageLocation.Left);

            this.progressRing.SetValue(Canvas.TopProperty, this.splashScreen.ImageLocation.Top + this.splashScreen.ImageLocation.Height + 50);
            this.progressRing.SetValue(Canvas.LeftProperty, this.splashScreen.ImageLocation.Left + this.splashScreen.ImageLocation.Width / 2 - this.progressRing.Width / 2);
        }
    }
}
