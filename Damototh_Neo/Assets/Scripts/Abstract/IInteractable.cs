using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum InteractableType
{
    Mechanism,
    Corpse
}

public interface IInteractable 
{
    MonoBehaviour mono { get; }
    string Name { get; }
    bool CanBeInteracted { get; }
    InteractableType InteractableType { get; }

    Vector3 InteractPosition { get; }

    void Interact();
}


