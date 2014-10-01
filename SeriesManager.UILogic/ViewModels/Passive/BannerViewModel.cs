using Microsoft.Practices.Prism.Mvvm;
using SeriesManager.UILogic.Repositories;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using TheTVDBSharp.Services;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SeriesManager.UILogic.ViewModels.Passive
{
    public class BannerViewModel : ViewModel
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly string _remotePath;
        private readonly SynchronizationContext _syncContext;
        private ImageSource _image;

        public ImageSource Image
        {
            get { return _image; }
            private set { base.SetProperty(ref _image, value); }
        }

        #region Constructor

        protected BannerViewModel()
        {

        }

        public BannerViewModel(IBannerRepository bannerRepository, 
            string remotePath)
            : this()
        {
            if (bannerRepository == null) throw new ArgumentNullException("bannerRepository");

            _bannerRepository = bannerRepository;
            _remotePath = remotePath;
            _syncContext = SynchronizationContext.Current;
        }

        #endregion

        internal async Task LoadImage()
        {
            if (string.IsNullOrWhiteSpace(_remotePath) || _image != null) return;

            byte[] byteRaw;

            try
            {
                byteRaw = await _bannerRepository.Get(_remotePath);
            }
            catch (BadResponseException)
            {
                return;
            }
            catch (ServerNotAvailableException)
            {
                return;
            }

            _syncContext.Post(async state =>
            {
                using (var stream = new InMemoryRandomAccessStream())
                {
                    var bitmapImage = new BitmapImage();
                    await stream.WriteAsync(byteRaw.AsBuffer());
                    stream.Seek(0);

                    bitmapImage.SetSource(stream);
                    Image = bitmapImage;
                }
            }, null);
        }
    }
}
