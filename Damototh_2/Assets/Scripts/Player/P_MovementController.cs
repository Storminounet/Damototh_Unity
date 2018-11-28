using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingState
{
    Idle,
    Walking,
    Running,
    Sprinting,
    Dodging,
    InAir,
    VelocityOverriden
}

public class P_MovementController : P_Component
{
    public P_MovementController(P_References playerReferences, P_PlayerController master) : base(playerReferences, master) { }

    private bool _grounded;
    private bool _askingSprint;
    private bool _canInputMoveInAnyWay = true;

    private float _groundCheckRayDistance;
    private float _velocityMagnitude;
    private float _currentVelocityMagnitudeLimit;
    private float _currentGroundAngle;
    private float _currentFeetHeight;
    private float _currentClimbSpeedFactor = 1f;
    private float _currentFeetHeightChangedSpeedFactor = 1f;
    private float _currentSpeedFactor = 1f;

    private Vector3 _moveVector;
    private Vector3 _moveDirection;
    private Vector3 _lastMoveDirection;
    private Vector3 _sbSpaceVelocity;
    private Vector3 _velocityDirection;
    private Vector3 _currentGroundNormal;

    private Ray _groundCheckRay;
    private RaycastHit _groundRayCheckHit;

    private MovingState _movingState = MovingState.Idle;

    private Coroutine _velocityOverrideCoroutine = null;

    public bool Grounded { get { return _grounded; } }

    public Vector3 MoveVector { get { return _moveVector; } }
    public Vector3 MoveDirection { get { return _moveDirection; } }
    public Vector3 LastMoveDirection { get { return _lastMoveDirection; } }

    public MovingState MovingState { get { return _movingState; } }

    public override void Awake()
    {
        _lastMoveDirection = Vector3.forward;
    }

    public override void MainFixedUpdate()
    {
        if (_canInputMoveInAnyWay == true)
        {
            if (_grounded == true && master.InputingMovement == true)
            {
                RotateVelocitySpace();
                ComputeSideBrake();
                ApplySideBrake();
            }

            ComputeVelocityInfos();

            if (master.InputingMovement == true)
            {
                ApplyMoveVector();
            }
        }

        if (_grounded == true)
        {
            ApplyBrake();
        }

    }
    private void RotateVelocitySpace()
    {
        pRefs.VelocitySpace.rotation = Quaternion.LookRotation(_moveVector);
    }
    private void ComputeSideBrake()
    {
        _sbSpaceVelocity = pRefs.VelocitySpace.InverseTransformDirection(Velocity);
        _sbSpaceVelocity.x = _sbSpaceVelocity.x.SubtractAbsolute(MData.SideBrake * WorldData.FixedDeltaTime);
    }
    private void ApplySideBrake()
    {
        Velocity = pRefs.VelocitySpace.TransformDirection(_sbSpaceVelocity);
    }
    private void ComputeVelocityInfos()
    {
        _velocityMagnitude = Velocity.magnitude;
        _velocityDirection = Velocity / _velocityMagnitude;

        switch (_movingState)
        {
            case MovingState.Idle:
                _currentVelocityMagnitudeLimit = MData.WalkMaxSpeed;
                break;
            case MovingState.Walking:
                _currentVelocityMagnitudeLimit = MData.WalkMaxSpeed;
                break;
            case MovingState.Running:
                _currentVelocityMagnitudeLimit = MData.RunMaxSpeed;
                break;
            case MovingState.Sprinting:
                _currentVelocityMagnitudeLimit = MData.SprintMaxSpeed;
                break;
            case MovingState.InAir:
                _currentVelocityMagnitudeLimit = Mathf.Infinity;
                break;
        }

        if (_currentClimbSpeedFactor < 1f)
        {
            _currentVelocityMagnitudeLimit *= _currentClimbSpeedFactor;
        }
        else
        {
            _currentVelocityMagnitudeLimit *= _currentFeetHeightChangedSpeedFactor;
        }
    }
    private void ApplyMoveVector()
    {
        if (_velocityMagnitude > _currentVelocityMagnitudeLimit)
        {
            ApplyVelocityLimits();
        }
        else
        {
            Velocity += MoveVector;
        }
    }
    private void ApplyVelocityLimits()
    {
        _velocityMagnitude = Mathf.MoveTowards(_velocityMagnitude, _currentVelocityMagnitudeLimit, MData.AutoBrake);
        Velocity = _velocityDirection * _velocityMagnitude;
    }
    private void ApplyBrake()
    {
        Velocity = Velocity.SubtractMagnitude(MData.AutoBrake * WorldData.FixedDeltaTime);
    }

