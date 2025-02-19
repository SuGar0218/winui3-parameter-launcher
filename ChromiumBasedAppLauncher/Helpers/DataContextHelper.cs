﻿using Microsoft.UI.Xaml;

namespace ChromiumBasedAppLauncherGUI.Helpers;

public class DataContextHelper
{
    public static T GetDataContext<T>(object frameworkElement) => (T) ((FrameworkElement) frameworkElement).DataContext;
}
