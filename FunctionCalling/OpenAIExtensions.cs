using System.Text.Json;
using Logic;
using OpenAI.Responses;

namespace FunctionCalling;

#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

public static class OpenAIResponseClientExtensions
{
    public static async Task<string> CreateAssistantResponseAsync(
        this OpenAIResponseClient client,
        string userMessage,
        string systemPrompt,
        string? previousResponseId)
    {
        List<ResponseItem> input = [
          ResponseItem.CreateUserMessageItem(userMessage),
        ];

        while (true)
        {
            var response = await client.CreateResponseAsync(input, new()
            {
                Instructions = systemPrompt,
                ToolChoice = ResponseToolChoice.CreateAutoChoice(),
                Tools = { PasswordGenerationFunctions.BuildPasswordTool, },
                StoredOutputEnabled = true,
                PreviousResponseId = previousResponseId,
                ReasoningOptions = new() { ReasoningEffortLevel = ResponseReasoningEffortLevel.Low },
            });

            input.Clear();

            foreach (var item in response.Value.OutputItems)
            {
                if (item is FunctionCallResponseItem functionCallItem)
                {
                    var result = "";
                    string? error = null;

                    Console.WriteLine($"\t{functionCallItem.CallId}: Calling {functionCallItem.FunctionName} with {functionCallItem.FunctionArguments}");
                    switch (functionCallItem.FunctionName)
                    {
                        case nameof(PasswordGenerationFunctions.BuildPasswordTool):
                            var parameters = functionCallItem.FunctionArguments.ToObjectFromJson<BuildPasswordParameters>(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                            if (parameters is null)
                            {
                                error = "Missing or invalid parameters";
                                break;
                            }

                            result = PasswordGenerationFunctions.BuildPassword(parameters.MinimumPasswordLength, parameters.ReplaceSpecialCharacters);
                            input.Add(ResponseItem.CreateFunctionCallOutputItem(functionCallItem.CallId, result));
                            break;
                        default:
                            error = $"Unknown function: {functionCallItem.FunctionName}";
                            break;
                    }

                    input.Add(ResponseItem.CreateFunctionCallOutputItem(functionCallItem.CallId, error ?? result));
                    previousResponseId = response.Value.Id;
                }
                else if (item is MessageResponseItem messageItem)
                {
                    Console.WriteLine(messageItem.Content[0].Text);
                    return response.Value.Id;
                }
            }
        }
    }
}

record BuildPasswordParameters(
    int MinimumPasswordLength,
    bool ReplaceSpecialCharacters
);