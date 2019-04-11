using Glimpse.Core.Services.General;
using Glimpse.Core.Utility;
using Glimpse.Localization;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Glimpse.Core.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        private string _currentLanguage;
        private List<string> _languages;

        public SettingsViewModel(IMvxMessenger messenger) : base(messenger)
        {
          
        }

        
        public List<string> Languages
        {
            get
            {
                return _languages;
            }
            set
            {
                _languages = value;
                RaisePropertyChanged(() => Languages);
            }
        }


        public string CurrentLanguage
        {
            get
            {
                return _currentLanguage;
            }
            set
            {
                _currentLanguage = value;                              
                RaisePropertyChanged(() => CurrentLanguage);
                Settings.Language = _currentLanguage;
                if (_currentLanguage == "Français")
                {
                    var re = Mvx.GetSingleton<IMvxTextProvider>();
                    ((ResxTextProvider)re).ResourceManager = Strings_Fr.ResourceManager;
                }
                else if (_currentLanguage == "English")
                {
                    var re = Mvx.GetSingleton<IMvxTextProvider>();
                    ((ResxTextProvider)re).ResourceManager = Strings.ResourceManager;
                }
                SelectLanguageMessage = TextSource.GetText("SelectLanguageMsg");
               AboutMessage = TextSource.GetText("AboutContentMsg");
               
            }
        }

        private string _selectLanguageMessage;
        public string SelectLanguageMessage
        {
            get { return _selectLanguageMessage; }
            set
            {
                _selectLanguageMessage = value;
                RaisePropertyChanged(() => SelectLanguageMessage);
            }
        }

        private string _aboutMessage;
        public string AboutMessage
        {
            get { return _aboutMessage; }
            set
            {
                _aboutMessage = value;
                RaisePropertyChanged(() => AboutMessage);
            }
        }


        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        protected override Task InitializeAsync()
        {
            return Task.Run(() =>
            {
                //adding the current language in the settings to the list of languages first
                CurrentLanguage = Settings.Language;
                Languages = new List<string> { CurrentLanguage };
                // adding the remaining language
                if (CurrentLanguage == "English")
                    _languages.Add("Français");
                else if (CurrentLanguage == "Français")
                    _languages.Add("English");
            });
        }

        /// <summary>
        /// Triggered when the language is selected
        /// </summary>
        public MvxCommand SwitchLanguageCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                   
                });
            }
        }
    }
}