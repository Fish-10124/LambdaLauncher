using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using MinecraftLaunch.Components.Parser;
using MinecraftLaunch.Components.Provider;
using System;
using System.IO;
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

    public static async Task<string> SaveSkinToLocalAsync(Stream skinStream, string fileName)
    {
        var folder = ApplicationData.Current.LocalFolder;
        var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

        using var output = await file.OpenStreamForWriteAsync();
        skinStream.Position = 0;
        await skinStream.CopyToAsync(output);

        return file.Path; // 返回保存路径
    }

    /// <summary>
    /// 从本地获取皮肤图片
    /// </summary>
    /// <param name="fileName">要获取的皮肤文件名</param>
    /// <returns>获取皮肤, 如果获取不到, 将返回史蒂夫</returns>
    public static async Task<BitmapImage> LoadSkinFromLocalAsync(string? fileName = null)
    {
        var folder = ApplicationData.Current.LocalFolder;
        var file = (StorageFile)await folder.TryGetItemAsync(fileName) ?? await folder.GetFileAsync("steve.png");

        using var stream = await file.OpenReadAsync();
        var bitmap = new BitmapImage();
        await bitmap.SetSourceAsync(stream);
        return bitmap;
    }
}