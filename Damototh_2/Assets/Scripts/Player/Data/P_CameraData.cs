using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "PlayerData/CameraData", order = 1000)]
public class P_CameraData : ScriptableObject
{

    [Header("Collision with Walls")]
    [Space]
    [SerializeField] private float _normalDisplacementFactor = 0.01f;
    [SerializeField] private float _directionDisplacementfactor = 0.01f;
    [Space]
    [SerializeField] private float _returnToPosSpeed = 1f;

    [Header("Auto Rotation")]
    [Space]
    [SerializeField, Range(0.05f, 1f)] private float _autoRotLerpSpeed;
    [SerializeField] private float _xRotTarget = 10f;
    [SerializeField] private float _unlockRotatingTime = 0.25f;

    [Header("Lock")]
    [Space]
    [SerializeField, Range(0.05f, 1f)] private float _lockLerpSpeed;
    [SerializeField] private float _horizontalOffset = 1f;


    public float NormalDisplacementFactor { get { return _normalDisplacementFactor; } }
    public float DirectionDisplacementfactor { get { return _directionDisplacementfactor; } }

    public float ReturnToPosSpeed { get { return _returnToPosSpeed; } }

    public float AutoRotLerpSpeed { get { return _autoRotLerpSpeed; } }
    public float XRotTarget { get { return _xRotTarget; } }
    public float UnlockRotatingTime { get { return _unlockRotatingTime; } }

    public float LockLerpSpeed { get { return _lockLerpSpeed; } }
    public float HorizontalOffset { get { return _horizontalOffset; } }
}
