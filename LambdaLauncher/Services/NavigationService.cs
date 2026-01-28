using LambdaLauncher.Extensions;
using LambdaLauncher.Models.Record;
using LambdaLauncher.UserControls;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace LambdaLauncher.Services;

public class NavigationService
{
    private BreadcrumbsHeader? BreadcrumbHeader { get; set; }
    private Frame? Frame { get; set; }

    public void Initialize(BreadcrumbsHeader header, Frame frame)
    {
        BreadcrumbHeader = header;
        BreadcrumbHeader.ItemClicked += BreadcrumbHeaderItemClicked;
        Frame = frame;
    }

    public void SetHeader(params PageHeader[] headers)
    {
        if (BreadcrumbHeader is null)
            throw new InvalidOperationException("NavigationService is not initialized.");

        BreadcrumbHeader.HeaderItems.Clear();
        BreadcrumbHeader.HeaderItems.AddRange(headers);
    }

    public void AddHeader(PageHeader header)
    {
        if (BreadcrumbHeader is null)
            throw new InvalidOperationException("NavigationService is not initialized.");

        BreadcrumbHeader.HeaderItems.Add(header);
    }

    public void ClearHeader()
    {
        if (BreadcrumbHeader is null)
            throw new InvalidOperationException("NavigationService is not initialized.");

        BreadcrumbHeader.HeaderItems.Clear();
    }

    public void Navigate(Type pageType, object? parameter = null, NavigationTransitionInfo? infoOverride = null)
    {
        if (Frame is null || BreadcrumbHeader is null)
            throw new InvalidOperationException("NavigationService is not initialized.");

        Frame.Navigate(pageType, parameter, infoOverride);
        Frame.BackStack.Clear();
    }

    private void BreadcrumbHeaderItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
    {
        var item = (PageHeader)args.Item;
        Navigate(item.Page!, item.Parameter, item.InfoOverride);
    }
}
