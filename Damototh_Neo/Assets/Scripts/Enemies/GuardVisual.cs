using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GuardVisual : GuardComponent 
{
    public GuardVisual(GuardReferences refs, GuardController master) : base(refs, master) { }

    public override void MainUpdate()
    {
        UpdateVisualTransform();
    }

    private void UpdateVisualTransform()
    {
        Refs.VisualBody.position = Position;
        Refs.VisualBody.rotation = Refs.PhysicBody.rotation;
    }
}