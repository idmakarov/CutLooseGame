using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _moveSpeedAdj, _moveSpeedMax;
    [SerializeField] private float _jumpSpeedAdj;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _groundCheckOffset = 0.1f;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    private Rigidbody _characterRigidbody;
    private PlayerInput _playerInput;
    private InputAction _moveAction, _jumpAction;
    private Animator _animatorCharacter;
    private Vector2 _directionMoveAction;
    private Vector3 _cameraForwardVector, _cameraRightVector, _directionMove;
    private bool _isGrounded, _isPressedSpace;
    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _animatorCharacter = GetComponent<Animator>();
        _characterRigidbody = GetComponent<Rigidbody>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _jumpAction = _playerInput.actions.FindAction("Jump");
    }
    private bool IsGrounded()
    {
        Vector3 sphereCenter = transform.position + Vector3.down * _groundCheckOffset;
        return Physics.CheckSphere(sphereCenter, _groundCheckRadius, _groundLayerMask);
    }
    private void Update()
    {
        if(_jumpAction.WasPressedThisFrame())
        {
            _isPressedSpace = true;
        }
    }
    private void FixedUpdate()
    {
        _isGrounded = IsGrounded();
        //if (_isGrounded)
        //{
        //    _animatorCharacter.SetBool("Jump", false);
        //}
        _cameraForwardVector = _camera.transform.forward;
        _cameraForwardVector.y = 0.0f;
        _cameraForwardVector.Normalize();
        _cameraRightVector = _camera.transform.right;
        _cameraRightVector.y = 0.0f;
        _cameraRightVector.Normalize();
        _directionMoveAction = _moveAction.ReadValue<Vector2>();
        _directionMove = (_cameraForwardVector * _directionMoveAction.y + _cameraRightVector * _directionMoveAction.x).normalized;
        _characterRigidbody.AddForce(_directionMove * _moveSpeedAdj * Time.fixedDeltaTime, ForceMode.Force);
        _characterRigidbody.linearVelocity = Vector3.ClampMagnitude(_characterRigidbody.linearVelocity, _moveSpeedMax);
        if (_moveAction.IsPressed())
        {
            _animatorCharacter.SetFloat("Speed", _characterRigidbody.linearVelocity.magnitude, 0.2f, Time.fixedDeltaTime);
        }
        else
        {
            _animatorCharacter.SetFloat("Speed", 0.0f);
        }
        if (_isPressedSpace && _isGrounded)
        {
            {
                _characterRigidbody.AddForce(Vector3.up * _jumpSpeedAdj, ForceMode.Impulse);
                _animatorCharacter.SetTrigger("Jump");
                //_animatorCharacter.SetBool("Jump", true);
            }
            _isPressedSpace = false;
        }
        _animatorCharacter.SetBool("IsGrounded", _isGrounded);
    }
}
