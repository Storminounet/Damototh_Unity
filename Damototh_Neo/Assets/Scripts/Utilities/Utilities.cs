using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour 
{
    public static Vector2 SubtractMagnitude(Vector2 v, float magnitude)
    {
        if (v.magnitude > magnitude)
            return v.normalized * (v.magnitude - magnitude);
        else
            return Vector3.zero;
    }

    public static Vector2 DeadzoneVector(Vector2 vec, float innerDeadzone = 0.1f, float outterDeadzone = 0f, float linearity = 1f)
    {
        if (vec.magnitude < innerDeadzone)
        {
            return Vector2.zero;
        }
        else if (vec.magnitude > 1 - outterDeadzone)
        {
            return vec.normalized;
        }
        else
        {
            vec = SubtractMagnitude(vec, innerDeadzone);
            vec *= 1 / (1 - outterDeadzone - innerDeadzone);
            float x = Mathf.Pow(Mathf.Abs(vec.x), linearity);
            float y = Mathf.Pow(Mathf.Abs(vec.y), linearity);

            x *= vec.x < 0 ? -1 : 1;
            y *= vec.y < 0 ? -1 : 1;
            return new Vector2(x, y);
        }
    }


    public static void SetLayerOfAllChildrens(Transform transform, LayerMask layer)
    {
        transform.gameObject.layer = layer;

        foreach (Transform t in transform)
        {
            SetLayerOfAllChildrens(t, layer);
        }
    }
}
