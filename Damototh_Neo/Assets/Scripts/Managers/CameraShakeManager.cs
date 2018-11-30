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

    private Vector2 ScreenDirectionToRotationDirection(Vector2 screenDirection)
    {
        Vector2 rotDir = new Vector2();
        rotDir.x = -screenDirection.y;
        rotDir.y = screenDirection.x;
        return rotDir;
    }

    //Coroutines
    private IEnumerator HitCameraShakeCoroutine(AttackData attack)
    {
        Vector2 angles = Vector2.zero;
        float count = 0f, progress = 0f;
        float updateCount = 0f;
        Vector2 shakeDirection = ScreenDirectionToRotationDirection(Vector2.down);
        bool shakePositive = true;

        CameraShakeData.OneShotShakeData shake = attack.CameraShake;

        while (count < shake.ShakeDuration)
        {
            count += WorldData.DeltaTime;
            updateCount += WorldData.DeltaTime;

            if (updateCount > shake.TickDuration)
            {
                updateCount -= shake.TickDuration;

                progress = count / shake.ShakeDuration;
                progress = shake.IntensityCurve.Evaluate(progress);

                angles =
                    shakeDirection *
                    progress *
                    shake.ShakeIntensity *
                    (shakePositive == true ? 1 : -1);

                shakePositive = !shakePositive;
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(angles), shake.TickLerpSpeed);

            yield return null;
        }
    }

    //Static Events
    public static void OnPlayerEndHitFreezeFeedback(AttackData attack)
    {
        Instance.StartHitCameraShake(attack);
    }
}
