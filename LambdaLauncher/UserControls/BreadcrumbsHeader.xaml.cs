using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LambdaLauncher.Models.Record;
using System.Collections.Generic;
using Windows.Foundation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.UserControls;

public sealed partial class BreadcrumbsHeader : UserControl
{
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(IEnumerable<PageHeader>),
        typeof(BreadcrumbsHeader),
        new PropertyMetadata(default));

    public IEnumerable<PageHeader> Header
    {
        get => (IEnumerable<PageHeader>)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

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