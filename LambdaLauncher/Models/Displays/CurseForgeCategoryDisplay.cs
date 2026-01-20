using Microsoft.UI.Xaml;

namespace LambdaLauncher.Models.Displays;

public record CurseForgeCategoryDisplay : CategoryDisplay
{
    public required int? Id { get; init; }
    public required string IdText { get; init; }
    public int Indentation { get; init; } = 0;

    public Thickness IndentationMargin => new(10 * Indentation, 0, 0, 0);
}