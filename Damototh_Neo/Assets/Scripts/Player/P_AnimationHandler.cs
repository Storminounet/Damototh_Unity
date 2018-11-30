using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class P_AnimationHandler : P_Component 
{
    public P_AnimationHandler(P_References playerReferences, P_PlayerController master) : base(playerReferences, master) { }

    private Animator Animator { get { return pRefs.Animator; } }
    private float _startFreezeTime;
    private float _animatorSpeed;

    private void PlayAnimation(AnimationClip animation, float time)
    {
        if (animation == null)
        {
            return;
        }

        _animatorSpeed = 1 / time;
        Animator.speed = _animatorSpeed;
        Animator.Play(animation.name);
    }

    //Events
    public void OnStartHitFreezeFeedback()
    {
        _startFreezeTime = WorldData.Time;
        Animator.speed = 0f;
    }
    public void OnEndHitFreezeFeedback()
    {
        /*float timeToAdd = 
        Animator.Play(Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, )*/
        Animator.speed = _animatorSpeed;
    }

    public void OnEnterCastState(AttackData attack)
    {
        PlayAnimation(attack.Animations.CastAnim, attack.Timings.CastTime);
    }

    public void OnEnterAttackState(AttackData attack)
    {
        PlayAnimation(attack.Animations.AttackAnim, attack.Timings.AttackTime);
    }

    public void OnEnterRecoverState(AttackData attack)
    {
        PlayAnimation(attack.Animations.ForcedRecoverAnim, attack.Timings.ForcedRecoverTime + attack.Timings.ComboRecoverTime);
    }
}
