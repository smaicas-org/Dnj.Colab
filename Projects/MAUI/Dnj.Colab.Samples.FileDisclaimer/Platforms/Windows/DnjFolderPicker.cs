/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using Dnj.Colab.Samples.FileDisclaimer.Abstractions;

namespace Dnj.Colab.Samples.FileDisclaimer.Platforms.Windows;

public class DnjFolderPicker : IFolderPicker
{
    public async Task<string> PickFolder()
    {
        global::Windows.Storage.Pickers.FolderPicker folderPicker = new();

        // Might be needed to make it work on Windows 10
        folderPicker.FileTypeFilter.Add("*");

        // Get the current window's HWND by passing in the Window object
        IntPtr hwnd = ((MauiWinUIWindow)App.Current.Windows[0].Handler.PlatformView).WindowHandle;

        // Associate the HWND with the file picker
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

        global::Windows.Storage.StorageFolder result = await folderPicker.PickSingleFolderAsync();

        return result?.Path;
    }
}