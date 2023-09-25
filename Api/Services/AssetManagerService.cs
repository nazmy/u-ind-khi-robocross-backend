using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace khi_robocross_api.Services;

public class AssetManagerService : IAssetManagerService
{
    private BlobServiceClient _blobServiceClient;
    private string _accountName;
    private string _accountKey;

    private string blobUri;

    public AssetManagerService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task ListContainers(string prefix, int? segmentSize)
    {
        try
        {
            var resultSegment = _blobServiceClient.GetBlobContainersAsync(BlobContainerTraits.Metadata, prefix, default)
                .AsPages(default, segmentSize);

            await foreach (Page<BlobContainerItem> containerPage in resultSegment)
            {
                foreach (BlobContainerItem containerItem in containerPage.Values)
                {
                    Console.WriteLine("Container Name : {0}", containerItem.Name);
                }
            }
        }
        catch (RequestFailedException e)
        {
            throw new RequestFailedException(e.Message);
        }
    }

    public Task CreateCollection()
    {
        throw new NotImplementedException();
    }
}