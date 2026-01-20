using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using MinecraftLaunch.Components.Parser;
using MinecraftLaunch.Components.Provider;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace LambdaLauncher.Models;

public class Utils
{
    public static readonly ResourceLoader ResourceLoader = ResourceLoader.GetForViewIndependentUse();

    public static readonly CurseforgeProvider CFProvider = new();
    public static readonly ModrinthProvider MRProvider = new();
    public static readonly MinecraftParser GameParser = null!;

    public static BitmapImage? GetIconFromResources(string iconName)
    {
        return App.Current.Resources[iconName] as BitmapImage;
    }

    public static void CopyToClipboard(string text)
    {
        var dataPackage = new DataPackage();
        dataPackage.SetText(text);
        Clipboard.SetContent(dataPackage);
        Clipboard.Flush();
    }

    public static async Task<T?> TryExecuteAsync<T>(Func<Task<T>> func, T? defaultValue = default)
    {
        try
        {
            return await func();
        }
        catch
        {
            return defaultValue;
        }
    }

    public static async Task<StorageFolder?> PickFolderAsync(Window window)
    {
        var folderPicker = new FolderPicker();
        folderPicker.FileTypeFilter.Add("*");

        var hwnd = WindowNative.GetWindowHandle(window);
        InitializeWithWindow.Initialize(folderPicker, hwnd);

        var folder = await folderPicker.PickSingleFolderAsync();
        return folder;
    }
}