using Azure.Storage.Blobs;
using domain.Dto;

namespace khi_robocross_api.Services;

public interface IAssetManagerService
{
    Task CreateCollection(string collectionName,IDictionary<string,string> metadata);
    
    ValueTask<IEnumerable<CollectionResponse>> GetCollections(string prefix, int? segmentSize);
    
    Task DeleteCollection(string collectionName);

    ValueTask<IEnumerable<AssetResponse>> ListFileInCollection(string collectionName);

    Task DownloadFile(string collectionName, string fileName, FileStream fileStream);
    
    Task UpdateFile(UpdateAssetInput updateAssetInput);
    
    Task<bool> UploadFileToCollection(string collectionName, string filename, FileStream fileStream, IDictionary<string,string> metadata);
    
    Task DeleteFile(string collectionName, string fileName);
}