using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftLaunch.Base.Models.Game;
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
    public partial ObservableCollection<JavaEntry> Javas { get; set; } = [];
}
