using Windows.UI.Xaml.Media;

namespace SeriesManager.DesignViewModels
{
    class BannerDesignViewModel
    {
        public ImageSource Image { get; private set; }

        public BannerDesignViewModel()
        {
            Image = null;
        }
    }
}
