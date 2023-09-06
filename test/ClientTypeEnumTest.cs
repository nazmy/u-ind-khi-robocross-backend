using Domain.Helper;

namespace test;

[TestFixture]
public class ClientTypeEnumTest
{
    [Test]
    public void Validate_Client_Type()
    {
        Enum serviceClientType = ClientTypeEnum.Service;
        
        Assert.IsNotNull(serviceClientType);
        Assert.AreEqual("Service", serviceClientType.ToString());
        
        Enum industryClientType = ClientTypeEnum.Industry;
        
        Assert.IsNotNull(industryClientType);
        Assert.AreEqual("Industry", industryClientType.ToString());

    }
}