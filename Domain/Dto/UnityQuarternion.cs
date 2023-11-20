namespace domain.Dto;

public struct UnityQuarternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public UnityQuarternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
}