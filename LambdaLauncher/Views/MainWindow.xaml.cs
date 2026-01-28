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
    public MainWindow()
    {
        InitializeComponent();

        this.ExtendsContentIntoTitleBar = true;
        this.SetTitleBar(appTitle);

        var manager = WindowManager.Get(this);
        this.SetWindowSize(1080, 750);
        manager.MinWidth = 520;
        manager.MinHeight = 360;

        contentFrame.Navigate(typeof(MainNavigation));
    }
}