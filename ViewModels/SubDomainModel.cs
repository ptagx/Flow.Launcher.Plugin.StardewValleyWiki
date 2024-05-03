using System.Collections.ObjectModel;

namespace Flow.Launcher.Plugin.StardewValleyWiki.ViewModels
{
    public class SubDomainModel : BaseModel
    {
        private readonly ObservableCollection<I18nSubDomain> _subDomains = new();
        public SubDomainModel(PluginInitContext context, Settings settings)
        {
            Context = context;
            Settings = settings;
            _subDomains.Add(new I18nSubDomain("English", "en"));
            _subDomains.Add(new I18nSubDomain("Deutsch", "de"));
            _subDomains.Add(new I18nSubDomain("Español", "es"));
            _subDomains.Add(new I18nSubDomain("Français", "fr"));
            _subDomains.Add(new I18nSubDomain("Magyar", "hu"));
            _subDomains.Add(new I18nSubDomain("Italiano", "it"));
            _subDomains.Add(new I18nSubDomain("日本語", "ja"));
            _subDomains.Add(new I18nSubDomain("한국어", "ko"));
            _subDomains.Add(new I18nSubDomain("Português", "pt"));
            _subDomains.Add(new I18nSubDomain("Русский", "ru"));
            _subDomains.Add(new I18nSubDomain("Türkçe", "tr"));
            _subDomains.Add(new I18nSubDomain("中文", "zh"));
        }

        public Settings Settings { get; init; }
        internal PluginInitContext Context { get; set; }
        public ObservableCollection<I18nSubDomain> SubDomains { get { return _subDomains; } }

        public string Url()
        {
            I18nSubDomain s = CurrentSubDomain();

            return s.Url;
        }

        public I18nSubDomain CurrentSubDomain()
        {
            foreach (var i in _subDomains) {
                if (i.Langcode == Settings.LanguageCode) {
                    return i;
                }
            }
            return _subDomains[0];
        }

        public void Save()
        {
            Context.API.SaveSettingJsonStorage<Settings>();
        }
    }
}