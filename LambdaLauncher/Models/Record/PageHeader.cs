using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace LambdaLauncher.Models.Record;

public record PageHeader
{
    public Type? Page { get; set; }
    public string? Text { get; set; }
    public object? Parameter { get; set; } = null;
    public NavigationTransitionInfo? InfoOverride { get; set; } = null;
}