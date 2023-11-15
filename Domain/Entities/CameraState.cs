using System.Numerics;

namespace Domain.Entities;

public struct CameraState
{
    public Vector3 Position;
    public Vector3 Forward;
    public float FieldOfView;
    public float AspectRatio;
    public float PivotDistance;
}