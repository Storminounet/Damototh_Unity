using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "InteractionData", menuName = "PlayerData/InteractionData", order = 1000)]
public class P_InteractionData : ScriptableObject 
{
    [Header("Detection parameters")]
    [Space]
    [SerializeField] private float _detectionDistance;

    [Header("Drink parameters")]
    [Space]
    [SerializeField] private float _drinkDistance;
    [SerializeField] private float _drinkAngle;

    public float DetectionDistance { get { return _detectionDistance; } }

    public float DrinkDistance { get { return _drinkDistance; } }
    public float DrinkAngle { get { return _drinkAngle; } }
}