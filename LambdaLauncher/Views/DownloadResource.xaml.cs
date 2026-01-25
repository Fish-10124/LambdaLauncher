using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLaunch.Base.Enums;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Record;
using LambdaLauncher.ViewModels.DownloadResourceModel;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DownloadResource : Page
{
    public DownloadResourceModel ViewModel { get; set; } = null!;
    public ResourceType Type { get; set; }

    public DownloadResource()
    {
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        Type = (ResourceType)e.Parameter;
        (ViewModel, string header) = Type switch
        {
            ResourceType.Mod => ((DownloadResourceModel)new DownloadModModel(this), "Header-Mod"),
            ResourceType.Modpack => (new DownloadModPackModel(this), "Header-ModPack"),
            ResourceType.Resourcepack => (new DownloadResourcePackModel(this), "Header-ResourcePack"),
            ResourceType.Shaderpack => (new DownloadShaderModel(this), "Header-ShaderPack"),
            _ => throw new NotImplementedException()
        };

        App.BreadcrumbService.SetHeader(
            new PageHeader()
            {
                Page = typeof(DownloadResource),
                Text = Utils.ResourceLoader.GetString(header)
            }
        );

        await ViewModel.SearchLastAsync();
    }
}