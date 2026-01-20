using MinecraftLaunch.Base.Enums;
using LambdaLauncher.Models.Interface;

namespace LambdaLauncher.Models.Displays;

public record ModLoaderSelectDisplay : IDataDisplay
{
    public ModLoaderType LoaderType { get; init; }
    public string DisplayText { get; init; }

    public ModLoaderSelectDisplay(ModLoaderType loaderType, string displayText)
    {
        LoaderType = loaderType;
        DisplayText = displayText;
    }
}