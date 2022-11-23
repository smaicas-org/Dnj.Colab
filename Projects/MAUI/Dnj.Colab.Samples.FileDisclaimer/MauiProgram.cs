using Dnj.Colab.Samples.FileDisclaimer.Abstractions;
using Dnj.Colab.Samples.FileDisclaimer.RCL.ViewModels;
using Dnj.Colab.Samples.FileDisclaimer.ViewModels;
using Microsoft.Extensions.Logging;

namespace Dnj.Colab.Samples.FileDisclaimer;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddTransient<IAddFileDisclaimerVm, AddFileDisclaimerVm>();

#if WINDOWS
        builder.Services.AddTransient<IFolderPicker, Platforms.Windows.DnjFolderPicker>();
#endif

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}