namespace Application.Dtos.Tag;

public class AssignTagsToVideoDto
{
    public long VideoId { get; set; }
    public List<long> TagIds { get; set; }
}