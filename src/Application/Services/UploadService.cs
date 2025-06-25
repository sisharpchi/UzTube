using Application.Contracts.Repository;
using Application.Contracts.Sevice;
using Application.Dtos.Upload;
using CG.Web.MegaApiClient;

namespace Application.Services;

public class UploadService : IUploadService
{
    private readonly MegaApiClient megaApiClient;
    private readonly IVideoRepository videoRepository;

    public UploadService(MegaApiClient megaApiClient, IVideoRepository videoRepository)
    {
        this.megaApiClient = megaApiClient;
        this.videoRepository = videoRepository;
    }

    public async Task DeleteFileAsync(string nodeId)
    {
        var nodes = megaApiClient.GetNodes();
        var node = nodes.FirstOrDefault(x => x.Id == nodeId);

        if (node is null)
            throw new InvalidOperationException("Node topilmadi yoki sizga tegishli emas.");

        megaApiClient.Delete(node, moveToTrash: false);
    }

    public async Task<UploadResult> UploadVideoOrImageAsync(Stream fileStream, string fileName)
    {
        // Root papkani olish
        var nodes = await megaApiClient.GetNodesAsync();
        var root = nodes.Single(n => n.Type == NodeType.Root);
        // Faylni yuklash
        var uploadedNode = await megaApiClient.UploadAsync(fileStream, fileName, root);
        var fileUrl = megaApiClient.GetDownloadLink(uploadedNode).ToString();

        videoRepository.AddAsync

        return new UploadResult
        {
            FileUrl = fileUrl,
            NodeId = uploadedNode.Id
        };
    }
}
