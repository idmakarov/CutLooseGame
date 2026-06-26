using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ButtonPhysical : MonoBehaviour
{
    [SerializeField] Renderer buttonBodyRenderer;
    [SerializeField] Color pressedColor;
    Color normalColor;

    [SerializeField] float pushDistance;
    [SerializeField] float animationDuration = 0.5f;

    private bool isPlayerInside;
    private bool isAnimating;
    [SerializeField] PlayerInput playerInput;
    private InputAction inputAction;

    public UnityEvent OnClick;

    void Start()
    {
        normalColor = buttonBodyRenderer.material.color;
        inputAction = playerInput.actions.FindAction("Use");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Игрок вошел в зону!");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Игрок вышел из зоны!");
        }
    }

    private void Update()
    {
        if (isPlayerInside && inputAction.WasPressedThisFrame())
        {
            PressButton();
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
        
        buttonSequence.Append(buttonBodyRenderer.transform.DOLocalMoveY(-pushDistance, animationDuration));
        buttonSequence.Join(buttonBodyRenderer.material.DOColor(pressedColor, animationDuration));
        
        buttonSequence.Append(buttonBodyRenderer.transform.DOLocalMoveY(0, animationDuration));
        buttonSequence.Join(buttonBodyRenderer.material.DOColor(normalColor, animationDuration));
        
        buttonSequence.OnComplete(() => isAnimating = false);
        
        isAnimating = true;
        Debug.Log("Button pressed!");
        OnClick?.Invoke();
    }
}