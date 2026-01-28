using MinecraftLaunch.Base.Enums;
using LambdaLauncher.Models.Interface;

namespace LambdaLauncher.Models.Displays;

public record ModLoaderSelectDisplay : IDataDisplay
{
    private string _displayText = null!;

    public ModLoaderType LoaderType { get; init; }

    public string DisplayText
    {
        get => Utils.ResourceLoader.GetString(_displayText);
        init => _displayText = value;
    }
}