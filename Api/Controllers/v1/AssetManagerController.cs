using khi_robocross_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace khi_robocross_api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class AssetManagerController : ControllerBase
{
    private readonly IAssetManagerService _assetManagerService;

    public AssetManagerController(IAssetManagerService assetManagerService)
    {
        _assetManagerService = assetManagerService;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    public async Task Get()
    {
        await _assetManagerService.ListContainers("",10);
    }
}