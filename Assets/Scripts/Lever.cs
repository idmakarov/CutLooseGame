using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    [SerializeField] float _angleOn;
    [SerializeField] float _angleOff;
    [SerializeField] Transform _lewer;
    [SerializeField] PlayerInput _input;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _audioClip;
    [SerializeField] Renderer _lightRenderer;
    [SerializeField] private LeverManager _leverManager;
    [SerializeField] private TextMeshProUGUI _useBox;
    private bool _isPlayerInside = false;
    private bool _isTurn = false;
    private InputAction _inputAction;
    private void Start()
    {
        _inputAction = _input.actions.FindAction("Use");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            _useBox.gameObject.SetActive(true);
            Debug.Log("Игрок вошел в зону!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            _useBox.gameObject.SetActive(false);
            Debug.Log("Игрок вышел из зоны!");
        }
    }
    private void SwitchLever()
    {
        _isTurn = !_isTurn;
        _lewer.DOKill();
        _lewer.DOLocalRotate(new Vector3(_isTurn ? _angleOn : _angleOff, 90.0f, 0.0f), 0.2f);
        _lightRenderer.materials[0].DOColor(_isTurn ? new Color(0.0f, 255.0f, 0.0f, 10.0f) : new Color(255.0f, 0.0f, 0.0f, 1.0f), 0.2f);
        _leverManager.OnTurnLever(_isTurn);
        _audioSource.PlayOneShot(_audioClip);
    }
    private void Update()
    {
        if (_isPlayerInside && _inputAction.WasPressedThisFrame())
        {
            SwitchLever();
        }
    }
}
