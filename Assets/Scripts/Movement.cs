using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private AudioClip _landedSound;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _moveSpeedAdj, _moveSpeedMax;
    [SerializeField] private float _jumpSpeedAdj;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _groundCheckOffset = 0.1f;
    [SerializeField] private float _groundCheckRadius = 0.2f;

    private AudioSource _audioSourceCharacter;
    private Rigidbody _characterRigidbody;
    private InputAction _moveAction, _jumpAction;
    private Animator _animatorCharacter;

    private Vector2 _directionMoveAction;
    private Vector3 _cameraForwardVector, _cameraRightVector;

    private bool _isGrounded;
    private bool _allowedJumping;

    private Vector3 _groundNormal = Vector3.up;

    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        _animatorCharacter = GetComponent<Animator>();
        _characterRigidbody = GetComponent<Rigidbody>();
        _audioSourceCharacter = GetComponent<AudioSource>();

        _moveAction = _playerInput.actions.FindAction("Move");
        _jumpAction = _playerInput.actions.FindAction("Jump");
    }

    private void LandedSound()
    {
        _audioSourceCharacter.PlayOneShot(_landedSound);
    }

    private void JumpSound()
    {
        _audioSourceCharacter.PlayOneShot(_jumpSound);
    }

    private bool GetGroundInfo()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        if (Physics.SphereCast(
            origin,
            _groundCheckRadius,
            Vector3.down,
            out RaycastHit hit,
            _groundCheckOffset + 0.2f,
            _groundLayerMask))
        {
            _groundNormal = hit.normal;
            return hit.distance <= _groundCheckOffset + 0.25f;
        }

        _groundNormal = Vector3.up;
        return false;
    }

    private void Update()
    {
        if (_jumpAction.WasPressedThisFrame())
        {
            _allowedJumping = true;
        }
    }

    private void MoveCharacter()
    {
        _cameraForwardVector = _camera.transform.forward;
        _cameraForwardVector.y = 0f;
        _cameraForwardVector.Normalize();

        _cameraRightVector = _camera.transform.right;
        _cameraRightVector.y = 0f;
        _cameraRightVector.Normalize();

        _directionMoveAction = _moveAction.ReadValue<Vector2>();

        Vector3 inputDir =
            (_cameraForwardVector * _directionMoveAction.y +
             _cameraRightVector * _directionMoveAction.x).normalized;

        Vector3 slopeDir = Vector3.ProjectOnPlane(inputDir, _groundNormal).normalized;

        _characterRigidbody.AddForce(slopeDir * _moveSpeedAdj, ForceMode.Acceleration);

        _characterRigidbody.linearVelocity =
            Vector3.ClampMagnitude(_characterRigidbody.linearVelocity, _moveSpeedMax);
    }

    private void JumpCharacter()
    {
        if (_allowedJumping && _isGrounded)
        {
            _characterRigidbody.AddForce(Vector3.up * _jumpSpeedAdj, ForceMode.Acceleration);
            _animatorCharacter.SetTrigger("Jump");
        }

        _allowedJumping = false;
    }

    private void UpdateAnimation()
    {
        float speed = new Vector3(
            _characterRigidbody.linearVelocity.x,
            0f,
            _characterRigidbody.linearVelocity.z
        ).magnitude;

        if (_isGrounded && _moveAction.IsPressed())
        {
            _animatorCharacter.SetFloat("Speed", speed, 0.2f, Time.fixedDeltaTime);
        }
        else
        {
            _animatorCharacter.SetFloat("Speed", 0f);
        }

        if (!_isGrounded && _characterRigidbody.linearVelocity.y < -1f)
        {
            _animatorCharacter.SetBool("IsFalling", true);
        }
        else if (_isGrounded)
        {
            _animatorCharacter.SetBool("IsFalling", false);
        }

        _animatorCharacter.SetBool("IsGrounded", _isGrounded);
    }

    private void FixedUpdate()
    {
        _isGrounded = GetGroundInfo();

        MoveCharacter();
        JumpCharacter();
        UpdateAnimation();
    }
}