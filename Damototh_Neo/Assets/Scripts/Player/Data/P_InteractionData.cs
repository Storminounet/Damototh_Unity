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
    [SerializeField] private float _interactionAngle;

    [Header("Drink parameters")]
    [Space]
    [SerializeField] private float _drinkDistance;
    [SerializeField] private float _outsideCombatDrinkTime;
    [SerializeField] private float _insideCombatDrinkTime;

    public float DetectionDistance { get { return _detectionDistance; } }

    public float DrinkDistance { get { return _drinkDistance; } }
    public float InteractionAngle { get { return _interactionAngle; } }
    public float OutsideCombatDrinkTime { get { return _outsideCombatDrinkTime; } }
    public float InsideCombatDrinkTime { get { return _insideCombatDrinkTime; } }

}