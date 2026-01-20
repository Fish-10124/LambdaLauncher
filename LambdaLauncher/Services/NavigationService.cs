using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaLauncher.Services;

public class NavigationService
{
    public NavigationView? NavigationView { get; private set; }
    public Frame? Frame { get; private set; }

    public void Initialize(NavigationView navView, Frame frame)
    {
        NavigationView = navView;
        Frame = frame;
    }

    public void Navigate(Type pageType, object? parameter = null, NavigationTransitionInfo? infoOverride = null)
    {
        if (Frame is null)
        {
            throw new InvalidOperationException("NavigationService is not initialized.");
        }
        Frame.Navigate(pageType, parameter, infoOverride);
    }
}
