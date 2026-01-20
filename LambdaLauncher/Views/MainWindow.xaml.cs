using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LambdaLauncher.Models.Record;
using LambdaLauncher.ViewModels;
using System.Collections.Generic;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.Views;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; private set; } = new();

    public MainWindow()
    {
        InitializeComponent();

        this.ExtendsContentIntoTitleBar = true;
        this.SetTitleBar(appTitle);

        var manager = WindowManager.Get(this);
        this.SetWindowSize(1080, 750);
        manager.MinWidth = 520;
        manager.MinHeight = 360;

        App.BreadcrumbService.BreadcrumbChanged += BreadcrumbService_BreadcrumbChanged;
        App.NavigationService.Initialize(navView, contentFrame);

        navView.SelectedItem = navItemHome;
        App.NavigationService.Navigate(typeof(Home));
    }

    private void breadcrumbsHeader_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        var item = (PageHeader)args.Item;
        App.NavigationService.Navigate(item.Page, item.Parameter, item.InfoOverride);
    }

    private void BreadcrumbService_BreadcrumbChanged(IReadOnlyList<PageHeader> items)
    {
        breadcrumbsHeader.Header = items;
    }

    private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.Navigate(args);
    }
}