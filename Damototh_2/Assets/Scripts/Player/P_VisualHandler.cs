using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_VisualHandler : P_Component
{
    public P_VisualHandler(P_References playerReferences, P_PlayerController master) : base(playerReferences, master) { }

    private float _currentYOffset;
    private Quaternion _targetQuaternion;

    public Quaternion TargetQuaternion { get { return _targetQuaternion; } }


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
            _targetQuaternion = Quaternion.LookRotation(master.CameraController.ToLockedDirection.SetY(0));
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

    //Events
    public void OnFeetHeightChanged(float heightDifference)
    {
        _currentYOffset -= heightDifference;
        _currentYOffset = Mathf.Clamp(_currentYOffset, -VData.MaxDisplacement, VData.MaxDisplacement);
    }
}
