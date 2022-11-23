using System.Text;
using Dnj.Colab.Samples.FileDisclaimer.Abstractions;
using Dnj.Colab.Samples.FileDisclaimer.RCL.ViewModels;

namespace Dnj.Colab.Samples.FileDisclaimer.ViewModels;
public class AddFileDisclaimerVm : IAddFileDisclaimerVm
{
    private readonly IFolderPicker _folderPicker;

    public AddFileDisclaimerVm(IFolderPicker folderPicker) => _folderPicker = folderPicker ?? throw new ArgumentNullException(nameof(folderPicker));
    public async Task SelectDestinyFolder() => DestinyFolder = await _folderPicker.PickFolder();
    public string? DestinyFolder { get; set; }
    public string? DisclaimerText { get; set; } = Messages.DisclaimerText;
    public bool Loading { get; set; }
    public string? Error { get; set; }
    public int ProcessedFileNumber { get; set; }

    public async Task<AddFileDisclaimerResult> Generate()
    {
        Loading = true;
        AddFileDisclaimerResult res = await ProcessDisclaimGeneration();
        Error = res?.Error;
        ProcessedFileNumber = res.ProcessedFileNumber;
        Loading = false;
        return res;
    }

    private async Task<AddFileDisclaimerResult> ProcessDisclaimGeneration()
    {
        if (DestinyFolder == default) return Ko(nameof(DestinyFolder));
        if (DisclaimerText == default) return Ko(nameof(DestinyFolder));
        int processedFiles = default;
        foreach (FileExtensionComment allowedFileExtension in AllowedFileExtensions)
        {
            StringBuilder sb = new(allowedFileExtension.CommentOpen);
            sb.Append(DisclaimerText);
            sb.Append(allowedFileExtension.CommentClose);
            sb.Append(Environment.NewLine);

            string[] files = Directory.GetFiles(DestinyFolder, $"*.{allowedFileExtension.Value}");
            foreach (string file in files)
            {
                string content = await File.ReadAllTextAsync(file, Encoding.UTF8);
                int n = sb.ToString().IndexOf(Environment.NewLine, StringComparison.Ordinal);
                int contentFirstLineN = content.IndexOf(Environment.NewLine, StringComparison.Ordinal);
                if (contentFirstLineN == n && sb.ToString().Substring(0, n).Equals(content.Substring(0, n))) continue;

                sb.Append(content);
                await File.WriteAllTextAsync(file, sb.ToString(), Encoding.UTF8);
                processedFiles++;
            }
        }

        return Ok(processedFiles);
    }

    private static AddFileDisclaimerResult Ok(int processedFileNumber) =>
        new()
        {
            State = AddFileDisclaimerResultState.OK,
            ProcessedFileNumber = processedFileNumber
        };

    private static AddFileDisclaimerResult Ko(string error) =>
        new()
        {
            State = AddFileDisclaimerResultState.KO,
            Error = $"Empty {error}"
        };

    public FileExtensionComment[] AllowedFileExtensions { get; set; } = new FileExtensionComment[]
    {
        new FileExtensionComment()
        {
            Value = "cs",
            CommentOpen = "/* ",
            CommentClose = " */"
        },
        new FileExtensionComment()
        {
            Value = "razor",
            CommentOpen = "@* ",
            CommentClose = " *@"
        }
    };

}
