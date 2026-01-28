using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Interfaces;
using MinecraftLaunch.Base.Models.Network;
using LambdaLauncher.Extensions;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LambdaLauncher.ViewModels.ResourceModel;

public abstract partial class ResourceModel : ObservableObject
{
    private readonly IEnumerable<string> AllGameVersions = App.InstanceVersions.Select(v => v.Id);

    [ObservableProperty]
    public partial IResource Resource { get; set; }

    [ObservableProperty]
    public partial BitmapImage? Icon { get; private set; } = null;

    [ObservableProperty]
    public partial ObservableCollection<string> CategoryDisplays { get; private set; } = [];

    [ObservableProperty]
    public partial string Description { get; private set; }

    [ObservableProperty]
    public partial ObservableCollection<string> SupportVersions { get; set; } = [];

    [ObservableProperty]
    public partial string WebSourceDisplay { get; private set; }

    public void Init(IResource resource)
    {
        CategoryDisplays.Clear();
        CategoryDisplays.AddRange(GetCategories(resource));
        Description = MergeSupportVersions(resource.MinecraftVersions);
        var resourceInfo = GetResourceInfo(resource);
        if (!string.IsNullOrEmpty(resourceInfo))
        {
            Description = $"{GetResourceInfo(resource)} | {Description}";
        }
        WebSourceDisplay = ConvertWebSource(resource);
        if (!string.IsNullOrEmpty(resource.IconUrl))
        {
            Icon = new(new Uri(resource.IconUrl));
        }
        this.Resource = resource;

        // 获取版本信息
        SupportVersions = [Utils.ResourceLoader.GetString("AllGameVersion"), .. resource.MinecraftVersions.OrderByList(AllGameVersions)];
    }

    protected virtual string GetResourceInfo(IResource resource)
    {
        return "";
    }

    protected abstract IEnumerable<string> GetCategories(IResource resource);

    public static string MergeSupportVersions(IEnumerable<string> versions)
    {
        List<Version> filteredVersions = [.. versions.Where(v => !v.Any(char.IsLetter)).Select(Version.Parse)];
        filteredVersions.Sort();

        var majorVersions = filteredVersions.Select(v => new Version(v.Major, v.Minor)).Distinct().ToList();

        // Find continuous version ranges
        List<string> ranges = [];
        int start = 0;

        while (start < majorVersions.Count)
        {
            int end = start;
            while (end + 1 < majorVersions.Count && majorVersions[end + 1].Minor == majorVersions[end].Minor + 1)
            {
                end++;
            }

            if (end == majorVersions.Count - 1 && majorVersions[end].Minor == 21)
            {
                ranges.Add($"{majorVersions[start].Major}.{majorVersions[start].Minor}+");
            }
            else if (start == end)
            {
                ranges.Add($"{majorVersions[start].Major}.{majorVersions[start].Minor}");
            }
            else
            {
                ranges.Add($"{majorVersions[start].Major}.{majorVersions[start].Minor}-{majorVersions[end].Major}.{majorVersions[end].Minor}");
            }

            start = end + 1;
        }

        return string.Join(", ", ranges);
    }

    public static string ConvertWebSource(IResource source)
    {
        return source switch
        {
            CurseforgeResource => Utils.ResourceLoader.GetString("WebSource-CurseForge"),
            ModrinthResource => Utils.ResourceLoader.GetString("WebSource-Modrinth"),
            _ => "",
        };
    }

    public static SettingsCardDisplay ConvertFileToCardDisplay(IResourceFile file)
    {
        var loaderStrings = file.Loaders.SelectMany(l =>
        {
            string? key = l switch
            {
                ModLoaderType.Forge => "Forge",
                ModLoaderType.NeoForge => "NeoForge",
                ModLoaderType.Fabric => "Fabric",
                ModLoaderType.Quilt => "Quilt",
                ModLoaderType.Canvas => "Canvas",
                ModLoaderType.Iris => "Iris",
                ModLoaderType.OptiFine => "OptiFine",
                ModLoaderType.Vanilla => "Vanilla",
                _ => null
            };

            return key == null ? Enumerable.Empty<string>() : [Utils.ResourceLoader.GetString($"Loader-{key}")];
        });

        return new() 
        {
            Header = file.DisplayName,
            Description = $"{string.Format(Utils.ResourceLoader.GetString("FileDetails-FileName"), file.FileName)}, " +
                         $"{string.Format(Utils.ResourceLoader.GetString("FileDetails-DownloadCount"), file.DownloadCount)}, " +
                         $"{string.Format(Utils.ResourceLoader.GetString("FileDetails-ReleaseDate"), file.Published.ToShortDateString())}",
            Icon = Utils.GetIconFromResources(file.ReleaseType switch
            {
                FileReleaseType.Release => "ReleaseIcon",
                FileReleaseType.Beta => "BetaIcon",
                FileReleaseType.Alpha => "AlphaIcon",
                _ => throw new NotImplementedException(),
            })!,
            Parameter = file.GameVersions.CartesianProduct(loaderStrings, (ver, loader) => $"{loader} {ver}", EmptyCartesianBehavior.ReturnSource)
        };
    }
}