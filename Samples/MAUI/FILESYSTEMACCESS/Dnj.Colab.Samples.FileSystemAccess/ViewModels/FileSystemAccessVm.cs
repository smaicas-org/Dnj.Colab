using System.Security;
using Dnj.Colab.Samples.FileDisclaimer.Abstractions;
using Dnj.Colab.Samples.FileSystemAccess.Abstractions;
using Dnj.Colab.Samples.FileSystemAccess.Extensions;
using Microsoft.AspNetCore.Components.Forms;

namespace Dnj.Colab.Samples.FileSystemAccess.Services;
public class FileSystemAccessVm : IFileSystemAccessVm
{

    private readonly IFolderPicker _folderPicker;

    public FileSystemAccessVm(IFolderPicker folderPicker) => _folderPicker = folderPicker ?? throw new ArgumentNullException(nameof(folderPicker));

    /// <exception cref="ArgumentNullException">Thrown when the arguments are <see langword="null"/></exception>
    /// <exception cref="FileNotFoundException">File not found in path parameter.</exception>
    public Task<string> ReadFileAsync(string path)
    {
        if (path == null) throw new ArgumentNullException();
        if (!File.Exists(path)) throw new FileNotFoundException();
        return File.ReadAllTextAsync(path);
    }

    /// <exception cref="IOException">Thrown if the file's length exceeds the <paramref name="maxAllowedSize" /> value.</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="path" /> specifies a file that is read-only.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The number of characters is larger than <see cref="System.Int32.MaxValue">Int32.MaxValue</see>.</exception>
    /// <exception cref="ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    private const long maxFileSize = 1024 * 3000;

    public async Task ReadFileAsync(IBrowserFile file)
    {
        string fileName = string.Empty.RandomString(12);

        await using FileStream fs = new($"{Microsoft.Maui.Storage.FileSystem.CacheDirectory}/{fileName}", FileMode.Create);

        await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
        fs.Seek(0, SeekOrigin.Begin);

        StreamReader reader = new(fs);
        string content = await reader.ReadToEndAsync();
        InMemoryFiles.Add(file.Name, content);
        LocalFiles.Add(file.Name, fileName);
    }
    public IDictionary<string, long> MemoryUsed { get; set; } = new Dictionary<string, long>();
    public IDictionary<string, long> TimesLocalAccess { get; set; } = new Dictionary<string, long>();

    /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="path" /> specifies a file that is read-only.</exception>
    /// <exception cref="IOException">An I/O error, such as specifying <see langword="FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.  
    ///  -or-  
    ///  The stream has been closed.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string.</exception>
    public string ReadFromLocalCache(string fileName)
    {
        using FileStream fs = new($"{Microsoft.Maui.Storage.FileSystem.CacheDirectory}/{fileName}", FileMode.Open);
        StreamReader reader = new(fs);
        return reader.ReadToEnd();
    }
    /// <exception cref="IOException">Thrown if the file's length exceeds the <paramref name="maxAllowedSize" /> value.</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="path" /> specifies a file that is read-only.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The number of characters is larger than <see cref="System.Int32.MaxValue">Int32.MaxValue</see>.</exception>
    /// <exception cref="ObjectDisposedException">The stream has been disposed.</exception>
    /// <exception cref="InvalidOperationException">The reader is currently in use by a previous read operation.</exception>
    public async Task ReadMultipleFilesAsync(IReadOnlyList<IBrowserFile> files)
    {
        foreach (IBrowserFile f in files)
        {
            long memoryNow = GC.GetTotalMemory(true);
            await ReadFileAsync(f);
            long memory2 = GC.GetTotalMemory(true);
            MemoryUsed.Add(f.Name, memory2 - memoryNow);
        }
    }

    public IDictionary<string, string> InMemoryFiles { get; set; } = new Dictionary<string, string>();
    public IDictionary<string, string> LocalFiles { get; set; } = new Dictionary<string, string>();

    public string SelectedFolder { get; set; }

    /// <exception cref="DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
    /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="FileNotFoundException">The file cannot be found, such as when <paramref name="mode" /> is <see langword="FileMode.Truncate" /> or <see langword="FileMode.Open" />, and the file specified by <paramref name="path" /> does not exist. The file must already exist in these modes.</exception>
    /// <exception cref="UnauthorizedAccessException"><paramref name="path" /> specifies a file that is read-only.</exception>
    /// <exception cref="IOException">An I/O error, such as specifying <see langword="FileMode.CreateNew" /> when the file specified by <paramref name="path" /> already exists, occurred.  
    ///  -or-  
    ///  The stream has been closed.</exception>
    public async Task OpenFolderPicker()
    {
        SelectedFolder = await _folderPicker.PickFolder();
        if (Directory.Exists(SelectedFolder))
        {
            List<FileInfo> files = new DirectoryInfo(SelectedFolder).GetFiles("*.*").ToList();
            foreach (FileInfo f in files)
            {
                await using FileStream fs = new(f.FullName, FileMode.Open);
                StreamReader reader = new(fs);
                InMemoryFiles.Add(f.Name, await reader.ReadToEndAsync()); ;
            }

        }
    }
}