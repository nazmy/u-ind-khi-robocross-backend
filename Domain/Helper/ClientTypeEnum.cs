using System.Runtime.Serialization;

namespace Domain.Helper;

public enum ClientTypeEnum
{ 
        [EnumMember(Value = "Industry")]
        Industry,  
        [EnumMember(Value = "Service")]
        Service,
        [EnumMember(Value = "SIer")]
        SIer,
}