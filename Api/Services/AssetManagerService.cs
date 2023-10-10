using System.Text.RegularExpressions;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using domain.Dto;

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

    public async  ValueTask<IEnumerable<CollectionResponse>> GetCollections(string prefix, int? segmentSize)
    {
        CollectionResponse collection = new CollectionResponse();
        List<CollectionResponse> collectionList = new List<CollectionResponse>();
        try
        {
            
            var resultSegment = _blobServiceClient.GetBlobContainersAsync(BlobContainerTraits.Metadata, prefix, default) 
                .AsPages(default, segmentSize);

            await foreach (Page<BlobContainerItem> containerPage in resultSegment)
            {
                foreach (BlobContainerItem containerItem in containerPage.Values)
                { 
                    collection = new CollectionResponse();
                    collection.Name = containerItem.Name;
                    collection.Metadata = containerItem.Properties.Metadata;
                    collection.LastModified = containerItem.Properties.LastModified;
                    collectionList.Add(collection);
                }
            }

            return collectionList;
        }
        catch (RequestFailedException rex)
        {
            throw rex;
        }
    }

    public async ValueTask<IEnumerable<AssetResponse>> ListFileInCollection(string collectionName)
    {
        if (!isValidFolderNamingConvention(collectionName))
        {
            throw new ArgumentException($"Collection Name '{collectionName}' is invalid. It should only contains alphanumeric characters");
        }
        
        List<AssetResponse> assetList = new List<AssetResponse>();
        try
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(collectionName);
            
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(BlobTraits.Metadata))
            {
                AssetResponse assetResponse = new AssetResponse();
                assetResponse.Name = blobItem.Name;
                assetResponse.Metadata = blobItem.Metadata;
                assetResponse.LastModified = blobItem.Properties.LastModified;
                assetList.Add(assetResponse); 
            }
            return assetList;
        }
        catch (RequestFailedException rex)
        {
            throw rex;
        }
    }

    public async Task CreateCollection(String collectionName, IDictionary<string,string> metadata)
    {
        try
        {
            if (!isValidFolderNamingConvention(collectionName))
            {
                throw new ArgumentException($"Collection Name '{collectionName}' is invalid. It should only contains alphanumeric characters");
            }
            
            foreach (KeyValuePair<string,string> metadataItem in metadata)
            {
                if (!isValidFolderNamingConvention(metadataItem.Key))
                {
                    throw new ArgumentException($"Metadata key '{metadataItem.Key}' is invalid. It should only contains alphanumeric characters");
                }
            }
            
            BlobContainerClient blobContainer = await _blobServiceClient.CreateBlobContainerAsync(collectionName, PublicAccessType.BlobContainer, metadata);
        }
        catch (RequestFailedException fex)
        {
            throw fex;
        }
    }
    
    
    public async Task DeleteCollection(string collectionName)
    {
        if (collectionName == null)
            throw new ArgumentException("Collection Name is Invalid");
        
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(collectionName);
        await containerClient.DeleteAsync();
    }

    public async Task DownloadFile(string collectionName, string fileName, FileStream fileStream)
    {
        if (!isValidFolderNamingConvention(collectionName))
        {
            throw new ArgumentException($"Collection Name '{collectionName}' is invalid. It should only contains alphanumeric characters");
        }

        try
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(collectionName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
           
            await blobClient.DownloadToAsync(fileStream);
        }
        catch (RequestFailedException rex)
        {
            throw rex;
        }
    }

    public async Task<bool> UploadFileToCollection(string collectionName, string fileName, FileStream fileStream, IDictionary<string,string>? metadata)
    {
        try
        {
            if (metadata != null)
            {
                foreach (KeyValuePair<string, string> metadataItem in metadata)
                {
                    if (!isValidFolderNamingConvention(metadataItem.Key))
                    {
                        throw new ArgumentException(
                            $"Metadata key '{metadataItem.Key}' is invalid. It should only contains alphanumeric characters");
                    }
                }
            }

            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(collectionName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            fileStream.Seek(0, SeekOrigin.Begin);
            await blobClient.UploadAsync(fileStream);

            return true;
        }
        catch (RequestFailedException fex)
        {
            throw fex;
        }
        finally
        {
            fileStream.Close();
        }
    }

    public async Task UpdateFile(UpdateAssetInput updateAssetInput, string collectionName)
    {
        try
        {
            if (updateAssetInput.Metadata != null)
            {
                foreach (KeyValuePair<string, string> metadataItem in updateAssetInput.Metadata)
                {
                    if (!isValidFolderNamingConvention(metadataItem.Key))
                    {
                        throw new ArgumentException(
                            $"Metadata key '{metadataItem.Key}' is invalid. It should only contains alphanumeric characters");
                    }
                }
            }

            BlobContainerClient containerClient =
                _blobServiceClient.GetBlobContainerClient(collectionName);
            BlobClient blobClient = containerClient.GetBlobClient(updateAssetInput.Name);
            await blobClient.SetMetadataAsync(updateAssetInput.Metadata);
        }
        catch (RequestFailedException rex)
        {
            throw rex;
        }
    }

    public async Task DeleteFile(string collectionName, string fileName)
    {
        if (collectionName == null)
            throw new ArgumentException("Collection Name is Invalid");
        
        if (fileName == null)
            throw new ArgumentException("File Name is Invalid");
        
        if (!isValidFolderNamingConvention(collectionName))
        {
            throw new ArgumentException($"Collection Name '{collectionName}' is invalid. It should only contains alphanumeric characters");
        }
        try
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(collectionName);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.DeleteAsync();
        }
        catch (RequestFailedException rex)
        {
            throw rex;
        }
    }


    /// <summary> 
    /// bool validate if the folder/metadataKey is valid following azure container/metadata naming rule.  
    /// </summary>
    private bool isValidFolderNamingConvention(String name)
    {
        return (Regex.IsMatch(name, @"^[a-zA-Z0-9_-]*$")) ? true : false;
    }
    /// <summary> 
    /// string GetRandomBlobName(string filename): Generates a unique random file name to be uploaded  
    /// </summary> 
    private string GetRandomBlobName(string filename)
    {
        string ext = Path.GetExtension(filename);
        return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
    } 
}