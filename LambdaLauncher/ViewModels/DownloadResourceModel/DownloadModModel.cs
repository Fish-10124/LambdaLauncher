using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Record;

using Microsoft.UI.Xaml.Controls;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Network;
using System;
using System.Threading.Tasks;

namespace LambdaLauncher.ViewModels.DownloadResourceModel;

public partial class DownloadModModel : DownloadResourceModel
{
    public static readonly AllCategoryDisplay[] CategoryDisplays =
    [
        new() { CurseForgeId = null, CurseForgeIdText = "All",                       ModrinthId =  "",               DisplayText = "Category-All" },
        new() { CurseForgeId =  412, CurseForgeIdText = "Technology",                ModrinthId =  "technology",     DisplayText = "Category-Technology" },
        new() { CurseForgeId =  414, CurseForgeIdText = "Player Transport",          ModrinthId =  "transportation", DisplayText = "Category-PlayerTransport" },
        new() { CurseForgeId =  434, CurseForgeIdText = "Armor, Tools, and Weapons", ModrinthId =  "equipment",      DisplayText = "Category-ArmorToolsAndWeapons" },
        new() { CurseForgeId =  424, CurseForgeIdText = "Cosmetic",                  ModrinthId =  "decoration",     DisplayText = "Category-Cosmetic" },
        new() { CurseForgeId =  421, CurseForgeIdText = "API and Library",           ModrinthId =  "library",        DisplayText = "Category-APIAndLibrary" },
        new() { CurseForgeId =  419, CurseForgeIdText = "Magic",                     ModrinthId =  "magic",          DisplayText = "Category-Magic" },
        new() { CurseForgeId =  436, CurseForgeIdText = "Food",                      ModrinthId =  "food",           DisplayText = "Category-Food" },
        new() { CurseForgeId = 6814, CurseForgeIdText = "Performance",               ModrinthId =  "optimization",   DisplayText = "Category-Performance" },
        new() { CurseForgeId =  435, CurseForgeIdText = "Server Utility",            ModrinthId =  "utility",        DisplayText = "Category-ServerUtility" },
        new() { CurseForgeId =  420, CurseForgeIdText = "Storage",                   ModrinthId =  "storage",        DisplayText = "Category-Storage" },
    ];

