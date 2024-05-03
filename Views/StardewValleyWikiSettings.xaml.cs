using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Flow.Launcher.Plugin.StardewValleyWiki.ViewModels;

namespace Flow.Launcher.Plugin.StardewValleyWiki.Views
{
    /// <summary>
    /// Interaction logic for StardewValleyWikiSettings.xaml
    /// </summary>
    public partial class StardewValleyWikiSettings : UserControl
    {
        private readonly SubDomainModel _viewModel;
        private PluginInitContext _context;
        private Settings _settings;
        private ObservableCollection<I18nSubDomain> _subDomains;

        public StardewValleyWikiSettings(SubDomainModel model)
        {
            _viewModel = model;
            _settings = model.Settings;
            DataContext = model;

            InitializeComponent();

            DataContext = model;
        }

        private void StardewValleyWikiSettings_Loaded(object sender, RoutedEventArgs e)
        {
            LanguageComboBox.SelectedItem = _viewModel.CurrentSubDomain();
        }
    }
}