using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "GuardBeingData", menuName = "EnemyData/GuardBeingData", order = 1000)]
public class GuardBeingData : EntityBeingData
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(GuardBeingData))]
public class GuardBeingDataEditor : Editor
{
	private GuardBeingData Instance;

    private void OnEnable()
    {
        Instance = (GuardBeingData)target;
    }
}
#endif

