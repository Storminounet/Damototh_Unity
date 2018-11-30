using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_VisualHandler : P_Component
{
    public P_VisualHandler(P_References playerReferences, P_PlayerController master) : base(playerReferences, master) { }

    private float _currentYOffset;
    private Quaternion _targetQuaternion;
    private Vector3 _toLockedVector;

    public Quaternion TargetQuaternion { get { return _targetQuaternion; } }
    public Vector3 ToLockedVector { get { return _toLockedVector; } }

    public override void MainUpdate()
    {
        UpdateYOffset();
        UpdateVisualPosition();

        if (master.CameraController.AutoRotatingLock == false)
        {
            UpdateVisualRotation();
        }
    }
    private void UpdateYOffset()
    {
        _currentYOffset = Mathf.MoveTowards(_currentYOffset, 0f, GetCurrentDisplacementSpeed() * WorldData.DeltaTime);
    }
    private void UpdateVisualPosition()
    {
        pRefs.VisualBody.position = Position + Vector3.up * _currentYOffset;
    }
    private void UpdateVisualRotation()
    {
        if (master.CameraController.Locked == false)
        {
            _targetQuaternion = Quaternion.LookRotation(master.MovementController.LastMoveDirection);
        }
        else
        {
            _toLockedVector = (master.CameraController.LockedTransform.position - pRefs.VisualBody.position).SetY(0);
            _targetQuaternion = Quaternion.LookRotation(_toLockedVector);
        }

        pRefs.VisualBody.rotation = Quaternion.RotateTowards(
            pRefs.VisualBody.rotation,
            _targetQuaternion,
            VData.RotationSpeed * WorldData.DeltaTime);
    }


    //Utilities
    private float GetCurrentDisplacementSpeed()
    {
        if (master.MovementController.MovingState == MovingState.Dodging)
        {
            return VData.DodgeDisplacementSpeed;
        }
        else
        {
            return VData.DisplacementSpeed;
        }
    }

    private void SpawnDrinkFX()
    {
        Transform parent;
        if (master.InteractionController.SelectedEntity == null)
        {
            parent = master.InteractionController.SelectedInteractable.mono.transform;
        }
        else
        {
            parent = master.InteractionController.SelectedEntity.Refs.Collision.transform;
        }

        DrinkFX fx = Object.Instantiate(
            VData.DrinkBloodFXModel,
            ((IDrinkable)master.InteractionController.SelectedInteractable).DrinkPosition,
            Quaternion.identity,
            parent).GetComponent<DrinkFX>();


        fx.StartFX(pRefs.DrinkFXTarget, master.IsInCombat);
    }

    //Events
    public void OnFeetHeightChanged(float heightDifference)
    {
        _currentYOffset -= heightDifference;
        _currentYOffset = Mathf.Clamp(_currentYOffset, -VData.MaxDisplacement, VData.MaxDisplacement);
    }

    public void OnDrink()
    {
        SpawnDrinkFX();
    }
}
