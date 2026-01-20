using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using LambdaLauncher.Models.Displays;
using System;

namespace LambdaLauncher.Converter;

public partial class ModLoaderStateConverter : IValueConverter
{
    public bool ConvertToVisibility { get; set; } = false;
    public bool Invert { get; set; } = false;
    public ModLoaderState CompareMode { get; set; } = ModLoaderState.Available;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not ModLoaderState v)
        {
            throw new ArgumentException($"value must be {typeof(ModLoaderState)}");
        }

        bool condition = v == CompareMode;
        if (Invert)
        {
            condition = !condition;
        }
        if (ConvertToVisibility)
        {
            return condition ? Visibility.Collapsed : Visibility.Visible;
        }
        return condition;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}