using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class IAManager : Singleton<IAManager> 
{
    [Header("Enemies")]
    [SerializeField] private List<EntityController> _enemies;
    [SerializeField] private List<EntityController> _IAEnemies;

    [Header("Utilities")]
    [Space]
    [SerializeField] private Transform _deltaPosCalculator;

    private static Transform s_DeltaPosCalculator { get { return Instance._deltaPosCalculator; } }

    public static List<EntityController> Enemies { get { return Instance._enemies; } }
    public static List<EntityController> IAEnemies { get { return Instance._IAEnemies; } }

    private void Awake()
    {
        SetInstance(this);
    }

    public static void TellMasterIExist(IEntityIA IA)
    {
        IAEnemies.Add(IA.EntityMaster);
    }

    public static void OnEntityDeath(EntityController entity)
    {
        if (entity.Faction != EntityFaction.Player)
        {
            if (IAEnemies.Contains(entity))
            {
                IAEnemies.Remove(entity);
                Enemies.Remove(entity);
            }
            else if (Enemies.Contains(entity))
            {
                Enemies.Remove(entity);
            }
        }
    }
    public static void OnEntityStartDodging(EntityController entity)
    {
        for (int i = 0; i < IAEnemies.Count; i++)
        {
            if (IAEnemies[i].IA.Target == entity)
            {
                IAEnemies[i].IA.OnTargetStartDodge();
            }
        }
    }
    public static void OnEntityEndDodging(EntityController entity)
    {
        for (int i = 0; i < IAEnemies.Count; i++)
        {
            if (IAEnemies[i].IA.Target == entity)
            {
                IAEnemies[i].IA.OnTargetEndDodge();
            }
        }
    }

    public static void OnEntityStartAttacking(EntityController entity, AttackData attack)
    {
        for (int i = 0; i < IAEnemies.Count; i++)
        {
            if (IAEnemies[i].IA.Target == entity)
            {
                IAEnemies[i].IA.OnTargetStartAttack(attack);
            }
        }
    }
    public static void OnEntityEndAttacking(EntityController entity)
    {
        for (int i = 0; i < IAEnemies.Count; i++)
        {
            if (IAEnemies[i].IA.Target == entity)
            {
                IAEnemies[i].IA.OnTargetEndAttack();
            }
        }
    }
    public static void OnEntityHit(EntityController attackingEntity, EntityController hitEntity, AttackData hitAttack)
    {
        if (hitEntity.HasIA == false)
        {
            return;
        }

        hitEntity.IA.OnTargetHitAttack(hitAttack);
    }
    public static void OnEntityKill(EntityController killingEntity, EntityController killedEntity, AttackData killingAttack)
    {
        if (killedEntity.HasIA == false)
        {
            return;
        }

        killedEntity.IA.OnTargetKillYourself(killingAttack);
    }

#if UNITY_EDITOR
    public void EditorForceAwake()
    {
        Awake();
    }
#endif
}