using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class Interaction : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private bool isPlayerInside = false;
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _interactionAction;
    private void Start()
    {
        _interactionAction = _playerInput.actions.FindAction("Use");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interaction"))
        {
            isPlayerInside = true;
            Debug.Log("Игрок вошел в зону!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interaction"))
        {
            isPlayerInside = false;
            Debug.Log("Игрок вышел из зоны!");
        }
    }
    public void Interact()
    {
        _audioSource.Play();
        Debug.Log("Взаимодействие с зоной!");
        ShowMessage("Я БАНАН, Я Я БАНАН БАНАН");
    }
    private void Update()
    {
        if (isPlayerInside && _interactionAction.WasPressedThisFrame())
        {
            Interact();
        }
    }

    public TextMeshProUGUI messageText;
    public void ShowMessage(string text)
    {
        messageText.text = text;
    }
}