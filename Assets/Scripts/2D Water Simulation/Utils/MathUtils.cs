using UnityEngine;

public static class MathUtils
{
    public static float SmoothStep(float edge0, float edge1, float x)
    {
        x = Mathf.Clamp01((x - edge0) / (edge1 - edge0));
        return x * x * (3f - 2f * x);
    }

    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * Mathf.Clamp01(t);
    }

    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * Mathf.Clamp01(t);
    }

    public static float Clamp(float value, float min, float max)
    {
        return Mathf.Max(min, Mathf.Min(max, value));
    }

    public static Vector2 Clamp(Vector2 value, Vector2 min, Vector2 max)
    {
        return new Vector2(
            Clamp(value.x, min.x, max.x),
            Clamp(value.y, min.y, max.y)
        );
    }
} 