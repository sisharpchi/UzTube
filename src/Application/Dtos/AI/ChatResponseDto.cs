namespace Application.Dtos.AI;

public class ChatResponseDto
{
    public string Answer { get; set; }

    public ChatResponseDto(string answer)
    {
        Answer = answer;
    }
}