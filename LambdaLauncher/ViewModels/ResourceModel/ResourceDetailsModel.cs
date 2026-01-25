using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftLaunch.Base.Interfaces;
using MinecraftLaunch.Base.Models.Network;
using LambdaLauncher.Extensions;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using CommunityToolkit.WinUI.Collections;

namespace LambdaLauncher.ViewModels.ResourceModel;

public abstract partial class ResourceDetailsModel : ResourceModel
{
    [ObservableProperty]
    public partial AdvancedCollectionView FilesDisplay { get; set; }

    [ObservableProperty]
    public partial bool IsInitializing { get; set; } = false;

    [ObservableProperty]
    public partial bool IsFiltering { get; set; } = false;

    [ObservableProperty]
    public partial string SelectedFilterTag { get; set; }

    async partial void OnSelectedFilterTagChanged(string value)
    {
        if (FilesDisplay == null)
            return;

        IsFiltering = true;

        await Utils.WaitForNextFrameAsync(); // 确保过滤器加载动画可以显示出来

        FilesDisplay.Filter = x =>
        {
            if (SupportVersions.First() == value)
                return true;

            var g = (GroupInfoList<SettingsCardDisplay>)x!;
            var key = (string)g.Key;
            var keyPart = key.Contains(' ') ? key.Split(' ')[1] : key;
            return keyPart == value;
        };

        await Utils.WaitForNextFrameAsync();

        FilesDisplay.RefreshFilter();
        IsFiltering = false;
    }

    public async Task InitAsync(IResource resource)
    {
        IsInitializing = true;

        Init(resource);

        await GetFileInfosAsync();
        IsInitializing = false;
    }

    private async Task GetFileInfosAsync()
    {
        IEnumerable<IResourceFile> files = Resource switch
        {
            CurseforgeResource cfRes => await Utils.CFProvider.GetResourceFilesByModIdAsync(cfRes.Id),
            ModrinthResource mrRes => await Utils.MRProvider.GetModFilesByProjectIdAsync(mrRes.ProjectId),
            _ => throw new NotImplementedException()
        };

        // 对文件信息进行分组
        var grouped = files.Select(ConvertFileToCardDisplay).GroupBy(g => (IEnumerable<string>)g.Parameter!);
        var ordered = grouped.OrderByList(Global.InstanceVersions.Select(v => v.Id), g => ((string)g.Key).Split(' ')[1], MissingItemsPlacement.AtStart);
        List<GroupInfoList<SettingsCardDisplay>> groupedFiles = [];
        foreach (var item in ordered)
        {
            var split = ((string)item.Key).Split(' ');
            groupedFiles.Add(new(split[0] == "" ? split[1] : item.Key, item));
        }
        // 使用 AdvancedCollectionView 来表示分组集合，方便在 UI 侧进行过滤
        FilesDisplay = new AdvancedCollectionView(groupedFiles.ToList(), true);
    }

    [RelayCommand]
    private void CopyLinkToClipboard()
    {
        Utils.CopyToClipboard(Resource.WebsiteUrl);
    }

    [RelayCommand]
    private async Task GotoResourceLink()
    {
        var success = await Launcher.LaunchUriAsync(new Uri(Resource.WebsiteUrl));

        if (!success)
        {
            Debug.WriteLine("Cannot launch uri link.");
        }
    }
}