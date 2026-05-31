using UnityEngine;

/// <summary>
/// Furukubo
/// </summary>
public static class OriginalCalculateUtils
{
    public static float DirectionToAngle(Vector2 direction)
    {
        Vector3 normalized = direction.normalized;
        return Mathf.Atan2(normalized.y, normalized.x) * Mathf.Rad2Deg;
    }

    public static Vector2 AngleToDirection(float angle)
    {
        float radiun = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radiun), Mathf.Sin(radiun));
    }

    public static int Loop(int min, int max, int value)
    {
        if (max <= min) return max;
        int range = (max - min);
        return ((value - min) % range + range) % range;
    }
}