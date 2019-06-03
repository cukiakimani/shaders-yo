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

    public static bool Chance(float chance)
    {
        return Random.Range(0, 100) <= chance;
    }

    public static float RandomSign(float chanceForPositive)
    {
        return(Chance(chanceForPositive) ? 1 : -1);
    }

    public static Vector3 SmoothApproach(Vector3 pastValue, Vector3 pastTargetValue, Vector3 targetValue, float speed)
    {
        float t = Time.deltaTime * speed;
        Vector3 v = (targetValue - pastTargetValue) / t;
        Vector3 f = pastValue - pastTargetValue + v;
        return targetValue - v + f * Mathf.Exp(-t);
    }
    
    public static float BobWave(float from, float to, float duration, float offset)
    {
        var arg4 = (to - from) * 0.5f;
        return from + arg4 + Mathf.Sin((((Time.time) + duration * offset) / duration) * 6.283185f) * arg4;
    }

    public static float SmoothApproach(float pastValue, float pastTargetValue, float targetValue, float speed)
    {
        Debug.Log("Target val: " + targetValue);
        Debug.Log("Past target val: " + pastTargetValue);

        float t = Time.deltaTime * speed;
        float v = (targetValue - pastTargetValue) / t;
        Debug.Log("V: " + v);
        float f = pastValue - pastTargetValue + v;
        Debug.Log("F: " + f);
        float final = targetValue - v + f * Mathf.Exp(-t);
        Debug.Log("Final: " + final);
        return final;
    }

    public static float SmoothApproach(float startVal, float endVal, float speed)
    {
        float min = Mathf.Min(startVal + speed, endVal);
        float max = Mathf.Max(startVal - speed, endVal);
        return startVal < endVal ? min : max;
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
            b = UtilityFunctions.RotateVector2(angle, b);
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

    // angle in degrees
    public static Vector2 RotateVector2(float angle, Vector2 v)
    {
        var x = v.x * Mathf.Cos(angle * Mathf.Deg2Rad) - v.y * Mathf.Sin(angle * Mathf.Deg2Rad);
        var y = v.x * Mathf.Sin(angle * Mathf.Deg2Rad) + v.y * Mathf.Cos(angle * Mathf.Deg2Rad);

        return new Vector2(x, y);
    }

    public static Vector3 ScaleTransformVector(Vector3 vector, Vector3 scale)
    {
        return new Vector3(vector.x * scale.x, vector.y * scale.y);
    }
    
    public static bool CompareVectors(Vector3 a, Vector3 b, float angleError)
    {
        //if they aren't the same length, don't bother checking the rest.
        if (!Mathf.Approximately(a.magnitude, b.magnitude))
            return false;

        var cosAngleError = Mathf.Cos(angleError * Mathf.Deg2Rad);

        //A value between -1 and 1 corresponding to the angle.
        var cosAngle = Vector3.Dot(a.normalized, b.normalized);

        //The dot product of normalized Vectors is equal to the cosine of the angle between them.
        //So the closer they are, the closer the value will be to 1.  Opposite Vectors will be -1
        //and orthogonal Vectors will be 0.
 
        //If angle is greater, that means that the angle between the two vectors is less than the error allowed.
        return cosAngle >= cosAngleError ? true : false;
    }

    public static bool OnSameRenderingLayer(GameObject Object1, GameObject Object2, bool HigherLayerOkay, bool HigherOrderOkay)
    {
        int layer1 = Object1.GetComponent<Renderer>().sortingLayerID;
        int layer2 = Object2.GetComponent<Renderer>().sortingLayerID;

        if (layer1 != layer2)
        {
            if (layer2 < layer1)
            {
                return false;
            }
            else if (layer2 > layer1)
            {
                return HigherLayerOkay;
            }
        }

        if (layer1 == layer2)
        {
            int order1 = Object1.GetComponent<Renderer>().sortingOrder;
            int order2 = Object2.GetComponent<Renderer>().sortingOrder;
            if (order2 >= order1)
            {
                return (order1 == order2 ? true : HigherOrderOkay);
            }
            else
            {
                return false;
            }
        }

        return false;

    }

    public static bool IsEqualToAnyInt(int valueToCompare, int value1, int value2)
    {
        return (valueToCompare == value1 || valueToCompare == value2);
    }

    public static bool IsEqualToAnyInt(int valueToCompare, int[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (valueToCompare == values[i])
                return true;
        }
        return false;
    }

    public static bool IsEqualToAnyInt(int valueToCompare, int[] values, int oddValue)
    {
        if (valueToCompare == oddValue)
            return true;

        for (int i = 0; i < values.Length; i++)
        {
            if (valueToCompare == values[i])
                return true;
        }

        return false;
    }
}
