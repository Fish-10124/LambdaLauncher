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

public abstract partial class DownloadResourceModel : ObservableObject
{
    public const int PageSize = 50;

    public Dictionary<SortMethodType, SortMethodDisplay> ResourceSortOrders { get; } = new()
    {
        {SortMethodType.Relevance,        new() { SortType = SortMethodType.Relevance,        CurseForgeSortType = SortField.Featured,         ModrinthSortType = ModrinthSearchIndex.Relevance,     DisplayText = "SortMethod-Relevance" } },
        {SortMethodType.Downloads,        new() { SortType = SortMethodType.Downloads,        CurseForgeSortType = SortField.TotalDownloads,   ModrinthSortType = ModrinthSearchIndex.Downloads,     DisplayText = "SortMethod-TotalDownloads" } },
        {SortMethodType.Follows,          new() { SortType = SortMethodType.Follows,          CurseForgeSortType = null,                       ModrinthSortType = ModrinthSearchIndex.Follows,       DisplayText = "SortMethod-Follows" } },
        {SortMethodType.ReleasedDate,     new() { SortType = SortMethodType.ReleasedDate,     CurseForgeSortType = SortField.ReleasedDate,     ModrinthSortType = ModrinthSearchIndex.DatePublished, DisplayText = "SortMethod-ReleasedDate" } },
        {SortMethodType.LatestUpdated,    new() { SortType = SortMethodType.LatestUpdated,    CurseForgeSortType = SortField.LastUpdated,      ModrinthSortType = ModrinthSearchIndex.DateUpdated,   DisplayText = "SortMethod-LatestUpdate" } },
        {SortMethodType.Featured,         new() { SortType = SortMethodType.Featured,         CurseForgeSortType = SortField.Featured,         ModrinthSortType = null,                              DisplayText = "SortMethod-Featured" } },
        {SortMethodType.Popularity,       new() { SortType = SortMethodType.Popularity,       CurseForgeSortType = SortField.Popularity,       ModrinthSortType = null,                              DisplayText = "SortMethod-Popularity" } },
        {SortMethodType.Name,             new() { SortType = SortMethodType.Name,             CurseForgeSortType = SortField.Name,             ModrinthSortType = null,                              DisplayText = "SortMethod-Name" } },
        {SortMethodType.Author,           new() { SortType = SortMethodType.Author,           CurseForgeSortType = SortField.Author,           ModrinthSortType = null,                              DisplayText = "SortMethod-Author" } },
        {SortMethodType.TotalDownloads,   new() { SortType = SortMethodType.TotalDownloads,   CurseForgeSortType = SortField.TotalDownloads,   ModrinthSortType = null,                              DisplayText = "SortMethod-TotalDownloads" } },
        {SortMethodType.Category,         new() { SortType = SortMethodType.Category,         CurseForgeSortType = SortField.Category,         ModrinthSortType = null,                              DisplayText = "SortMethod-Category" } },
        {SortMethodType.GameVersion,      new() { SortType = SortMethodType.GameVersion,      CurseForgeSortType = SortField.GameVersion,      ModrinthSortType = null,                              DisplayText = "SortMethod-GameVersion" } },
        {SortMethodType.EarlyAccess,      new() { SortType = SortMethodType.EarlyAccess,      CurseForgeSortType = SortField.EarlyAccess,      ModrinthSortType = null,                              DisplayText = "SortMethod-EarlyAccess" } },
        {SortMethodType.FeaturedReleased, new() { SortType = SortMethodType.FeaturedReleased, CurseForgeSortType = SortField.FeaturedReleased, ModrinthSortType = null,                              DisplayText = "SortMethod-FeaturedReleased" } },
        {SortMethodType.Rating,           new() { SortType = SortMethodType.Rating,           CurseForgeSortType = SortField.Rating,           ModrinthSortType = null,                              DisplayText = "SortMethod-Rating" } }
    };

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
        ModLoader = App.ModLoaderSelectDisplays[ModLoaderType.Any],
        PageIndex = 0
    };

    [ObservableProperty]
    public partial ObservableCollection<IResource> ResourceList { get; protected set; } = [];

    [ObservableProperty]
    public partial int TotalPage { get; protected set; } = 0;

    [ObservableProperty]
    public partial bool IsSearching { get; protected set; } = false;

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
    }

    protected abstract Task<CurseForgeSearchResult> SearchCFResourceAsync(int index);

    protected abstract Task<ModrinthSearchResult> SearchMRResourceAsync(int index);

    [RelayCommand]
    private async Task ExecuteSearch(ResourceSearchEventArgs? searchArgs)
    {
        if (searchArgs is null)
            return;

        var args = new ResourceSearchArgs(searchArgs);
        if (args != SearchArgs)
        {
            SearchArgs = args;
            await SearchAsync();
        }
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
        App.NavigationService.Navigate(typeof(DownloadResourceDetails), card.Tag, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
    }
}