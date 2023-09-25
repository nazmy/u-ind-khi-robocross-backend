using Azure.Storage.Blobs;

namespace khi_robocross_api.Services;

public interface IAssetManagerService
{
    Task CreateCollection();
    Task ListContainers(string prefix, int? segmentSize);
}