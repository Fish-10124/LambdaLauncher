using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Interfaces;
using LambdaLauncher.ViewModels.ResourceModel;
using System;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.UserControls;

public sealed partial class ResourceCard : UserControl
{
    public static readonly DependencyProperty ResourceModelProperty =
        DependencyProperty.Register(
            nameof(ResourceModel),
            typeof(ResourceModel),
            typeof(ResourceCard),
            new PropertyMetadata(null));

    public new static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register(
            nameof(Content),
            typeof(object),
            typeof(ResourceCard),
            new PropertyMetadata(null));

    public new static readonly DependencyProperty TagProperty =
        DependencyProperty.Register(
            nameof(Tag),
            typeof(object),
            typeof(ResourceCard),
            new PropertyMetadata(null, OnTagChanged));

    public static readonly DependencyProperty ResourceDisplayProperty =
        DependencyProperty.Register(
            nameof(ResourceDisplay),
            typeof(IResource),
            typeof(ResourceCard),
            new PropertyMetadata(default, OnResourceChanged));

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(ResourceCard),
            new PropertyMetadata(null));

    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(
            nameof(CommandParameter),
            typeof(object),
            typeof(ResourceCard),
            new PropertyMetadata(null));

    public ResourceModel? ResourceModel
    {
        get => (ResourceModel?)GetValue(ResourceModelProperty);
        set => SetValue(ResourceModelProperty, value);
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

    public IResource ResourceDisplay
    {
        get => (IResource)GetValue(ResourceDisplayProperty);
        set => SetValue(ResourceDisplayProperty, value);
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

    public ResourceCard()
    {
        InitializeComponent();
    }

    public event RoutedEventHandler Clicked
    {
        add => rootCard.Click += value;
        remove => rootCard.Click -= value;
    }

    public event RoutedEventHandler? Click;

    private static void OnTagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (ResourceCard)d;
        if (control?.rootCard is not null)
        {
            control.rootCard.Tag = e.NewValue;
        }
    }

    private static void OnResourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (ResourceCard)d;
        if (control is null)
        {
            return;
        }

        var resource = (IResource)e.NewValue;
        control.ResourceModel = resource.ResourceType switch
        {
            ResourceType.Mod => new ModDetailsResourceModel(),
            ResourceType.Modpack => new ModPackDetailsResourceModel(),
            ResourceType.Resourcepack => new ResourcePackDetailsResourceModel(),
            ResourceType.Shaderpack => new ShaderDetailsResourceModel(),
            _ => throw new NotImplementedException()
        };
        control.ResourceModel?.Init(control.ResourceDisplay);
    }

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