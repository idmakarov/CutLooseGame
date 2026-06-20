using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] float _speedAdj, _speedMax;
    private Rigidbody _characterRigidbody;
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
        Vector3 moveDirection = (_cameraForward * Input.GetAxis("Vertical") * Time.fixedDeltaTime + _cameraRight * Input.GetAxis("Horizontal") * Time.fixedDeltaTime).normalized;
        //Ограничение скорости
        if (_characterRigidbody.linearVelocity.magnitude < _speedMax)
        {
            _characterRigidbody.AddForce(moveDirection * _speedAdj * Time.fixedDeltaTime, ForceMode.Force);
            _characterRigidbody.transform.eulerAngles = new Vector3(0.0f, _camera.transform.eulerAngles.y, _camera.transform.eulerAngles.z);
        }
    }
}
