using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Visual Data", menuName = "PlayerData/Visual Data", order = 1000)]
public class P_VisualData : ScriptableObject 
{
    [Header("Step Visual Smoothing")]
    [Space]
    [SerializeField] private float _maxDisplacement;
    [SerializeField] private float _displacementSpeed;
    [SerializeField] private float _dodgeDisplacementSpeed;
    [Header("Rotation")]
    [Space]
    [SerializeField] private float _rotationSpeed = 60;

    public float MaxDisplacement { get { return _maxDisplacement; } }
    public float DisplacementSpeed { get { return _displacementSpeed; } }
    public float DodgeDisplacementSpeed { get { return _dodgeDisplacementSpeed; } }

    public float RotationSpeed { get { return _rotationSpeed; } }

#if UNITY_EDITOR
    public void OnValidate()
    {
        
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(P_VisualData))]
public class P_VisualDataEditor : Editor
{
	private P_VisualData Instance;

    private void OnEnable()
    {
        Instance = (P_VisualData)target;
    }
}
#endif
