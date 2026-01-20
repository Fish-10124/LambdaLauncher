using LambdaLauncher.Models.Enums;
using LambdaLauncher.Models.Interface;

namespace LambdaLauncher.Models.Displays;

public record WebSourceSelectDisplay : IDataDisplay
{
    public WebResourceSource WebSource { get; init; }
    public string DisplayText { get; init; }

    public WebSourceSelectDisplay(WebResourceSource webSource, string displayText)
    {
        WebSource = webSource;
        DisplayText = displayText;
    }
}