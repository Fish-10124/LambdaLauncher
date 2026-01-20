using LambdaLauncher.Models;
using LambdaLauncher.Models.Record;
using LambdaLauncher.Services;
using LambdaLauncher.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static Window? MainWindow { get; private set; }
        public static Window? SetupWindow { get; private set; }

        public static BreadcrumbService BreadcrumbService { get; } = new();
        public static NavigationService NavigationService { get; } = new();

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (await TryLoadConfig())
            {
                MainWindow = new MainWindow();
                MainWindow.Activate();
                return;
            }

            SetupWindow = new SetupWindow();
            SetupWindow.Closed += SetupWindowClosed;
            SetupWindow.Activate();
        }

        private void SetupWindowClosed(object sender, WindowEventArgs args)
        {
            if (false /*!await TryLoadConfig()*/)
            {
                // User closed the setup window without completing setup
                SetupWindow = null;
                MainWindow = null;
                this.Exit();
            }
            MainWindow = new MainWindow();
            MainWindow.Activate();

            SetupWindow?.Closed -= SetupWindowClosed;
            SetupWindow = null;
        }

        private static async Task<bool> TryLoadConfig()
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync("config.json");

                string configJson = await FileIO.ReadTextAsync(file);
                Global.LocalConfig = JsonSerializer.Deserialize<LocalConfig>(configJson)!;

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Faild to read config file: {ex.Message}");
                return false;
            }
        }
    }
}
