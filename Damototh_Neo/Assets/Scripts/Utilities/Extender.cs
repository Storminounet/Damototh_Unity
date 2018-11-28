using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extender 
{
    #region float
    public static float SubtractAbsolute(this float f, float amount)
    {
        bool signed = f < 0;
        f = Mathf.Abs(f);
        if (f <= amount)
        {
            return 0f;
        }
        else
        {
            return (f - amount) * (signed ? -1 : 1);
        }
    }
    public static float Abs(this float f)
    {
        return Mathf.Abs(f);
    }
    #endregion

    #region Vector 3
    public static Vector3 SetX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }
    public static Vector3 SetY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }
    public static Vector3 SetZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }
    public static Vector3 AddX(this Vector3 v, float x)
    {
        v.x += x;
        return v;
    }
    public static Vector3 AddY(this Vector3 v, float y)
    {
        v.y += y;
        return v;
    }
    public static Vector3 AddZ(this Vector3 v, float z)
    {
        v.z += z;
        return v;
    }
    public static Vector3 SwapXY(this Vector3 v)
    {
        float temp = v.x;
        v.x = v.y;
        v.y = temp;
        return v;
    }
    public static Vector3 SwapXZ(this Vector3 v)
    {
        float temp = v.x;
        v.x = v.z;
        v.z = temp;
        return v;
    }
    public static Vector3 SwapYZ(this Vector3 v)
    {
        float temp = v.y;
        v.y = v.z;
        v.z = temp;
        return v;
    }
    public static Vector2 ToV2(this Vector3 v, bool Z_AxisIs_Y_Axis = false)
    {
        return new Vector2(v.x, Z_AxisIs_Y_Axis ? v.z : v.y);
    }
    public static Vector3 SubtractMagnitude(this Vector3 v, float amount)
    {
        //Store magntiude
        float magnitude = v.magnitude;
        //if magnitude is 0
        if (magnitude == 0f)
        {
            return v;
        }
        //Normalize vector
        v /= magnitude;
        //Calculate new magnitude
        magnitude = Mathf.Clamp(magnitude - amount, 0f, Mathf.Infinity);
        //Return vector with correct magnitude
        return v * magnitude;
    }
    #endregion

    #region Vector2
    public static Vector2 SetX(this Vector2 v, float x)
    {
        v.x = x;
        return v;
    }
    public static Vector2 SetY(this Vector2 v, float y)
    {
        v.y = y;
        return v;
    }
    public static Vector2 AddX(this Vector2 v, float x)
    {
        v.x += x;
        return v;
    }
    public static Vector2 AddY(this Vector2 v, float y)
    {
        v.y += y;
        return v;
    }
    public static Vector2 SwapXY(this Vector2 v)
    {
        float temp = v.x;
        v.x = v.y;
        v.y = temp;
        return v;
    }
    public static Vector2 ToV3(this Vector2 v, float zValue = 0)
    {
        return new Vector3(v.x, v.y, zValue);
    }
    public static Vector2 ToV3_YisZ(this Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }
    #endregion

    #region Colors
    public static Color SetA(this Color c, float a)
    {
        c.a = a;
        return c;
    }
    public static Color SetR(this Color c, float r)
    {
        c.r = r;
        return c;
    }
    public static Color SetG(this Color c, float g)
    {
        c.g = g;
        return c;
    }
    public static Color SetB(this Color c, float b)
    {
        c.b = b;
        return c;
    }
    #endregion

}
