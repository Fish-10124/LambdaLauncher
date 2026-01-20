using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LambdaLauncher.Models.UserEventArgs;
using LambdaLauncher.ViewModels;
using System;
using System.Windows.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.UserControls;

public sealed partial class PageChanger : UserControl
{
    private PageChangerModel ViewModel { get; set; } = new();

    public static readonly DependencyProperty TotalPageProperty = DependencyProperty.Register(
        nameof(TotalPage),
        typeof(int),
        typeof(PageChanger),
        new PropertyMetadata(0));

    public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
        nameof(CurrentPage),
        typeof(int),
        typeof(PageChanger),
        new PropertyMetadata(0, OnCurrentPageChanged));

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command),
        typeof(ICommand),
        typeof(PageChanger),
        new PropertyMetadata(null));

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter),
        typeof(object),
        typeof(PageChanger),
        new PropertyMetadata(null));

    public int TotalPage
    {
        get => (int)GetValue(TotalPageProperty);
        set => SetValue(TotalPageProperty, value);
    }

    public int CurrentPage
    {
        get => (int)GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public event EventHandler<PageChangedEventArgs>? PageChanged;

    public PageChanger()
    {
        InitializeComponent();
    }

    private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (PageChanger)d;
        control.ViewModel.CurrentPageDisplay = (int)e.NewValue + 1;
    }

    private void pageInputBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        var value = (int)args.NewValue;
        if (args.NewValue > TotalPage)
        {
            value = TotalPage;
        }
        else if (args.NewValue < 1)
        {
            value = 1;
        }
        pageInputBox.Value = value;
        value--;

        if (value != CurrentPage)
        {
            CurrentPage = value;
            pageInputFlyout.Hide();
            PageChanging();
        }
    }

    private void PageChanging()
    {
        var pageChangedArgs = GetPageChangedArgs();
        PageChanged?.Invoke(this, pageChangedArgs);

        var parameter = CommandParameter ?? pageChangedArgs;
        if (Command?.CanExecute(parameter) == true)
            Command.Execute(parameter);
    }

    private PageChangedEventArgs GetPageChangedArgs()
    {
        return new PageChangedEventArgs { CurrentPage = CurrentPage };
    }

    private void FirstPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentPage != 0)
        {
            CurrentPage = 0;
            PageChanging();
        }
    }

    private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
            PageChanging();
        }
    }

    private void NextPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentPage < TotalPage - 1)
        {
            CurrentPage++;
            PageChanging();
        }
    }

    private void LastPageButton_Click(object sender, RoutedEventArgs e)
    {
        if (CurrentPage != TotalPage - 1)
        {
            CurrentPage = TotalPage - 1;
            PageChanging();
        }
    }
}