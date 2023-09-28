
using khi_robocross_api.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace test;

[TestFixture]
public class AssetManagerBlobTest
{
    private readonly IAssetManagerService _assetManagerService;
    
    [Test]
    public void Validate_Create_New_Collection()
    {
        var assetManagerService = new Mock<IAssetManagerService>();
        assetManagerService
            .Setup(x => x.CreateCollection(It.IsAny<string>(), null));
        
        //await Program
        
        _assetManagerService.CreateCollection("test-collection", null);
    }
}