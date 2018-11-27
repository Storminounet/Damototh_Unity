using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GuardController : EntityController, IInteractable
{
#if UNITY_EDITOR
    [SerializeField] private bool _displayVisionCone = false;
#endif


    #region Refs fields
    private GuardReferences _gRefs;
    private GuardBeing _being;
    private GuardIA _IA;
    private GuardVisual _visual;

    public GuardReferences GRefs { get { return _gRefs; } }
    public GuardIAData IAData { get { return _gRefs.IAData; } }
    public GuardBeingData BeingData { get { return _gRefs.GuardBeingData; } }
    public GuardVisual Visual { get { return _visual; } }
    #endregion


    private bool _canBeInteracted = false;
    private InteractableType _interactableType = InteractableType.Corpse;

    public bool CanBeInteracted { get { return _canBeInteracted; } }
    public InteractableType InteractableType { get { return _interactableType; } }

    public Vector3 InteractPosition { get { return _gRefs.InteractTransform.position; } }

    protected override void Awake()
    {
        base.Awake();

        _gRefs = (GuardReferences)refs;

        _being = new GuardBeing(_gRefs, this);
        _IA = new GuardIA(_gRefs, this);
        _visual = new GuardVisual(_gRefs, this);

        AddComponent(_being);
        AddComponent(_IA);
        AddComponent(_visual);


        AwakeComponents();
    }

    public override void OnDeath()
    {
        _canBeInteracted = true;
    }

    public void Interact()
    {
        if (_canBeInteracted == false)
        {
            return;
        }

        _canBeInteracted = false;
        WorldManager.OnEntityDrank(this);
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        bool a = false;
        if (_gRefs == null)
        {
            base.Awake();

            _gRefs = (GuardReferences)refs;

            _being = new GuardBeing(_gRefs, this);
            _visual = new GuardVisual(_gRefs, this);

            AddComponent(_being);
            AddComponent(_visual);
            a = true;
        }
        if (_gRefs.GuardBeingData == null)
        {
            _gRefs.EditorAwake();
        }

        if (a)
        {
            AwakeComponents();
        }


        base.OnDrawGizmos();

        if (_displayVisionCone == true)
        {
            Gizmos.matrix = Matrix4x4.identity;
            DisplayVisionCone();
        }
    }

    void DisplayVisionCone()
    {
        float angleStep = 2;
        float radAngle;
        float progress;
        float angleDist;

        for (float currentAngle = 0; currentAngle < 360f; currentAngle += angleStep)
        {
            radAngle = (currentAngle + 90 - NativeYRotation - refs.VisualBody.localEulerAngles.y) * Mathf.Deg2Rad;

            Vector3 pos = new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle));

            angleDist = Mathf.DeltaAngle(0f, currentAngle).Abs();

            if (angleDist > IAData.VisionAngle * 0.5f)
            {
                progress = 1 - (angleDist - IAData.VisionAngle * 0.5f) / (180 - IAData.VisionAngle * 0.5f);
                progress = Mathf.Pow(progress, IAData.VisionAngleDistanceDecreaseLinearity);
            }
            else
            {
                progress = 1;
            }

            pos *= IAData.VisionMinDistance + (IAData.VisionDistance - IAData.VisionMinDistance) * progress;

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(Position + pos, Position + pos + Vector3.up);
        }
    }
#endif
}
