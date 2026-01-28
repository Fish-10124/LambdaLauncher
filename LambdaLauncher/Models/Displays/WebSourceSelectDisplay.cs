using LambdaLauncher.Models.Enums;
using LambdaLauncher.Models.Interface;

namespace LambdaLauncher.Models.Displays;

public record WebSourceSelectDisplay : IDataDisplay
{
    private string _displayText = null!;

    public WebResourceSource WebSource { get; init; }

    public required string DisplayText
    {
        get => Utils.ResourceLoader.GetString(_displayText);
        init => _displayText = value;
    }
}