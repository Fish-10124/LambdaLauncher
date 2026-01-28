using LambdaLauncher.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SetupNavigation : Page
{
    public SetupViewModel ViewModel { get; private set; }

    public SetupNavigation()
    {
        InitializeComponent();
        ViewModel = new SetupViewModel(navView, contentFrame);

        navView.SelectedItem = setupWelcome;
    }

    private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.Navigate(args);
        ViewModel.CurrentPageIndex = navView.MenuItems.IndexOf(args.SelectedItem);
    }

    private void nextButton_Click(object sender, RoutedEventArgs e)
    {
        var currentIndex = navView.MenuItems.IndexOf(navView.SelectedItem);
        if (ViewModel.CanFinish)
        {
            ViewModel.SaveOptions();

            App.SetupWindow!.Close();
        }
    }
}
