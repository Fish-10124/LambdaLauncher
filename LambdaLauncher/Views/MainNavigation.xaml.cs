using LambdaLauncher.Models.Record;
using LambdaLauncher.UserControls;
using LambdaLauncher.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainNavigation : Page
    {
        public MainNavigationModel ViewModel { get; private set; } = new();

        public MainNavigation()
        {
            InitializeComponent();
            App.NavigationService.Initialize(breadcrumbsHeader, contentFrame);

            navView.SelectedItem = navItemHome;
            App.NavigationService.Navigate(typeof(Home));
        }

        private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            ViewModel.Navigate(args);
        }
    }
}
