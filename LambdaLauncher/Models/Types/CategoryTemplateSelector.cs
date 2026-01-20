using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using LambdaLauncher.Models.Displays;

namespace LambdaLauncher.Models.Types;

public partial class CategoryTemplateSelector : DataTemplateSelector
{
    public DataTemplate? AllCategoryTemplate { get; set; }
    public DataTemplate? CurseForgeCategoryTemplate { get; set; }
    public DataTemplate? ModrinthCategoryTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        return item switch
        {
            AllCategoryDisplay => AllCategoryTemplate,
            CurseForgeCategoryDisplay => CurseForgeCategoryTemplate,
            ModrinthCategoryDisplay => ModrinthCategoryTemplate,
            _ => null
        } ?? base.SelectTemplateCore(item, container);
    }
}