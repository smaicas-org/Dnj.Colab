using Microsoft.AspNetCore.Components.Forms;

namespace Dnj.Colab.Samples.FileSystemAccess.Abstractions;
public interface IFileSystemAccessVm
{
    Task<string> ReadFileAsync(string path);
    Task ReadFileAsync(IBrowserFile file);
    Task ReadMultipleFilesAsync(IReadOnlyList<IBrowserFile> file);
    string SelectedFolder { get; set; }
    Task OpenFolderPicker();
    IDictionary<string, string> InMemoryFiles { get; set; }
    IDictionary<string, string> LocalFiles { get; set; }
    string ReadFromLocalCache(string fileName);
    IDictionary<string, long> MemoryUsed { get; set; }
    IDictionary<string, long> TimesLocalAccess { get; set; }
}
