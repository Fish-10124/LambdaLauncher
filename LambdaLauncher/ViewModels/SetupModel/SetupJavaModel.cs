using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using MinecraftLaunch.Base.Models.Game;
using MinecraftLaunch.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaLauncher.ViewModels.SetupModel;

public partial class SetupJavaModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsSearching { get; set; } = false;

    [ObservableProperty]
    public partial ObservableCollection<JavaEntry> Javas { get; set; } = [];

    [RelayCommand]
    public async Task SearchJavas()
    {
        IsSearching = true;
        await foreach (var java in JavaUtil.EnumerableJavaAsync())
        {
            Javas.Add(java);
        }
        IsSearching = false;
    }

    [RelayCommand]
    public async Task SelectJava()
    {
        var file = await Utils.PickFileAsync(App.SetupWindow!, ".exe");
        if (file is null)
        {
            return;
        }
        var javaEntry = await JavaUtil.GetJavaInfoAsync(file.Path);

        if (!Javas.Contains(javaEntry))
        {
            Javas.Add(javaEntry);
        }
    }
}
