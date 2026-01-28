using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Interfaces;
using LambdaLauncher.Models.Record;
using LambdaLauncher.ViewModels.ResourceModel;
using System;
using Microsoft.UI.Xaml;

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
        ViewModel = resource.ResourceType switch
        {
            ResourceType.Mod => new ModDetailsResourceModel(),
            ResourceType.Modpack => new ModPackDetailsResourceModel(),
            ResourceType.Resourcepack => new ResourcePackDetailsResourceModel(),
            ResourceType.Shaderpack => new ShaderDetailsResourceModel(),
            _ => throw new NotImplementedException()
        };

        App.NavigationService.AddHeader(
            new PageHeader()
            {
                Page = typeof(DownloadResourceDetails),
                Text = resource.Name
            }
        );

        await ViewModel.InitAsync(resource);
    }

    private void gameVersionFilter_Loaded(object sender, RoutedEventArgs e)
    {
        var combo = (ComboBox)sender;
        if (combo.SelectedIndex < 0 && combo.Items.Count > 0)
        {
            combo.SelectedIndex = 0;
        }
    }
}