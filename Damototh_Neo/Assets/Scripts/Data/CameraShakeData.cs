using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeData", menuName = "GlobalData/CameraShakeData", order = 1000)]
public class CameraShakeData : ScriptableObject
{
    [Header("Attack Hit Camera Shake")]
    [Space]
    [SerializeField] private float _attackHitIntensity;
    [SerializeField] private float _attackHitDamagesFactor;
    [SerializeField] private float _attackHitDuration;
    [SerializeField] private float _attackHitTickDuration;
    [SerializeField] private float _attackHitTickLerpSpeed;
    [SerializeField] private AnimationCurve _attackHitCurve;


    public float AttackHitIntensity { get { return _attackHitIntensity; } }
    public float AttackHitDamagesFactor { get { return _attackHitDamagesFactor; } }
    public float AttackHitDuration { get { return _attackHitDuration; } }
    public float AttackHitTickDuration { get { return _attackHitTickDuration; } }
    public float AttackHitTickLerpSpeed { get { return _attackHitTickLerpSpeed; } }
    public AnimationCurve AttackHitCurve { get { return _attackHitCurve; } }

}

