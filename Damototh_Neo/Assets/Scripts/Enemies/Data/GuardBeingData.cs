using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "GuardBeingData", menuName = "EnemyData/GuardBeingData", order = 1000)]
public class GuardBeingData : EntityBeingData, IDrinkableEntityBeingData
{
    [SerializeField] private float _drinkableBlood;

    public float DrinkableBlood { get { return _drinkableBlood; } }
}