using LambdaLauncher.Models;
using LambdaLauncher.Models.Record;
using LambdaLauncher.ViewModels.DownloadResourceModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace LambdaLauncher.Views;

public sealed partial class DownloadMod : Page
{
    public DownloadModModel ViewModel { get; set; } = new();

    public DownloadMod()
    {
        this.InitializeComponent();
        _ = ViewModel.SearchAsync();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        App.NavigationService.SetHeader(
            new PageHeader()
            {
                Page = typeof(DownloadMod),
                Text = Utils.ResourceLoader.GetString("Header-Mod")
            }
        );
    }
}
