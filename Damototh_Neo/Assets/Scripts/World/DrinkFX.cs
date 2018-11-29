using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class DrinkFX : MonoBehaviour 
{
    [SerializeField] private new ParticleSystem particleSystem;
    [SerializeField] private P_VisualData _VData;

    [SerializeField] private float velocityChangeSpeed = 1;
    [SerializeField] private float speedOverDistanceFactor = 1;
    [SerializeField] private AnimationCurve speedOverDistance;

    [SerializeField] private float repulsionStick = 1;
    [SerializeField] private float repulsionDistanceFactor = 1;
    [SerializeField] private AnimationCurve repulsionFromCenterOverDistance;

    [SerializeField] private float perturbations = 1;


    private bool _started = false;

    private float _emissionEndTime;

    private Transform _target;

    private ParticleSystem.Particle[] particles;


    private void Awake()
    {
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }

    private void Update()
    {
        if (_started == true)
        {
            ChangeVelocityTowardTarget();
        }
    }
    private void ChangeVelocityTowardTarget()
    {
        int length = particleSystem.GetParticles(particles);
        float distanceFromTarget;
        float distanceFromCenter;
        float expectedDistance;

        float distanceFromTargetProgress;
        float maxDist = Vector3.Distance(transform.position, _target.position);
        Vector3 vel = Vector3.zero;
        Vector3 localTargetPos;
        Vector3 localParticlePos;
        Vector3 fromCenterToParticle;
        Vector3 fromCenterToParticleNormalized;
        Vector3 expectedDeltaPos;

        for (int i = 0; i < length; i++)
        {
            vel.x = 0;
            vel.y = 0;
            vel.z = 0;
            localTargetPos = transform.InverseTransformPoint(_target.position);
            localParticlePos = transform.InverseTransformPoint(particles[i].position);

            distanceFromTarget =
                localTargetPos.z -
                localParticlePos.z;

            if (distanceFromTarget < 0.1f)
            {
                particles[i].position = Vector3.up * 100000000;
            }

            distanceFromTargetProgress = Mathf.Clamp01(1 - distanceFromTarget / maxDist);

            if (distanceFromTargetProgress > 0)
            {
                fromCenterToParticle = localParticlePos.SetZ(localTargetPos.z) - localTargetPos;
                distanceFromCenter = fromCenterToParticle.magnitude;
                fromCenterToParticleNormalized = fromCenterToParticle / distanceFromCenter;

                expectedDistance = repulsionFromCenterOverDistance.Evaluate(distanceFromTargetProgress) * repulsionDistanceFactor;

                expectedDeltaPos = (fromCenterToParticleNormalized * expectedDistance - fromCenterToParticle);

                vel += expectedDeltaPos * WorldData.DeltaTime * repulsionStick;
                vel.z = speedOverDistance.Evaluate(distanceFromTargetProgress) * speedOverDistanceFactor * distanceFromTarget;
                if (WorldData.Time > _emissionEndTime)
                {
                    vel.z += WorldData.Time - _emissionEndTime;
                }
                vel += fromCenterToParticleNormalized * Random.Range(-perturbations, perturbations) * WorldData.DeltaTime * vel.z;

                vel = transform.TransformVector(vel);
            }


            particles[i].velocity = Vector3.MoveTowards(particles[i].velocity, vel, velocityChangeSpeed);
        }

        particleSystem.SetParticles(particles, length);
    }

    public void StartFX(Transform target, bool insideCombat)
    {
        float time = insideCombat == true ? _VData.InsideCombatFXEmittDuration : _VData.OutsideCombatFXEmittDuration;

        Destroy(gameObject, time + 5f);
        Invoke("StopEmission", time);
        _target = target;
        _started = true;

        StartCoroutine(LookAtTargetCoroutine());
    }

    private IEnumerator LookAtTargetCoroutine()
    {
        while (true)
        {
            transform.LookAt(_target);
            yield return null;
        }
    }

    private void StopEmission()
    {
        _emissionEndTime = WorldData.Time;
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = false;
    }
}
