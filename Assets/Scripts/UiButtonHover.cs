using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class UIButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;

    [Header("Scale")]
    public float multiplier;
    public float duration;

    [Header("Highlight")]
    public Image highlightImage;
    [Range(0, 255)]
    public float highlightAlpha = 135f;

    void Start()
    {
        originalScale = transform.localScale;

        if (highlightImage != null)
        {
            Color color = highlightImage.color;
            color.a = 0;
            highlightImage.color = color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Увеличение кнопки
        transform.DOScale(originalScale * multiplier, duration);

        // Появление подсветки
        if (highlightImage != null)
        {
            highlightImage.DOFade(highlightAlpha / 255f, duration);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Возврат размера
        transform.DOScale(originalScale, duration);

        // Убираем подсветку
        if (highlightImage != null)
        {
            highlightImage.DOFade(0, duration);
        }
    }
}