namespace LambdaLauncher.Models.Displays;

public record AllCategoryDisplay : CategoryDisplay
{
    public required int? CurseForgeId { get; init; }
    public required string CurseForgeIdText { get; init; }
    public required string ModrinthId { get; init; }
}