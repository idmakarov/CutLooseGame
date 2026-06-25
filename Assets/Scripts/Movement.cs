using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [SerializeField] private AudioClip _landedSound;
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _moveSpeedAdj, _moveSpeedMax;
    [SerializeField] private float _jumpSpeedAdj;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _groundCheckOffset = 0.1f;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    private AudioSource _audioSourceCharacter;
    private Rigidbody _characterRigidbody;
    private PlayerInput _playerInput;
    private InputAction _moveAction, _jumpAction;
    private Animator _animatorCharacter;
    private Vector2 _directionMoveAction;
    private Vector3 _cameraForwardVector, _cameraRightVector, _directionMove;
    private bool _isGrounded;
    private bool _allowedJumping;
    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
        _playerInput = GetComponent<PlayerInput>();
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
    private bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position + Vector3.down * _groundCheckOffset, _groundCheckRadius, _groundLayerMask);
    }
    private void Update()
    {
        if(_jumpAction.WasPressedThisFrame() && _isGrounded)
        {
            _allowedJumping = true;
        }
    }
    //Движение персонажа в направление камеры.
    private void MoveCharacter()
    {
        _cameraForwardVector = _camera.transform.forward;
        _cameraForwardVector.y = 0.0f;
        _cameraForwardVector.Normalize();
        _cameraRightVector = _camera.transform.right;
        _cameraRightVector.y = 0.0f;
        _cameraRightVector.Normalize();
        _directionMoveAction = _moveAction.ReadValue<Vector2>();
        _directionMove = (_cameraForwardVector * _directionMoveAction.y + _cameraRightVector * _directionMoveAction.x).normalized;
        _characterRigidbody.AddForce(_directionMove * _moveSpeedAdj, ForceMode.Acceleration);
        _characterRigidbody.linearVelocity = Vector3.ClampMagnitude(_characterRigidbody.linearVelocity, _moveSpeedMax);
    }
    //Прыжок песонажа.
    private void JumpCharacter()
    {
        if (_allowedJumping)
        {
            _characterRigidbody.AddForce(Vector3.up * _jumpSpeedAdj, ForceMode.Acceleration);
            _animatorCharacter.SetTrigger("Jump");
        }
        _allowedJumping = false;
    }
    //Анимация движения.
    private void UpdateAnimation()
    {
        if (_moveAction.IsPressed() && _isGrounded)
        {
            _animatorCharacter.SetFloat("Speed", _characterRigidbody.linearVelocity.magnitude, 0.2f, Time.fixedDeltaTime);
        }
        else
        {
            _animatorCharacter.SetFloat("Speed", 0.0f);
        }
        if (_isGrounded == false && _characterRigidbody.linearVelocity.y < -2.3f)
        {
            _animatorCharacter.SetBool("IsFalling", true);  
        }
        _animatorCharacter.SetBool("IsGrounded", _isGrounded);
    }
    private void FixedUpdate()
    {
        Debug.Log(_characterRigidbody.linearVelocity.y);
        _isGrounded = IsGrounded();
        MoveCharacter();
        JumpCharacter();
        UpdateAnimation();
    }
}
