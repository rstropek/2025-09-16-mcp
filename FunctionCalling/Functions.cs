using Logic;
using OpenAI.Responses;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

namespace FunctionCalling;

public static class PasswordGenerationFunctions
{
    public static readonly ResponseTool BuildPasswordTool = ResponseTool.CreateFunctionTool(
        functionName: nameof(BuildPasswordTool),
        functionDescription: "Generates an easy to remember password by concatenating a random word from the list of frequent words with a random number.",
        functionParameters: BinaryData.FromBytes("""
        {
            "type": "object",
            "properties": {
                "minimumPasswordLength": {
                    "type": "integer",
                    "description": "Minimum length of the password. Must be between 6 and 1000."
                },
                "replaceSpecialCharacters": {
                    "type": "boolean",
                    "description": "If true, replaces certain characters with special symbols."
                }
            },
            "required": ["minimumPasswordLength", "replaceSpecialCharacters"],
            "additionalProperties": false
        }
        """u8.ToArray()),
        strictModeEnabled: true
    );

    public static string BuildPassword(int minimumPasswordLength, bool replaceSpecialCharacters)
    {
        try
        {
            return PasswordGenerator.GeneratePassword(minimumPasswordLength, replaceSpecialCharacters);
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}