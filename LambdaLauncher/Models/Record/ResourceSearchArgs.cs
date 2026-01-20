using MinecraftLaunch.Base.Enums;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Enums;
using LambdaLauncher.Models.UserEventArgs;
using LambdaLauncher.ViewModels.DownloadResourceModel;

namespace LambdaLauncher.Models.Record;

public record ResourceSearchArgs
{
    public string SearchText { get; set; } = "";
    public WebResourceSource ResourceSource { get; set; }

    /// <summary>
    /// 为null表示任意版本
    /// </summary>
    public string? GameVersion { get; set; } = null;

    public bool IsAnyVersionSelected => GameVersion is null;

    public CategoryDisplay Category { get; set; } = DownloadModModel.CategoryDisplays[0];
    public SortMethodType SortMethod { get; set; } = SortMethodType.Relevance;
    public ModLoaderSelectDisplay ModLoader { get; set; } = Global.ModLoaderSelectDisplays[ModLoaderType.Any];
    public int PageIndex { get; set; } = 0;

    public ResourceSearchArgs()
    {
    }

    public ResourceSearchArgs(ResourceSearchArgs origin)
    {
        SearchText = origin.SearchText;
        ResourceSource = origin.ResourceSource;
        GameVersion = origin.GameVersion;
        Category = origin.Category;
        SortMethod = origin.SortMethod;
        ModLoader = origin.ModLoader;
        PageIndex = origin.PageIndex;
    }

    public ResourceSearchArgs(ResourceSearchEventArgs args)
    {
        SearchText = args.SearchText;
        ResourceSource = args.ResourceSource;
        GameVersion = args.GameVersion;
        Category = args.Category;
        SortMethod = args.SortMethod;
        ModLoader = args.ModLoader;
        PageIndex = 0;
    }
}