    public override void AfterFixedUpdate()
    {
        ComputeGrounded();

        if (_grounded == true)
        {
            StickToGround();
        }
    }
    private void ComputeGrounded()
    {
        _groundCheckRay = GetGroundCheckRay(out _groundCheckRayDistance);

        List<RaycastHit> hits = new List<RaycastHit>(Physics.SphereCastAll(
            _groundCheckRay,
            MData.SphereCastRadius,
            _groundCheckRayDistance,
            WorldData.DefaultSolidLayer));

        if (hits.Count > 0)
        {
            int index = GetNearestHitIndex(hits);
            bool canClimb = false;
            float groundDifference = hits[index].point.y - _currentFeetHeight;

            if (_grounded == true)
            {
                if (groundDifference < 0)
                {
                    canClimb = groundDifference >= -MData.StepDownHeight;
                }
                else
                {
                    canClimb = groundDifference <= MData.StepUpHeight;
                }
            }
            else
            {
                canClimb = true;
            }

            float hitNormalAngle = (ComputeHitNormalAngle(hits[index], out _groundRayCheckHit));
            while (canClimb == false || hitNormalAngle > MData.ClimbMaxAngle)
            {
                hits.RemoveAt(index);
                
                if (hits.Count <= 0)
                {
                    break;
                }

                index = GetNearestHitIndex(hits);
                groundDifference = hits[index].point.y - _currentFeetHeight;
                if (groundDifference < 0)
                {
                    canClimb = groundDifference > -MData.StepDownHeight;
                }
                else
                {
                    canClimb = groundDifference < MData.StepUpHeight;
                }
                hitNormalAngle = (ComputeHitNormalAngle(hits[index], out _groundRayCheckHit));
            }
            
            if (hits.Count > 0)
            {
                ComputeGroundInfos();
                UpdateGrounded(true);
            }
            else
            {
                UpdateGrounded(false);
            }
        }
        else
        {
            UpdateGrounded(false);
        }
    }
    private void UpdateGrounded(bool grounded)
    {
        if (_grounded == !grounded)
        {
            if (grounded == true)
            {
                OnGroundEnter();
            }
            else
            {
                OnGroundLeave();
            }
        }
        _grounded = grounded;
    }
    private void ComputeGroundInfos()
    {
        //Store normal
        _currentGroundNormal = _groundRayCheckHit.normal;
        _currentGroundAngle = ComputeNormalAngle(_currentGroundNormal);
        _currentClimbSpeedFactor = _currentGroundNormal.y;
    }
    private void StickToGround()
    {
        Position = Position.SetY(_groundRayCheckHit.point.y);
    }

