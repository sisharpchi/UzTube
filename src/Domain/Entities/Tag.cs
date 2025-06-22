namespace Domain.Entities;

public class Tag
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<VideoTag> VideoTags { get; set; } 
}
