using MinecraftLaunch.Base.Interfaces;
using MinecraftLaunch.Base.Models.Network;
using LambdaLauncher.Models.Interface;
using LambdaLauncher.ViewModels.DownloadResourceModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaLauncher.ViewModels.ResourceModel;

public partial class ResourcePackDetailsResourceModel : ResourceDetailsModel
{
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
                CurseforgeResource => DownloadResourcePackModel.CurseForgeCategoryDisplays.FirstOrDefault(c => c.IdText == category),
                ModrinthResource => DownloadResourcePackModel.ModrinthCategoryDisplays.FirstOrDefault(c => c.Id == category),
                _ => throw new NotImplementedException()
            };

            if (display != null)
            {
                result.Add(display.DisplayText);
            }
        }

        return result;
    }
}