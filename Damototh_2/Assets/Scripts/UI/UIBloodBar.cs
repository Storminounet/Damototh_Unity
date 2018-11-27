using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class UIBloodBar : UIFillBar 
{
    [SerializeField] private P_PlayerController _player;

    protected override void Update()
    {
        base.Update();
        SetFill(_player.Being.CurrentHealth / _player.Being.MaxHealth);
    }
}
