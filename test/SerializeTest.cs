using System.Xml.Linq;
using Newtonsoft.Json;
using Domain.Dto;
using GeoJSON.Net;

namespace test;

[TestFixture]
internal class SerializeTest : TestBase
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Can_Deserialize_Create_Compound()
    {
        var json = GetExpectedJson();

        CreateCompoundInputDto compound = JsonConvert.DeserializeObject<CreateCompoundInputDto>(json);

        Assert.IsNotNull(compound);
        Assert.IsNotNull(compound.ClientId);
        Assert.IsNotNull(compound.Coordinates);
        Assert.IsNotNull(compound.Coordinates.Coordinates);

        Assert.AreEqual(GeoJSONObjectType.Point, compound.Coordinates.Type);
        Assert.AreEqual(125.6, compound.Coordinates.Coordinates.Longitude);
        Assert.AreEqual(10.1, compound.Coordinates.Coordinates.Latitude);
        Assert.AreEqual(456, compound.Coordinates.Coordinates.Altitude);
    }
}
