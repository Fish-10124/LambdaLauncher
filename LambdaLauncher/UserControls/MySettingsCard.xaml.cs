using CommunityToolkit.WinUI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.UserControls;

public sealed partial class MySettingsCard : UserControl
{
    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(MySettingsCard),
            new PropertyMetadata(default));

    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(
            nameof(Description),
            typeof(string),
            typeof(MySettingsCard),
            new PropertyMetadata(default));

    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(
            nameof(Icon),
            typeof(BitmapImage),
            typeof(MySettingsCard),
            new PropertyMetadata(default));

    public new static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(
            nameof(Content),
            typeof(object),
            typeof(MySettingsCard),
            new PropertyMetadata(null));

    public new static readonly DependencyProperty TagProperty =
        DependencyProperty.Register(
            nameof(Tag),
            typeof(object),
            typeof(MySettingsCard),
            new PropertyMetadata(null));

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(MySettingsCard),
            new PropertyMetadata(null));

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(
            nameof(CommandParameter),
            typeof(object),
            typeof(MySettingsCard),
            new PropertyMetadata(null));

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

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public SettingsCard RootCard => rootCard;

    public MySettingsCard()
    {
        InitializeComponent();
    }

    public event RoutedEventHandler? Click;

    private void rootCard_Click(object sender, RoutedEventArgs e)
    {
        Click?.Invoke(this, e);

        if (Command?.CanExecute(CommandParameter) == true)
            Command.Execute(CommandParameter);
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (Content is not null) rootCard.Content = Content;
        if (Tag is not null) rootCard.Tag = Tag;
        CommandParameter ??= rootCard;
    }
}