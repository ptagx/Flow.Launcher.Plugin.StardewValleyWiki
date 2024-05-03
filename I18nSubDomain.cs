namespace Flow.Launcher.Plugin.StardewValleyWiki;

public class I18nSubDomain
{
    public I18nSubDomain(string language, string langcode)
    {
        this.Language = language;
        this.Langcode = langcode;
        if (langcode == "en" || langcode == "") {
            this.Url = "https://stardewvalleywiki.com";
        } else {
            this.Url = $"https://{langcode}.stardewvalleywiki.com";
        }
    }

    public string Language { get; }
    public string Langcode { get; }
    public string Url { get; }
}
