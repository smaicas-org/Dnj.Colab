using Dnj.Colab.Samples.FileSystemAccess.Abstractions;
using Dnj.Colab.Samples.FileSystemAccess.Services;
using Microsoft.Extensions.Logging;

namespace Dnj.Colab.Samples.FileSystemAccess;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"));

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IFileSystemAccessService, FileSystemAccessService>();

        return builder.Build();
    }
}
