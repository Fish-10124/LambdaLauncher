using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Record;
using Microsoft.UI.Xaml.Controls;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Network;
using System;
using System.Threading.Tasks;

namespace LambdaLauncher.ViewModels.DownloadResourceModel;

public partial class DownloadResourcePackModel : DownloadResourceModel
{
    public static readonly AllCategoryDisplay[] CategoryDisplays = 
    [
        new() { CurseForgeId = null, CurseForgeIdText = "All",             ModrinthId = "",       DisplayText = "Category-All" },
        new() { CurseForgeId =  393, CurseForgeIdText = "16x",             ModrinthId = "16x",    DisplayText = "Category-16x" },
        new() { CurseForgeId =  394, CurseForgeIdText = "32x",             ModrinthId = "32x",    DisplayText = "Category-32x" },
        new() { CurseForgeId =  395, CurseForgeIdText = "64x",             ModrinthId = "64x",    DisplayText = "Category-64x" },
        new() { CurseForgeId =  396, CurseForgeIdText = "128x",            ModrinthId = "128x",   DisplayText = "Category-128x" },
        new() { CurseForgeId =  397, CurseForgeIdText = "256x",            ModrinthId = "256x",   DisplayText = "Category-256x" },
        new() { CurseForgeId =  398, CurseForgeIdText = "512x and Higher", ModrinthId = "512x+",  DisplayText = "Category-512xAndHigher" },
        new() { CurseForgeId = 5244, CurseForgeIdText = "Font Packs",      ModrinthId = "Fonts",  DisplayText = "Category-FontPacks" },
        new() { CurseForgeId = 4465, CurseForgeIdText = "Mod Support",     ModrinthId = "Models", DisplayText = "Category-ModSupport" }
    ];

    public static readonly CurseForgeCategoryDisplay[] CurseForgeCategoryDisplays = 
    [
        new() { Id = null, IdText = "All",             DisplayText = "Category-All" },
        new() { Id =  393, IdText = "16x",             DisplayText = "Category-16x" },
        new() { Id =  394, IdText = "32x",             DisplayText = "Category-32x" },
        new() { Id =  395, IdText = "64x",             DisplayText = "Category-64x" },
        new() { Id =  396, IdText = "128x",            DisplayText = "Category-128x" },
        new() { Id =  397, IdText = "256x",            DisplayText = "Category-256x" },
        new() { Id =  398, IdText = "512x and Higher", DisplayText = "Category-512xAndHigher" },
        new() { Id =  403, IdText = "Traditional",     DisplayText = "Category-Traditional" },
        new() { Id =  402, IdText = "Medieval",        DisplayText = "Category-Medieval" },
        new() { Id =  401, IdText = "Modern",          DisplayText = "Category-Modern" },
        new() { Id =  399, IdText = "Steampunk",       DisplayText = "Category-Steampunk" },
        new() { Id =  400, IdText = "Photo Realistic", DisplayText = "Category-PhotoRealistic" },
        new() { Id =  404, IdText = "Animated",        DisplayText = "Category-Animated" },
        new() { Id = 4465, IdText = "Mod Support",     DisplayText = "Category-ModSupport" },
        new() { Id = 5193, IdText = "Data Packs",      DisplayText = "Category-DataPacks" },
        new() { Id = 5244, IdText = "Font Packs",      DisplayText = "Category-FontPacks" },
        new() { Id =  405, IdText = "Miscellaneous",   DisplayText = "Category-Miscellaneous" },
        new() { Id = 8939, IdText = "ModJam 2025",     DisplayText = "Category-ModJam2025" }
    ];

    public static readonly ModrinthCategoryDisplay[] ModrinthCategoryDisplays = 
    [
        new() { Id = "",             DisplayText = "Category-All" },
                                     
        // 分辨率                    
        new() { Id = "8x-",          DisplayText = "Category-8xAndLower" },
        new() { Id = "16x",          DisplayText = "Category-16x" },
        new() { Id = "32x",          DisplayText = "Category-32x" },
        new() { Id = "48x",          DisplayText = "Category-48x" },
        new() { Id = "64x",          DisplayText = "Category-64x" },
        new() { Id = "128x",         DisplayText = "Category-128x" },
        new() { Id = "256x",         DisplayText = "Category-256x" },
        new() { Id = "512x+",        DisplayText = "Category-512xAndHigher" },

        // 功能                      
        new() { Id = "audio",        DisplayText = "Category-Audio" },
        new() { Id = "blocks",       DisplayText = "Category-Blocks" },
        new() { Id = "core-shaders", DisplayText = "Category-CoreShaders" },
        new() { Id = "entities",     DisplayText = "Category-Entities" },
        new() { Id = "environment",  DisplayText = "Category-Environment" },
        new() { Id = "equipment",    DisplayText = "Category-Equipment" },
        new() { Id = "fonts",        DisplayText = "Category-FontPacks" },
        new() { Id = "gui",          DisplayText = "Category-GUI" },
        new() { Id = "items",        DisplayText = "Category-Items" },
        new() { Id = "locale",       DisplayText = "Category-Locale" },
        new() { Id = "models",       DisplayText = "Category-ModSupport" },

        // 类别                      
        new() { Id = "combat",       DisplayText = "Category-Combat" },
        new() { Id = "cursed",       DisplayText = "Category-TwitchIntegration" },
        new() { Id = "decoration",   DisplayText = "Category-Cosmetic" },
        new() { Id = "modded",       DisplayText = "Category-Moded" },
        new() { Id = "realistic",    DisplayText = "Category-Realistic" },
        new() { Id = "simplistic",   DisplayText = "Category-Simplistic" },
        new() { Id = "themed",       DisplayText = "Category-Themed" },
        new() { Id = "tweaks",       DisplayText = "Category-Tweaks" },
        new() { Id = "utility",      DisplayText = "Category-UtilityQoL" },
        new() { Id = "vanilla-like", DisplayText = "Category-VanillaLike" }
    ];

    protected override async Task<CurseForgeSearchResult> SearchCFResourceAsync(int index)
    {
        var curseforgeOptions = new CurseforgeSearchOptions()
        {
            ClassId = ClassId.ResourcePacks,
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
            ProjectType = "resourcepack",
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