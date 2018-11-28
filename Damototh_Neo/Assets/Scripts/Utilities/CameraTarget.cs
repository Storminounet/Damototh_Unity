using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraTarget : MonoBehaviour 
{
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraTarget))]
public class CameraTargetEditor : Editor
{
    private CameraTarget _instance;

    private void OnEnable()
    {
        _instance = (CameraTarget)target;
    }

    public override void OnInspectorGUI()
    {
        P_References pRefs = _instance.GetComponentInParent<P_References>();

        if (GUILayout.Button("Select Camera"))
        {
            Transform cam = pRefs.CamTransform;
            if (cam == null)
            {
                return;
            }
            else
            {
                Selection.activeTransform = cam.transform;
            }
        }

        if (UnityEditor.EditorApplication.isPlaying == true)
        {
            return;
        }

        pRefs.CamFollower.position = _instance.transform.position;
    }
}
#endif