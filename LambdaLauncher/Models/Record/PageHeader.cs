using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace LambdaLauncher.Models.Record;

public record PageHeader
{
    public required Type Page { get; init; }
    public required string Text { get; init; }
    public object? Parameter { get; init; } = null;
    public NavigationTransitionInfo? InfoOverride { get; init; } = null;
}