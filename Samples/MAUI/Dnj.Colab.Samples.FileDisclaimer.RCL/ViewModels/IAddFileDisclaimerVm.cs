namespace Dnj.Colab.Samples.FileDisclaimer.RCL.ViewModels;
public interface IAddFileDisclaimerVm
{
    Task SelectDestinyFolder();
    string? DestinyFolder { get; set; }
    FileExtensionComment[] AllowedFileExtensions { get; set; }
    string? DisclaimerText { get; set; }
    bool Loading { get; set; }
    string? Error { get; set; }
    int ProcessedFileNumber { get; set; }
    Task<AddFileDisclaimerResult> Generate();
}

public class AddFileDisclaimerResult
{
    public AddFileDisclaimerResultState State { get; set; }
    public string? Error { get; set; }
    public int ProcessedFileNumber { get; set; }
}

public enum AddFileDisclaimerResultState
{
    OK,
    KO
}

public class FileExtensionComment
{
    public string? Value { get; set; }
    public string? CommentOpen { get; set; }
    public string? CommentClose { get; set; }
}
