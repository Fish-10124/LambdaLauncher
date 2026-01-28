using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Network;
using LambdaLauncher.Models.Enums;
using LambdaLauncher.Models.Interface;

namespace LambdaLauncher.Models.Displays;

public record SortMethodDisplay : IDataDisplay
{
    private string _displayText = null!;

    /// <summary>
    /// 为null表示CurseForge没有这个排序类型, 即Modrinth专属
    /// </summary>
    public SortField? CurseForgeSortType { get; init; }

    /// <summary>
    /// 为null表示Modrinth没有这个排序类型, 即CurseForge专属
    /// </summary>
    public ModrinthSearchIndex? ModrinthSortType { get; init; }

    public SortMethodType SortType { get; init; }

    public required string DisplayText 
    {
        get => Utils.ResourceLoader.GetString(_displayText);
        init => _displayText = value;
    }
}