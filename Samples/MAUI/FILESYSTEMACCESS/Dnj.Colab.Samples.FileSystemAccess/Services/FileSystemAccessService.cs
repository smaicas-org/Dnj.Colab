using System.Security;
using Dnj.Colab.Samples.FileSystemAccess.Abstractions;
using Microsoft.AspNetCore.Components.Forms;

namespace Dnj.Colab.Samples.FileSystemAccess.Services;
public class FileSystemAccessService : IFileSystemAccessService
{
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
    public async Task<string> ReadFileAsync(IBrowserFile file)
    {
        await using FileStream fs = new($"{Microsoft.Maui.Storage.FileSystem.CacheDirectory}/temp", FileMode.Create);
        await file.OpenReadStream().CopyToAsync(fs);
        fs.Seek(0, SeekOrigin.Begin);
        StreamReader reader = new(fs);
        return await reader.ReadToEndAsync();
    }
}