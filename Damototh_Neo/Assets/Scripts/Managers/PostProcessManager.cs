using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class PostProcessManager : Singleton<PostProcessManager> 
{
    [Header("References")]
    [Space]
    [SerializeField] private PostProcessData _data;
    [SerializeField] private PostProcessVolume _lockedPP;
    [SerializeField] private PostProcessVolume _hitPP;

    private static bool _lockedPPActivated = false;

    private Coroutine _hitPostProcessCoroutine = null;

    private void Awake()
    {
        SetInstance(this);
    }

    private void Update() 
	{
        UpdateLockedPPWeight();

    }
    private void UpdateLockedPPWeight()
    {
        if (_lockedPPActivated == false)
        {
            _lockedPP.weight = Mathf.Lerp(_lockedPP.weight, 0f, _data.LockedPostProcessLerpSpeed);
        }
        else
        {
            _lockedPP.weight = Mathf.Lerp(_lockedPP.weight, 1f, _data.LockedPostProcessLerpSpeed);
        }
    }

    //Utilities
    private void StartHitPostProcess()
    {
        if (_hitPostProcessCoroutine != null)
        {
            StopCoroutine(_hitPostProcessCoroutine);
        }

        _hitPostProcessCoroutine = StartCoroutine(HitPostProcessCoroutine());
    }

    //Coroutines
    private IEnumerator HitPostProcessCoroutine()
    {
        float elapsedTime = 0f, progress = 0f;
        while (elapsedTime < _data.HitPostProcessActivateDuration)
        {
            elapsedTime += WorldData.DeltaTime;

            progress = elapsedTime / _data.HitPostProcessActivateDuration;
            progress = _data.HitPostProcessActivateCurve.Evaluate(progress);

            _hitPP.weight = progress;

            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < _data.HitPostProcessDeactivateDuration)
        {
            elapsedTime += WorldData.DeltaTime;

            progress = elapsedTime / _data.HitPostProcessDeactivateDuration;
            progress = _data.HitPostProcessDeactivateCurve.Evaluate(progress);

            _hitPP.weight = progress;

            yield return null;
        }

        _hitPP.weight = 0;
    }

    //Static events
    public static void OnPlayerLock()
    {
        _lockedPPActivated = true;
    }

    public static void OnPlayerUnlock()
    {
        _lockedPPActivated = false;
    }

    public static void OnPlayerHitEntity()
    {
        Instance.StartCoroutine(Instance.HitPostProcessCoroutine());
    }

    public static void OnPlayerKillEntity()
    {
        Instance.StartCoroutine(Instance.HitPostProcessCoroutine());
    }
}