using System.Globalization;
using System.Resources;
using MvvmCross.Localization;

namespace Glimpse.Core.Utility
{
    public class ResxTextProvider : IMvxTextProvider
    {
        private ResourceManager _resourceManager;

        public ResxTextProvider(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            CurrentLanguage = new CultureInfo("");
        }

        public CultureInfo CurrentLanguage { get; set; }
        public ResourceManager ResourceManager
        {
            get
            {
                return _resourceManager;
            }
            set
            {
                _resourceManager = value;
            }
        }
        public string GetText(string namespaceKey,
            string typeKey, string name)
        {
            string resolvedKey = name;

            if (!string.IsNullOrEmpty(typeKey))
            {
                resolvedKey = $"{typeKey}.{resolvedKey}";
            }

            if (!string.IsNullOrEmpty(namespaceKey))
            {
                resolvedKey = $"{namespaceKey}.{resolvedKey}";
            }

            return _resourceManager.GetString(resolvedKey, CurrentLanguage);
        }

        public string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            string baseText = GetText(namespaceKey, typeKey, name);

            if (string.IsNullOrEmpty(baseText))
            {
                return baseText;
            }

            return string.Format(baseText, formatArgs);
        }
    }
}
