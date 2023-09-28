using Azure;
using domain.Dto;
using khi_robocross_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace khi_robocross_api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class CollectionsController : ControllerBase
{
    private readonly IAssetManagerService _assetManagerService;
    private readonly ILogger<CollectionsController> _logger;

    public CollectionsController(IAssetManagerService assetManagerService)
    {
        _assetManagerService = assetManagerService;
        this._logger = new LoggerFactory().CreateLogger<CollectionsController>();
    }

    
    [HttpGet]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Get()
    {
        try
        {
            IEnumerable<CollectionResponse> collectionLists  = await _assetManagerService.GetCollections(null, null);
            return Ok(collectionLists);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error on V1 GetCollection API :{e.StackTrace.ToString()}");
            throw;
        }
      
    }
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post(CreateCollectionInput createCollectionInput)
    {
        try
        {
            await _assetManagerService.CreateCollection(createCollectionInput.Name, createCollectionInput.Metadata);
            return Created("OK", $"Collection {createCollectionInput.Name} has been created successfully");
        }
        catch (ArgumentException aex)
        {
            _logger.LogInformation($"Invalid Argument on V1 Create Collection API :{aex.Message}");
            return BadRequest(aex.Message);
        }
        catch (RequestFailedException fex)
        {
             _logger.LogError($"Error on V1 Create Collection API :{fex.StackTrace}");
            return BadRequest(fex.Message);
           
        }
    }
    
    [HttpDelete]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string collectionName)
    {
        await _assetManagerService.DeleteCollection(collectionName);
        return Ok($"Collection {collectionName} is deleted");
    }
}