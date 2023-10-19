using System.Runtime.Serialization;

namespace Domain.Helper;

public enum MessageTypeEnum
{ 
        [EnumMember(Value = "Message")]
        Message,  
        [EnumMember(Value = "Notification")]
        Notification,
        [EnumMember(Value = "Annotation")]
        Annotation,
        [EnumMember(Value = "Measurement")]
        Measurement
}