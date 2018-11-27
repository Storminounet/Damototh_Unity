using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Component : IEntityComponent
{
    private DebugLog _print;
    protected delegate void DebugLog(object item);
    protected DebugLog Print { get { return _print; } }

    protected P_References pRefs;
    protected P_PlayerController master;

    public P_BeingData BData { get { return master.BData; } }
    public P_InputData IData { get { return master.IData; } }
    public P_MovementData MData { get { return master.MData; } }
    public P_AttackData AData { get { return master.AData; } }
    public P_CameraData CData { get { return master.CData; } }
    public P_VisualData VData { get { return master.VData; } }

    public Vector3 Position { get { return master.Position; } set { master.Position = value; } }
    public Quaternion Rotation { get { return master.Rotation; } set { master.Rotation = value; } }
    public Vector3 Velocity { get { return master.Velocity; } set { master.Velocity = value; } }

    public EntityController EntityMaster { get { return master; } }

    public P_Component(P_References playerReferences, P_PlayerController master)
    {
        pRefs = playerReferences;
        this.master = master;

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
