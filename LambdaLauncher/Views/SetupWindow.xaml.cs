using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LambdaLauncher.Models;
using LambdaLauncher.ViewModels;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.Views;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SetupWindow : Window
{
    public SetupViewModel ViewModel { get; private set; } = new();

    public SetupWindow()
    {
        InitializeComponent();

        this.ExtendsContentIntoTitleBar = true;

        var manager = WindowManager.Get(this);
        this.SetWindowSize(1080, 750);
        manager.MinWidth = 520;
        manager.MinHeight = 360;

        App.NavigationService.Initialize(navView, contentFrame);
        App.NavigationService.Navigate(typeof(SetupWelcome));

        navView.SelectedItem = setupWelcome;
    }

    private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.Navigate(args);

        nextButton.Content = Utils.ResourceLoader.GetString(App.NavigationService.NavigationView?.MenuItems.IndexOf(args.SelectedItem) < App.NavigationService.NavigationView?.MenuItems.Count - 1 ? "Next" : "Finish");
    }

    private void nextButton_Click(object sender, RoutedEventArgs e)
    {
        var currentIndex = navView.MenuItems.IndexOf(navView.SelectedItem);
        if (currentIndex >= navView.MenuItems.Count - 1)
        {
            ViewModel.SaveOptions();

            this.Close();
        }
    }
}
