using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class EntityBeing : IEntityComponent
{
    [Header("Life")]
    [Space]
    [SerializeField] private float _maxLife = 10;
    [SerializeField] private float _startLife = 10;

    [Header("Hit Taking")]
    [Space]
    [SerializeField] private float _knockbackFactor = 1;
    [SerializeField] private float _stunResistance = 1;

    private float _currentLife = 0f;

    public virtual void Awake()
    {
        AddHealth(_startLife);
    }

    public virtual void Start()
    {
    }

    public virtual void MainFixedUpdate()
    {
    }

    public virtual void AfterFixedUpdate()
    {
    }

    public virtual void MainUpdate()
    {
    }

    public virtual void LateUpdate()
    {
    }


    //Utilities
    public void TakeHit(Attack attack)
    {
        
    }
    protected void AddHealth(float amount)
    {
        _currentLife = Mathf.Clamp(_currentLife + amount, 0f, _maxLife);
    }

#if UNITY_EDITOR
    public virtual void OnDrawGizmos()
    {
    }
#endif
}
