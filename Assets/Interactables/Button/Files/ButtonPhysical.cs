using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ButtonPhysical : MonoBehaviour
{
    public bool interactable = true;

    [SerializeField] Renderer buttonBodyRenderer;
    [SerializeField] Color pressedColor;
    [SerializeField] Color disabledColor;
    Color normalColor;

    [SerializeField] float pressedLocalY;
    float normalLocalY;
    [SerializeField] float animationDuration = 0.5f;

    private bool isPlayerInside;
    private bool isAnimating;
    [SerializeField] PlayerInput playerInput;
    private InputAction inputAction;

    public UnityEvent OnClick;
    

    void Start()
    {
        if (playerInput == null)
        {
            Debug.LogWarning("You should assign PlayerInput! Using .Find() for now.");
            playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();
        }

        normalLocalY = buttonBodyRenderer.transform.localPosition.y;
        normalColor = buttonBodyRenderer.sharedMaterial.color;
        inputAction = playerInput.actions.FindAction("Use");
        
        // Apply initial interactable state
        UpdateInteractableVisual();
    }

    // Method to change interactable state
    public void SetInteractable(bool value)
    {
        if (interactable != value)
        {
            interactable = value;
            UpdateInteractableVisual();
        }
    }

    // Update the visual based on current interactable state
    private void UpdateInteractableVisual()
    {
        if (interactable)
        {
            // Transition to normal color (from disabled or current state)
            buttonBodyRenderer.material.DOColor(normalColor, animationDuration);
        }
        else
        {
            // Transition to disabled color
            buttonBodyRenderer.material.DOColor(disabledColor, animationDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Игрок вошел в зону!");
            CanvasHolder.Instance.useETip.gameObject.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Игрок вышел из зоны!");
            CanvasHolder.Instance.useETip.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInside && inputAction.WasPressedThisFrame())
        {
            if (interactable)
            {
                PressButton();
            }
            else
            {
                // TODO: show disabled message
                Debug.Log("Button is disabled!");
            }
        }
    }

    private void PressButton()
    {
        // Kill existing tweens and reset to normal state before starting new animation
        buttonBodyRenderer.transform.DOKill();
        buttonBodyRenderer.material.DOKill();
        
        // Reset to normal state
        buttonBodyRenderer.transform.localPosition = new Vector3(
            buttonBodyRenderer.transform.localPosition.x,
            0,
            buttonBodyRenderer.transform.localPosition.z
        );
        buttonBodyRenderer.material.color = normalColor;
        
        // Start fresh animation
        Sequence buttonSequence = DOTween.Sequence();
        
        buttonSequence.Append(buttonBodyRenderer.transform.DOLocalMoveY(pressedLocalY, animationDuration));
        buttonSequence.Join(buttonBodyRenderer.material.DOColor(pressedColor, animationDuration));
        
        buttonSequence.Append(buttonBodyRenderer.transform.DOLocalMoveY(normalLocalY, animationDuration));
        buttonSequence.Join(buttonBodyRenderer.material.DOColor(normalColor, animationDuration));
        
        buttonSequence.OnComplete(() => isAnimating = false);
        
        isAnimating = true;
        Debug.Log("Button pressed!");
        OnClick?.Invoke();
    }
}