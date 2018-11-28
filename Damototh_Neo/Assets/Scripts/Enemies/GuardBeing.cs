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

    private Coroutine _stunCoroutine;

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
        if (_livingState != LivingState.Dead)
        {
            ApplyHealthRegen();
            ApplyStunResistanceRegen();
        }
        else
        {
            DeathUpdate();
        }
    }
    private void ApplyHealthRegen()
    {
        AddHealth(BData.HealthRegenPerSecond * WorldData.DeltaTime);
    }
    private void ApplyStunResistanceRegen()
    {
        if (_livingState == LivingState.Living)
        {
            AddStunResistance(BData.StunResistanceRegenPerSecond * WorldData.DeltaTime);
        }
    }

    private void DeathUpdate()
    {
        Refs.PhysicBody.eulerAngles = Vector3.MoveTowards(Refs.PhysicBody.eulerAngles, Refs.PhysicBody.eulerAngles.SetX(90f), Time.deltaTime * 90f);
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
            Master.StopCoroutine(_stunCoroutine);
        }

        _stunCoroutine = Master.StartCoroutine(StunCoroutine());

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
        if (_stunCoroutine != null)
        {
            Master.StopCoroutine(_stunCoroutine);
        }

        _livingState = LivingState.Dead;
        Master.OnDeath();
        WorldManager.OnEntityDeath(EntityMaster);
    }
}