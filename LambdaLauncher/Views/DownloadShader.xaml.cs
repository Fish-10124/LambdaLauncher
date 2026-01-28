using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using LambdaLauncher.ViewModels.DownloadResourceModel;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Record;

namespace LambdaLauncher.Views;

public sealed partial class DownloadShader : Page
{
    public DownloadShaderModel ViewModel { get; set; } = new();

    public DownloadShader()
    {
        this.InitializeComponent();
        _ = ViewModel.SearchAsync();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        App.NavigationService.SetHeader(
            new PageHeader()
            {
                Page = typeof(DownloadShader),
                Text = Utils.ResourceLoader.GetString("Header-ShaderPack")
            }
        );
    }
}
