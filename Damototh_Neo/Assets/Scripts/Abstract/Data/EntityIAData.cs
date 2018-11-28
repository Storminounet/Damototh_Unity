using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class EntityIAData : ScriptableObject 
{
    [Header("Vision")]
    [Space]
    [SerializeField] private float _visionDistance = 10;
    [SerializeField] private float _visionMinDistance = 1;
    [SerializeField] private float _visionAngle = 10;
    [SerializeField] private float _visionAngleDistanceDecreaseLinearity = 1;
    [SerializeField] private float _visionDistanceBackward = 1;

    public float VisionDistance { get { return _visionDistance; } }
    public float VisionMinDistance { get { return _visionMinDistance; } }
    public float VisionAngle { get { return _visionAngle; } }
    public float VisionAngleDistanceDecreaseLinearity { get { return _visionAngleDistanceDecreaseLinearity; } }
    public float VisionDistanceBackward { get { return _visionDistanceBackward; } }
}
