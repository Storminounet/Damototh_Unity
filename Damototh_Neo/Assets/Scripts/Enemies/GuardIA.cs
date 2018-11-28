using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GuardIA : GuardComponent, IEntityIA
{
    public GuardIA(GuardReferences refs, GuardController master) : base(refs, master) { }

    private bool _hasTarget = false;
    private float _targetDistance = 0f;
    private float _targetSqrDistance = 0f;
    private EntityController _target = null;


    public float TargetDistance { get { return _targetDistance; } }
    public float TargetSqrDistance { get { return _targetSqrDistance; } }
    public EntityController Target { get { return _target; } }


    public override void MainUpdate()
    {
        if (_hasTarget == false)
        {

        }
        else
        {

        }
    }
    private void UpdateTargetInfos()
    {

    }

    public void OnTargetComeCloser()
    {
    }

    public void OnTargetEndAttack()
    {
    }

    public void OnTargetEndDodge()
    {
    }

    public void OnTargetGoFurther()
    {
    }

    public void OnTargetHitAttack(AttackData attack)
    {
    }

    public void OnTargetKillYourself(AttackData attack)
    {
    }

    public void OnTargetStartAttack(AttackData attack)
    {
    }

    public void OnTargetStartDodge()
    {
    }
}