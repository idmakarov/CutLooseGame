using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speedAdj, _speedMax;
    private Rigidbody _characterRigidbody;
    private PlayerInput _playerInput;
    private InputAction _moveAction, _jumpAction;
    private Animator _animatorCharacter;
    private Vector2 _directionMoveAction;
    private Vector3 _cameraForwardVector, _cameraRightVector, _directionMove;
    private bool _isGrounded = true;
    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _animatorCharacter = GetComponent<Animator>();
        _characterRigidbody = GetComponent<Rigidbody>();
        _moveAction = _playerInput.actions.FindAction("Move");
        _jumpAction = _playerInput.actions.FindAction("Jump");
    }
    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }
    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGrounded = value;
        }
    }
    private void FixedUpdate()
    {
        _animatorCharacter.SetBool("Jump", false);
        _cameraForwardVector = _camera.transform.forward;
        _cameraForwardVector.y = 0.0f;
        _cameraForwardVector.Normalize();
        _cameraRightVector = _camera.transform.right;
        _cameraRightVector.y = 0.0f;
        _cameraRightVector.Normalize();
        _directionMoveAction = _moveAction.ReadValue<Vector2>();
        _directionMove = (_cameraForwardVector * _directionMoveAction.y + _cameraRightVector * _directionMoveAction.x).normalized;
        _directionMove = Vector3.ClampMagnitude(_directionMove, _speedMax);
        _characterRigidbody.AddForce(_directionMove * _speedAdj * Time.fixedDeltaTime, ForceMode.Force);
        if (_moveAction.IsPressed())
        {
            _animatorCharacter.SetFloat("Speed", _characterRigidbody.linearVelocity.magnitude, 0.2f, Time.fixedDeltaTime);
        }
        else
        {
            _animatorCharacter.SetFloat("Speed", 0.0f);
        }

        if (_jumpAction.IsPressed() && _isGrounded)
        {
            {
                Debug.Log("Я ГЕЙ");
                //_characterRigidbody.AddForce(Vector3.up * _speedAdj * 10.0f * Time.fixedDeltaTime, ForceMode.Force);
                _animatorCharacter.SetBool("Jump", true);
            }
        }
    }
}
