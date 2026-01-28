using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using LambdaLauncher.ViewModels.DownloadResourceModel;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Record;

namespace LambdaLauncher.Views;

public sealed partial class DownloadModpack : Page
{
    public DownloadModPackModel ViewModel { get; set; } = new();

    public DownloadModpack()
    {
        this.InitializeComponent();
        _ = ViewModel.SearchAsync();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        App.NavigationService.SetHeader(
            new PageHeader()
            {
                Page = typeof(DownloadModpack),
                Text = Utils.ResourceLoader.GetString("Header-ModPack")
            }
        );
    }
}
