using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "MovementData", menuName = "PlayerData/MovementData", order = 1000)]
public class P_MovementData : ScriptableObject
{
    [Header("Grounded")]
    [Space]
    [SerializeField] private float _moveAcceleration;
    [Space]
    [SerializeField, Range(0f, 1f)] private float _walkInputThreshold;
    [SerializeField] private float _walkMaxSpeed;
    [Space]
    [SerializeField, Range(0f, 1f)] private float _runInputThreshold;
    [SerializeField] private float _runMaxSpeed;
    [Space]
    [SerializeField, Range(0.2f, 1f)] private float _sprintInputThreshold;
    [SerializeField] private float _sprintMaxSpeed;
    [Space]
    [SerializeField] private float _sideBrake;
    [SerializeField] private float _autoBrake;
    [Space]
    [SerializeField, Range(1f, 89f)] private float _climbMaxAngle;

    [Header("Dodge")]
    [Space]
    [SerializeField] private float _dodgeDuration;
    [SerializeField] private float _dodgeSpeed;
    [SerializeField] private AnimationCurve _dodgeSpeedOverDuration;
    [SerializeField] private float _dodgeCooldown;

    [Header("Fall Damages")]
    [Space]
    [SerializeField] private float _fallDamageMinVelocity;
    [SerializeField] private float _fallDamageBase;
    [SerializeField] private float _fallDamagePower;

    [Header("Ground Detection")]
    [Space]
    [SerializeField] private float _sphereCastRadius;
    [SerializeField] private float _sphereCastAdditionalLength;

    [Header("Body Shape")]
    [Space]
    [SerializeField] private float _bodyRadius = 0.25f;
    [SerializeField] private float _standHeight = 1.85f;
    [SerializeField] private float _stepDownHeight = 0.3f;
    [SerializeField] private float _stepUpHeight = 0.3f;

    public float MoveAcceleration { get { return _moveAcceleration; } }

    public float WalkInputThreshold { get { return _walkInputThreshold; } }
    public float WalkMaxSpeed { get { return _walkMaxSpeed; } }

    public float RunInputThreshold { get { return _runInputThreshold; } }
    public float RunMaxSpeed { get { return _runMaxSpeed; } }

    public float SprintInputThreshold { get { return _sprintInputThreshold; } }
    public float SprintMaxSpeed { get { return _sprintMaxSpeed; } }

    public float SideBrake { get { return _sideBrake; } }
    public float AutoBrake { get { return _autoBrake; } }

    public float ClimbMaxAngle { get { return _climbMaxAngle; } }

    public float DodgeDuration { get { return _dodgeDuration; } }
    public float DodgeSpeed { get { return _dodgeSpeed; } }
    public AnimationCurve DodgeSpeedOverDuration { get { return _dodgeSpeedOverDuration; } }
    public float DodgeCooldown { get { return _dodgeCooldown; } }

    public float FallDamageMinVelocity { get { return _fallDamageMinVelocity; } }
    public float FallDamageBase { get { return _fallDamageBase; } }
    public float FallDamagePower { get { return _fallDamagePower; } }

    public float SphereCastRadius { get { return _sphereCastRadius; } }
    public float SphereCastAdditionalLength { get { return _sphereCastAdditionalLength; } }

    public float BodyRadius { get { return _bodyRadius; } }
    public float StandHeight { get { return _standHeight; } }
    public float StepDownHeight { get { return _stepDownHeight; } }
    public float StepUpHeight { get { return _stepUpHeight; } }


#if UNITY_EDITOR
    public void OnValidate()
    {
        if (_standHeight < _bodyRadius * 2)
        {
            _standHeight = _bodyRadius * 2;
        }
        if (_stepUpHeight < 0f)
        {
            _stepUpHeight = 0f;
        }
        if (_stepUpHeight > _standHeight - _bodyRadius * 2)
        {
            _stepUpHeight = _standHeight - _bodyRadius * 2;
        }

        P_References f;
        if (f = FindObjectOfType<P_References>())
        {
            if (f.PlayerBeingData == null)
            f.EditorForceAwake();
        }
        P_PlayerController p;
        if (p = FindObjectOfType<P_PlayerController>())
        {
            if (p.Being == null)
            p.EditorForceAwake();
            p.MovementController.SetBodyShape(_standHeight, _stepUpHeight, _bodyRadius);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(P_MovementData))]
public class P_MovementDataEditor : Editor
{
    private P_MovementData Instance;

    private void OnEnable()
    {
        Instance = (P_MovementData)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Instance.OnValidate();
    }
}
#endif
