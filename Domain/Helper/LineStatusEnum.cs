using System.Runtime.Serialization;

namespace Domain.Helper;

public enum LineStatusEnum
{
    Initial = 0,
    Draft = 1,
    Submitted = 2,
    Assigned = 3,
    InProgress = 4,
    NewProposal = 5,
    SubmittingFeedback = 6,
    FeedbackSubmitted = 7,
    ProposalEditing = 8,
    UpdatedProposal = 9,
    ProposalApproved = 10
}