﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Component : EntityComponent
{
    protected P_References pRefs;
    protected P_PlayerController master;

    protected delegate void DebugLog(object item);
    protected DebugLog Print;

    public P_InputData IData { get { return master.IData; } }
    public P_MovementData MData { get { return master.MData; } }
    public P_AttackData AData { get { return master.AData; } }
    public P_CameraData CData { get { return master.CData; } }
    public P_VisualData VData { get { return master.VData; } }

    public Vector3 Position { get { return master.Position; } set { master.Position = value; } }
    public Quaternion Rotation { get { return master.Rotation; } set { master.Rotation = value; } }
    public Vector3 Velocity { get { return master.Velocity; } set { master.Velocity = value; } }

    public P_Component(P_References playerReferences, P_PlayerController master)
    {
        pRefs = playerReferences;
        this.master = master;
        Print = Debug.Log;
    }

}