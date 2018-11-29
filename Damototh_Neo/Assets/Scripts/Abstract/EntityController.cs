using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum EntityFaction
{
    Player,
    Prison,
    Evil
}

public class EntityController : MonoBehaviour
{
    [SerializeField] private string _entityName;
    [SerializeField] private EntityFaction _faction;

    private bool _hasIA = false;
    private Vector3 _oldPosition;
    private Vector3 _deltaPosition;

    private List<IEntityComponent> _components = new List<IEntityComponent>();

    private IEntityBeing _being = null;
    private IEntityIA _IA = null;

    protected EntityReferences refs;

    public bool HasIA { get { return _hasIA; } }
    public float NativeYRotation { get { return transform.localEulerAngles.y; } }

    public string Name { get { return _entityName; } }
    public EntityFaction Faction { get { return _faction; } }
    public LivingState LivingState { get { return Being.LivingState; } }

    public EntityReferences Refs { get { return refs; } }
    public EntityBeingData BData { get { return Refs.BeingData; } }

    public IEntityBeing Being { get { return _being; } }
    public IEntityIA IA { get { return _IA; } }

    //Utilities
    public Vector3 Position { get { return refs.PhysicBody.position; } set { refs.Rigidbody.MovePosition(value); } }
    public Vector3 DeltaPosition { get { return refs.PhysicBody.position; } set { refs.Rigidbody.MovePosition(value); } }
    public Quaternion Rotation { get { return refs.PhysicBody.rotation; } set { refs.Rigidbody.MoveRotation(value); } }
    public Vector3 Velocity { get { return refs.Rigidbody.velocity; } set { refs.Rigidbody.velocity = value; } }

    protected virtual void Awake()
    {
        refs = GetComponent<EntityReferences>();

        StartCoroutine(AfterFixedUpdateCoroutine());
    }

    protected virtual void AwakeComponents()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].Awake();
        }
    }

    protected virtual void Start()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].Start();
        }
    }

    protected virtual void FixedUpdate()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].MainFixedUpdate();
        }
    }

    private IEnumerator AfterFixedUpdateCoroutine()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            AfterFixedUpdate();
        }
    }

    protected virtual void AfterFixedUpdate()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].AfterFixedUpdate();
        }
    }

    protected virtual void Update()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].MainUpdate();
        }

        UpdateOldPosition();
    }

    protected virtual void LateUpdate()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].LateUpdate();
        }
    }

    protected virtual void UpdateOldPosition()
    {
        if (_oldPosition != Position)
        {
            _deltaPosition = Position - _oldPosition;
            _oldPosition = Position;
        }
    }

    //Utilities
    protected void AddComponent(IEntityComponent component)
    {
        if (component as IEntityBeing != null)
        {
            if (_being != null)
            {
                Debug.LogError("Two being detected");
            }
            _being = (IEntityBeing)component;
        }

        if (component as IEntityIA != null)
        {
            if (_IA != null)
            {
                Debug.LogError("Two IA detected");
            }

            _IA = (IEntityIA)component;
            _hasIA = true;

            IAManager.TellMasterIExist(_IA);
        }

        _components.Add(component);
    }

    //Events
    public virtual void OnAttackStart(AttackData attack)
    {

    }
    public virtual void TakeHit(EntityController attacker, AttackData attackReceived)
    {
        _being.TakeHit(attackReceived);
    }
    public virtual void OnHitSuccessful(EntityController victim, AttackData hitAttack)
    {
        if (victim.Being.LivingState == LivingState.Dead)
        {
            OnEntityKilled(victim, hitAttack);
        }
        else
        {
            OnEntityHit(victim, hitAttack);
        }
    }
    public virtual void OnEntityHit(EntityController hitEntity, AttackData hitAttack)
    {

    }
    public virtual void OnEntityKilled(EntityController killedEntity, AttackData killingAttack)
    {

    }
    public virtual void OnDeath()

    {

    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (IAManager.Instance == null)
        {
            FindObjectOfType<IAManager>().EditorForceAwake();
        }
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].OnDrawGizmos();
        }
        if (refs == null)
        {
            Awake();
        }
        if (WorldManager.Instance == null)
        {
            WorldManager.EditorForceAwake();
        }
        if (Being == null)
        {
            Awake();
        }

        if (WorldManager.SeeHealthBar == true)
        {
            Gizmos.color = Color.red;
            float offset = WorldManager.HealthBarSize * (BData.MaxHealth - Being.CurrentHealth) * 0.5f;
            Gizmos.matrix = Matrix4x4.TRS(Position + Vector3.up * 2.2f + (refs.VisualBody.right * offset), refs.VisualBody.rotation, new Vector3(WorldManager.HealthBarSize * Being.CurrentHealth, 0.1f, 0.01f));
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
        if (WorldManager.SeeStunBar == true)
        {
            Gizmos.color = Color.green;
            float offset = WorldManager.StunBarSize * (BData.MaxStunResistance - Being.CurrentStunResistance) * 0.5f;
            Gizmos.matrix = Matrix4x4.TRS(Position + Vector3.up * 2.05f + (refs.VisualBody.right * offset), refs.VisualBody.rotation, new Vector3(WorldManager.StunBarSize * Being.CurrentStunResistance, 0.1f, 0.01f));
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        }
    }
#endif
}
