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

    private bool _canPerformActions = true;

    private P_References _pRefs;
    private P_Being _being;
    private P_CameraController _cameraController;
    private P_MovementController _movementController;
    private P_InteractionController _interactionController;
    private P_AttackController _attackController;
    private P_VisualHandler _visualHandler;

    #region Entity Props
    //Refs
    public P_References pRefs { get { return _pRefs; } }

    public new P_BeingData BData { get { return pRefs.PlayerBeingData; } }
    public P_InputData IpData { get { return pRefs.InputData; } }
    public P_CameraData CData { get { return pRefs.CameraData; } }
    public P_MovementData MData { get { return pRefs.MovementData; } }
    public P_AttackData AData { get { return pRefs.AttackData; } }
    public P_InteractionData ItData { get { return pRefs.InteractionData; } }
    public P_VisualData VData { get { return pRefs.VisualData; } }

    public new P_Being Being { get { return _being; } }
    public P_CameraController CameraController { get { return _cameraController; } }
    public P_MovementController MovementController { get { return _movementController; } }
    public P_InteractionController InteractionController { get { return _interactionController; } }
    public P_AttackController AttackController { get { return _attackController; } }
    public P_VisualHandler VisualHandler { get { return _visualHandler; } }

    //Useful for components
    //Inputs
    public bool InputingMovement { get { return InputManager.InputingMovement; } }
    public bool InputingLook { get { return InputManager.InputingLook; } }

    public float MoveInputMagntiude { get { return InputManager.MoveInputMagntiude; } }
    public float LookInputMagntiude { get { return InputManager.LookInputMagntiude; } }

    public Vector2 MoveInput { get { return InputManager.MoveInput; } }
    public Vector2 MoveInputNormalized { get { return InputManager.MoveInputNormalized; } }
    public Vector2 LookInput { get { return InputManager.LookInput; } }
    public Vector2 LookInputNormalized { get { return InputManager.LookInputNormalized; } }

    //Others
    public bool CanPerformActions { get { return _canPerformActions; } }
    //Useful for components end
    #endregion

    //Being
    public LivingState LivingState { get { return Being.LivingState; } }

    //Move
    public MovingState MovingState { get { return MovementController.MovingState; } }
    public bool Sprint { get { return InputManager.Sprint; } }
    public bool Dodge { get { return InputManager.Dodge; } }

    //Cam
    public bool AutoRotate { get { return InputManager.AutoRotate; } }
    public bool LockDown { get { return InputManager.LockDown; } }
    public bool LockUp { get { return InputManager.LockUp; } }
    public bool LockLeftTarget { get { return InputManager.LockLeftTarget; } }
    public bool LockRightTarget { get { return InputManager.LockRightTarget; } }

    //Combat
    public AttackState AttackState { get { return AttackController.AttackState; } }
    public bool LightAttack { get { return InputManager.LightAttack; } }
    public bool HeavyAttack { get { return InputManager.HeavyAttack; } }
    public bool HydraAttackOne { get { return InputManager.HydraAttackOne; } }
    public bool HydraAttackTwo { get { return InputManager.HydraAttackTwo; } }


    protected override void Awake()
    {
        base.Awake();
        _pRefs = (P_References)refs;

        _being = new P_Being(_pRefs, this);
        _cameraController = new P_CameraController(_pRefs, this);
        _movementController = new P_MovementController(_pRefs, this);
        _interactionController = new P_InteractionController(_pRefs, this);
        _attackController = new P_AttackController(_pRefs, this);
        _visualHandler = new P_VisualHandler(_pRefs, this);

        AddComponent(_being);
        AddComponent(_cameraController);
        AddComponent(_movementController);
        AddComponent(_interactionController);
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

    protected override void LateUpdate()
    {
        base.LateUpdate();

        UpdateCanPerformActions();
    }
    private void UpdateCanPerformActions()
    {
        if (MovingState == MovingState.Dodging ||
            MovingState == MovingState.VelocityOverriden ||
            AttackState != AttackState.None ||
            LivingState == LivingState.Dead ||
            LivingState == LivingState.Stunned)
        {
            _canPerformActions = false;
        }
        else
        {
            _canPerformActions = true;
        }
    }

    //Events
    public void OnFeetHeightChanged(float heightDifference)
    {
        _visualHandler.OnFeetHeightChanged(heightDifference);
    }
    public void OnTakeFallDamages(float damages)
    {
        Being.AddHealth(-damages);
    }
    public override void OnAttackStart(AttackData attack)
    {
        _movementController.OnAttackStart(attack);
    }
    public override void OnEntityHit(EntityController hitEntity, AttackData hitAttack)
    {
        WorldManager.OnPlayerHit(hitEntity, hitAttack);
    }
    public override void OnEntityKilled(EntityController killedEntity, AttackData killingAttack)
    {
        WorldManager.OnPlayerKill(killedEntity, killingAttack);
        CameraController.OnEntityKilled(killedEntity, killingAttack);
    }
    public override void OnDeath()
    {
        CameraController.OnDeath();
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

    protected override void OnDrawGizmos()
    {
        if (_cameraController == null)
        {
            Awake();
        }

        base.OnDrawGizmos();
    }
#endif
    #endregion
}
