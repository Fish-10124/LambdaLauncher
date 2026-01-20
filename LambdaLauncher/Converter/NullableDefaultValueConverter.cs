using Microsoft.UI.Xaml.Data;
using System;

namespace LambdaLauncher.Converter;

public partial class NullableDefaultValueConverter : IValueConverter
{
    public object DefaultValue { get; set; } = null!;

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value ?? DefaultValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
