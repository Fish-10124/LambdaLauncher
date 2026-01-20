using MinecraftLaunch.Base.Enums;
using MinecraftLaunch.Base.Interfaces;
using MinecraftLaunch.Base.Models.Network;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Interface;
using LambdaLauncher.ViewModels.DownloadResourceModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaLauncher.ViewModels.ResourceModel;

public partial class ShaderDetailsResourceModel : ResourceDetailsModel
{
    protected override string GetResourceInfo(IResource resource)
    {
        return MergeSupportLoaders(resource.Loaders);
    }

    protected override IEnumerable<string> GetCategories(IResource resource)
    {
        return GetCategoryDisplay(resource);
    }

    public static IEnumerable<string> GetCategoryDisplay(IResource resource)
    {
        List<string> result = [];
        foreach (var category in resource.Categories)
        {
            IDataDisplay? display = resource switch
            {
                CurseforgeResource => DownloadShaderModel.CurseForgeCategoryDisplays.FirstOrDefault(c => c.IdText == category),
                ModrinthResource => DownloadShaderModel.ModrinthCategoryDisplays.FirstOrDefault(c => c.Id == category),
                _ => throw new NotImplementedException()
            };

            if (display != null)
            {
                result.Add(display.DisplayText);
            }
        }

        return result;
    }

    public static string MergeSupportLoaders(IEnumerable<ModLoaderType> loaders)
    {
        IEnumerable<string> loaderStrings = [];
        if (loaders.Contains(ModLoaderType.Any))
        {
            return $"{Utils.ResourceLoader.GetString("AnyModLoader")}";
        }

        loaderStrings = loaders.SelectMany(l =>
        {
            string? key = l switch
            {
                ModLoaderType.Canvas => "Canvas",
                ModLoaderType.Iris => "Iris",
                ModLoaderType.OptiFine => "OptiFine",
                ModLoaderType.Vanilla => "Vanilla",
                _ => null
            };

            return key == null ? Enumerable.Empty<string>() : [Utils.ResourceLoader.GetString($"Loader-{key}")];
        });

        if (loaderStrings.Any())
        {
            return $"{string.Join(", ", loaderStrings)}";
        }

        return "";
    }
}