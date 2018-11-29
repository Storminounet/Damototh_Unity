using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class EntityBeingData : ScriptableObject 
{
    [Header("Health")]
    [Space]
    [SerializeField] private float _maxHealth = 10;
    [SerializeField] private float _startHealth = 10;
    [SerializeField] private float _healthRegenPerSecond = 1;

    [Header("Stun Resistance")]
    [Space]
    [SerializeField] private float _maxStunResistance = 10;
    [SerializeField] private float _startStunResistance = 10;
    [SerializeField] private float _stunResistanceRegenPerSecond = 1;
    [Space]
    [SerializeField] private float _stunTimeAtNoHealth = 1;
    [SerializeField] private float _stunTimeAtFullHealth = 1;

    [Header("Hit Taking")]
    [Space]
    [SerializeField] private float _knockbackFactor = 1;

    public float MaxHealth { get { return _maxHealth; } }
    public float StartHealth { get { return _startHealth; } }
    public float HealthRegenPerSecond { get { return _healthRegenPerSecond; } }

    public float MaxStunResistance { get { return _maxStunResistance; } }
    public float StartStunResistance { get { return _startStunResistance; } }
    public float StunResistanceRegenPerSecond { get { return _stunResistanceRegenPerSecond; } }

    public float KnockbackFactor { get { return _knockbackFactor; } }
    public float StunTimeAtNoHealth { get { return _stunTimeAtNoHealth; } }
    public float StunTimeAtFullHealth { get { return _stunTimeAtFullHealth; } }
}
