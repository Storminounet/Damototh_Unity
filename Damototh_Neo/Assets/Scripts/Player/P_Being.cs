using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class P_Being : P_Component, IEntityBeing
{
    public P_Being(P_References references, P_PlayerController master) : base(references, master) {}

    private float _currentHealth;
    private float _currentStunResistance;

    private LivingState _livingState = LivingState.Living;
    private Coroutine _stunCoroutine;

    public float CurrentHealth { get { return _currentHealth; } }
    public float CurrentStunResistance { get { return _currentStunResistance; } }
    public float MaxHealth { get { return BData.MaxHealth; } }
    public float MaxStunResistance { get { return BData.MaxStunResistance; } }

    public LivingState LivingState { get { return _livingState; } }

    public override void Awake()
    {
#if UNITY_EDITOR
        try
        {
#endif
            AddHealth(BData.StartHealth);
            AddStunResistance(BData.StartStunResistance);
#if UNITY_EDITOR
        }
        catch
        {

        }
#endif
    }

    public override void MainUpdate()
    {
        ApplyHealthRegen();
        ApplyStunResistanceRegen();
    }

    private void ApplyHealthRegen()
    {
        if (_livingState != LivingState.Dead)
        {
            AddHealth(BData.HealthRegenPerSecond * WorldData.DeltaTime);
        }
    }

    private void ApplyStunResistanceRegen()
    {
        if (_livingState == LivingState.Living)
        {
            AddStunResistance(BData.StunResistanceRegenPerSecond * WorldData.DeltaTime);
        }
    }

    public void AddHealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, BData.MaxHealth);


        if (_currentHealth <= 0f)
        {
            Death();
        }
    }
    public void AddStunResistance(float amount)
    {
        _currentStunResistance = Mathf.Clamp(_currentStunResistance + amount, 0f, BData.MaxStunResistance);

        if (_currentStunResistance <= 0f)
        {
            Stun();
        }
    }

    public void TakeHit(AttackData attack)
    {
        AddHealth(-attack.Damages);

        if (_livingState == LivingState.Living)
        {
            AddStunResistance(-attack.StunPower);
        }
    }

    private void Stun()
    {
        if (_stunCoroutine != null)
        {
            master.StopCoroutine(_stunCoroutine);
        }

        _stunCoroutine = master.StartCoroutine(StunCoroutine());

        AddStunResistance(BData.StartStunResistance);
    }

    private IEnumerator StunCoroutine()
    {
        _livingState = LivingState.Stunned;
        yield return new WaitForSeconds(Mathf.Lerp(BData.StunTimeAtNoHealth, BData.StunTimeAtFullHealth, _currentHealth / BData.MaxHealth));
        _livingState = LivingState.Living;

        _stunCoroutine = null;
    }


    private void Death()
    {
        _livingState = LivingState.Dead;
        master.OnDeath();
    }


}