using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Models.Network;
using LambdaLauncher.Models.Enums;
using LambdaLauncher.Models.Interface;

namespace LambdaLauncher.Models.Displays;

public record SortMethodDisplay : IDataDisplay
{
    /// <summary>
    /// 为null表示CurseForge没有这个排序类型, 即Modrinth专属
    /// </summary>
    public SortField? CurseForgeSortType { get; init; }
    /// <summary>
    /// 为null表示Modrinth没有这个排序类型, 即CurseForge专属
    /// </summary>
    public ModrinthSearchIndex? ModrinthSortType { get; init; }
    public SortMethodType SortType { get; init; }
    public string DisplayText { get; init; }

    public SortMethodDisplay(SortMethodType sortType, SortField? curseForgeSortType, ModrinthSearchIndex? modrinthSortType, string displayText)
    {
        SortType = sortType;
        CurseForgeSortType = curseForgeSortType;
        ModrinthSortType = modrinthSortType;
        DisplayText = displayText;
    }
}