using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


#if UNITY_EDITOR
public class SelectCamera : MonoBehaviour 
{
}
#endif


#if UNITY_EDITOR
[CustomEditor(typeof(SelectCamera))]
public class SelectCameraEditor : Editor
{
    private SelectCamera _instance;

    private void OnEnable()
    {
        _instance = (SelectCamera)target;
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

        pRefs.CamTarget.position = _instance.transform.position;
    }
}
#endif
