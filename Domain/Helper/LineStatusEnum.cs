using System.Runtime.Serialization;

namespace Domain.Helper;

public class LineStatusEnum
{

    private LineStatusEnum(string status, string title)
    {
        Status = status;
        Title = title;
    }

    public static LineStatusEnum GetLineStatusEnum(int status)
    {
        switch (status)
        {
            case  0:
                return LineStatusEnum.Initial();
            case  1:
                return LineStatusEnum.Draft();
            case  2:
                return LineStatusEnum.Submitted();
            case  3:
                return LineStatusEnum.Assigned();
            case  4:
                return LineStatusEnum.InProgress();
            case  5:
                return LineStatusEnum.NewProposal();
            case  6:
                return LineStatusEnum.SubmittingFeedback();
            case  7:
                return LineStatusEnum.FeedbackSubmitted();
            case  8:
                return LineStatusEnum.ProposalEditing();
            case  9:
                return LineStatusEnum.UpdatedProposal();
            case  10:
                return LineStatusEnum.ProposalApprove();
            default:
                return LineStatusEnum.Initial();
        }
    }
    
    public string Status { get; private set; }
    
    public string Title { get; private set; }
    
    //Customer Received iPad to Scan
    public static LineStatusEnum Initial(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 : //Customer
                return new("N/A","N/A");
            case 1 : //Integrator
                return new("N/A","N/A");
            case 2: //KHI
                return new("N/A","N/A");
            default:
                return new("N/A","N/A");
        }
    }

    //Customer scanned and saved project locally, it's for client app only
    public static LineStatusEnum Draft(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("Submitting Project", "Uploading in progress");
            case 1 :
                return new("N/A","N/A");
            case 2:
                return new("N/A","N/A");
            default:
                return new("Submitting Project", "Uploading in progress");
        }
    }
    //Customer has submitted a new project to the cloud successfully
    public static LineStatusEnum Submitted(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("Project Submitted", "Project sent to integrator");
            case 1 :
                return new("N/A", "N/A");
            case 2:
                return new("Unassigned","Assign an integrator to review");
            default:
                return new("Project Submitted", "Project sent to integrator");
        }
    }

    //KHI just asigned a new project to integrator
    public static LineStatusEnum Assigned(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("Project Assigned","Pending integrator's review");
            case 1 :
                return new("New Project", "Pending Review");
            case 2:
                return new("In Progress","Pending integrator's review");
            default:
                return new("Project Assigned","Pending integrator's review");
        }
    }

    //Integrator starts to work on the project
    public static LineStatusEnum InProgress(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("In Progress", "Integrator is reviewing");
            case 1 :
                return new("In Progress", "Review and submit the project");
            case 2:
                return new("In Progress", "Integrator is reviewing");
            default:
                return new("In Progress", "Integrator is reviewing");
        }
    }
    
    //Integrator published the proposal
    public static LineStatusEnum NewProposal(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("New Proposal","Review integrator's proposal");
            case 1 :
                return new("Waiting for Feedback","Pending customer's review");
            case 2:
                return new("Waiting for Feedback","Pending customer's review");
            default:
                return new("New Proposal","Review integrator's proposal");
        }
    }
    
    //Customer published the feedback, but can't upload (local status only)
    public static LineStatusEnum SubmittingFeedback(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("Submitting Feedback","Uploading in progress");
            case 1 :
                return new("Waiting for Feedback","Pending customer's review");
            case 2:
                return new("Waiting for Feedback","Pending customer's review");
            default:
                return new("Submitting Feedback","Pending customer's review");
        }
    }
    
    //Customer published the feedback successfully to the cloud
    public static LineStatusEnum FeedbackSubmitted(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("Feedback Submitted","Pending integrator's review");
            case 1 :
                return new("New Feedback","Pending review");
            case 2:
                return new("In Progress","Pending integrator's review");
            default:
                return new("Feedback Submitted","Pending integrator's review");
        }
    }
    
    //Integrator starts to work on the feedback 
    public static LineStatusEnum ProposalEditing(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("In Progress","Pending integrator's review");
            case 1 :
                return new("In Progress","Review and submit the project");
            case 2:
                return new("In Progress","Pending integrator's review");
            default:
                return new("In Progress","Pending integrator's review");
        }
    }
    
    //Proposal is reviewed/updated and submitted back
    public static LineStatusEnum UpdatedProposal(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("Updated Proposal","Review integrator's proposal");
            case 1 :
                return new("Waiting For Feedback", "Pending customer's review");
            case 2:
                return new("Waiting For Feedback", "Pending customer's review");
            default:
                return new("Updated Proposal","Review integrator's proposal");
        }
    }
    
    //Customer approved the proposal 
    public static LineStatusEnum ProposalApprove(int userRole = 0)
    {
        switch (userRole)
        {
            case 0 :
                return new("Approved", "Project has been approved");
            case 1 :
                return new("Approved", "Project has been approved");
            case 2:
                return new("Approved","Project has been approved");
            default:
                return new("Approved", "Project has been approved");
        }
    }
    
    
}