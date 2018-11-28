using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public interface IEntityIA : IEntityComponent 
{
    EntityController Target { get; }
    float TargetDistance { get; }
    float TargetSqrDistance { get; }

    void OnTargetStartAttack(AttackData attack);
    void OnTargetEndAttack();
    void OnTargetHitAttack(AttackData attack);
    void OnTargetKillYourself(AttackData attack);
    void OnTargetComeCloser();
    void OnTargetGoFurther();
    void OnTargetStartDodge();
    void OnTargetEndDodge();
}