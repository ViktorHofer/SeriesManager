using System.ComponentModel;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.Services
{
    public interface ISettingsService : INotifyPropertyChanged
    {
        Language SelectedLanguage { get; set; }

        bool HideNonImageSearchResults { get; set; }
    }
}
