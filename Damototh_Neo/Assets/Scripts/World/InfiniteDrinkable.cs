using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class InfiniteDrinkable : MonoBehaviour, IDrinkable
{
    [SerializeField] private float _drinkableBlood;
    public float DrinkableBlood
    {
        get
        {
            return _drinkableBlood;
        }
    }

    public MonoBehaviour mono { get { return this; } }
    public Vector3 DrinkPosition { get { return transform.position; } }
    public string Name { get { return name; } }
    public bool CanBeInteracted { get { return true; } }
    public InteractableType InteractableType { get { return global::InteractableType.Corpse; } }
    public Vector3 InteractPosition { get { return transform.position + Vector3.up; } }


    public void Interact()
    {
        
    }
}