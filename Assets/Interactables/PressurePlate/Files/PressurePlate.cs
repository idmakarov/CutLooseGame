using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] string boxTag = "MovableBox";
    public bool pressed;

    [SerializeField] Renderer plateRenderer;
    [SerializeField] Color pressedColor;
    Color normalColor;
    [SerializeField] float pressedLocalY;
    float normalLocalY;
    [SerializeField] float animationDuration = 0.3f;

    public UnityEvent<bool> OnStateChanged;


    void Start()
    {
        normalLocalY = plateRenderer.transform.localPosition.y;
        normalColor = plateRenderer.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag(boxTag) || other.CompareTag("Player")) && !pressed)
        {
            pressed = true;
            AnimatePress();
            OnStateChanged?.Invoke(pressed);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag(boxTag) || other.CompareTag("Player")) && pressed)
        {
            pressed = false;
            AnimateRelease();
            OnStateChanged?.Invoke(pressed);
        }
    }

    private void AnimatePress()
    {
        // Kill existing tweens to prevent conflicts
        plateRenderer.transform.DOKill();
        plateRenderer.material.DOKill();

        // Animate to pressed state
        plateRenderer.transform.DOLocalMoveY(pressedLocalY, animationDuration);
        plateRenderer.material.DOColor(pressedColor, animationDuration);
    }

    private void AnimateRelease()
    {
        // Kill existing tweens to prevent conflicts
        plateRenderer.transform.DOKill();
        plateRenderer.material.DOKill();

        // Animate back to normal state
        plateRenderer.transform.DOLocalMoveY(normalLocalY, animationDuration);
        plateRenderer.material.DOColor(normalColor, animationDuration);
    }
}