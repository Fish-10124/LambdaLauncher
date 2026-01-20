using CommunityToolkit.Mvvm.ComponentModel;
using Flurl.Http;
using Microsoft.UI.Dispatching;
using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Components.Installer;
using LambdaLauncher.Models.Displays;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LambdaLauncher.ViewModels;

public partial class DownloadInstanceDetailsModel : ObservableObject
{
    private readonly DispatcherQueue Dispatcher = DispatcherQueue.GetForCurrentThread();

    [ObservableProperty]
    public partial Dictionary<ModLoaderType, ModLoaderInstallDisplay> ModLoaders { get; private set; } = new()
    {
        { ModLoaderType.OptiFine, new(ModLoaderType.OptiFine) },
        { ModLoaderType.Forge, new(ModLoaderType.Forge) },
        { ModLoaderType.NeoForge, new(ModLoaderType.NeoForge) },
        { ModLoaderType.Fabric, new(ModLoaderType.Fabric) },
        { ModLoaderType.Quilt, new(ModLoaderType.Quilt) }
    };

    [ObservableProperty]
    public partial SettingsCardDisplay InstanceDisplay { get; private set; }

    [ObservableProperty]
    public partial bool IsInitializing { get; private set; } = false;

    public void ItemsSelect(IEnumerable<object> selectedItems)
    {
        var selectedLoaders = selectedItems.Select(i => (ModLoaderInstallDisplay)i);

        foreach (var item in ModLoaders.Values)
        {
            if (item.State == ModLoaderState.Loading)
                continue;

            if (!selectedLoaders.Contains(item))
            {
                item.IsSelected = false;
                if (item.State == ModLoaderState.Available && selectedLoaders.Any())
                {
                    item.State = ModLoaderState.Conflict;
                }
                else if (item.State == ModLoaderState.Conflict && !selectedLoaders.Any())
                {
                    item.State = ModLoaderState.Available;
                }
            }
            else
            {
                item.IsSelected = true;
            }
        }

        if (ModLoaders[ModLoaderType.Forge].IsSelected && ModLoaders[ModLoaderType.OptiFine].Versions.Any())
            ModLoaders[ModLoaderType.OptiFine].State = ModLoaderState.Available;
        if (ModLoaders[ModLoaderType.OptiFine].IsSelected && ModLoaders[ModLoaderType.Forge].Versions.Any())
            ModLoaders[ModLoaderType.Forge].State = ModLoaderState.Available;
    }

    /// <summary>
    /// 多线程同时获取模组加载器
    /// </summary>
    /// <returns></returns>
    public async Task InitAsync(SettingsCardDisplay InstanceDisplay)
    {
        IsInitializing = false;

        this.InstanceDisplay = InstanceDisplay;

        var forgeTask = ForgeInstaller.EnumerableForgeAsync(InstanceDisplay.Header, false).ContinueWith(t => ProcessLoaderResult(t, ModLoaderType.Forge));
        var neoForgeTask = ForgeInstaller.EnumerableForgeAsync(InstanceDisplay.Header, true).ContinueWith(t => ProcessLoaderResult(t, ModLoaderType.NeoForge));
        var fabricTask = FabricInstaller.EnumerableFabricAsync(InstanceDisplay.Header).ContinueWith(t => ProcessLoaderResult(t, ModLoaderType.Fabric));
        var optiFineTask = OptifineInstaller.EnumerableOptifineAsync(InstanceDisplay.Header).ContinueWith(t => ProcessLoaderResult(t, ModLoaderType.OptiFine));
        var quiltTask = QuiltInstaller.EnumerableQuiltAsync(InstanceDisplay.Header).ContinueWith(t => ProcessLoaderResult(t, ModLoaderType.Quilt));

        try
        {
            await Task.WhenAll(forgeTask, neoForgeTask, fabricTask, optiFineTask, quiltTask);
        }
        catch (FlurlHttpException e)
        {
            // 忽略异常，之后处理
            Debug.WriteLine(e.Message);
        }
        finally
        {
            IsInitializing = true;
        }
    }

    private void ProcessLoaderResult<T>(Task<IEnumerable<T>> task, ModLoaderType type)
    {
        if (!task.IsCompletedSuccessfully)
        {
            Dispatcher.TryEnqueue(() =>
            {
                ModLoaders[type].Versions = [];
                ModLoaders[type].State = ModLoaderState.Error;
            });
            return;
        }
        var loader = task.Result.Cast<object>();

        Dispatcher.TryEnqueue(() =>
        {
            ModLoaders[type].Versions = loader;
            ModLoaders[type].State = loader.Any() ? ModLoaderState.Available : ModLoaderState.NotAvailable;
        });
    }
}