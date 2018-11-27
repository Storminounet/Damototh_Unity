using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class CameraShakeManager : Singleton<CameraShakeManager> 
{
    [Header("References")]
    [Space]
    [SerializeField] CameraShakeData _data;

    private Coroutine _hitCameraShakePostProcess = null;

    private void Awake()
    {
        SetInstance(this);
    }

    //Utilities
    private void StartHitCameraShake(AttackData attack)
    {
        if (_hitCameraShakePostProcess != null)
        {
            StopCoroutine(_hitCameraShakePostProcess);
        }

        _hitCameraShakePostProcess = StartCoroutine(HitCameraShakeCoroutine(attack));
    }

    //Coroutines
    private IEnumerator HitCameraShakeCoroutine(AttackData attack)
    {
        Vector2 angles = Vector2.zero;
        float count = 0f, progress = 0f;
        while (count < _data.AttackHitDuration)
        {
            count += _data.AttackHitTickDuration;

            progress = count / _data.AttackHitDuration;
            progress = _data.AttackHitCurve.Evaluate(progress);

            angles = Random.insideUnitCircle.normalized * progress;
            angles *= (_data.AttackHitIntensity + attack.Damages * _data.AttackHitDamagesFactor);
            transform.localRotation = Quaternion.Euler(angles);

            yield return new WaitForSeconds(_data.AttackHitTickDuration);
        }
    }

    //Static Events
    public static void OnPlayerHitEntity(AttackData attack)
    {
        Instance.StartHitCameraShake(attack);
    }

    public static void OnPlayerKillEntity(AttackData attack)
    {
        Instance.StartHitCameraShake(attack);
    }
}
