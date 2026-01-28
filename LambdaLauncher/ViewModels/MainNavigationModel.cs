using CommunityToolkit.Mvvm.ComponentModel;
using LambdaLauncher.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using MinecraftLaunch;
using MinecraftLaunch.Components.Installer;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LambdaLauncher.ViewModels;

public partial class MainNavigationModel : ObservableObject
{
    public MainNavigationModel()
    {
        InitializeHelper.Initialize(settings =>
        {
            settings.MaxThread = 256; // 最大下载线程
            settings.MaxFragment = 128; // 最大文件分片数量
            settings.IsEnableMirror = false; // 是否启用 Minecraft 国内下载镜像源（BMCLAPI）
            settings.CurseForgeApiKey = "$2a$10$VGFy23o3WipEqFXpGyd67.OfYA13D/9NUym2jGnp3hznXKCmcHala"; // Curseforge API Key 设置（可选）
            settings.UserAgent = "MLTest/1.0"; // 自定义请求时所使用的 User Agent
        });

        _ = TryGetInstances();
    }

    public void Navigate(NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            App.NavigationService.Navigate(typeof(Settings), null, new DrillInNavigationTransitionInfo());
            return;
        }

        Type? page = args.SelectedItemContainer.Name switch
        {
            "navItemHome" => typeof(Home),
            "navItemDownloadInstance" => typeof(DownloadInstance),
            "navItemDownloadMod" => typeof(DownloadMod),
            "navItemDownloadModPack" => typeof(DownloadModpack),
            "navItemDownloadResourcePack" => typeof(DownloadResourcePack),
            "navItemDownloadShader" => typeof(DownloadShader),
            "navItemAccount" => typeof(AccountManagement),
            _ => null
        };

        if (page is not null)
        {
            App.NavigationService.Navigate(page, null, new DrillInNavigationTransitionInfo());
        }
    }

    public async Task TryGetInstances()
    {
        // 获取版本列表
        try
        {
            App.InstanceVersions = await VanillaInstaller.EnumerableMinecraftAsync() ?? throw new NullReferenceException();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Catched an exception when getting minecraft versions: {ex.Message}");
        }
    }
}
