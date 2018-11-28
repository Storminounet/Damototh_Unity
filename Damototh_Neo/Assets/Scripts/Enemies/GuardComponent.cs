using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GuardComponent : IEntityComponent
{
    private DebugLog _print;
    protected delegate void DebugLog(object item);

    protected DebugLog Print { get { return _print; } }

    private GuardReferences _refs;
    private GuardController _master;

    public GuardReferences Refs { get { return _refs; } }
    public GuardController Master { get { return _master; } }
    public EntityController EntityMaster { get { return _master; } }

    public GuardIAData IAData { get { return _master.IAData; } }
    public GuardBeingData BData { get { return _master.BeingData; } }

    //Utilities
    public Vector3 Position { get { return _master.Position; } set { _master.Position = value; } }
    public Quaternion Rotation { get { return _master.Rotation; } set { _master.Rotation = value; } }
    public Vector3 Velocity { get { return _master.Velocity; } set { _master.Velocity = value; } }

    public GuardComponent(GuardReferences refs, GuardController master)
    {
        _refs = refs;
        _master = master;
        _print = Debug.Log;
    }

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
    }

    public virtual void MainFixedUpdate()
    {
    }

    public virtual void AfterFixedUpdate()
    {
    }

    public virtual void MainUpdate()
    {
    }

    public virtual void LateUpdate()
    {
    }

#if UNITY_EDITOR
    public virtual void OnDrawGizmos()
    {
    }
#endif

}