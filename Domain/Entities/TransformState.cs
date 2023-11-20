using System.Numerics;
using domain.Dto;

namespace Domain.Entities;

public struct TransformState
{
    public Vector3 Position;
    public UnityQuarternion Rotation;
    public Vector3 Scale;
    public Boolean isLocal;
}