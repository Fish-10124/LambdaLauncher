using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using MinecraftLaunch.Base.Enums;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using LambdaLauncher.Models.Enums;
using LambdaLauncher.Models.Record;
using LambdaLauncher.Models.UserEventArgs;
using LambdaLauncher.ViewModels.DownloadResourceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LambdaLauncher.UserControls;

public sealed partial class SearchHeader : UserControl
{
    private IEnumerable<WebSourceSelectDisplay> ResourceSources { get; } = Global.WebResourceSourceDisplays.Values;

    private IEnumerable<ModLoaderSelectDisplay> ModLoaders { get; } = Global.ModLoaderSelectDisplays.Values;

    private IEnumerable<string> GameVersions { get; } = [
        Utils.ResourceLoader.GetString("AllGameVersion"),
        ..Global.InstanceVersions.Where(v => v.Type == "release").Select(v => v.Id)];

    private string AllGameVersionString => GameVersions.First();

    private readonly IEnumerable<SortMethodDisplay> SortMethods = Global.ResourceSortMethods.Values;

    public static readonly DependencyProperty ResourceTypeProperty = 
        DependencyProperty.Register(
            nameof(ResourceType),
            typeof(ResourceType),
            typeof(SearchHeader),
            new PropertyMetadata(default));

    public static readonly DependencyProperty IsLoaderTypeVisibleProperty = 
        DependencyProperty.Register(
            nameof(IsLoaderTypeVisible),
            typeof(bool),
            typeof(SearchHeader),
            new PropertyMetadata(default));

    public static readonly DependencyProperty SearchArgsProperty = 
        DependencyProperty.Register(
            nameof(SearchArgs),
            typeof(ResourceSearchArgs),
            typeof(SearchHeader),
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

    public event EventHandler<ResourceSearchEventArgs>? Submit;

    public ResourceType ResourceType
    {
        get => (ResourceType)GetValue(ResourceTypeProperty);
        set
        {
            SetValue(ResourceTypeProperty, value);
            category.SelectedIndex = 0;
        }
    }

    public bool IsLoaderTypeVisible
    {
        get => (bool)GetValue(IsLoaderTypeVisibleProperty);
        set
        {
            SetValue(IsLoaderTypeVisibleProperty, value);
            Grid.SetColumnSpan(gameVersion, value ? 1 : 2);
            modLoaderType.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public ResourceSearchArgs? SearchArgs
    {
        get => (ResourceSearchArgs?)GetValue(SearchArgsProperty);
        set => SetValue(SearchArgsProperty, value);
    }

    public bool IsAnyVersionSelected
    {
        get => (string)gameVersion.SelectedItem == GameVersions.FirstOrDefault();
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

    public SearchHeader()
    {
        InitializeComponent();
    }

    private void resetConditions_Click(object sender, RoutedEventArgs e)
    {
        searchBox.Text = "";
        resourceSource.SelectedIndex = 0;
        gameVersion.SelectedIndex = 0;
        modLoaderType.SelectedIndex = 0;
        category.SelectedIndex = 0;
        sortMethod.SelectedIndex = 0;
    }

    private void searchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        var searchArgs = GetSearchArgs();
        Submit?.Invoke(this, searchArgs);

        var parameter = CommandParameter ?? searchArgs;
        if (Command?.CanExecute(parameter) == true)
            Command.Execute(parameter);
    }

    private ResourceSearchEventArgs GetSearchArgs()
    {
        return new ResourceSearchEventArgs()
        {
            SearchText = searchBox.Text,
            ResourceSource = ((WebSourceSelectDisplay)resourceSource.SelectedItem).WebSource,
            Category = (CategoryDisplay)category.SelectedItem,
            SortMethod = ((SortMethodDisplay)sortMethod.SelectedItem).SortType,
            ModLoader = (ModLoaderSelectDisplay)modLoaderType.SelectedItem,
            GameVersion = gameVersion.SelectedIndex == 0 ? null : (string)gameVersion.SelectedItem
        };
    }

    private void WebSourceChange(WebResourceSource selectedItem)
    {
        sortMethod.ItemsSource = selectedItem switch
        {
            WebResourceSource.All => SortMethods.Where(x => x.CurseForgeSortType is not null && x.ModrinthSortType is not null),
            WebResourceSource.CurseForge => SortMethods.Where(x => x.CurseForgeSortType is not null),
            WebResourceSource.Modrinth => SortMethods.Where(x => x.ModrinthSortType is not null),
            _ => throw new NotImplementedException()
        };

        category.ItemsSource = (ResourceType, selectedItem) switch
        {
            (ResourceType.Mod, WebResourceSource.All) => DownloadModModel.CategoryDisplays,
            (ResourceType.Mod, WebResourceSource.CurseForge) => DownloadModModel.CurseForgeCategoryDisplays,
            (ResourceType.Mod, WebResourceSource.Modrinth) => DownloadModModel.ModrinthCategoryDisplays,
            (ResourceType.Modpack, WebResourceSource.All) => DownloadModPackModel.CategoryDisplays,
            (ResourceType.Modpack, WebResourceSource.CurseForge) => DownloadModPackModel.CurseForgeCategoryDisplays,
            (ResourceType.Modpack, WebResourceSource.Modrinth) => DownloadModPackModel.ModrinthCategoryDisplays,
            (ResourceType.Resourcepack, WebResourceSource.All) => DownloadResourcePackModel.CategoryDisplays,
            (ResourceType.Resourcepack, WebResourceSource.CurseForge) => DownloadResourcePackModel.CurseForgeCategoryDisplays,
            (ResourceType.Resourcepack, WebResourceSource.Modrinth) => DownloadResourcePackModel.ModrinthCategoryDisplays,
            (ResourceType.Shaderpack, WebResourceSource.All) => DownloadShaderModel.CategoryDisplays,
            (ResourceType.Shaderpack, WebResourceSource.CurseForge) => DownloadShaderModel.CurseForgeCategoryDisplays,
            (ResourceType.Shaderpack, WebResourceSource.Modrinth) => DownloadShaderModel.ModrinthCategoryDisplays,
            _ => throw new NotImplementedException(),
        };
    }

    private void resourceSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = ((WebSourceSelectDisplay)e.AddedItems.FirstOrDefault()!).WebSource;

        WebSourceChange(selectedItem);
        sortMethod.SelectedIndex = 0;
        category.SelectedIndex = 0;

        SearchArgs?.ResourceSource = selectedItem;
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var webSourceItem = ResourceSources.FirstOrDefault(x => x.WebSource == SearchArgs?.ResourceSource);
        WebSourceChange(webSourceItem!.WebSource);
        resourceSource.SelectedItem = webSourceItem;
        modLoaderType.SelectedItem = ModLoaders.FirstOrDefault(x => x.LoaderType == SearchArgs?.ModLoader.LoaderType);
        sortMethod.SelectedItem = SortMethods.FirstOrDefault(x => x.SortType == SearchArgs?.SortMethod);
        category.SelectedItem = SearchArgs?.Category;
        gameVersion.SelectedItem = SearchArgs?.GameVersion is null ? GameVersions.First() : SearchArgs?.GameVersion;

        resourceSource.SelectionChanged += resourceSource_SelectionChanged;
    }
}