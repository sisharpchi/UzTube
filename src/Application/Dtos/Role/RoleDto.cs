namespace Application.Dtos.Role;

public class RoleDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    public int UserCount { get; set; }
}