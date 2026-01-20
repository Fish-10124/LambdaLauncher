using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Network;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Enums;
using LambdaLauncher.Models.Record;
using System.Collections.Generic;

namespace LambdaLauncher.Models;

public class Global
{
    public static readonly Dictionary<ModLoaderType, ModLoaderSelectDisplay> ModLoaderSelectDisplays = new()
    {
        {ModLoaderType.Any, new(ModLoaderType.Any, Utils.ResourceLoader.GetString("AllModLoader"))},
        {ModLoaderType.Forge, new(ModLoaderType.Forge, Utils.ResourceLoader.GetString("Loader-Forge"))},
        {ModLoaderType.NeoForge, new(ModLoaderType.NeoForge, Utils.ResourceLoader.GetString("Loader-NeoForge"))},
        {ModLoaderType.Fabric, new(ModLoaderType.Fabric, Utils.ResourceLoader.GetString("Loader-Fabric"))},
        {ModLoaderType.Quilt, new(ModLoaderType.Quilt, Utils.ResourceLoader.GetString("Loader-Quilt"))}
    };

    public static readonly Dictionary<WebResourceSource, WebSourceSelectDisplay> WebResourceSourceDisplays = new()
        {
            {WebResourceSource.All, new(WebResourceSource.All, Utils.ResourceLoader.GetString("AllWebResourceSources"))},
            {WebResourceSource.CurseForge, new(WebResourceSource.CurseForge, Utils.ResourceLoader.GetString("WebSource-CurseForge"))},
            {WebResourceSource.Modrinth, new(WebResourceSource.Modrinth, Utils.ResourceLoader.GetString("WebSource-Modrinth"))}
        };

    public static readonly Dictionary<SortMethodType, SortMethodDisplay> ResourceSortMethods = new()
        {
            {SortMethodType.Relevance, new(SortMethodType.Relevance, SortField.Featured, ModrinthSearchIndex.Relevance, Utils.ResourceLoader.GetString("SortMethod-Relevance"))},
            {SortMethodType.Downloads, new(SortMethodType.Downloads, SortField.TotalDownloads, ModrinthSearchIndex.Downloads, Utils.ResourceLoader.GetString("SortMethod-TotalDownloads"))},
            {SortMethodType.Follows, new(SortMethodType.Follows, null, ModrinthSearchIndex.Follows, Utils.ResourceLoader.GetString("SortMethod-Follows"))},
            {SortMethodType.ReleasedDate, new(SortMethodType.ReleasedDate, SortField.ReleasedDate, ModrinthSearchIndex.DatePublished, Utils.ResourceLoader.GetString("SortMethod-ReleasedDate"))},
            {SortMethodType.LatestUpdated, new(SortMethodType.LatestUpdated, SortField.LastUpdated, ModrinthSearchIndex.DateUpdated, Utils.ResourceLoader.GetString("SortMethod-LatestUpdate"))},
            {SortMethodType.Featured, new(SortMethodType.Featured, SortField.Featured, null, Utils.ResourceLoader.GetString("SortMethod-Featured"))},
            {SortMethodType.Popularity, new(SortMethodType.Popularity, SortField.Popularity, null, Utils.ResourceLoader.GetString("SortMethod-Popularity"))},
            {SortMethodType.Name, new(SortMethodType.Name, SortField.Name, null, Utils.ResourceLoader.GetString("SortMethod-Name"))},
            {SortMethodType.Author, new(SortMethodType.Author, SortField.Author, null, Utils.ResourceLoader.GetString("SortMethod-Author"))},
            {SortMethodType.TotalDownloads, new(SortMethodType.TotalDownloads, SortField.TotalDownloads, null, Utils.ResourceLoader.GetString("SortMethod-TotalDownloads"))},
            {SortMethodType.Category, new(SortMethodType.Category, SortField.Category, null, Utils.ResourceLoader.GetString("SortMethod-Category"))},
            {SortMethodType.GameVersion, new(SortMethodType.GameVersion, SortField.GameVersion, null, Utils.ResourceLoader.GetString("SortMethod-GameVersion"))},
            {SortMethodType.EarlyAccess, new(SortMethodType.EarlyAccess, SortField.EarlyAccess, null, Utils.ResourceLoader.GetString("SortMethod-EarlyAccess"))},
            {SortMethodType.FeaturedReleased, new(SortMethodType.FeaturedReleased, SortField.FeaturedReleased, null, Utils.ResourceLoader.GetString("SortMethod-FeaturedReleased"))},
            {SortMethodType.Rating, new(SortMethodType.Rating, SortField.Rating, null, Utils.ResourceLoader.GetString("SortMethod-Rating"))}
        };

    public static Dictionary<ResourceType, ResourceSearchArgs?> LastSearchArgs { get; set; } = new()
    {
        { ResourceType.Mod, null },
        { ResourceType.Modpack, null  },
        { ResourceType.Resourcepack, null },
        { ResourceType.Shaderpack, null }
    };

    public static IEnumerable<VersionManifestEntry> InstanceVersions { get; set; } = [];

    public static LocalConfig? LocalConfig { get; set; }
}