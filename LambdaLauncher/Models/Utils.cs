using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using MinecraftLaunch.Components.Authenticator;
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

    public static readonly MicrosoftAuthenticator MicrosoftAuthenticator = new("0f801576-94de-4c1a-8a0e-6f0434331984");
    public static readonly OfflineAuthenticator OfflineAuthenticator = new();
    // public static readonly YggdrasilAuthenticator YggdrasilAuthenticator = new();

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

    public static async Task<StorageFile?> PickFileAsync(Window window, params string[] filters)
    {
        var filePicker = new FileOpenPicker();

        if (filters.Length is 0)
            filters = ["*"];

        foreach (var filter in filters)
        {
            filePicker.FileTypeFilter.Add(filter);
        }

        var hwnd = WindowNative.GetWindowHandle(window);
        InitializeWithWindow.Initialize(filePicker, hwnd);

        var file = await filePicker.PickSingleFileAsync();
        return file;
    }

    public static Task WaitForNextFrameAsync()
    {
        var tcs = new TaskCompletionSource<object>();

        void handler(object? sender, object e)
        {
            CompositionTarget.Rendering -= handler;
            tcs.SetResult(null!);
        }

        CompositionTarget.Rendering += handler;
        return tcs.Task;
    }


}