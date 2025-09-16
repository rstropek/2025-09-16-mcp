using System.ComponentModel;
using ModelContextProtocol.Server;

namespace Streamable;

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
}