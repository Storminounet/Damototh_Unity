using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GuardVisual : GuardComponent 
{
    public GuardVisual(GuardReferences refs, GuardController master) : base(refs, master) { }

    private bool _isDeadAndDepleted = false;
    private Material[] _transparentMats;

    public override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        if (EditorApplication.isPlaying == false)
        {
            return;
        }
#endif
    }

    public override void MainUpdate()
    {
        UpdateVisualTransform();

        if (_isDeadAndDepleted == true)
        {
            UpdateVisualTransparency();
        }
    }

    private void UpdateVisualTransform()
    {
        Refs.VisualBody.position = Position;
        if (Master.LivingState != LivingState.Dead)
        {
            Refs.VisualBody.rotation = Refs.PhysicBody.rotation;
        }
        else
        {
            Refs.VisualBody.rotation = Refs.Collision.transform.rotation;
        }
    }

    public void OnDrunk()
    {
        _isDeadAndDepleted = true;
        MeshRenderer[] renderers = Refs.VisualBody.GetComponentsInChildren<MeshRenderer>();
        _transparentMats = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            _transparentMats[i] = new Material(WorldData.TransparentMat);
            _transparentMats[i].color = renderers[i].material.color;
            _transparentMats[i].SetTexture("_MainTex", renderers[i].material.GetTexture("_MainTex"));
            _transparentMats[i].SetFloat("_Metallic", renderers[i].material.GetFloat("_Metallic"));
            _transparentMats[i].SetFloat("_Glossiness", renderers[i].material.GetFloat("_Glossiness"));

            renderers[i].material = _transparentMats[i];
        }
    }

    private void UpdateVisualTransparency()
    {
        for (int i = 0; i < _transparentMats.Length; i++)
        {
            _transparentMats[i].color = _transparentMats[i].color.SetA(Mathf.MoveTowards(_transparentMats[i].color.a, 0f, WorldData.DeltaTime * 0.2f));
        }
    }
}