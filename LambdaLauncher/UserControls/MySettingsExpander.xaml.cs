using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.UserControls;

public sealed partial class MySettingsExpander : UserControl
{
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(MySettingsExpander),
            new PropertyMetadata(default));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(
            nameof(Description),
            typeof(string),
            typeof(MySettingsExpander),
            new PropertyMetadata(default));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(
            nameof(Icon),
            typeof(BitmapImage),
            typeof(MySettingsExpander),
            new PropertyMetadata(default));

    public new static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(
            nameof(Content),
            typeof(object),
            typeof(MySettingsExpander),
            new PropertyMetadata(null, OnContentChanged));

    public new static readonly DependencyProperty TagProperty =
        DependencyProperty.Register(
            nameof(Tag),
            typeof(object),
            typeof(MySettingsExpander),
            new PropertyMetadata(null, OnTagChanged));

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(
            nameof(Items),
            typeof(IList<object>),
            typeof(MySettingsExpander),
            new PropertyMetadata(null, OnItemsChanged));

    public static readonly DependencyProperty IsExpandedProperty =
        DependencyProperty.Register(
            nameof(IsExpanded),
            typeof(bool),
            typeof(MySettingsExpander),
            new PropertyMetadata(default));

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public BitmapImage Icon
    {
        get => (BitmapImage)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public new object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public new object Tag
    {
        get => GetValue(TagProperty);
        set => SetValue(TagProperty, value);
    }

    public IList<object> Items
    {
        get => (IList<object>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public SettingsExpander RootExpander => rootExpander;

    public MySettingsExpander()
    {
        InitializeComponent();

        Items = [];
    }

    private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = d as MySettingsExpander;
        if (control?.rootExpander is not null)
        {
            control.rootExpander.Content = e.NewValue;
        }
    }

    private static void OnTagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = d as MySettingsExpander;
        if (control?.rootExpander is not null)
        {
            control.rootExpander.Tag = e.NewValue;
        }
    }

    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = d as MySettingsExpander;
        if (control?.rootExpander is not null)
        {
            control.rootExpander.Items = (IList<object>)e.NewValue;
        }
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (Content is not null) rootExpander.Content = Content;
        if (Tag is not null) rootExpander.Tag = Tag;
        if (Items is not null) rootExpander.Items = Items;
    }
}