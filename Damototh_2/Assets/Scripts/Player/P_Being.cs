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
        AddHealth(BData.HealthRegenPerSecond * WorldData.DeltaTime);
        AddStunResistance(BData.StunResistanceRegenPerSecond * WorldData.DeltaTime);
    }

    public void AddHealth(float amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, MaxHealth);

        if (_currentHealth <= 0f)
        {
            Death();
        }
    }

    public void AddStunResistance(float amount)
    {
        _currentStunResistance = Mathf.Clamp(_currentStunResistance + amount, 0f, MaxStunResistance);

        if (_currentStunResistance <= 0f)
        {
            Stun();
        }
    }

    public void TakeHit(AttackData attack)
    {
        AddHealth(-attack.Damages);
        AddStunResistance(-attack.StunPower);
    }


    private void Death()
    {
        _livingState = LivingState.Dead;
        master.OnDeath();
    }

    private void Stun()
    {

    }


}