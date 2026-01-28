using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using LambdaLauncher.ViewModels.DownloadResourceModel;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Record;

namespace LambdaLauncher.Views;

public sealed partial class DownloadResourcePack : Page
{
    public DownloadResourcePackModel ViewModel { get; set; } = new();

    public DownloadResourcePack()
    {
        this.InitializeComponent();
        _ = ViewModel.SearchAsync();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        App.NavigationService.SetHeader(
            new PageHeader()
            {
                Page = typeof(DownloadResourcePack),
                Text = Utils.ResourceLoader.GetString("Header-ResourcePack")
            }
        );
    }
}
