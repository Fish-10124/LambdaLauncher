using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Record;
using Microsoft.UI.Xaml.Controls;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Network;
using System;
using System.Threading.Tasks;

namespace LambdaLauncher.ViewModels.DownloadResourceModel;

public partial class DownloadModPackModel(Page framePage) : DownloadResourceModel(framePage)
{
    public static readonly AllCategoryDisplay[] CategoryDisplays = 
    [
        new() { CurseForgeId = null, CurseForgeIdText = "All",               ModrinthId = "",            DisplayText = "Category-All" },
        new() { CurseForgeId = 4481, CurseForgeIdText = "Small / Light",     ModrinthId = "lightweight", DisplayText = "Category-Lightweight" },
        new() { CurseForgeId = 4483, CurseForgeIdText = "Combat / PvP",      ModrinthId = "combat",      DisplayText = "Category-Combat" },
        new() { CurseForgeId = 4475, CurseForgeIdText = "Adventure and RPG", ModrinthId = "adventure",   DisplayText = "Category-AdventureAndRPG" },
        new() { CurseForgeId = 4478, CurseForgeIdText = "Quests",            ModrinthId = "quests",      DisplayText = "Category-Quests" },
        new() { CurseForgeId = 4472, CurseForgeIdText = "Tech",              ModrinthId = "technology",  DisplayText = "Category-Technology" },
        new() { CurseForgeId = 4484, CurseForgeIdText = "Multiplayer",       ModrinthId = "multiplayer", DisplayText = "Category-Multiplayer" },
        new() { CurseForgeId = 4473, CurseForgeIdText = "Magic",             ModrinthId = "magic",       DisplayText = "Category-Magic" }
    ];

    public static readonly CurseForgeCategoryDisplay[] CurseForgeCategoryDisplays = 
    [
        new() { Id = null, IdText = "All",               DisplayText = "Category-All" },
        new() { Id = 4482, IdText = "Extra Large",       DisplayText = "Category-ExtraLarge" },
        new() { Id = 4481, IdText = "Small / Light",     DisplayText = "Category-Lightweight" },
        new() { Id = 4483, IdText = "Combat / PvP",      DisplayText = "Category-Combat" },
        new() { Id = 4474, IdText = "Sci-Fi",            DisplayText = "Category-SciFi" },
        new() { Id = 4475, IdText = "Adventure and RPG", DisplayText = "Category-AdventureAndRPG" },
        new() { Id = 4487, IdText = "FTB Official Pack", DisplayText = "Category-FTBOfficialPack" },
        new() { Id = 4478, IdText = "Quests",            DisplayText = "Category-Quests" },
        new() { Id = 4472, IdText = "Tech",              DisplayText = "Category-Technology" },
        new() { Id = 4736, IdText = "Skyblock",          DisplayText = "Category-Skyblock" },
        new() { Id = 4480, IdText = "Map Based",         DisplayText = "Category-MapBased" },
        new() { Id = 7418, IdText = "Horror",            DisplayText = "Category-Horror" },
        new() { Id = 4484, IdText = "Multiplayer",       DisplayText = "Category-Multiplayer" },
        new() { Id = 4477, IdText = "Mini Game",         DisplayText = "Category-Minigame" },
        new() { Id = 4473, IdText = "Magic",             DisplayText = "Category-Magic" },
        new() { Id = 5128, IdText = "Vanilla+",          DisplayText = "Category-VanillaPlus" },
        new() { Id = 4479, IdText = "Hardcore",          DisplayText = "Category-Hardcore" },
        new() { Id = 4476, IdText = "Exploration",       DisplayText = "Category-Exploration" }
    ];

    public static readonly ModrinthCategoryDisplay[] ModrinthCategoryDisplays = 
    [
        new() { Id = "",             DisplayText = "Category-All" },
        new() { Id = "adventure",    DisplayText = "Category-AdventureAndRPG" },
        new() { Id = "challenging",  DisplayText = "Category-Challenging" },
        new() { Id = "combat",       DisplayText = "Category-Combat" },
        new() { Id = "kitchensink",  DisplayText = "Category-KitchenSink" },
        new() { Id = "lightweight",  DisplayText = "Category-Lightweight" },
        new() { Id = "magic",        DisplayText = "Category-Magic" },
        new() { Id = "multiplayer",  DisplayText = "Category-Multiplayer" },
        new() { Id = "optimization", DisplayText = "Category-Optimization" },
        new() { Id = "quests",       DisplayText = "Category-Quests" },
        new() { Id = "technology",   DisplayText = "Category-Technology" }
    ];

    protected override ResourceSearchArgs? ReadSearchArgs()
    {
        return Global.LastSearchArgs[ResourceType.Modpack];
    }

    protected override void SaveSearchArgs()
    {
        Global.LastSearchArgs[ResourceType.Modpack] = SearchArgs;
    }

    protected override async Task<CurseForgeSearchResult> SearchCFResourceAsync(int index)
    {
        var curseforgeOptions = new CurseforgeSearchOptions()
        {
            ClassId = ClassId.Modpacks,
            SearchFilter = SearchArgs.SearchText,
            GameVersion = SearchArgs.IsAnyVersionSelected ? null : SearchArgs.GameVersion,
            CategoryId = SearchArgs.Category switch
            {
                AllCategoryDisplay all => all.CurseForgeId,
                CurseForgeCategoryDisplay cf => cf.Id,
                _ => throw new NotImplementedException()
            },
            SortField = Global.ResourceSortMethods[SearchArgs.SortMethod].CurseForgeSortType!.Value,
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
            ProjectType = "modpack",
            SearchFilter = SearchArgs.SearchText,
            Version = SearchArgs.IsAnyVersionSelected ? null : SearchArgs.GameVersion,
            Category = SearchArgs.Category switch
            {
                AllCategoryDisplay all => all.ModrinthId,
                ModrinthCategoryDisplay mr => mr.Id,
                _ => throw new NotImplementedException()
            },
            Index = Global.ResourceSortMethods[SearchArgs.SortMethod].ModrinthSortType!.Value,
            ModLoader = SearchArgs.ModLoader.LoaderType,
            Limit = PageSize,
            Offset = index
        };

        return await Utils.MRProvider.SearchAsync(modrinthOptions) ?? throw new NullReferenceException();
    }
}