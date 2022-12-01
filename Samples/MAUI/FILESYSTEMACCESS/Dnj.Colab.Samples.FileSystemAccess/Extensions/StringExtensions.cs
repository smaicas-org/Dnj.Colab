namespace Dnj.Colab.Samples.FileSystemAccess.Extensions;

public static class StringExtensions
{
    public static string RandomString(this string availableCharacters, int length = 24)
    {
        Random random = new();

        return new string(Enumerable
            .Repeat(
                string.IsNullOrEmpty(availableCharacters)
                    ? "abcdefghijklmnopqrstuvwxyz0123456789"
                    : availableCharacters, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
