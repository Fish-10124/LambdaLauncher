using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace LambdaLauncher.Converter;

public partial class BoolToVisibilityConverter : IValueConverter
{
    public bool Invert { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not bool v)
        {
            throw new ArgumentException($"value must be {typeof(bool)}");
        }

        var condition = Invert ? !v : v;
        return condition ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is not Visibility v)
        {
            throw new ArgumentException($"value must be {typeof(Visibility)}");
        }

        return v == Visibility.Visible ? Invert : !Invert;
    }
}