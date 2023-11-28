using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using domain.Dto;
using Newtonsoft.Json;

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
                assetResponse.Metadata = new Dictionary<string, string>();
                assetResponse.Name = blobItem.Name;
                
                var jsonSerializerSettings = new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } };
                
                foreach (var blobMetadata in blobItem.Metadata)
                {
                    switch (blobMetadata.Key)
                    {
                        case "Id":
                            assetResponse.Id = blobMetadata.Value;
                            break;
                        case "Name":
                            assetResponse.Name = blobMetadata.Value;
                            break;
                        case "GlbModelUrl":
                            assetResponse.GlbModelUrl = blobMetadata.Value;
                            break;
                        case "PngThumbnailUrl":
                            assetResponse.PngThumbnailUrl = blobMetadata.Value;
                            break;        
                        case "ComponentJson":
                            assetResponse.ComponentJson = blobMetadata.Value;
                            break;  
                        case "Tags":
                            assetResponse.Tags = blobMetadata.Value;
                            break;
                        case "SlotsCompatibleLibraryIds":
                            assetResponse.SlotsCompatibleLibraryIds = blobMetadata.Value;
                            break;
                        case "ConstantSlotCount":
                            bool constantSlotCountFlag;
                            if (Boolean.TryParse(blobMetadata.Value.ToLower(), out constantSlotCountFlag))
                            {
                                assetResponse.ConstantSlotCount = constantSlotCountFlag;    
                            }
                            break;
                        case "IsDeleted":
                            bool isDeletedFlag;
                            if (Boolean.TryParse(blobMetadata.Value.ToLower(), out isDeletedFlag))
                            {
                                assetResponse.ConstantSlotCount = isDeletedFlag;    
                            }
                            break;
                        case "CreatedBy":
                            assetResponse.CreatedBy = blobMetadata.Value;
                            break;
                        case "CreatedAt":
                            assetResponse.CreatedAt = DateTimeOffset.Parse(blobMetadata.Value); 
                            break;
                        case "LastUpdatedBy":
                            assetResponse.LastUpdatedBy = blobMetadata.Value;
                            break;
                        case "LastUpdatedAt":
                            assetResponse.LastUpdatedAt = DateTimeOffset.Parse(blobMetadata.Value); 
                            break;
                        default:
                            assetResponse.Metadata.Add(blobMetadata.Key,blobMetadata.Value);
                            break;
                    }
                }
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

    public async Task UpdateFile(UpdateAssetInput updateAssetInput, string collectionName, string assetName)
    {

        IDictionary<string, string> assetMetadata;
        assetMetadata = PopulateAssetMetadata(updateAssetInput);
        assetMetadata.Add("Name",assetName);
        
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
                    else
                    {
                        assetMetadata.Add(metadataItem.Key,metadataItem.Value);
                    }
                }
            }

            BlobContainerClient containerClient =
                _blobServiceClient.GetBlobContainerClient(collectionName);
            BlobClient blobClient = containerClient.GetBlobClient(assetName);
            await blobClient.SetMetadataAsync(assetMetadata);
        }
        catch (RequestFailedException rex)
        {
            throw rex;
        }
    }

    private IDictionary<string, string> PopulateAssetMetadata(UpdateAssetInput updateAssetInput)
    {
        IDictionary<string, string> assetMetadata = new Dictionary<string, string>();
        assetMetadata.Add("Id",updateAssetInput.Id);
        assetMetadata.Add("GlbModelUrl",updateAssetInput.GlbModelUrl);
        assetMetadata.Add("PngThumbnailUrl",updateAssetInput.PngThumbnailUrl);
        assetMetadata.Add("ComponentJson",updateAssetInput.ComponentJson);
        assetMetadata.Add("Tags",updateAssetInput.Tags);
        assetMetadata.Add("SlotsCompatibleLibraryIds",updateAssetInput.SlotsCompatibleLibraryIds);
        assetMetadata.Add("ConstantSlotCount",updateAssetInput.ConstantSlotCount.ToString());
        assetMetadata.Add("IsDeleted",updateAssetInput.IsDeleted.ToString());
        assetMetadata.Add("CreatedAt",updateAssetInput.CreatedAt.ToString());
        assetMetadata.Add("CreatedBy",updateAssetInput.CreatedBy);
        assetMetadata.Add("LastUpdatedAt",updateAssetInput.LastUpdatedAt.ToString());
        assetMetadata.Add("LastUpdatedBy",updateAssetInput.LastUpdatedBy);

        return assetMetadata;
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