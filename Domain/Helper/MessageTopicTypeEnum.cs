using System.Runtime.Serialization;

namespace Domain.Helper;

public enum MessageTopicTypeEnum
{ 
        [EnumMember(Value = "Line")]
        Line,  
        [EnumMember(Value = "Unit")]
        Unit,
        [EnumMember(Value = "Robot")]
        Robot,
        [EnumMember(Value = "Client")]
        Client,
        [EnumMember(Value = "User")]
        User,
}