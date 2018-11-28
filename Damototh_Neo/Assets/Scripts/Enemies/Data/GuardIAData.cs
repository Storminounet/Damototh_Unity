using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "GuardIAData", menuName = "EnemyData/GuardIAData", order = 1000)]
public class GuardIAData : EntityIAData
{
}

#if UNITY_EDITOR
[CustomEditor(typeof(GuardIAData))]
public class GuardIADataEditor : Editor
{
	private GuardIAData Instance;

    private void OnEnable()
    {
        Instance = (GuardIAData)target;
    }
}
#endif
