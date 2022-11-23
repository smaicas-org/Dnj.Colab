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
