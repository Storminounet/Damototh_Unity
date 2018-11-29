using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class BezierCurve : MonoBehaviour 
{
    [SerializeField] private Vector3[] points;
	
	private void Awake() 
	{
		if (points == null || points.Length == 0)
        {
            ResetCurve();
        }
	}

    private void ResetCurve()
    {
        points = new Vector3[]
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 2),
                new Vector3(0, 0, 3),
                new Vector3(0, 0, 4)
            };
    }
	
	
	private void Update() 
	{
		
	}

    public Vector3 GetControlPoint(int id)
    {
        return transform.TransformPoint(points[id]);
    }

    public void SetControlPoint(int id, Vector3 value)
    {
        points[id] = transform.InverseTransformPoint(value);
    }

    public Vector3 GetPoint(float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return
            oneMinusT * oneMinusT * oneMinusT * GetControlPoint(0) +
            3f * oneMinusT * oneMinusT * t * GetControlPoint(1) +
            3f * oneMinusT * t * t * GetControlPoint(2) +
            t * t * t * GetControlPoint(3);
    }

    private void OnDrawGizmos()
    {
        Awake();
        int lineStep = 100;

        Vector3 lastPoint = GetPoint(0);
        Vector3 curPoint;
        for (int i = 1; i < lineStep; i++)
        {
            curPoint = GetPoint((float)i / lineStep);
            Gizmos.DrawLine(lastPoint, curPoint);
            lastPoint = curPoint;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor
{
	private BezierCurve Instance;

    private void OnEnable()
    {
        Instance = (BezierCurve)target;
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < 4; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 point = Instance.GetControlPoint(i);
            point = Handles.DoPositionHandle(point, Quaternion.identity);

            if (EditorGUI.EndChangeCheck())
            {
                Instance.SetControlPoint(i, point);
            }
        }

        Handles.color = Color.cyan;
        Handles.DrawLine(Instance.GetControlPoint(0), Instance.GetControlPoint(1));
        Handles.DrawLine(Instance.GetControlPoint(2), Instance.GetControlPoint(3));
    }
}
#endif
