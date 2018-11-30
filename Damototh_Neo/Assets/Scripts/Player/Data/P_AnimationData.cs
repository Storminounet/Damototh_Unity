using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "AnimationData", menuName = "PlayerData/AnimationData", order = 1000)]
public class P_AnimationData : ScriptableObject
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(P_AnimationData))]
public class P_AnimationDataEditor : Editor
{
	private P_AnimationData Instance;

    private void OnEnable()
    {
        Instance = (P_AnimationData)target;
    }
}
#endif
