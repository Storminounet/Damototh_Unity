using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum AttackState
{
    None,
    Casting,
    Attacking,
    ForcedRecovering,
    ComboRecovering
}

public class P_AttackController : P_Component
{
    public P_AttackController(P_References playerReferences, P_PlayerController master) : base(playerReferences, master) { }

    private int _currentAttackId = 0;
    private AttackState _currentAttackState = AttackState.None;
    private AttackData _currentAttack = null;
    private GameObject _currentAttackGO = null;
    private Attack _currentAttackMono = null;

    private Coroutine _attackCoroutine = null;
    private Coroutine _attackVelocityOverridesCoroutine = null;

    public AttackState AttackState { get { return _currentAttackState; } }
    public AttackData CurrentAttack { get { return _currentAttack; } }
    public GameObject CurrentAttackGO { get { return _currentAttackGO; } }
    public Attack CurrentAttackMono { get { return _currentAttackMono; } }

    public override void MainUpdate()
    {
        CheckAttacks();
    }

    private void CheckAttacks()
    {
        if (_currentAttack != null)
        {
            if (_currentAttack.CanCombo == false)
            {
                return;
            }
            else if (_currentAttackState != AttackState.ComboRecovering)
            {
                return;
            }
        }


        if (master.LightAttack)
        {
            _currentAttackId++;
            StartAttack(AData.GetAttack(P_AttackData.AttackType.Light, _currentAttackId)); 
        }
        else if (master.HeavyAttack)
        {
            _currentAttackId++;
            StartAttack(AData.GetAttack(P_AttackData.AttackType.Heavy, _currentAttackId));
        }
    }

    private void StartAttack(AttackData attack)
    {
        OnAttackStart(attack);

        if (_attackCoroutine != null)
        {
            master.StopCoroutine(_attackCoroutine);
        }
        if (_attackVelocityOverridesCoroutine != null)
        {
            master.StopCoroutine(_attackVelocityOverridesCoroutine);
        }

        _attackCoroutine = master.StartCoroutine(AttackCoroutine(attack));
        _attackVelocityOverridesCoroutine = master.StartCoroutine(AttackVelocityOverridesCoroutine(attack.VelocityOverrideInitialWait, attack.AttackVelocityOverrides));
    }

    private IEnumerator AttackCoroutine(AttackData attack)
    {
        _currentAttack = attack;
        _currentAttackState = AttackState.Casting;

        yield return new WaitForSeconds(attack.Timings.CastTime);

        _currentAttackState = AttackState.Attacking;

        InstantiateAttackHitbox(attack);

        yield return new WaitForSeconds(attack.Timings.AttackTime);

        _currentAttackState = AttackState.ForcedRecovering;

        yield return new WaitForSeconds(attack.Timings.ForcedRecoverTime);

        _currentAttackState = AttackState.ComboRecovering;

        yield return new WaitForSeconds(attack.Timings.ComboRecoverTime);

        _currentAttackState = AttackState.None;
        ResetAttackVars();
    }

    private IEnumerator AttackVelocityOverridesCoroutine(float initialWait, AttackData.AttackVelocityOverride[] velocityOverrides)
    {
        yield return new WaitForSeconds(initialWait);

        for (int i = 0; i < velocityOverrides.Length; i++)
        {
            master.OnStartVelocityOverride(velocityOverrides[i].Velocity, true);

            if (velocityOverrides[i].Duration <= 0)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(velocityOverrides[i].Duration);
            }

            master.OnStopVelocityOverride();
        }
    }

        private void InstantiateAttackHitbox(AttackData attack)
    {
        _currentAttackGO = Object.Instantiate(
            attack.Model,
            Position,
            Quaternion.LookRotation(master.MovementController.LastMoveDirection),
            master.transform);

        _currentAttackMono = _currentAttackGO.GetComponent<Attack>();

        _currentAttackMono.Initialize(master, attack);
    }

    private void ResetAttackVars()
    {
        _currentAttackGO = null;
        _currentAttackMono = null;
        _currentAttack = null;
        _attackCoroutine = null;
        _currentAttackId = 0;
    }


    //Events
    private void OnAttackStart(AttackData attack)
    {
        master.OnAttackStart(attack);
    }

}
