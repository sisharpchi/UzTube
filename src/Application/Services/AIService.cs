using Application.Contracts.Sevice;
using Application.Dtos.AI;
using Microsoft.Extensions.Configuration;
using Mistral.SDK;
using Mistral.SDK.DTOs;

namespace Application.Services;

public class AIService : IAIService
{
    private readonly MistralClient _client;

    public AIService(IConfiguration config)
    {
        var apiKey = config["Mistral:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentNullException("Mistral:ApiKey");

        _client = new MistralClient(apiKey);
    }

    public async Task<ChatResponseDto> GetChatResponseAsync(ChatRequestDto dto)
    {
        var request = new ChatCompletionRequest(
            ModelDefinitions.MistralMedium, // yoki kerakli model nomi
            new List<ChatMessage>
            {
                new ChatMessage(ChatMessage.RoleEnum.User, dto.Message)
            },
            safePrompt: true,
            temperature: (decimal?)Convert.ToDecimal(0.7f),
            maxTokens: 500,
            topP: 1
        );

        var response = await _client.Completions.GetCompletionAsync(request);
        var text = response.Choices.First().Message.Content;

        return new ChatResponseDto(text);
    }
}