    public override void MainUpdate()
    {
        if (_canInputMoveInAnyWay == true)
        {
            UpdateMovingState();
        }
        UpdateFeetHeightSpeedFactor();
        UpdateCurrentSpeedFactor();

        if (_canInputMoveInAnyWay == true)
        {
            ComputeMoveVector();
        }


        UpdateFeetHeight();

        if (_grounded == true)
        {
            CheckForSprint();
            CheckForDodge();
        }

        if (_askingSprint == true)
        {
            CheckForSprintStop();
        }
    }
    private void UpdateMovingState()
    {
        if (_grounded == false)
        {
            _movingState = MovingState.InAir;
        }
        else
        {
            if (_askingSprint == true)
            {
                _movingState = MovingState.Sprinting;
            }
            else if (master.MoveInputMagntiude < MData.WalkInputThreshold)
            {
                _movingState = MovingState.Idle;
            }
            else if (master.MoveInputMagntiude < MData.RunInputThreshold)
            {
                _movingState = MovingState.Walking;
            }
            else
            {
                _movingState = MovingState.Running;
            }
        }
    }
    private void UpdateFeetHeightSpeedFactor()
    {
        _currentFeetHeightChangedSpeedFactor = Mathf.MoveTowards(_currentFeetHeightChangedSpeedFactor, 1f, WorldData.DeltaTime * 0.5f);
    }
    private void UpdateCurrentSpeedFactor()
    {
        if (_currentClimbSpeedFactor < 1f)
        {
            _currentSpeedFactor = _currentClimbSpeedFactor;
        }
        else
        {
            _currentSpeedFactor = _currentFeetHeightChangedSpeedFactor;
        }
    }
    private void ComputeMoveVector()
    {
        if (_grounded == true && master.InputingMovement == true)
        {

            if (master.CameraController.AutoRotatingLock == false)
            {
                _moveVector =
                pRefs.CamYRotator.forward * master.MoveInputNormalized.y +
                pRefs.CamYRotator.right * master.MoveInputNormalized.x;
            }
            else
            {
                _moveVector = _lastMoveDirection * master.MoveInputMagntiude;
            }

            _moveVector *= _currentSpeedFactor;
            _moveDirection = _moveVector.normalized;
            _lastMoveDirection = _moveDirection;
        }
        else
        {
            _moveVector.x = 0;
            _moveVector.y = 0;
            _moveVector.z = 0;

            _moveDirection = _moveVector;
        }
    }
    private void UpdateFeetHeight()
    {
        if (_grounded == true)
        {
            if (_currentFeetHeight != Position.y)
            {
                OnFeetHeightChanged(Position.y - _currentFeetHeight);
                _currentFeetHeight = Position.y;
            }
        }
        else
        {
            _currentFeetHeight = Position.y;
        }
        
    }
    private void CheckForSprint()
    {
        if (master.Sprint == true)
        {
            _askingSprint = true;
        }
    }
    private void CheckForDodge()
    {
        if (master.Dodge == true && _movingState != MovingState.Dodging)
        {
            master.StartCoroutine(DodgeCoroutine());
        }
    }
    private void CheckForSprintStop()
    {
        if (master.MoveInputMagntiude < MData.SprintInputThreshold)
        {
            _askingSprint = false;
        }
    }

    public override void LateUpdate()
    {
        CheckCanMoveInAnyWay();
    }
    private void CheckCanMoveInAnyWay()
    {
        if (_movingState == MovingState.Dodging ||
            _movingState == MovingState.VelocityOverriden ||
            master.AttackController.AttackState != AttackState.None)
        {
            _canInputMoveInAnyWay = false;
        }
        else
        {
            _canInputMoveInAnyWay = true;
        }
    }

    //Coroutines
    private IEnumerator DodgeCoroutine()
    {
        _movingState = MovingState.Dodging;
        float count = 0f, progress = 0f;

        while (count < MData.DodgeDuration && _grounded == true)
        {
            progress = count / MData.DodgeDuration;
            progress = MData.DodgeSpeedOverDuration.Evaluate(progress);

            Velocity = MoveDirection * progress * MData.DodgeSpeed * _currentSpeedFactor;

            yield return null;
            count += WorldData.DeltaTime;
        }

        progress = MData.DodgeSpeedOverDuration.Evaluate(1);
        Velocity = MoveDirection * progress * MData.DodgeSpeed;

        _movingState = MovingState.Idle;
        UpdateMovingState();
    }
    private IEnumerator VelocityOverrideCoroutine(Vector3 velocity)
    {
        while(_grounded == true)
        {
            Velocity = velocity;
            yield return null;
        }

        OnStopVelocityOverride();
    }

