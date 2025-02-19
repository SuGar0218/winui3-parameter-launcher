﻿using Microsoft.UI.Xaml;

using Windows.Storage.Pickers;

namespace ChromiumBasedAppLauncherGUI.Helpers.ForFilePicker;

public abstract class DirectoryOpenPickerHelper : DirectoryPickerHelper
{
    protected DirectoryOpenPickerHelper(Window window) : base(window) { }

    public abstract DirectoryOpenPickerHelper SetViewMode(PickerViewMode viewMode);
    public abstract DirectoryOpenPickerHelper AddFileTypeFilter(string fileExtension);
}
