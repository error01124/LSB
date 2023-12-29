using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public State CurrentState => _currentState;
    public bool IsGrounded => _isGrounded;
    public bool IsMoving => _isMoving;
    public bool MoveEnabled => _moveEnabled;

    [SerializeField] private Inventory _inventory;
    [SerializeField] private float _walkSpeed = 10;
    [SerializeField] private float _sprintSpeed = 20;
    [SerializeField] private float _crouchSpeed = 5;
    [SerializeField] private float _jumpHeight = 2;
    [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _maxLookAngle = 50f;
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private bool _invertCamera = false;
    [SerializeField] private bool _cameraCanMove = true;
    [SerializeField] private KeyCode _lockKey = KeyCode.Escape;
    [SerializeField] private Camera _camera;
    [SerializeField] private GroudChecker _groundChecker;
    [SerializeField] private bool _enableHeadBob = true;
    [SerializeField] private Transform _joint;
    [SerializeField] private float _bobSpeed = 5f;

    private State _previousState;
    private State _currentState;
    private bool _isGrounded = false;
    private bool _isMoving = false;
    private bool _moveEnabled = true;
    private Rigidbody _rigidbody;
    private float _yaw = 0.0f;
    private float _pitch = 0.0f;
    private float _gravity;
    private Vector3 _moveVelocity;
    private Vector3 _bobAmount = new Vector3(0.15f, 0.05f, 0f);
    private float _crouchHeight = 0.75f;
    private float _timer = 0f;
    private Vector3 _jointOriginalPosition;
    private Vector3 _originalScale;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _jointOriginalPosition = _joint.localPosition;
        _originalScale = transform.localScale;
        _gravity = Mathf.Abs(Physics.gravity.y);
    }

    private void Start()
    {
        if (isLocalPlayer)
            _camera.gameObject.SetActive(true);

        Idle();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _groundChecker.GroundedChanged += SetGrounded;
        _inventory.Opened += OnInventoryOpened;
        _inventory.Closed += OnInventoryClosed;
    }

    private void OnDisable()
    {
        _groundChecker.GroundedChanged -= SetGrounded;
        _inventory.Opened -= OnInventoryOpened;
        _inventory.Closed -= OnInventoryClosed;
    }
    
    private void OnInventoryOpened()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnInventoryClosed()
    {
        Cursor.lockState= CursorLockMode.Locked;
    }

    private void SetGrounded(bool value) => _isGrounded = value;

    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(_lockKey))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }    

        if (ShouldJump())
            Jump();

        if (CanCameraMove())
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            _yaw = transform.localEulerAngles.y + mouseX * _mouseSensitivity;

            if (_invertCamera)
                _pitch += _mouseSensitivity * mouseY;
            else
                _pitch -= _mouseSensitivity * mouseY;

            _pitch = Mathf.Clamp(_pitch, -_maxLookAngle, _maxLookAngle);

            transform.localEulerAngles = new Vector3(0, _yaw, 0);
            _camera.transform.localEulerAngles = new Vector3(_pitch, 0, 0);
        }

        if (_enableHeadBob)
            HeadBob();
    }

    private void HeadBob()
    {
        if (_isMoving)
        {
            if (_currentState == State.Sprint)
                _timer += Time.deltaTime * _bobSpeed * 1.5f;
            else if (_currentState == State.Crouch)
                _timer += Time.deltaTime * _bobSpeed * 0.5f;
            else
                _timer += Time.deltaTime * _bobSpeed;

            _joint.localPosition = new Vector3(_jointOriginalPosition.x + Mathf.Sin(_timer) * _bobAmount.x, _jointOriginalPosition.y + Mathf.Sin(_timer) * _bobAmount.y, _jointOriginalPosition.z + Mathf.Sin(_timer) * _bobAmount.z);
        }
        else
        {
            _timer = 0;
            _joint.localPosition = new Vector3(Mathf.Lerp(_joint.localPosition.x, _jointOriginalPosition.x, Time.deltaTime * _bobSpeed), Mathf.Lerp(_joint.localPosition.y, _jointOriginalPosition.y, Time.deltaTime * _bobSpeed), Mathf.Lerp(_joint.localPosition.z, _jointOriginalPosition.z, Time.deltaTime * _bobSpeed));
        }
    }

    private void FixedUpdate()
    {
        _previousState = _currentState;

        if (CanMove())
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontal, 0, vertical);
            direction = transform.TransformDirection(direction).normalized;
            _isMoving = direction != Vector3.zero;
            bool shouldCrouch = Input.GetKey(_crouchKey);

            if (_isMoving)
            {
                if (Input.GetKey(_sprintKey))
                    Sprint(direction);
                else if (!shouldCrouch)
                    Walk(direction);
            }
            else
            {
                Idle();
            }

            if (shouldCrouch)
                Crouch(direction);
        }
        else
        {
            _isMoving = false;
        }

        MoveAtMoment(_moveVelocity);
        ApplyCrouch();
    }

    private bool CanMove() => _moveEnabled && _isGrounded && !_inventory.IsOpened;

    private bool CanCameraMove() => _cameraCanMove && !_inventory.IsOpened;

    private void Idle()
    {
        _currentState = State.Idle;
        _moveVelocity = Vector3.zero;
    }

    private void Walk(Vector3 direction)
    {
        _currentState = State.Walking;
        _moveVelocity = direction * _walkSpeed;
    }

    private void Sprint(Vector3 direction)
    {
        _currentState = State.Sprint;
        _moveVelocity = direction * _sprintSpeed;
    }

    private void Crouch(Vector3 direction)
    {
        _currentState = State.Crouch;
        _moveVelocity = direction * _crouchSpeed;
    }

    private void ApplyCrouch()
    {
        if (_previousState == _currentState) return;

        if (_currentState == State.Crouch)
            transform.localScale = new Vector3(_originalScale.x, _crouchHeight, _originalScale.z);
        else
            transform.localScale = new Vector3(_originalScale.x, _originalScale.y, _originalScale.z);
    }

    private bool ShouldJump() => Input.GetKeyDown(_jumpKey) && CanMove();

    private void Jump()
    {
        _currentState = State.Jump;
        Vector3 velocity = Vector3.up * Mathf.Sqrt(2 * _gravity * _jumpHeight);
        _rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        _isGrounded = false;
        _moveVelocity *= 0.5f;
        MoveAtMoment(_moveVelocity);
    }

    public void MoveAtMoment(Vector3 velocity)
    {
        //_rigidbody.AddForce(velocity - _previousMoveVelocity, ForceMode.VelocityChange);
        //_rigidbody.MovePosition(velocity);
        //Debug.Log(velocity);
        //_previousMoveVelocity = velocity;
        _rigidbody.MovePosition(_rigidbody.position + velocity * Time.fixedDeltaTime);
    }

    public enum State
    {
        Walking,
        Sprint,
        Crouch,
        Idle, 
        Jump
    }
}
