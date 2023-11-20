using System.Numerics;

namespace Domain.Entities;

public struct TransformState
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;
    public Boolean isLocal;
}