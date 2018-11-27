using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GuardBeing : GuardComponent, IEntityBeing
{
    public GuardBeing(GuardReferences refs, GuardController master) : base(refs, master) { }

    private float _currentHealth = 0f;
    private float _currentStunResistance = 0f;
    private LivingState _livingState = LivingState.Living;

    public LivingState LivingState { get { return _livingState; } }
    public float CurrentHealth { get { return _currentHealth; } }
    public float CurrentStunResistance { get { return _currentStunResistance; } }

    public override void Awake()
    {
        AddHealth(BData.StartHealth);
        AddStunResistance(BData.StartStunResistance);
    }

    public override void MainUpdate()
    {
        AddHealth(BData.HealthRegenPerSecond * WorldData.DeltaTime);
        AddStunResistance(BData.StunResistanceRegenPerSecond * WorldData.DeltaTime);
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
        AddStunResistance(-attack.StunPower);
    }

    private void Stun()
    {

    }

    private void Death()
    {
        _livingState = LivingState.Dead;
        WorldManager.OnEntityDeath(EntityMaster);
        Object.Destroy(EntityMaster.gameObject);
    }
}