using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Record;
using LambdaLauncher.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DownloadInstance : Page
{
    public DownloadInstanceModel ViewModel { get; private set; }

    public DownloadInstance()
    {
        ViewModel = new(this);
        this.DataContext = ViewModel;
        _ = ViewModel.InitAsync();

        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        App.NavigationService.SetHeader(
            new PageHeader()
            {
                Page = typeof(DownloadInstance), 
                Text = Utils.ResourceLoader.GetString("Header-Instance")
            }
        );
    }

    private void filterComboBox_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var combo = (ComboBox)sender; 
        if (combo.SelectedIndex < 0 && combo.Items.Count > 0) 
        { 
            combo.SelectedIndex = 0;
        }
    }
}