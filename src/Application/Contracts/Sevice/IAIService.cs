using Application.Dtos.AI;

namespace Application.Contracts.Sevice;

public interface IAIService
{
    Task<ChatResponseDto> GetChatResponseAsync(ChatRequestDto dto);
}
