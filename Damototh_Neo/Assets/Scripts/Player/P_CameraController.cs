using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_CameraController : P_Component
{
    public P_CameraController(P_References playerReferences, P_PlayerController master) : base(playerReferences, master) { }

    private bool _autoRotating = false;
    private bool _autoRotatingLock = false;
    private bool _askingLock = false;
    private bool _locked = false;
    private float _xRot = 0f;
    private float _yRot = 0f;
    private float _autoLookYRot = 0f;
    private float _lockYAngle = 0f;
    private float _lockXAngle = 0f;
    private float _targetFov = 0f;


    private Vector3 _toLockedDirection;

    private Transform _lockedTransform;
    private EntityController _lockedEntity;
    private List<Transform> _visibleEnemiesTransforms = new List<Transform>();
    private List<float> _visibleEnemiesXScreenPos = new List<float>();

    public bool AutoRotating { get { return _autoRotating; } }
    public bool AutoRotatingLock { get { return _autoRotatingLock; } }
    public bool AskingLock { get { return _askingLock; } }
    public bool Locked { get { return _locked; } }

    public Vector3 ToLockedDirection { get { return _toLockedDirection; } }

    public Transform LockedTransform { get { return _lockedTransform; } }

    public override void Awake()
    {
        _targetFov = CData.NormalFOV;
    }

    public override void MainUpdate()
    {
        if (master.CanPerformActions == true)
        {
            CheckLock();
            AskLock();
            CheckChangeLockTarget();

            if (_autoRotating == false && _locked == false)
            {
                CheckAutoRotate();
            }
        }

        if (_autoRotating == false && master.HitFreezed == false)
        {
            UpdateRotations();
        }

        ApplyRotations();
        ApplyFOV();
    }
    private void CheckLock()
    {
        if (master.LockDown == true)
        {
            if (_locked == false)
            {
                _askingLock = true;
            }
            else
            {
                Unlock();
            }
        }
        else if (master.LockUp == true)
        {
            _askingLock = false;
        }
    }
    private void AskLock()
    {
        if (_askingLock == true)
        {
            TryLock();
        }
    }

    private void CheckChangeLockTarget()
    {
        if (_locked == true)
        {
            ComputeVisibleEnemies();

            if (master.LockLeftTarget == true)
            {
                ComputeVisibleEnemies();
                LockLeftTarget();
            }
            else if (master.LockRightTarget == true)
            {
                ComputeVisibleEnemies();
                LockRightTarget();
            }
        }
    }
    private void UpdateRotations()
    {
        if (_locked == false)
        {
            _xRot -= master.LookInput.y;
            _yRot += master.LookInput.x;
        }
        else
        {
            ComputeLockedRotations();
            _yRot = Mathf.LerpAngle(_yRot, _lockYAngle, CData.LockLerpSpeed);
            _xRot = Mathf.LerpAngle(_xRot, _lockXAngle, CData.LockLerpSpeed);
        }
        

        _xRot = Mathf.Clamp(_xRot, -89f, 89f);
        if (_yRot > 360f)
        {
            _yRot -= 360f;
        }
        else if (_yRot < 0f)
        {
            _yRot += 360f;
        }
    }
    private void ComputeLockedRotations()
    {
        UpdateToLockedDirection();

        _lockYAngle = Vector3.SignedAngle(Vector3.forward, _toLockedDirection.SetY(0), Vector3.up) - master.NativeYRotation;
        _lockXAngle = Vector3.SignedAngle(Vector3.forward, (Vector3.forward.AddY(_toLockedDirection.y)), Vector3.right);
    }
    private void UpdateToLockedDirection()
    {
        pRefs.RotationCalculator.LookAt(_lockedTransform.position);

        _toLockedDirection =  
            ((_lockedTransform.position +
            pRefs.RotationCalculator.right * CData.HorizontalOffset +
            pRefs.RotationCalculator.up * CData.VerticalOffset) -
            pRefs.CamFollower.position).normalized;
    }
    private void ApplyRotations()
    {
        pRefs.CamXRotator.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
        pRefs.CamYRotator.localRotation = Quaternion.Euler(0f, _yRot, 0f);
    }
    private void ApplyFOV()
    {
        if (_askingLock == true || _locked == true)
        {
            _targetFov = CData.LockedFOV;
        }
        else
        {
            _targetFov = CData.NormalFOV;
        }

        pRefs.Camera.fieldOfView = Mathf.Lerp(pRefs.Camera.fieldOfView, _targetFov, CData.FOVChangeSpeed);
    }

    public override void LateUpdate()
    {
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        pRefs.CamFollower.position = pRefs.VisualBody.position + pRefs.CamTarget.localPosition;
    }
    private void CheckAutoRotate()
    {
        if (master.AutoRotate)
        {
            master.StartCoroutine(AutoRotateCoroutine());
            master.StartCoroutine(UnlockAutoRotatingCoroutine());
        }
    }

    //Coroutines
    private IEnumerator AutoRotateCoroutine()
    {
        _autoRotating = true;
        //Get auto look rot
        _autoLookYRot = Vector3.SignedAngle(Vector3.forward, pRefs.VisualBody.forward, Vector3.up) - master.NativeYRotation;

        while (Mathf.Abs(Mathf.DeltaAngle(_yRot, _autoLookYRot)) > 0.1f)
        {
            //Apply it
            _yRot = Mathf.LerpAngle(_yRot, _autoLookYRot, CData.AutoRotLerpSpeed);
            _xRot = Mathf.LerpAngle(_xRot, CData.XRotTarget, CData.AutoRotLerpSpeed);
            yield return null;
        }
        _yRot = _autoLookYRot;
        _xRot = CData.XRotTarget;

        _autoRotating = false;
    }
    private IEnumerator UnlockAutoRotatingCoroutine()
    {
        _autoRotatingLock = true;
        yield return new WaitForSeconds(CData.UnlockRotatingTime);
        _autoRotatingLock = false;
    }


    //utilities
    public void TryLock()
    {
        if (IAManager.Enemies.Count == 0)
        {
            return;
        }
        ComputeVisibleEnemies();
        EntityController temp = GetNearestEnemyFromScreenCenter();

        if (temp != null)
        {
            Lock(temp);
        }
    }
    public void Lock(EntityController entity)
    {
        _lockedEntity = entity;
        _lockedTransform = _lockedEntity.Refs.PhysicBody;
        _locked = true;
        _askingLock = false;

        WorldManager.OnPlayerLock(_lockedEntity);
    }
    public void Unlock()
    {
        _locked = false;
        _lockedTransform = null;

        OnUnlock();
    }
    private void LockLeftTarget()
    {
        int index = _visibleEnemiesTransforms.IndexOf(_lockedTransform);
        index--;
        if (index < 0)
        {
            //index = _visibleEnemiesTransforms.Count - 1;
            index = 0;
        }

        Lock(_visibleEnemiesTransforms[index].GetComponentInParent<EntityController>());
    }
    private void LockRightTarget()
    {
        int index = _visibleEnemiesTransforms.IndexOf(_lockedTransform);
        index++;
        if (index >= _visibleEnemiesTransforms.Count)
        {
            //index = 0;
            index = _visibleEnemiesTransforms.Count - 1;
        }

        Lock(_visibleEnemiesTransforms[index].GetComponentInParent<EntityController>());
    }

    private Vector3 GetCamTargetPos()
    {
        return Position + pRefs.CamTarget.localPosition;
    }
    private void ComputeVisibleEnemies()
    {
        _visibleEnemiesTransforms.Clear();
        _visibleEnemiesXScreenPos.Clear();
        Vector2 screenPos;
        Vector3 localPos;
        
        for (int i = 0; i < IAManager.Enemies.Count; i++)
        {
            localPos = pRefs.CamYRotator.InverseTransformPoint(IAManager.Enemies[i].Position);

            if (localPos.z <= 0f)
            {
                continue;
            }
            else
            {
                screenPos = pRefs.Camera.WorldToScreenPoint(IAManager.Enemies[i].Position);
                if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
                {
                    continue;
                }

                if (_visibleEnemiesTransforms.Count == 0)
                {
                    _visibleEnemiesTransforms.Add(IAManager.Enemies[i].Refs.PhysicBody);
                    _visibleEnemiesXScreenPos.Add(screenPos.x);
                }
                else
                {
                    for (int j = 0; j < _visibleEnemiesTransforms.Count; j++)
                    {
                        if (_visibleEnemiesXScreenPos[j] > screenPos.x)
                        {
                            _visibleEnemiesTransforms.Insert(j, IAManager.Enemies[i].Refs.PhysicBody);
                            _visibleEnemiesXScreenPos.Insert(j, screenPos.x);
                            break;
                        }
                        else if (j == _visibleEnemiesTransforms.Count - 1)
                        {
                            _visibleEnemiesTransforms.Insert(j + 1, IAManager.Enemies[i].Refs.PhysicBody);
                            _visibleEnemiesXScreenPos.Insert(j + 1, screenPos.x);
                            break;
                        }
                    }
                }
            }
        }
    }
    private EntityController GetNearestEnemyFromScreenCenter()
    {
        if (_visibleEnemiesTransforms.Count == 0)
        {
            return null;
        }

        int index = 0;
        float minDist = Mathf.Abs(_visibleEnemiesXScreenPos[index] - Screen.width * 0.5f);
        float curDist;
        for (int i = 1; i < _visibleEnemiesTransforms.Count; i++)
        {
            curDist = Mathf.Abs(_visibleEnemiesXScreenPos[i] - Screen.width * 0.5f);
            if (curDist < minDist)
            {
                minDist = curDist;
                index = i;
            }
        }

        return _visibleEnemiesTransforms[index].GetComponentInParent<EntityController>();
    }

    //Events
    public void OnEntityKilled(EntityController killedEntity, AttackData killingAttack)
    {
        if (_locked == true && killedEntity == _lockedEntity)
        {
            Unlock();
            TryLock();
        }
    }
    public void OnDeath()
    {
        if (_locked == true)
        {
            Unlock();
        }
    }
    private void OnUnlock()
    {
        master.OnCameraUnlock();
    }
}
