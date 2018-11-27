using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class EntityComponent 
{
    public virtual void Awake()
    {

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

#if UNITY_EDITOR
    public virtual void OnDrawGizmos()
    {

    }
#endif
}
