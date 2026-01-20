using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Record;
using LambdaLauncher.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class DownloadInstanceDetails : Page
{
    private DownloadInstanceDetailsModel ViewModel { get; set; } = new();

    public DownloadInstanceDetails()
    {
        InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        // 版本信息移动动画
        var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
        anim?.TryStart(instanceDetails.RootExpander.Header as UIElement);

        var display = (SettingsCardDisplay)e.Parameter;

        App.BreadcrumbService.SetHeader(
            new PageHeader()
            {
                Page = typeof(DownloadInstance),
                Text = Utils.ResourceLoader.GetString("Header-Instance"),
                InfoOverride = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft }
            },
            new PageHeader()
            {
                Page = typeof(DownloadInstanceDetails),
                Text = display.Header
            }
        );

        await ViewModel.InitAsync(display);
    }

    private void modLoadersView_SelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args)
    {
        ViewModel.ItemsSelect(sender.SelectedItems);
    }
}