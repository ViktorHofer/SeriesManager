using System;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.Services
{
    public interface ISettingsService
    {
        event EventHandler<EventArgs> SelectedLanguageChanged;

        event EventHandler<EventArgs> HideNonImageSearchResultsChanged;

        Language SelectedLanguage { get; set; }

        bool HideNonImageSearchResults { get; set; }
    }
}
