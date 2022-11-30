using Microsoft.AspNetCore.Components.Forms;

namespace Dnj.Colab.Samples.FileSystemAccess.Abstractions;
public interface IFileSystemAccessService
{
    Task<string> ReadFileAsync(string path);
    Task<string> ReadFileAsync(IBrowserFile file);
}
