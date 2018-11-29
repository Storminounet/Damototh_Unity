using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public interface IDrinkable : IInteractable 
{
    float DrinkableBlood { get; }

    Vector3 DrinkPosition { get; }
}