    public static readonly CurseForgeCategoryDisplay[] CurseForgeCategoryDisplays =
    [
        new() { Id = null, IdText = "All",                               DisplayText = "Category-All" },

        // 世界元素
        new() { Id =  406, IdText = "World Gen",                         DisplayText = "Category-WorldGen" },
        new() { Id =  407, IdText = "Biomes",                            DisplayText = "Category-Biomes" },
        new() { Id =  410, IdText = "Dimensions",                        DisplayText = "Category-Dimensions" },
        new() { Id =  411, IdText = "Mobs",                              DisplayText = "Category-Mobs" },
        new() { Id =  408, IdText = "Ores and Resources",                DisplayText = "Category-OresAndResources" },
        new() { Id =  409, IdText = "Structures",                        DisplayText = "Category-Structures" },

        // 科技
        new() { Id =  412, IdText = "Technology",                        DisplayText = "Category-Technology" },
        new() { Id = 4843, IdText = "Automation",                        DisplayText = "Category-Automation" },
        new() { Id =  417, IdText = "Energy",                            DisplayText = "Category-Energy" },
        new() { Id =  416, IdText = "Farming",                           DisplayText = "Category-Farming" },
        new() { Id =  418, IdText = "Genetics",                          DisplayText = "Category-Genetics" },
        new() { Id =  415, IdText = "Energy, Fluid, and Item Transport", DisplayText = "Category-EnergyFluidAndItemTransport" },
        new() { Id =  414, IdText = "Player Transport",                  DisplayText = "Category-PlayerTransport" },
        new() { Id =  413, IdText = "Processing",                        DisplayText = "Category-Processing" },

        // 附属模组
        new() { Id =  426, IdText = "Addons",                            DisplayText = "Category-Addons" },
        new() { Id =  432, IdText = "Buildcraft",                        DisplayText = "Category-BuildCraft" },
        new() { Id =  433, IdText = "Forestry",                          DisplayText = "Category-Forestry" },
        new() { Id =  429, IdText = "Industrial Craft",                  DisplayText = "Category-IndustrialCraft" },
        new() { Id =  430, IdText = "Thaumcraft",                        DisplayText = "Category-Thaumcraft" },
        new() { Id =  427, IdText = "Thermal Expansion",                 DisplayText = "Category-ThermalExpansion" },
        new() { Id =  428, IdText = "Tinker's Construct",                DisplayText = "Category-Tinker'sConstruct" },
        new() { Id = 4545, IdText = "Applied Energistics 2",             DisplayText = "Category-AppliedEnergistics2" },
        new() { Id = 4485, IdText = "Blood Magic",                       DisplayText = "Category-BloodMagic" },
        new() { Id = 4773, IdText = "CraftTweaker",                      DisplayText = "Category-CraftTweaker" },
        new() { Id = 6484, IdText = "Create",                            DisplayText = "Category-Create" },
        new() { Id = 5232, IdText = "Galacticraft",                      DisplayText = "Category-Galacticraft" },
        new() { Id = 6954, IdText = "Integrated Dynamics",               DisplayText = "Category-IntegratedDynamics" },
        new() { Id = 5314, IdText = "KubeJS",                            DisplayText = "Category-KubeJs" },
        new() { Id = 9049, IdText = "Refined Storage",                   DisplayText = "Category-RefinedStorage" },
        new() { Id = 6145, IdText = "Skyblock",                          DisplayText = "Category-SkyBlock" },
        new() { Id = 7669, IdText = "Twilight Forest",                   DisplayText = "Category-TwilightForest" },

        // 其它
        new() { Id =  422, IdText = "Adventure and RPG",                 DisplayText = "Category-AdventureAndRPG" },
        new() { Id =  434, IdText = "Armor, Tools, and Weapons",         DisplayText = "Category-ArmorToolsAndWeapons" },
        new() { Id = 6821, IdText = "Bug Fixes",                         DisplayText = "Category-BugFixes" },
        new() { Id =  424, IdText = "Cosmetic",                          DisplayText = "Category-Cosmetic" },
        new() { Id = 9026, IdText = "CreativeMode",                      DisplayText = "Category-CreativeMode" },
        new() { Id = 5299, IdText = "Education",                         DisplayText = "Category-Education" },
        new() { Id =  421, IdText = "API and Library",                   DisplayText = "Category-APIAndLibrary" },
        new() { Id =  419, IdText = "Magic",                             DisplayText = "Category-Magic" },
        new() { Id =  423, IdText = "Map and Information",               DisplayText = "Category-MapAndInformation" },
        new() { Id = 4906, IdText = "MCreator",                          DisplayText = "Category-MCreator" },
        new() { Id =  436, IdText = "Food",                              DisplayText = "Category-Food" },
        new() { Id =  425, IdText = "Miscellaneous",                     DisplayText = "Category-Miscellaneous" },
        new() { Id = 6814, IdText = "Performance",                       DisplayText = "Category-Performance" },
        new() { Id = 4558, IdText = "Redstone",                          DisplayText = "Category-Redstone" },
        new() { Id =  435, IdText = "Server Utility",                    DisplayText = "Category-ServerUtility" },
        new() { Id =  420, IdText = "Storage",                           DisplayText = "Category-Storage" },
        new() { Id = 4671, IdText = "Twitch Integration",                DisplayText = "Category-TwitchIntegration" },
        new() { Id = 5191, IdText = "Utility & QoL",                     DisplayText = "Category-UtilityQol" },
        new() { Id = 8937, IdText = "ModJam 2025",                       DisplayText = "Category-ModJam2025" }
    ];

    public static readonly ModrinthCategoryDisplay[] ModrinthCategoryDisplays =
    [
        new() { Id = "",               DisplayText = "Category-All" },

        new() { Id = "worldgen",       DisplayText = "Category-WorldGen" },
        new() { Id = "technology",     DisplayText = "Category-Technology" },
        new() { Id = "adventure",      DisplayText = "Category-AdventureAndRPG" },
        new() { Id = "equipment",      DisplayText = "Category-ArmorToolsAndWeapons" },
        new() { Id = "decoration",     DisplayText = "Category-Cosmetic" },
        new() { Id = "library",        DisplayText = "Category-APIAndLibrary" },
        new() { Id = "magic",          DisplayText = "Category-Magic" },
        new() { Id = "food",           DisplayText = "Category-Food" },
        new() { Id = "optimization",   DisplayText = "Category-Performance" },
        new() { Id = "utility",        DisplayText = "Category-ServerUtility" },
        new() { Id = "storage",        DisplayText = "Category-Storage" },
        new() { Id = "mobs",           DisplayText = "Category-Mobs" },
        new() { Id = "cursed",         DisplayText = "Category-TwitchIntegration" },
        new() { Id = "economy",        DisplayText = "Category-Economy" },
        new() { Id = "game-mechanics", DisplayText = "Category-GameMechanics" },
        new() { Id = "management",     DisplayText = "Category-Management" },
        new() { Id = "minigame",       DisplayText = "Category-Minigame" },
        new() { Id = "social",         DisplayText = "Category-Social" },
        new() { Id = "transportation", DisplayText = "Category-PlayerTransport" }
    ];

    protected override async Task<CurseForgeSearchResult> SearchCFResourceAsync(int index)
    {
        var curseforgeOptions = new CurseforgeSearchOptions()
        {
            ClassId = ClassId.Mods,
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
            ProjectType = "mod",
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