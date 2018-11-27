using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum LivingState
{
    Living,
    Stunned,
    Dead
}

public interface IEntityBeing : IEntityComponent
{
    LivingState LivingState { get; }
    float CurrentHealth { get; }
    float CurrentStunResistance { get; }

    void TakeHit(AttackData attack);

    void AddHealth(float amount);
    void AddStunResistance(float amount);
}
