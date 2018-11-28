using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Attack : MonoBehaviour 
{
    [SerializeField] private Transform _hitboxTransform;
    [SerializeField] private BoxCollider _hitbox;

    private bool _canAttack = true;
    private int _currentLife;

    private EntityController _owner;
    private AttackData _linkedData;

    public void Initialize(EntityController owner, AttackData data)
    {
        _owner = owner;
        _linkedData = data;

        _currentLife = data.MaximumEnemyNumberHit;

        if (_currentLife < 1)
        {
            _currentLife = 1;
        }

        Destroy(gameObject, data.Timings.AttackTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_canAttack == false)
        {
            return;
        }

        EntityController entity = other.transform.GetComponentInParent<EntityController>();

        if (entity != null)
        {
            if (entity.Faction == _owner.Faction)
            {
                return;
            }

            entity.TakeHit(_owner, _linkedData);
            _owner.OnHitSuccessful(entity, _linkedData);

            _currentLife--;
            if (_currentLife <= 0)
            {
                _canAttack = false;
                Destroy(gameObject);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (WorldManager.SeeHitboxes == false)
        {
            return;
        }
        if (_hitboxTransform == null)
        {
            return;
        }

        Gizmos.matrix = Matrix4x4.TRS(_hitboxTransform.position, _hitboxTransform.rotation, _hitbox.size);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
#endif
}

