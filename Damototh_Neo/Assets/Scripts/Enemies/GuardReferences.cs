﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GuardReferences : EntityReferences 
{
    [SerializeField] private GuardIAData _IAData;
    private GuardBeingData _guardBeingData;

    [Header("Ohters")]
    [Space]
    [SerializeField] private Transform _interactTransform;

    public GuardIAData IAData { get { return _IAData; } }
    public GuardBeingData GuardBeingData { get { return _guardBeingData; } }

    public Transform InteractTransform { get { return _interactTransform; } }

    private void Awake()
    {
        _guardBeingData = (GuardBeingData)BeingData;
    }

#if UNITY_EDITOR
    public void EditorAwake()
    {
        Awake();
    }
#endif
}
