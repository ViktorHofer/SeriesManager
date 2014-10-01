using System;
using System.Collections.Generic;
using TheTVDBSharp.Models;

namespace SeriesManager.DesignViewModels
{
    class SettingsControlDesignViewModel
    {
        public string AppVersion
        {
            get { return "1.0.0.0"; }
        }

        public bool HideNonImageSearchResults { get; set; }

        public IReadOnlyCollection<Language> Languages
        {
            get { return (Language[])Enum.GetValues(typeof(Language)); }
        }

        public Language SelectedLanguage { get; set; }

        public bool IsSeriesLanguagesUpdating
        {
            get { return true; }
        }

        public SettingsControlDesignViewModel()
        {
            SelectedLanguage = Language.English;
            HideNonImageSearchResults = true;
        }
    }
}