    //Utilities
    public void SetBodyShape(float height, float step, float radius)
    {
        pRefs.Collision.radius = radius;
        pRefs.Collision.height = height - step + radius;
        pRefs.Collision.center = Vector3.up * (height * 0.5f + step * 0.5f - radius * 0.5f);
    }
    public float ComputeNormalAngle(Vector3 normal)
    {
        return Mathf.Abs(Vector3.Angle(Vector3.forward, normal) - 90);
    }
    public float ComputeHitNormalAngle(RaycastHit hit, out RaycastHit rayHit)
    {
        RaycastHit h;
        Physics.Raycast(hit.point + Vector3.up * 0.001f, Vector3.down, out h, WorldData.DefaultSolidLayer);
        rayHit = h;
        return ComputeNormalAngle(h.normal);
    }
    private Ray GetGroundCheckRay(out float distance)
    {
        Ray ray = new Ray()
        {
            origin = GetFeetPos().AddY(MData.SphereCastRadius + MData.SphereCastAdditionalLength + MData.StandHeight * 0.5f),
            direction = Vector3.down
        };

        distance = MData.StepUpHeight + MData.StandHeight * 0.5f;

        if (_grounded == true)
        {
            distance += MData.StepDownHeight;
        }

        distance += MData.SphereCastAdditionalLength * 2;

        return ray;
    }
    private Vector3 GetFeetPos()
    {
        Vector3 vec = pRefs.PhysicBody.position.AddY(MData.StepUpHeight);
        return vec;
    }
    private int GetNearestHitIndex(List<RaycastHit> l)
    {
        int minHitIndex = 0;
        for (int i = 1; i < l.Count; i++)
        {
            if (l[i].distance < l[minHitIndex].distance)
            {
                minHitIndex = i;
            }
        }
        return minHitIndex;
    }

    //Events
    private void OnGroundEnter()
    {
        //Reset & Remove gravity
        Velocity = Velocity.SetY(0);
        pRefs.Rigidbody.useGravity = false;
    }
    private void OnGroundLeave()
    {
        pRefs.Rigidbody.useGravity = true;
    }

    private void OnFeetHeightChanged(float heightDifference)
    {
        master.OnFeetHeightChanged(heightDifference);

        _currentFeetHeightChangedSpeedFactor = 1 - (Mathf.Abs(heightDifference) / 0.64f);
    }

    public void OnAttackStart(AttackData attack)
    {
        ComputeMoveVector();
    }

    public void OnStartVelocityOverride(Vector3 velocity, bool isLocalOverride = false)
    {
        if (isLocalOverride == true)
        {
            velocity = pRefs.VelocitySpace.TransformVector(velocity);
        }

        if (_velocityOverrideCoroutine != null)
        {
            master.StopCoroutine(_velocityOverrideCoroutine);
        }

        _movingState = MovingState.VelocityOverriden;
        _velocityOverrideCoroutine = master.StartCoroutine(VelocityOverrideCoroutine(velocity));
    }
    public void OnStopVelocityOverride()
    {
        if (_velocityOverrideCoroutine != null)
        {
            master.StopCoroutine(_velocityOverrideCoroutine);
            _velocityOverrideCoroutine = null;
        }

        _movingState = MovingState.Idle;
        UpdateMovingState();
        ComputeMoveVector();
    }


#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        if (_grounded == true)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.cyan;
        }
        float distance;
        Ray ray = GetGroundCheckRay(out distance);
        //Gizmos.DrawWireSphere(ray.origin, MData.SphereCastRadius);
        Gizmos.DrawWireSphere(ray.origin + ray.direction * distance, MData.SphereCastRadius);

        Vector3 start = ray.origin + Vector3.right * MData.SphereCastRadius;
        Gizmos.DrawLine(start, start.AddY(-distance));
        start = ray.origin + Vector3.left * MData.SphereCastRadius;
        Gizmos.DrawLine(start, start.AddY(-distance));
        start = ray.origin + Vector3.forward * MData.SphereCastRadius;
        Gizmos.DrawLine(start, start.AddY(-distance));
        start = ray.origin + Vector3.back * MData.SphereCastRadius;
        Gizmos.DrawLine(start, start.AddY(-distance));
    }
#endif
}
