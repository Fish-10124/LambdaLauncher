using Microsoft.UI.Xaml.Media.Imaging;

namespace LambdaLauncher.Models.Displays;

public record SettingsCardDisplay
{
    public string? Header { get; init; }
    public string? Description { get; init; }
    public BitmapImage? Icon { get; init; }
    public object? Parameter { get; init; }
}