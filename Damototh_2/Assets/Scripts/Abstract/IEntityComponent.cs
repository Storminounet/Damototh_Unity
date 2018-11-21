using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public interface IEntityComponent 
{
    void Awake();
    void Start();
    void MainFixedUpdate();
    void AfterFixedUpdate();
    void MainUpdate();
    void LateUpdate();

#if UNITY_EDITOR
    void OnDrawGizmos();
#endif
}
