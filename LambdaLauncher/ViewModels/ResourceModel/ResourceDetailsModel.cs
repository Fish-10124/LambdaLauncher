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

namespace LambdaLauncher.ViewModels.ResourceModel;

public abstract partial class ResourceDetailsModel : ResourceModel
{
    private IEnumerable<GroupInfoList<SettingsCardDisplay>> GroupedFiles = [];

    [ObservableProperty]
    public partial ObservableCollection<GroupInfoList<SettingsCardDisplay>> FilesDisplay { get; set; } = [];

    [ObservableProperty]
    public partial bool IsInitializing { get; set; } = false;

    [ObservableProperty]
    public partial string SelectedFilterTag { get; set; } = Utils.ResourceLoader.GetString("AllGameVersion");

    partial void OnSelectedFilterTagChanged(string value)
    {
        FilesDisplay.Clear();
        var item = GroupedFiles.Where(g =>
        {
            var key = (string)g.Key;

            // 前半部分为游戏版本, 后半部分为模组加载器 (例如: "Forge 1.12.1")
            // 但部分旧版本的模组可能没有标明模组加载器(因为旧版本只有Forge一个加载器), 所以判断是否存在后半部分
            return (key.Contains(' ') ? key.Split(' ')[1] : key) == value;
        });
        FilesDisplay.AddRange(item.Any() ? item : GroupedFiles);
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
        GroupedFiles = groupedFiles;
        FilesDisplay.AddRange(GroupedFiles);
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