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

    private int _overlapCount = 0;

    void Start()
    {
        normalLocalY = plateRenderer.transform.localPosition.y;
        normalColor = plateRenderer.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(boxTag) || other.CompareTag("Player"))
        {
            _overlapCount++;
            if (!pressed)
            {
                pressed = true;
                AnimatePress();
                OnStateChanged?.Invoke(pressed);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(boxTag) || other.CompareTag("Player"))
        {
            _overlapCount = Mathf.Max(0, _overlapCount - 1);
            if (_overlapCount == 0 && pressed)
            {
                pressed = false;
                AnimateRelease();
                OnStateChanged?.Invoke(pressed);
            }
        }
    }

    private void AnimatePress()
    {
        plateRenderer.transform.DOKill();
        plateRenderer.material.DOKill();

        plateRenderer.transform.DOLocalMoveY(pressedLocalY, animationDuration);
        plateRenderer.material.DOColor(pressedColor, animationDuration);
    }

    private void AnimateRelease()
    {
        plateRenderer.transform.DOKill();
        plateRenderer.material.DOKill();

        plateRenderer.transform.DOLocalMoveY(normalLocalY, animationDuration);
        plateRenderer.material.DOColor(normalColor, animationDuration);
    }
}