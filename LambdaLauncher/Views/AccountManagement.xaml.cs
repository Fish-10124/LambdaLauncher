using LambdaLauncher.Models;
using LambdaLauncher.Models.Record;
using LambdaLauncher.ViewModels;
using LambdaLauncher.ViewModels.DownloadResourceModel;
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

namespace LambdaLauncher.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AccountManagement : Page
{
    public AccountManagementModel ViewModel { get; private set; } = new();

    public AccountManagement()
    {
        InitializeComponent();

        
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        App.NavigationService.SetHeader(
            new PageHeader()
            {
                Page = typeof(AccountManagement),
                Text = Utils.ResourceLoader.GetString("Header-Account")
            }
        );
    }
}
