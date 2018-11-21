using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_References : EntityReferences
{
    [Header("Data")]
    [Space]
    [SerializeField] private P_InputData _inputData;
    [SerializeField] private P_CameraData _cameraData;
    [SerializeField] private P_MovementData _movementData;
    [SerializeField] private P_AttackData _attackData;
    [SerializeField] private P_VisualData _visualData;

    [Header("Camera")]
    [Space]
    [SerializeField] private Transform _camTarget;
    [Space]
    [SerializeField] private Transform _camFollower;
    [SerializeField] private Transform _camYRotator;
    [SerializeField] private Transform _camOffsetArm;
    [SerializeField] private Transform _camXRotator;
    [SerializeField] private Transform _camAnimParentTransform;
    [SerializeField] private Transform _camScriptedParentTransform;
    [SerializeField] private Transform _camTransform;
    [Space]
    [SerializeField] private Transform _rotationCalculator;


    [Header("Components")]
    [Space]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CapsuleCollider _collision;
    [SerializeField] private Camera _camera;

    [Header("Others")]
    [Space]
    [SerializeField] private Transform _velocitySpace;

    public P_InputData InputData { get { return _inputData; } }
    public P_CameraData CameraData { get { return _cameraData; } }
    public P_MovementData MovementData { get { return _movementData; } }
    public P_AttackData AttackData { get { return _attackData; } }
    public P_VisualData VisualData { get { return _visualData; } }


    public Transform CamTarget { get { return _camTarget; } }

    public Transform CamFollower { get { return _camFollower; } }
    public Transform CamYRotator { get { return _camYRotator; } }
    public Transform CamOffsetArm { get { return _camOffsetArm; } }
    public Transform CamXRotator { get { return _camXRotator; } }
    public Transform CamAnimParentTransform { get { return _camAnimParentTransform; } }
    public Transform CamScriptedParentTransform { get { return _camScriptedParentTransform; } }
    public Transform CamTransform { get { return _camTransform; } }

    public Transform RotationCalculator { get { return _rotationCalculator; } }


    public Rigidbody Rigidbody { get { return _rigidbody; } }
    public CapsuleCollider Collision { get { return _collision; } }
    public Camera Camera { get { return _camera; } }

    public Transform VelocitySpace { get { return _velocitySpace; } }


}