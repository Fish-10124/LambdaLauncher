using Microsoft.UI.Xaml.Data;
using LambdaLauncher.Models;
using LambdaLauncher.Models.Displays;
using System;

namespace LambdaLauncher.Converter;

public partial class ModLoaderHintTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not ModLoaderState v)
        {
            throw new ArgumentException($"value must be {typeof(ModLoaderState)}");
        }

        if (v == ModLoaderState.Available)
        {
            return "";
        }

        return Utils.ResourceLoader.GetString(v switch
        {
            ModLoaderState.NotAvailable => "LoaderDescription-NotAvailable",
            ModLoaderState.Loading => "LoaderDescription-Loading",
            ModLoaderState.Conflict => "LoaderDescription-Conflict",
            ModLoaderState.Error => "LoaderDescription-Error",
            _ => throw new NotImplementedException()
        });
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}