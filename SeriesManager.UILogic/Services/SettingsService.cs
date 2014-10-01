using System.ComponentModel;
using TheTVDBSharp.Models;
using Windows.Storage;
using System.Linq;
using System;
using System.Runtime.CompilerServices;

namespace SeriesManager.UILogic.Services
{
    public class SettingsService : ISettingsService
    {
        private const string SelectLanguageKey = "selected-language";
        private const string HideNonImageSearchResultsKey = "hide-non-image-search-results";
        private readonly ApplicationDataContainer _applicationData = ApplicationData.Current.RoamingSettings;
        private Language _selectedLanguage;
        private bool _hideNonImageSearchResults;

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region ISettingsService

        public Language SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                if (_selectedLanguage == value) return;

                _applicationData.Values[SelectLanguageKey] = value.ToString();
                _selectedLanguage = value;
                OnPropertyChanged();
            }
        }

        public bool HideNonImageSearchResults
        {
            get { return _hideNonImageSearchResults; }
            set
            {
                if (_hideNonImageSearchResults == value) return;

                _applicationData.Values[HideNonImageSearchResultsKey] = value;
                _hideNonImageSearchResults = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Constructor

        public SettingsService()
        {
            Language selectedLanguage;
            object selectedLanguageRaw;
            if (_applicationData.Values.TryGetValue(SelectLanguageKey, out selectedLanguageRaw) && 
                selectedLanguageRaw is string && 
                Enum.TryParse(selectedLanguageRaw as string, out selectedLanguage))
            {
                _selectedLanguage = selectedLanguage;
            }
            else
            {
                _selectedLanguage = Windows.Globalization.ApplicationLanguages.Languages.First().StartsWith("de") ? 
                    Language.Deutsch : 
                    Language.English;
            }

            object hideNonImageSearchResults;
            if (_applicationData.Values.TryGetValue(HideNonImageSearchResultsKey, out hideNonImageSearchResults) && hideNonImageSearchResults is bool)
            {
                _hideNonImageSearchResults = (bool)hideNonImageSearchResults;
            }
            else
            {
                _hideNonImageSearchResults = true;
            }
        }

        #endregion

        private void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
