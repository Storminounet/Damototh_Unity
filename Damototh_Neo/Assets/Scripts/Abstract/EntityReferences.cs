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
    [Header("Components")]
    [Space]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _collision;
    [Header("Data")]
    [Space]
    [SerializeField] private EntityBeingData _beingData;

    public Transform PhysicBody { get { return _physicBody; } }
    public Transform VisualBody { get { return _visualBody; } }

    public Rigidbody Rigidbody { get { return _rigidbody; } }
    public CapsuleCollider Collision { get { return _collision; } }

    public EntityBeingData BeingData { get { return _beingData; } }

}
