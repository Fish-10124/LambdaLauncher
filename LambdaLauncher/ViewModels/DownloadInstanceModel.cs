using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using MinecraftLaunch.Base.Models.Network;
using MinecraftLaunch.Components.Installer;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.Collections;

namespace LambdaLauncher.ViewModels;

public partial class DownloadInstanceModel(Page framePage) : ObservableObject
{
    private readonly Page FramePage = framePage;
    private readonly HashSet<string> AprilFoolVersionId = ["25w14craftmine", "24w14potato", "23w13a_or_b", "22w13oneblockatatime", "20w14invinite", "3D Shareware v1.34", "1.RV-Pre1", "15w14a"];

    [ObservableProperty]
    public partial ObservableCollection<SettingsCardDisplay> LatestVersionDisplay { get; private set; } = [];

    [ObservableProperty]
    public partial AdvancedCollectionView VersionDisplay { get; private set; }

    [ObservableProperty]
    public partial bool IsFiltering { get; private set; } = false;

    [ObservableProperty]
    public partial bool IsInitializing { get; private set; } = false;

    [ObservableProperty]
    public partial string SelectedFilterTag { get; set; }

    async partial void OnSelectedFilterTagChanged(string value)
    {
        IsFiltering = true;

        await Utils.WaitForNextFrameAsync(); // 确保过滤器加载动画可以显示出来

        VersionDisplay.Filter = x =>
        {
            var display = (SettingsCardDisplay)x;
            var manifestEntry = (VersionManifestEntry)display.Parameter!;
            return value switch
            {
                "all" => true,
                "aprilfool" => AprilFoolVersionId.Contains(manifestEntry.Id),
                _ => manifestEntry.Type == value
            };
        };

        await Utils.WaitForNextFrameAsync();

        VersionDisplay.RefreshFilter();
        IsFiltering = false;
    }

    public async Task InitAsync()
    {
        IsInitializing = true;

        try
        {
            Global.InstanceVersions = await VanillaInstaller.EnumerableMinecraftAsync() ?? throw new NullReferenceException();
        }
        catch (Exception)
        {
            Global.InstanceVersions = [];
            throw new Exception("Faild to get instance versions");
        }

        // 获取最新版本
        VersionManifestEntry latestRelease = null!;
        VersionManifestEntry latestSnapshot = null!;
        foreach (var v in Global.InstanceVersions)
        {
            if (v.Type == "release" && latestRelease is null)
                latestRelease = v;
            if (v.Type == "snapshot" && latestSnapshot is null)
                latestSnapshot = v;
            if (latestRelease is not null && latestSnapshot is not null)
                break;
        }
        LatestVersionDisplay = [GetVersionDisplay(latestRelease!), GetVersionDisplay(latestSnapshot!)];

        // 其余版本
        VersionDisplay = new(Global.InstanceVersions.Select(GetVersionDisplay).ToList(), true);
        IsInitializing = false;
    }

    private bool IsAprilFoolEntry(VersionManifestEntry entry)
    {
        return AprilFoolVersionId.Contains(entry.Id) /* || (entry.ReleaseTime.Month == 4 && entry.ReleaseTime.Day == 1) */;
    }

    private SettingsCardDisplay GetVersionDisplay(VersionManifestEntry entry)
    {
        string idText = "";
        BitmapImage icon;

        if (IsAprilFoolEntry(entry))
        {
            idText = Utils.ResourceLoader.GetString("InstanceType-AprilFool");
            icon = Utils.GetIconFromResources("Furnace")!;
        }
        else
        {
            (string, BitmapImage) Get(string idKey, string iconKey)
            {
                return (Utils.ResourceLoader.GetString(idKey), Utils.GetIconFromResources(iconKey)!);
            }

            (idText, icon) = entry.Type switch
            {
                "snapshot" => Get("InstanceType-Snapshot", "CraftingTable"),
                "release" => Get("InstanceType-Release", "GrassBlock"),
                "old_alpha" => Get("InstanceType-Old_alpha", "Path"),
                "old_beta" => Get("InstanceType-Old_beta", "Path"),
                _ => throw new NotImplementedException()
            };
        }

        string description = $"{idText} {entry.ReleaseTime:d} {entry.ReleaseTime:t}";
        return new(entry.Id, description, icon, entry);
    }

    [RelayCommand]
    private void NavigateToDetails(SettingsCard? card)
    {
        if (card is null)
            return;

        // 版本信息移动动画
        var anim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", card.Header as UIElement);
        anim.Configuration = new DirectConnectedAnimationConfiguration();
        FramePage.Frame.Navigate(typeof(DownloadInstanceDetails), card.Tag, new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromRight });
    }
}