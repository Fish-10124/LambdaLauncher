using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using LambdaLauncher.Models;
using LambdaLauncher.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaLauncher.ViewModels;

public partial class SetupViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool CanPrevious { get; private set; } = false;

    public void Navigate(NavigationViewSelectionChangedEventArgs args)
    {
        Type? page = args.SelectedItemContainer.Name switch
        {
            "setupWelcome" => typeof(SetupWelcome),
            "setupGameRoot" => typeof(SetupGameRoot),
            "setupJava" => typeof(SetupJava),
            "setupAccount" => typeof(SetupAccount),
            "setupFinish" => typeof(SetupFinish),
            _ => null
        };

        if (page != null)
        {
            App.NavigationService.Navigate(page, null, args.RecommendedNavigationTransitionInfo);

            var currentIndex = App.NavigationService.NavigationView!.MenuItems.IndexOf(args.SelectedItem);
            CanPrevious = currentIndex > 0;
        }
    }

    public void SaveOptions()
    {
        // TODO: Save setup options
    }

    [RelayCommand]
    private void Previous(NavigationViewItem selectedItem)
    {
        int currentIndex = App.NavigationService.NavigationView!.MenuItems.IndexOf(selectedItem);
        if (currentIndex > 0)
        {
            App.NavigationService.NavigationView.SelectedItem = App.NavigationService.NavigationView.MenuItems[currentIndex - 1];
        }
    }

    [RelayCommand]
    private void Next(NavigationViewItem selectedItem)
    {
        var currentIndex = App.NavigationService.NavigationView!.MenuItems.IndexOf(selectedItem);
        if (currentIndex < App.NavigationService.NavigationView.MenuItems.Count - 1)
        {
            var item = (NavigationViewItem)App.NavigationService.NavigationView.MenuItems[currentIndex + 1];
            item.IsEnabled = true;
            App.NavigationService.NavigationView.SelectedItem = item;
        }
    }
}
