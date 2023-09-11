using Domain.Helper;

namespace test;

[TestFixture]
public class LineStatusEnumTest
{
    [Test]
    public void Validate_Line_Status()
    {
        LineStatusEnum userDraftLineStatus = LineStatusEnum.Draft();
        Assert.IsNotNull(userDraftLineStatus);
        Assert.AreEqual("Submitting Project",userDraftLineStatus.Status);
        Assert.AreEqual("Uploading in progress",userDraftLineStatus.Title);
        
        LineStatusEnum integratorDraftLineStatus = LineStatusEnum.Draft(1);
        Assert.IsNotNull(integratorDraftLineStatus);
        Assert.AreEqual("N/A",integratorDraftLineStatus.Status);
        Assert.AreEqual("N/A",integratorDraftLineStatus.Title);

        LineStatusEnum userAssignedLineStatus = LineStatusEnum.Assigned();
        Assert.IsNotNull(userAssignedLineStatus);
        Assert.AreEqual("Project Assigned",userAssignedLineStatus.Status);
        Assert.AreEqual("Pending integrator's review",userAssignedLineStatus.Title);

        LineStatusEnum integratorAssignedLineStatus = LineStatusEnum.Assigned(1);
        Assert.IsNotNull(integratorAssignedLineStatus);
        Assert.AreEqual("New Project",integratorAssignedLineStatus.Status);
        Assert.AreEqual("Pending Review",integratorAssignedLineStatus.Title);
        
        LineStatusEnum khiAssignedLineStatus = LineStatusEnum.Assigned(2);
        Assert.IsNotNull(khiAssignedLineStatus);
        Assert.AreEqual("In Progress",khiAssignedLineStatus.Status);
        Assert.AreEqual("Pending integrator's review",khiAssignedLineStatus.Title);
    }
}