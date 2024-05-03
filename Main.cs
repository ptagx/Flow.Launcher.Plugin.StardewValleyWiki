using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Flow.Launcher.Plugin.SharedCommands;
using Flow.Launcher.Plugin.StardewValleyWiki.ViewModels;
using Flow.Launcher.Plugin.StardewValleyWiki.Views;

namespace Flow.Launcher.Plugin.StardewValleyWiki
{
    public partial class StardewValleyWiki : IAsyncPlugin, ISettingProvider
    {
        private PluginInitContext _context;
        private Settings _settings;
        private SubDomainModel _viewModel;
        private string text_query_url;
        private string title_query_url;
        private string jsonResult;
        private string finalUrl;

        public Control CreateSettingPanel()
        {
            return new StardewValleyWikiSettings(_viewModel);
        }
        public async Task InitAsync(PluginInitContext context)
        {
            _context = context;
            _settings = context.API.LoadSettingJsonStorage<Settings>();
            _viewModel = new SubDomainModel(_context, _settings);
            title_query_url = "/mediawiki/api.php?action=query&list=search&srwhat=title&format=json&srsearch=";
            text_query_url = "/mediawiki/api.php?action=query&list=search&srwhat=text&format=json&srsearch=";
        }

        public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            string qs, query_url;
            string base_url = _viewModel.Url();
            Match m = MyRegex().Match(query.Search);
            if (m.Success) {
                qs = MyRegex1().Replace(query.Search, String.Empty);
                if (m.Groups[1].Value == "text")
                    query_url = base_url + text_query_url;
                else
                    query_url = base_url + title_query_url;
            } else {
                qs = query.Search;
                query_url = base_url + text_query_url;
            }
            using (var httpClient = new HttpClient()) {
                jsonResult = await httpClient.GetStringAsync(query_url + qs, token);
            }
            dynamic data = JsonNode.Parse(jsonResult);
            JsonObject jobj = JsonNode.Parse(jsonResult).AsObject();
            if (jobj.ContainsKey("error")) {
                var noResults = new List<Result> {
                    new() {
                        Title = $"Start typing to search the wiki...",
                        IcoPath = "icon.png"
                    }
                };
                return await Task.FromResult(noResults);
            }
            JsonArray jary = data["query"]["search"];
            if (jary.Count == 0) {
                var noResults = new List<Result> {
                    new() {
                        Title = $"No matches. try another query...",
                        IcoPath = "icon.png"
                    }
                };
                return await Task.FromResult(noResults);
            }
            var results = new List<Result>();
            foreach (var item in data["query"]["search"]) {
                string itemStr = item["title"].ToString();
                string itemWithunderscores = itemStr.Replace(" ", "_");

                results.Add(new Result {
                    Title = $"{itemStr}",
                    SubTitle = $"{base_url}" + "/" + itemWithunderscores,
                    Action = e => {
                        finalUrl = base_url + "/" + itemWithunderscores;
                        finalUrl.OpenInBrowserTab();
                        return true;
                    },
                    IcoPath = "icon.png"
                });
            }
            return await Task.FromResult(results);
        }

        [GeneratedRegex("^\\s*(text|title)\\s*")]
        private static partial Regex MyRegex();
        [GeneratedRegex("^\\s*(?:text|title)\\s*")]
        private static partial Regex MyRegex1();
    }
}