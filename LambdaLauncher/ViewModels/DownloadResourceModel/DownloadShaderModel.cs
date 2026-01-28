using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using Microsoft.UI.Xaml.Controls;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Network;
using System;
using System.Threading.Tasks;

namespace LambdaLauncher.ViewModels.DownloadResourceModel;

public partial class DownloadShaderModel : DownloadResourceModel
{
    public static readonly AllCategoryDisplay[] CategoryDisplays = 
    [
        new() { CurseForgeId = null, CurseForgeIdText = "All",       ModrinthId = "",             DisplayText = "Category-All" },
        new() { CurseForgeId = 6553, CurseForgeIdText = "Realistic", ModrinthId = "realistic",    DisplayText = "Category-Realistic" },
        new() { CurseForgeId = 6554, CurseForgeIdText = "Fantasy",   ModrinthId = "fantasy",      DisplayText = "Category-Fantasy" },
        new() { CurseForgeId = 6555, CurseForgeIdText = "Vanilla",   ModrinthId = "vanilla-like", DisplayText = "Category-VanillaLike" }
    ];

    public static readonly CurseForgeCategoryDisplay[] CurseForgeCategoryDisplays = 
    [
        new() { Id = null, IdText = "All",       DisplayText = "Category-All" },
        new() { Id = 6553, IdText = "Realistic", DisplayText = "Category-Realistic" },
        new() { Id = 6554, IdText = "Fantasy",   DisplayText = "Category-Fantasy" },
        new() { Id = 6555, IdText = "Vanilla",   DisplayText = "Category-VanillaLike" }
    ];

    public static readonly ModrinthCategoryDisplay[] ModrinthCategoryDisplays = 
    [
        new() { Id = "",                 DisplayText = "Category-All" },

        // 性能要求分类                   
        new() { Id = "screenshot",       DisplayText = "Category-Screenshot" }, // ppt
        new() { Id = "high",             DisplayText = "Category-High" },
        new() { Id = "medium",           DisplayText = "Category-Medium" },
        new() { Id = "low",              DisplayText = "Category-Low" },
        new() { Id = "potato",           DisplayText = "Category-Potato" },

        // 渲染风格/类型                  
        new() { Id = "vanilla-like",     DisplayText = "Category-VanillaLike" },
        new() { Id = "semi-realistic",   DisplayText = "Category-SemiRealistic" },
        new() { Id = "realistic",        DisplayText = "Category-Realistic" },
        new() { Id = "cartoon",          DisplayText = "Category-Cartoon" },
        new() { Id = "fantasy",          DisplayText = "Category-Fantasy" },
        new() { Id = "cursed",           DisplayText = "Category-TwitchIntegration" },

        // 特定视觉效果                   
        new() { Id = "bloom",            DisplayText = "Category-Bloom" },
        new() { Id = "shadows",          DisplayText = "Category-Shadows" },
        new() { Id = "reflections",      DisplayText = "Category-Reflections" },
        new() { Id = "colored-lighting", DisplayText = "Category-ColoredLighting" },

        // 高级/特殊渲染技术
        new() { Id = "path-tracing",     DisplayText = "Category-PathTracing" },
        new() { Id = "atmosphere",       DisplayText = "Category-Atmosphere" },
        new() { Id = "foliage",          DisplayText = "Category-Foliage" }
    ];

    protected override async Task<CurseForgeSearchResult> SearchCFResourceAsync(int index)
    {
        var curseforgeOptions = new CurseforgeSearchOptions()
        {
            ClassId = ClassId.Shaders,
            SearchFilter = SearchArgs.SearchText,
            GameVersion = SearchArgs.IsAnyVersionSelected ? null : SearchArgs.GameVersion,
            CategoryId = SearchArgs.Category switch
            {
                AllCategoryDisplay all => all.CurseForgeId,
                CurseForgeCategoryDisplay cf => cf.Id,
                _ => throw new NotImplementedException()
            },
            SortField = ResourceSortOrders[SearchArgs.SortMethod].CurseForgeSortType!.Value,
            ModLoaderType = SearchArgs.ModLoader.LoaderType,
            SortOrder = SortOrder.Desc,
            PageSize = PageSize,
            Index = index
        };

        return await Utils.CFProvider.SearchResourcesAsync(curseforgeOptions) ?? throw new NullReferenceException();
    }

    protected override async Task<ModrinthSearchResult> SearchMRResourceAsync(int index)
    {
        var modrinthOptions = new ModrinthSearchOptions()
        {
            ProjectType = "shader",
            SearchFilter = SearchArgs.SearchText,
            Version = SearchArgs.IsAnyVersionSelected ? null : SearchArgs.GameVersion,
            Category = SearchArgs.Category switch
            {
                AllCategoryDisplay all => all.ModrinthId,
                ModrinthCategoryDisplay mr => mr.Id,
                _ => throw new NotImplementedException()
            },
            Index = ResourceSortOrders[SearchArgs.SortMethod].ModrinthSortType!.Value,
            ModLoader = SearchArgs.ModLoader.LoaderType,
            Limit = PageSize,
            Offset = index
        };

        return await Utils.MRProvider.SearchAsync(modrinthOptions) ?? throw new NullReferenceException();
    }
}