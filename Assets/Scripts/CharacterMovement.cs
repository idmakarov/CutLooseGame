using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Camera _camera;
    private Rigidbody _characterRigidbody;
    public float _speedAdj, _speedMax;

    void Awake()
    {
        _characterRigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Vector3 _cameraForward = _camera.transform.forward;
        Vector3 _cameraRight = _camera.transform.right;
        _cameraForward.y = 0.0f;
        _cameraRight.y = 0.0f;
        _cameraForward.Normalize();
        _cameraRight.Normalize();
        Vector3 moveDirection = (_cameraForward * Input.GetAxis("Vertical") + _cameraRight * Input.GetAxis("Horizontal")).normalized;
        _characterRigidbody.AddForce(moveDirection * _speedAdj * Time.fixedDeltaTime, ForceMode.Force);
        if (_characterRigidbody.linearVelocity.magnitude > _speedMax)
        {
            _characterRigidbody.linearVelocity = _characterRigidbody.linearVelocity.normalized * _speedMax;
        }
    }
}
