using System.Runtime.Serialization;

namespace Domain.Helper;

public enum SceneObjectTypeEnum
{ 
        [EnumMember(Value = "Robot")]
        Robot,
        [EnumMember(Value = "Controller")]
        Controller,
        [EnumMember(Value = "Effector")]
        Effector,
        [EnumMember(Value = "Safety Devices")]
        SafetyDevices,
        [EnumMember(Value = "PLC")]
        PLC
}