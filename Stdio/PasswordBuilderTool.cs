using System.ComponentModel;
using ModelContextProtocol.Server;

namespace Stdio;

[McpServerToolType]
public static class PasswordBuilderTool
{
    [McpServerTool, Description("Generates an easy to remember password by concatenating a random word from the list of frequent words with a random number.")]
    public static string BuildPassword(
        [Description("Minimum length of the password. Must be between 6 and 1000.")]
        int minLength,
        [Description("If true, replaces certain characters with special symbols.")]
        bool replaceSpecialCharacters)
    {
        try
        {
            return Logic.PasswordGenerator.GeneratePassword(minLength, replaceSpecialCharacters);
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [McpServerTool, Description("Generates multiple easy to remember passwords by concatenating random words from the list of frequent words.")]
    public static string BuildMultiplePasswords(
        [Description("Number of passwords to generate. Must be between 1 and 100.")]
        int count,
        [Description("Minimum length of each password. Must be between 6 and 1000.")]
        int minLength,
        [Description("If true, replaces certain characters with special symbols.")]
        bool replaceSpecialCharacters)
    {
        try
        {
            var passwords = Logic.PasswordGenerator.GeneratePasswords(count, minLength, replaceSpecialCharacters);
            return string.Join(Environment.NewLine, passwords);
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}