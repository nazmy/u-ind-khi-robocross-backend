using Azure;
using domain.Dto;
using khi_robocross_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace khi_robocross_api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class AssetsController : ControllerBase
{
    private readonly IAssetManagerService _assetManagerService;
    private readonly ILogger<AssetsController> _logger;

    public AssetsController(IAssetManagerService assetManagerService)
    {
        _assetManagerService = assetManagerService;
        _logger = new LoggerFactory().CreateLogger<AssetsController>();
    }

    [HttpGet("Collection/{collectionName}")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get(string collectionName)
    {
        try
        {
            IEnumerable<AssetResponse> assetList = await _assetManagerService.ListFileInCollection(collectionName);
            return Ok(assetList);
        }
        catch (ArgumentException aex)
        {
            _logger.LogInformation($"Invalid Argument on V1 Get Asset API :{aex.Message}");
            return BadRequest(aex.Message);
        }
    }

    [HttpPost("Collection/{collectionName}")] 
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Post([FromForm] IFormFile file, string collectionName)
    {
        FileStream fileStream = new FileStream(file.FileName, FileMode.Create, FileAccess.ReadWrite);

        try
        {
            await file.CopyToAsync(fileStream);

            var content =
                await _assetManagerService.UploadFileToCollection(collectionName, file.FileName, fileStream, null);
            return Created("OK", $"Asset {file.FileName} has been uploaded successfully");
        }
        catch (ArgumentException aex)
        {
            _logger.LogInformation($"Invalid Argument on V1 Upload Asset API :{aex.Message}");
            return BadRequest(aex.Message);
        }
        catch (RequestFailedException fex)
        {
            _logger.LogError($"Error on V1 Upload Asset API :{fex.StackTrace}");
            return BadRequest(fex.Message);
        }
        finally
        {
            fileStream.Close();
        }
    }

    [HttpGet("Collection/{collectionName}/Download")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Download(string collectionName, string fileName)
    {
        try
        {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            await _assetManagerService.DownloadFile(collectionName, fileName,fs);
            
            if (fs.CanRead)
            {
                var stream = new FileStream(fs.Name, FileMode.Open);
                fs.Flush();
                fs.Close();
                return File(stream, "application/octet-stream", fileName);
            }

            return Problem($"Failed to download the blob {fileName} in {collectionName}");
        }
        catch (ArgumentException aex)
        {
            _logger.LogError($"Error on V1 Download Asset API :{aex.StackTrace}");
            return BadRequest(aex.Message);
        }
        catch (RequestFailedException rex)
        {
            _logger.LogError($"Error on V1 Download Asset API :{rex.StackTrace}");
            if (rex.Status == 404)
            {
                var errorString = $"The blob {fileName} in {collectionName} doesn't exist";
                _logger.LogError(errorString);
                return NotFound(errorString);
            }

            return Problem(rex.Message);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error on V1 Download Asset API :{e.StackTrace}");
            return Problem(e.Message);
        }

    }
    
    [HttpPut("Collection/{collectionName}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateMetada(UpdateAssetInput updateAssetInput, string collectionName)
    {
        try
        {
            await _assetManagerService.UpdateFile(updateAssetInput,collectionName);
            return NoContent();
        }
        catch (ArgumentException aex)
        {
            _logger.LogError($"Error on V1 Update Asset API :{aex.StackTrace}");
            return BadRequest(aex.Message);
        }
        catch (RequestFailedException rex)
        {
            _logger.LogError($"Error on V1 Update Asset API :{rex.StackTrace}");
            if (rex.Status == 404)
            {
                var errorString = $"The blob {updateAssetInput.Name} in {collectionName} doesn't exist";
                _logger.LogError(errorString);
                return NotFound(errorString);
            }
            throw;
        }
    }

    [HttpDelete]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(string collectionName, string filename)
    {
        try
        {
            await _assetManagerService.DeleteFile(collectionName, filename);
            return Ok($"Asset {filename} in {collectionName} collection is deleted");
        }
        catch (ArgumentException aex)
        {
            _logger.LogError($"Error on V1 Delete Asset API :{aex.StackTrace}");
            return BadRequest(aex.Message);
        }
        catch (RequestFailedException rex)
        {
            _logger.LogError($"Error on V1 Delete Asset API :{rex.StackTrace}");
            if (rex.Status == 404)
            {
                var errorString = $"The blob {filename} in {collectionName} doesn't exist";
                _logger.LogError(errorString);
                return NotFound(errorString);
            }
            throw;
        }
    }
}