using System.Collections;
using UnityEngine;

public static class UtilityFunctions
{
    public static float Map(float value, float initialMin, float initialMax, float destinationMin, float destinationMax)
    {
        var t = (value - initialMin) / (initialMax - initialMin);
        return Mathf.Lerp(destinationMin, destinationMax, t);
    }

    public static Vector2 RotateVector(float angle, Vector2 v)
    {
        var x = v.x * Mathf.Cos(angle * Mathf.Deg2Rad) - v.y * Mathf.Sin(angle * Mathf.Deg2Rad);
        var y = v.x * Mathf.Sin(angle * Mathf.Deg2Rad) + v.y * Mathf.Cos(angle * Mathf.Deg2Rad);

        return new Vector2(x, y);
    }

    public static void DebugDrawRect(Vector3 position, float width, float height, Color color)
    {
        var topLeft = position + new Vector3(-width * 0.5f, height * 0.5f);
        var topRight = position + new Vector3(width * 0.5f, height * 0.5f);
        var bottomLeft = position + new Vector3(-width * 0.5f, -height * 0.5f);
        var bottomRight = position + new Vector3(width * 0.5f, -height * 0.5f);

        Debug.DrawLine(bottomLeft, topLeft, color);
        Debug.DrawLine(topLeft, topRight, color);
        Debug.DrawLine(topRight, bottomRight, color);
        Debug.DrawLine(bottomRight, bottomLeft, color);
    }

    public static void DebugDrawCircle(Vector3 position, float radius, Color color)
    {
        var inc = 36f;
        var angle = 360f / inc;
        var a = position + Vector3.up * radius;

        for (int i = 0; i < inc; i++)
        {
            var b = a - position;
            b = UtilityFunctions.RotateVector(angle, b);
            b += position;
            Debug.DrawLine(a, b, color);
            a = b;
        }
    }

    public static void DebugDrawCross(Vector3 position, float size, Color color, float duration = 0f)
    {
        Vector3 top = position + Vector3.up * size * 0.5f;
        Vector3 bottom = position + Vector3.down * size * 0.5f;
        Vector3 right = position + Vector3.right * size * 0.5f;
        Vector3 left = position + Vector3.left * size * 0.5f;

        Debug.DrawLine(top, bottom, color, duration);
        Debug.DrawLine(right, left, color, duration);
    }
}
