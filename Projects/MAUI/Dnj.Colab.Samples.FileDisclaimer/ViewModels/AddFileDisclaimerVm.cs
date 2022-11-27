/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using System.Text;
using Dnj.Colab.Samples.FileDisclaimer.Abstractions;
using Dnj.Colab.Samples.FileDisclaimer.RCL.ViewModels;

namespace Dnj.Colab.Samples.FileDisclaimer.ViewModels;

public class AddFileDisclaimerVm : IAddFileDisclaimerVm
{
    private readonly IFolderPicker _folderPicker;

    public AddFileDisclaimerVm(IFolderPicker folderPicker)
    {
        _folderPicker = folderPicker ?? throw new ArgumentNullException(nameof(folderPicker));
    }

    public async Task SelectDestinyFolder()
    {
        DestinyFolder = await _folderPicker.PickFolder();
    }

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

    public FileExtensionComment[] AllowedFileExtensions { get; set; } =
    {
        new()
        {
            Value = "cs",
            CommentOpen = "/* ",
            CommentClose = " */"
        },
        new()
        {
            Value = "razor",
            CommentOpen = "@* ",
            CommentClose = " *@"
        }
    };

    private async Task<AddFileDisclaimerResult> ProcessDisclaimGeneration()
    {
        if (DestinyFolder == default) return Ko(nameof(DestinyFolder));
        if (DisclaimerText == default) return Ko(nameof(DestinyFolder));
        int processedFiles = default;
        foreach (FileExtensionComment allowedFileExtension in AllowedFileExtensions)
        {
            string[] files = Directory.GetFiles(DestinyFolder, $"*.{allowedFileExtension.Value}",
                SearchOption.AllDirectories);
            foreach (string file in files)
            {
                StringBuilder sb = new(allowedFileExtension.CommentOpen);
                sb.Append(DisclaimerText);
                sb.Append(allowedFileExtension.CommentClose);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

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

    private static AddFileDisclaimerResult Ok(int processedFileNumber)
    {
        return new AddFileDisclaimerResult
        {
            State = AddFileDisclaimerResultState.OK,
            ProcessedFileNumber = processedFileNumber
        };
    }

    private static AddFileDisclaimerResult Ko(string error)
    {
        return new AddFileDisclaimerResult
        {
            State = AddFileDisclaimerResultState.KO,
            Error = $"Empty {error}"
        };
    }
}