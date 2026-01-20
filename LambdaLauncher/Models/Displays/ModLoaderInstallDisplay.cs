using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media.Imaging;
using MinecraftLaunch.Base.Enums;
using System;
using System.Collections.Generic;

namespace LambdaLauncher.Models.Displays;

public partial class ModLoaderInstallDisplay(ModLoaderType type) : ObservableObject
{
    public readonly ModLoaderType Type = type;
    public readonly string TypeDisplay = Utils.ResourceLoader.GetString(type switch
    {
        ModLoaderType.OptiFine => "Loader-OptiFine",
        ModLoaderType.Forge => "Loader-Forge",
        ModLoaderType.NeoForge => "Loader-NeoForge",
        ModLoaderType.Fabric => "Loader-Fabric",
        ModLoaderType.Quilt => "Loader-Quilt",
        _ => throw new NotImplementedException()
    });
    public readonly string Description = Utils.ResourceLoader.GetString(type switch
    {
        ModLoaderType.OptiFine => "LoaderDescription-OptiFine",
        ModLoaderType.Forge => "LoaderDescription-Forge",
        ModLoaderType.NeoForge => "LoaderDescription-NeoForge",
        ModLoaderType.Fabric => "LoaderDescription-Fabric",
        ModLoaderType.Quilt => "LoaderDescription-Quilt",
        _ => throw new NotImplementedException()
    });
    public readonly BitmapImage Icon = Utils.GetIconFromResources(type switch
    {
        ModLoaderType.OptiFine => "OptiFineIcon",
        ModLoaderType.Forge => "ForgeIcon",
        ModLoaderType.NeoForge => "NeoForgeIcon",
        ModLoaderType.Fabric => "FabricIcon",
        ModLoaderType.Quilt => "QuiltIcon",
        _ => throw new NotImplementedException()
    })!;

    [ObservableProperty]
    public partial IEnumerable<object> Versions { get; set; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StateHintText))]
    public partial ModLoaderState State { get; set; } = ModLoaderState.Loading;

    public string StateHintText => State == ModLoaderState.Available ? "" : Utils.ResourceLoader.GetString(State switch
    {
        ModLoaderState.NotAvailable => "LoaderDescription-NotAvailable",
        ModLoaderState.Loading => "LoaderDescription-Loading",
        ModLoaderState.Conflict => "LoaderDescription-Conflict",
        ModLoaderState.Error => "LoaderDescription-Error",
        _ => throw new NotImplementedException()
    });

    [ObservableProperty]
    public partial bool IsSelected { get; set; } = false;

    public void ResetBindProperty()
    {
        Versions = [];
        State = ModLoaderState.Loading;
        IsSelected = false;
    }
}

public enum ModLoaderState
{
    Available,
    NotAvailable,
    Loading,
    Conflict,
    Error
}