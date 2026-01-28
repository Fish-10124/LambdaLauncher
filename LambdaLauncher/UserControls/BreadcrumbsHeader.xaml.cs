
using Microsoft.UI.Xaml.Controls;
using LambdaLauncher.Models.Record;
using Windows.Foundation;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.UserControls;

public sealed partial class BreadcrumbsHeader : UserControl
{
    public ObservableCollection<PageHeader> HeaderItems { get; set; } = [];

    public BreadcrumbsHeader()
    {
        InitializeComponent();
    }

    public event TypedEventHandler<BreadcrumbBar, BreadcrumbBarItemClickedEventArgs> ItemClicked
    {
        add => baseBreadcrumbBar.ItemClicked += value;
        remove => baseBreadcrumbBar.ItemClicked -= value;
    }
}