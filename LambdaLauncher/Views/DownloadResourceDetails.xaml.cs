using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Interfaces;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Record;
using LambdaLauncher.ViewModels.ResourceModel;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DownloadResourceDetails : Page
{
    public ResourceDetailsModel ViewModel { get; set; } = null!;

    public DownloadResourceDetails()
    {
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        // 资源信息移动动画
        var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
        anim?.TryStart(resourceDetails);

        var resource = (IResource)e.Parameter;
        (ViewModel, string headerText) = resource.ResourceType switch
        {
            ResourceType.Mod => ((ResourceDetailsModel)new ModDetailsResourceModel(), "Header-Mod"),
            ResourceType.Modpack => (new ModPackDetailsResourceModel(), "Header-ModPack"),
            ResourceType.Resourcepack => (new ResourcePackDetailsResourceModel(), "Header-ResourcePack"),
            ResourceType.Shaderpack => (new ShaderDetailsResourceModel(), "Header-ShaderPack"),
            _ => throw new NotImplementedException()
        };

        App.BreadcrumbService.SetHeader(
            new PageHeader()
            {
                Page = typeof(DownloadResource),
                Text = Utils.ResourceLoader.GetString(headerText),
                Parameter = resource.ResourceType,
                InfoOverride = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft }
            },
            new PageHeader()
            {
                Page = typeof(DownloadResourceDetails),
                Text = resource.Name
            }
        );

        await ViewModel.InitAsync(resource);
    }
}