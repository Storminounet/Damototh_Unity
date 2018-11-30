using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeData", menuName = "GlobalData/CameraShakeData", order = 1000)]
public class CameraShakeData : ScriptableObject
{
    [System.Serializable]
    public class OneShotShakeData
    {
        [SerializeField] private float _shakeIntensity;
        [SerializeField] private float _shakeDuration;
        [SerializeField] private float _tickDuration;
        [SerializeField] private float _tickLerpSpeed;
        [SerializeField] private AnimationCurve _intensityCurve;

        public float ShakeIntensity { get { return _shakeIntensity; } }
        public float ShakeDuration { get { return _shakeDuration; } }
        public float TickDuration { get { return _tickDuration; } }
        public float TickLerpSpeed { get { return _tickLerpSpeed; } }
        public AnimationCurve IntensityCurve { get { return _intensityCurve; } }
    }


    [Header("Stunning Attack Camera Shake")]
    [Space]
    [SerializeField] private float _saShakeIntensity;
    [SerializeField] private float _saDamagesFactor;
    [SerializeField] private float _saShakeDuration;
    [SerializeField] private float _saTickDuration;
    [SerializeField] private float _saTickLerpSpeed;
    [SerializeField] private AnimationCurve _saIntensityCurve;

    public float SaShakeIntensity { get { return _saShakeIntensity; } }
    public float SaDamagesFactor { get { return _saDamagesFactor; } }
    public float SaShakeDuration { get { return _saShakeDuration; } }
    public float SaTickDuration { get { return _saTickDuration; } }
    public float SaTickLerpSpeed { get { return _saTickLerpSpeed; } }
    public AnimationCurve SaCurve { get { return _saIntensityCurve; } }


}

