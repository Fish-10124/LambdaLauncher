using CommunityToolkit.Mvvm.ComponentModel;

namespace LambdaLauncher.ViewModels;

public partial class PageChangerModel : ObservableObject
{
    [ObservableProperty]
    public partial int CurrentPageDisplay { get; set; } = 1;
}