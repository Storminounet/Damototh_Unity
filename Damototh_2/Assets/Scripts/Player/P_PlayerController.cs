using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_PlayerController : EntityController
{
#if UNITY_EDITOR
    [ReadOnly] public bool e_Grounded;
    [ReadOnly] public bool e_InputingMovement;
    [ReadOnly] public MovingState e_MovingState;
    [ReadOnly] public AttackState e_AttackState;
    [ReadOnly] public string e_CurrentAttackName;
#endif

    private P_References _pRefs;
    private P_CameraController _cameraController;
    private P_MovementController _movementController;
    private P_AttackController _attackController;
    private P_VisualHandler _visualHandler;

    #region Entity Props
    //Refs
    public P_References pRefs { get { return _pRefs; } }

    public P_InputData IData { get { return pRefs.InputData; } }
    public P_CameraData CData { get { return pRefs.CameraData; } }
    public P_MovementData MData { get { return pRefs.MovementData; } }
    public P_AttackData AData { get { return pRefs.AttackData; } }
    public P_VisualData VData { get { return pRefs.VisualData; } }

    public P_CameraController CameraController { get { return _cameraController; } }
    public P_MovementController MovementController { get { return _movementController; } }
    public P_AttackController AttackController { get { return _attackController; } }
    public P_VisualHandler VisualHandler { get { return _visualHandler; } }

    //Useful for components
    public bool InputingMovement { get { return InputManager.InputingMovement; } }
    public bool InputingLook { get { return InputManager.InputingLook; } }

    public float MoveInputMagntiude { get { return InputManager.MoveInputMagntiude; } }
    public float LookInputMagntiude { get { return InputManager.LookInputMagntiude; } }

    public Vector2 MoveInput { get { return InputManager.MoveInput; } }
    public Vector2 MoveInputNormalized { get { return InputManager.MoveInputNormalized; } }
    public Vector2 LookInput { get { return InputManager.LookInput; } }
    public Vector2 LookInputNormalized { get { return InputManager.LookInputNormalized; } }

    //Utilities
    public Vector3 Position { get { return pRefs.PhysicBody.position; } set { pRefs.Rigidbody.MovePosition(value); } }
    public Quaternion Rotation { get { return pRefs.VisualBody.rotation; } set { pRefs.VisualBody.rotation = value; } }
    public Vector3 Velocity { get { return pRefs.Rigidbody.velocity; } set { pRefs.Rigidbody.velocity = value; } }
    #endregion

    //Move
    public bool Sprint { get { return InputManager.Sprint; } }
    public bool Dodge { get { return InputManager.Dodge; } }

    //Cam
    public bool AutoRotate { get { return InputManager.AutoRotate; } }
    public bool Lock { get { return InputManager.Lock; } }
    public bool LockLeftTarget { get { return InputManager.LockLeftTarget; } }
    public bool LockRightTarget { get { return InputManager.LockRightTarget; } }

    //Combat
    public bool LightAttack { get { return InputManager.LightAttack; } }
    public bool HeavyAttack { get { return InputManager.HeavyAttack; } }
    public bool HydraAttackOne { get { return InputManager.HydraAttackOne; } }
    public bool HydraAttackTwo { get { return InputManager.HydraAttackTwo; } }


    protected override void Awake()
    {
        base.Awake();
        _pRefs = (P_References)refs;

        _cameraController = new P_CameraController(_pRefs, this);
        _movementController = new P_MovementController(_pRefs, this);
        _attackController = new P_AttackController(_pRefs, this);
        _visualHandler = new P_VisualHandler(_pRefs, this);

        AddComponent(_cameraController);
        AddComponent(_movementController);
        AddComponent(_attackController);
        AddComponent(_visualHandler);

        AwakeComponents();
    }

    
    protected override void Update()
    {
        base.Update();

#if UNITY_EDITOR
        UpdateReadOnlyValues();
#endif
    }

    //Events
    public void OnFeetHeightChanged(float heightDifference)
    {
        _visualHandler.OnFeetHeightChanged(heightDifference);
    }

    public void OnAttackStart(AttackData attack)
    {
        _movementController.OnAttackStart(attack);
    }

    public void OnStartVelocityOverride(Vector3 velocity, bool isLocalOverride = false)
    {
        _movementController.OnStartVelocityOverride(velocity, isLocalOverride);
    }

    public void OnStopVelocityOverride()
    {
        _movementController.OnStopVelocityOverride();
    }

    #region Editor Only
#if UNITY_EDITOR
    public void EditorForceAwake()
    {
        Awake();
    }

    public void UpdateReadOnlyValues()
    {
        e_Grounded = MovementController.Grounded;
        e_InputingMovement = InputingMovement;
        e_MovingState = MovementController.MovingState;
        e_AttackState = AttackController.AttackState;
        if (AttackController.CurrentAttack == null)
        {
            e_CurrentAttackName = "None";
        }
        else
        {
            e_CurrentAttackName = AttackController.CurrentAttack.Model.name;
        }
    }

    private void OnDrawGizmos()
    {
        if (_cameraController == null)
        {
            Awake();
        }
        _cameraController.OnDrawGizmos();
        _movementController.OnDrawGizmos();
        _visualHandler.OnDrawGizmos();
    }
#endif
    #endregion
}
