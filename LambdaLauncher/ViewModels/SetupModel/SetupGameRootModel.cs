using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace LambdaLauncher.ViewModels.SetupModel;

public partial class SetupGameRootModel : ObservableObject
{
    [ObservableProperty]
    public partial ObservableCollection<FolderDisplay> GameRootFolders { get; set; } = [];

    [RelayCommand]
    private void AddDefaultFolder()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.minecraft";
        var folderDisplay = new FolderDisplay()
        {
            FullPath = folder
        };

        if (!GameRootFolders.Contains(folderDisplay))
        {
            GameRootFolders.Add(folderDisplay);
        }
    }

    [RelayCommand]
    private async Task SelectFolder()
    {
        var folder = await Utils.PickFolderAsync(App.SetupWindow!);
        if (folder is null)
        {
            return;
        }

        var folderDisplay = new FolderDisplay()
        {
            FullPath = folder.Path
        };

        if (!GameRootFolders.Contains(folderDisplay))
        {
            GameRootFolders.Add(folderDisplay);
        }
    }
}
