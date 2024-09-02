using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CommonInputCallback))]
public class CommonCharacterController : MonoBehaviour
{
    private Animator _animator;
    private PlayerInput _playerInput;
    private CharacterController _controller;
    private CommonInputCallback _input;

    private const float _threshold = 0.01f;

    private bool _hasAnimator;

    //Jump
    [Space(10)]
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;
    public bool Grounded = true;

    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    [Space(10)]
    public float JumpTimeout = 0.50f;
    public float FallTimeout = 0.15f;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;


    [Header("Player")]
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;
    public float SpeedChangeRate = 10.0f;
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDDeath;
    private int _animIDMotionSpeed;

    public bool IsDead 
    { 
        get 
        { 
            return _isDead; 
        } 
        set 
        { 
            _isDead = value;
            if(IsDead)
                Death(_isDead);

        } 
    }

    private bool _isDead = false;
    private Camera _mainCamera;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
    }

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _input = GetComponent<CommonInputCallback>();
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        AssignAnimationIDs();
    }

    private void Update()
    {
        _hasAnimator = TryGetComponent(out _animator);

        if (!GameManager.Instance.IsMovable())
            return;

        if (_isDead)
            return;

        GroundedCheck();
        Move();
    }

    #region Setting
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDDeath = Animator.StringToHash("Death");
    }
    #endregion

    #region Input
    private void Move()
    {

        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude =  _input.move.magnitude;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;


        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    private void GroundedCheck()
    {
        //Physics가 프로젝트 마다, 버전 마다 다르게 동작하는거 같다
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
    }



    private void Death(bool Isdead)
    {
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDDeath, Isdead);
        }
        _controller.enabled = false;
    }
    #endregion

    #region Animation Callback Receiver
    private void OnFootstep(AnimationEvent animationEvent)
    {
        
    }

    private void OnLand(AnimationEvent animationEvent)
    {

    }
    #endregion
}
