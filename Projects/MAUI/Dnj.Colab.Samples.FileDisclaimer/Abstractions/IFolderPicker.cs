namespace Dnj.Colab.Samples.FileDisclaimer.Abstractions;
public interface IFolderPicker
{
    Task<string> PickFolder();
}
