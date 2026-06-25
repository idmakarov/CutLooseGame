using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines.Interpolators;

public class AssBox : MonoBehaviour
{
    [SerializeField] private Transform _characterTransform;
    [SerializeField] private Vector3 _scaldeUp = new Vector3(0.8f, 1.2f, 0.8f);
    [SerializeField] private Vector3 _scaleDown = new Vector3(1.2f, 0.8f, 1.2f);
    [SerializeField] private float _scaleKoefficient;
    private void FixedUpdate()
    {
        Vector3 _relativePosition = _characterTransform.InverseTransformPoint(transform.position);
        _characterTransform.localScale = Lerp3(_scaleDown, Vector3.one, _scaldeUp, _relativePosition.y * _scaleKoefficient);
    }
    private Vector3 Lerp3(Vector3 _down, Vector3 _one, Vector3 _up, float _position)
    {
        if (_position < 0.0f)
            return Vector3.LerpUnclamped(_down, _one, _position + 1.0f);
        else
            return Vector3.LerpUnclamped(_one, _up, _position);
    }
}
