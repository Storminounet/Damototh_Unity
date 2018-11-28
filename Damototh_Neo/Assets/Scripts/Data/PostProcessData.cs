using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PostProcessData", menuName = "GlobalData/PostProcessData", order = 1000)]
public class PostProcessData : ScriptableObject
{
    [Header("Lock Post-Process")]
    [Space]
    [SerializeField, Range(0, 1f)] private float _lockedPostProcessLerpSpeed;

    [Header("Hit Post-Process")]
    [Space]
    [SerializeField] private float _hitPostProcessActivateDuration;
    [SerializeField] private AnimationCurve _hitPostProcessActivateCurve;
    [Space]
    [SerializeField] private float _hitPostProcessDeactivateDuration;
    [SerializeField] private AnimationCurve _hitPostProcessDeactivateCurve;

    public float LockedPostProcessLerpSpeed { get { return _lockedPostProcessLerpSpeed; } }

    public float HitPostProcessActivateDuration { get { return _hitPostProcessActivateDuration; } }
    public AnimationCurve HitPostProcessActivateCurve { get { return _hitPostProcessActivateCurve; } }

    public float HitPostProcessDeactivateDuration { get { return _hitPostProcessDeactivateDuration; } }
    public AnimationCurve HitPostProcessDeactivateCurve { get { return _hitPostProcessDeactivateCurve; } }

}

