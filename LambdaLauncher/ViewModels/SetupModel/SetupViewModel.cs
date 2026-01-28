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
using Microsoft.UI.Xaml.Media.Animation;

namespace LambdaLauncher.ViewModels;

public partial class SetupViewModel(NavigationView navView, Frame contentFrame) : ObservableObject
{
    private readonly NavigationView NavView = navView;
    private readonly Frame ContentFrame = contentFrame;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanPrevious))]
    [NotifyPropertyChangedFor(nameof(CanFinish))]
    [NotifyPropertyChangedFor(nameof(NextButtonText))]
    public partial int CurrentPageIndex { get; set; } = 0;

    public bool CanPrevious => CurrentPageIndex > 0;
    public bool CanFinish => CurrentPageIndex == NavView.MenuItems.Count - 1;
    public string NextButtonText => Utils.ResourceLoader.GetString(CanFinish ? "Finish" : "Next");

    [ObservableProperty]
    public partial bool CanNext { get; set; } = false;


    public void Navigate(NavigationViewSelectionChangedEventArgs args)
    {
        var selectedItem = (NavigationViewItem)args.SelectedItem;
        Navigate(selectedItem.Name, args.RecommendedNavigationTransitionInfo);
    }

    private void Navigate(string pageName, NavigationTransitionInfo? transitionInfo = null)
    {
        Type? page = pageName switch
        {
            "setupWelcome" => typeof(SetupWelcome),
            "setupGameRoot" => typeof(SetupGameRoot),
            "setupJava" => typeof(SetupJava),
            "setupFinish" => typeof(SetupFinish),
            _ => null
        };

        if (page is not null)
        {
            ContentFrame.Navigate(page, null, transitionInfo);
        }
    }

    public void SaveOptions()
    {
        // TODO: Save setup options
    }

    [RelayCommand]
    private void Previous(NavigationViewItem selectedItem)
    {
        if (CanPrevious)
        {
            CurrentPageIndex--;
            Navigate(GetPreviousItem(selectedItem).Name);
        }
    }

    [RelayCommand]
    private void Next(NavigationViewItem selectedItem)
    {
        if (CanNext)
        {
            CurrentPageIndex++;
            Navigate(GetNextItem(selectedItem).Name);
        }
    }

    private NavigationViewItem GetNextItem(NavigationViewItem selectedItem)
    {
        var currentIndex = NavView.MenuItems.IndexOf(selectedItem);
        if (currentIndex < 0 || currentIndex > NavView.MenuItems.Count - 1)
        {
            throw new IndexOutOfRangeException("Selected item is out of range.");
        }
        return (NavigationViewItem)NavView.MenuItems[currentIndex + 1];
    }

    private NavigationViewItem GetPreviousItem(NavigationViewItem selectedItem)
    {
        var currentIndex = NavView.MenuItems.IndexOf(selectedItem);
        if (currentIndex < 0 || currentIndex > NavView.MenuItems.Count - 1)
        {
            throw new IndexOutOfRangeException("Selected item is out of range.");
        }
        return (NavigationViewItem)NavView.MenuItems[currentIndex - 1];
    }
}
