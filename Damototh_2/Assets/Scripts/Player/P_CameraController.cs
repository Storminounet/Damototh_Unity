using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_CameraController : P_Component
{
    public P_CameraController(P_References playerReferences, P_PlayerController master) : base(playerReferences, master) { }

    private bool _autoRotating = false;
    private bool _autoRotatingLock = false;
    private bool _locked = false;
    private float _xRot = 0f;
    private float _yRot = 0f;
    private float _autoLookYRot = 0f;
    private float _lockYAngle = 0f;
    private float _lockXAngle = 0f;

    private Vector3 _toLockedDirection;

    private Transform _lockedTransform;
    private List<Transform> _visibleEnemiesTransforms = new List<Transform>();
    private List<float> _visibleEnemiesXScreenPos = new List<float>();

    public bool AutoRotating { get { return _autoRotating; } }
    public bool AutoRotatingLock { get { return _autoRotatingLock; } }
    public bool Locked { get { return _locked; } }

    public Vector3 ToLockedDirection { get { return _toLockedDirection; } }

    public Transform LockedTransform { get { return _lockedTransform; } }


    public override void MainUpdate()
    {
        CheckLock();
        CheckChangeLockTarget();

        if (_autoRotating == false)
        {
            UpdateRotations();
        }

        ApplyRotations();
    }

    private void CheckLock()
    {
        if (master.Lock == true)
        {
            if (_locked == false)
            {
                Lock();
            }
            else
            {
                Unlock();
            }
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

        _lockYAngle = Vector3.SignedAngle(Vector3.forward, _toLockedDirection.SetY(0), Vector3.up);
        _lockXAngle = Vector3.SignedAngle(Vector3.forward, (Vector3.forward.AddY(_toLockedDirection.y)), Vector3.right);
    }
    private void UpdateToLockedDirection()
    {
        pRefs.RotationCalculator.LookAt(_lockedTransform.position);

        _toLockedDirection =  
            ((_lockedTransform.position +
            pRefs.RotationCalculator.right * CData.HorizontalOffset) -
            pRefs.CamFollower.position).normalized;
    }
    private void ApplyRotations()
    {
        pRefs.CamXRotator.localRotation = Quaternion.Euler(_xRot, 0f, 0f);
        pRefs.CamYRotator.localRotation = Quaternion.Euler(0f, _yRot, 0f);
    }


    public override void LateUpdate()
    {
        if (_autoRotating == false && _locked == false)
        {
            CheckAutoRotate();
        }

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


    //utilities
    public void Lock()
    {
        if (WorldManager.Enemies.Count == 0)
        {
            return;
        }

        ComputeVisibleEnemies();
        _lockedTransform = GetNearestEnemyFromScreenCenter();

        if (_lockedTransform != null)
        {
            _locked = true;
        }
    }
    public void Unlock()
    {
        _locked = false;
        _lockedTransform = null;
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

        _lockedTransform = _visibleEnemiesTransforms[index];
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

        _lockedTransform = _visibleEnemiesTransforms[index];
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
        
        for (int i = 0; i < WorldManager.Enemies.Count; i++)
        {
            localPos = pRefs.CamYRotator.InverseTransformPoint(WorldManager.Enemies[i].position);

            if (localPos.z <= 0f)
            {
                continue;
            }
            else
            {
                screenPos = pRefs.Camera.WorldToScreenPoint(WorldManager.Enemies[i].position);
                if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
                {
                    continue;
                }

                if (_visibleEnemiesTransforms.Count == 0)
                {
                    _visibleEnemiesTransforms.Add(WorldManager.Enemies[i]);
                    _visibleEnemiesXScreenPos.Add(screenPos.x);
                }
                else
                {
                    for (int j = 0; j < _visibleEnemiesTransforms.Count; j++)
                    {
                        if (_visibleEnemiesXScreenPos[j] > screenPos.x)
                        {
                            _visibleEnemiesTransforms.Insert(j, WorldManager.Enemies[i]);
                            _visibleEnemiesXScreenPos.Insert(j, screenPos.x);
                            break;
                        }
                        else if (j == _visibleEnemiesTransforms.Count - 1)
                        {
                            _visibleEnemiesTransforms.Insert(j + 1, WorldManager.Enemies[i]);
                            _visibleEnemiesXScreenPos.Insert(j + 1, screenPos.x);
                            break;
                        }
                    }
                }
            }
        }
    }
    private Transform GetNearestEnemyFromScreenCenter()
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

        return _visibleEnemiesTransforms[index];
    }

    private IEnumerator AutoRotateCoroutine()
    {
        _autoRotating = true;
        //Get auto look rot
        _autoLookYRot = Vector3.SignedAngle(Vector3.forward, pRefs.VisualBody.forward, Vector3.up);

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

}
