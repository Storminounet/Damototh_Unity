using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class AttackData
{
    [System.Serializable]
    public class AttackTimings
    {
        [SerializeField] private float _castTime;
        [SerializeField] private float _attackTime;
        [SerializeField] private float _forcedRecoverTime;
        [SerializeField] private float _comboRecoverTime;

        public float CastTime { get { return _castTime; } }
        public float AttackTime { get { return _attackTime; } }
        public float ForcedRecoverTime { get { return _forcedRecoverTime; } }
        public float ComboRecoverTime { get { return _comboRecoverTime; } }
    }
    [System.Serializable]
    public class AttackAnimations
    {
        [SerializeField] private Animation _castAnim;
        [SerializeField] private Animation _attackAnim;
        [SerializeField] private Animation _forcedRecoverAnim;
        [SerializeField] private Animation _comboRecoverAnim;

        public Animation CastAnim { get { return _castAnim; } }
        public Animation AttackAnim { get { return _attackAnim; } }
        public Animation ForcedRecoverAnim { get { return _forcedRecoverAnim; } }
        public Animation ComboRecoverAnim { get { return _comboRecoverAnim; } }
    }
    [System.Serializable]
    public class AttackVelocityOverride
    {
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private float _duration;

        public Vector3 Velocity { get { return _velocity; } }
        public float Duration { get { return _duration; } }
    }

    [SerializeField] private GameObject _model;
    [SerializeField] private AttackAnimations _animations;
    [Space]
    [SerializeField] private AttackTimings _timings;
    [Space]
    [SerializeField] private float _velocityOverrideInitialWait;
    [SerializeField] private AttackVelocityOverride[] _attackVelocityOverrides;
    [Space]
    [SerializeField] private float _damages;
    [SerializeField] private float _stunPower;
    [SerializeField] private float _healthCost;
    [Space]
    [SerializeField] private int _maximumEnemyNumberHit = 1;
    [Space]
    [SerializeField] private bool _canCombo;

    public GameObject Model { get { return _model; } }
    public AttackAnimations Animations { get { return _animations; } }

    public AttackTimings Timings { get { return _timings; } }

    public float VelocityOverrideInitialWait { get { return _velocityOverrideInitialWait; } }
    public AttackVelocityOverride[] AttackVelocityOverrides { get { return _attackVelocityOverrides; } }

    public float Damages { get { return _damages; } }
    public float StunPower { get { return _stunPower; } }
    public float HealthCost { get { return _healthCost; } }

    public int MaximumEnemyNumberHit { get { return _maximumEnemyNumberHit; } }

    public bool CanCombo { get { return _canCombo; } }
}

[CreateAssetMenu(fileName = "AttackData", menuName = "PlayerData/AttackData", order = 1000)]
public class P_AttackData : ScriptableObject
{
    public enum AttackType
    {
        Light,
        Heavy,
        Hydra
    }
    [SerializeField] private AttackData _lightAttack_1;
    [SerializeField] private AttackData _lightAttack_2;
    [SerializeField] private AttackData _lightAttack_3;
    [SerializeField] private AttackData _heavyAttack_1;
    [SerializeField] private AttackData _heavyAttack_2;
    [SerializeField] private AttackData _heavyAttack_3;
    [SerializeField] private AttackData _hydraAttackOne;
    [SerializeField] private AttackData _hydraAttackTwo;

    public AttackData LightAttack_1 { get { return _lightAttack_1; } }
    public AttackData LightAttack_2 { get { return _lightAttack_2; } }
    public AttackData LightAttack_3 { get { return _lightAttack_3; } }
    public AttackData HeavyAttack_1 { get { return _heavyAttack_1; } }
    public AttackData HeavyAttack_2 { get { return _heavyAttack_2; } }
    public AttackData HeavyAttack_3 { get { return _heavyAttack_3; } }
    public AttackData HydraAttackOne { get { return _hydraAttackOne; } }
    public AttackData HydraAttackTwo { get { return _hydraAttackTwo; } }

    public AttackData GetAttack(AttackType attackType, int id)
    {
        switch (attackType)
        {
            case AttackType.Light:
                switch (id)
                {
                    case 1:
                        return _lightAttack_1;
                    case 2:
                        return _lightAttack_2;
                    case 3:
                        return _lightAttack_3;
                }
                break;
            case AttackType.Heavy:
                switch (id)
                {
                    case 1:
                        return _heavyAttack_1;
                    case 2:
                        return _heavyAttack_2;
                    case 3:
                        return _heavyAttack_3;
                }
                break;
            case AttackType.Hydra:
                switch (id)
                {
                    case 1:
                        return _hydraAttackOne;
                    case 2:
                        return _hydraAttackTwo;
                }
                break;
        }

#if UNITY_EDITOR
        Debug.LogError("Wrong ID for " + attackType + " ID : " + id);
#endif
        return null;
    }
}
