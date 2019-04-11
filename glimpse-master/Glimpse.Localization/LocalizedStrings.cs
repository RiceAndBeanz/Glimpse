namespace Glimpse.Localization
{
    public class LocalizedStrings
    {
        public LocalizedStrings() {}

        private static readonly Strings LocalizedStringsResources 
            = new Strings();

        public Strings Strings => LocalizedStringsResources;
    }
}
