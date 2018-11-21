using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityReferences : MonoBehaviour 
{
    [Header("References")]
    [Header("Body & Visual")]
    [Space]
    [SerializeField] private Transform _physicBody;
    [SerializeField] private Transform _visualBody;

    public Transform PhysicBody { get { return _physicBody; } }
    public Transform VisualBody { get { return _visualBody; } }
}
