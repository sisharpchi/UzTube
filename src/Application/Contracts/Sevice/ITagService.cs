using Application.Dtos.Tag;

namespace Application.Contracts.Sevice;

public interface ITagService
{
    Task<TagDto> CreateAsync(TagCreateDto dto);
    Task<TagDto> UpdateAsync(long tagId, TagUpdateDto dto);
    Task<bool> DeleteAsync(long tagId);

    Task<bool> AssignToVideoAsync(AssignTagsToVideoDto dto);
    Task<List<TagDto>> GetTagsByVideoAsync(long videoId);
    Task<List<TagDto>> GetAllAsync();
}