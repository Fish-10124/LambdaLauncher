using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Interfaces;
using MinecraftLaunch.Base.Models.Network;
using LambdaLauncher.Extensions;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Enums;
using LambdaLauncher.Models.Record;
using LambdaLauncher.Models.UserEventArgs;
using LambdaLauncher.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LambdaLauncher.ViewModels.DownloadResourceModel;

public abstract partial class DownloadResourceModel(Page framePage) : ObservableObject
{
    public const int PageSize = 50;

    private readonly Page FramePage = framePage;

    private int TotalCurseForgePage = 0;
    private int TotalModrinthPage = 0;

    [ObservableProperty]
    public partial ResourceSearchArgs SearchArgs { get; protected set; } = new()
    {
        SearchText = "",
        ResourceSource = WebResourceSource.All,
        Category = new AllCategoryDisplay()
        {
            CurseForgeId = null,
            CurseForgeIdText = "All",
            ModrinthId = "",
            DisplayText = "Category-All"
        },
        SortMethod = SortMethodType.Relevance,
        ModLoader = Global.ModLoaderSelectDisplays[ModLoaderType.Any],
        PageIndex = 0
    };

    [ObservableProperty]
    public partial ObservableCollection<IResource> ResourceList { get; protected set; } = [];

    [ObservableProperty]
    public partial int TotalPage { get; protected set; } = 0;

    [ObservableProperty]
    public partial bool IsSearching { get; protected set; } = false;

    /// <summary>
    /// 从Global读取
    /// </summary>
    protected abstract ResourceSearchArgs? ReadSearchArgs();

    /// <summary>
    /// 存储到Global
    /// </summary>
    protected abstract void SaveSearchArgs();

    public async Task SearchAsync()
    {
        IsSearching = true;
        ResourceList.Clear();

        int index = SearchArgs.PageIndex * PageSize;

        bool searchCF = SearchArgs.ResourceSource is WebResourceSource.All or WebResourceSource.CurseForge;
        bool searchMR = SearchArgs.ResourceSource is WebResourceSource.All or WebResourceSource.Modrinth;

        // 根据模式决定是否允许请求
        searchCF = searchCF && (TotalCurseForgePage <= 0 || SearchArgs.PageIndex < TotalCurseForgePage);
        searchMR = searchMR && (TotalModrinthPage <= 0 || SearchArgs.PageIndex < TotalModrinthPage);

        // 并行执行
        var cfTask = searchCF ? Utils.TryExecuteAsync(() => SearchCFResourceAsync(index)) : Task.FromResult<CurseForgeSearchResult?>(null);
        var mrTask = searchMR ? Utils.TryExecuteAsync(() => SearchMRResourceAsync(index)) : Task.FromResult<ModrinthSearchResult?>(null);

        await Task.WhenAll(cfTask, mrTask);

        var cfResult = cfTask.Result;
        var mrResult = mrTask.Result;

        int cfPageCount = TotalCurseForgePage;
        int mrPageCount = TotalModrinthPage;

        if (cfResult is not null)
        {
            ResourceList.AddRange(cfResult.Resources);

            double total = Math.Min(cfResult.TotalCount, 10000); // CurseForge 限制
            cfPageCount = (int)Math.Ceiling(total / cfResult.PageSize);
            TotalCurseForgePage = cfPageCount;
        }

        if (mrResult is not null)
        {
            ResourceList.AddRange(mrResult.Resources);

            mrPageCount = (int)Math.Ceiling((double)mrResult.TotalCount / mrResult.PageSize);
            TotalModrinthPage = mrPageCount;
        }

        TotalPage = Math.Max(cfPageCount, mrPageCount);

        ResourceList.GroupDuplicatesTogether(item => item.Name);

        IsSearching = false;
        SaveSearchArgs();
    }

    public async Task SearchLastAsync()
    {
        if (ReadSearchArgs() is ResourceSearchArgs args)
            SearchArgs = args;

        await SearchAsync();
    }

    protected abstract Task<CurseForgeSearchResult> SearchCFResourceAsync(int index);

    protected abstract Task<ModrinthSearchResult> SearchMRResourceAsync(int index);

    [RelayCommand]
    private async Task ExecuteSearch(ResourceSearchEventArgs? searchArgs)
    {
        if (searchArgs is null)
            return;

        SearchArgs = new(searchArgs);
        if (ReadSearchArgs() != SearchArgs)
            await SearchAsync();
    }

    [RelayCommand]
    private async Task ContinueSearch(PageChangedEventArgs args)
    {
        if (TotalPage <= 0)
            return;

        SearchArgs = SearchArgs with { PageIndex = args.CurrentPage };
        await SearchAsync();
    }

    [RelayCommand]
    private void NavigateToDetails(SettingsCard? card)
    {
        if (card is null)
        {
            return;
        }

        // 资源信息移动动画
        var anim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", card.Header as UIElement);
        anim.Configuration = new DirectConnectedAnimationConfiguration();
        FramePage.Frame.Navigate(typeof(DownloadResourceDetails), card.Tag, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
    }
}