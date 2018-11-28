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
    [SerializeField] private string _name;
    [SerializeField] private EntityFaction _faction;

    private List<EntityComponent> _components = new List<EntityComponent>();

    protected EntityReferences refs;

    public string Name { get { return _name; } }
    public EntityFaction Faction { get { return _faction; } }

    public EntityReferences Refs { get { return refs; } }

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
    }

    protected virtual void LateUpdate()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            _components[i].LateUpdate();
        }
    }

    //Utilities
    protected void AddComponent(EntityComponent component)
    {
        _components.Add(component);
    }

    //Events
    public virtual void TakeHit(AttackData attackReceived)
    {

    }
}
