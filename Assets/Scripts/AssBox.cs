using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines.Interpolators;

public class AssBox : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private Vector3 _scaldeUp = new Vector3(0.8f, 1.2f, 0.8f);
    [SerializeField] private Vector3 _scaleDown = new Vector3(1.2f, 0.8f, 1.2f);
    [SerializeField] private float _scaleKoefficient;
    [SerializeField] private float _rotationKoefficient;
    private void Update()
    {
        Vector3 _relativePosition = _playerTransform.InverseTransformPoint(transform.position);
        float _interpolant = _relativePosition.y * _scaleKoefficient;
        Vector3 _scale = Lerp3(_scaleDown, Vector3.one, _scaldeUp, _interpolant);
        _playerBody.localScale = _scale;
        _playerBody.localEulerAngles = new Vector3(_relativePosition.z, 0.0f, -_relativePosition.x) * _rotationKoefficient;
    }
    Vector3 Lerp3(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        if (t < 0.0f)
            return Vector3.LerpUnclamped(a, b, t + 1.0f);
        else
            return Vector3.LerpUnclamped(b, c, t);
    }
}
