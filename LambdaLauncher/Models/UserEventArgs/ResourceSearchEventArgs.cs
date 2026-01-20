using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Enums;
using System;

namespace LambdaLauncher.Models.UserEventArgs;

public class ResourceSearchEventArgs : EventArgs
{
    public required string SearchText { get; set; }
    public WebResourceSource ResourceSource { get; set; }

    /// <summary>
    /// 为null表示任意版本
    /// </summary>
    public string? GameVersion { get; set; } = null;

    public bool IsAnyVersionSelected => GameVersion is null;

    public required CategoryDisplay Category { get; set; }
    public required SortMethodType SortMethod { get; set; }
    public required ModLoaderSelectDisplay ModLoader { get; set; }
}