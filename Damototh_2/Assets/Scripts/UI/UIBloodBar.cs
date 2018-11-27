using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class UIBloodBar : UIFillBar 
{
    protected override void Update()
    {
        base.Update();
        SetFill(WorldManager.Player.Being.CurrentHealth / WorldManager.Player.Being.MaxHealth);
    }
}
