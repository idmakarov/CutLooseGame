using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BrightnessSliderOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<Image> disableTargets = new List<Image>();
    [SerializeField, Range(0f, 1f)] private float hoverAlpha = 0f;

    private List<float> originalAlphas = new List<float>();

    private void Awake()
    {
        originalAlphas.Clear();

        for (int i = 0; i < disableTargets.Count; i++)
        {
            if (disableTargets[i] != null)
                originalAlphas.Add(disableTargets[i].color.a);
            else
                originalAlphas.Add(1f);
        }
    }

    private void OnEnable()
    {
        RestoreAlpha();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAlpha(hoverAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        RestoreAlpha();
    }

    private void SetAlpha(float alpha)
    {
        for (int i = 0; i < disableTargets.Count; i++)
        {
            if (disableTargets[i] == null) continue;

            Color color = disableTargets[i].color;
            color.a = alpha;
            disableTargets[i].color = color;
        }
    }

    private void RestoreAlpha()
    {
        for (int i = 0; i < disableTargets.Count; i++)
        {
            if (disableTargets[i] == null) continue;

            Color color = disableTargets[i].color;
            color.a = originalAlphas[i];
            disableTargets[i].color = color;
        }
    }
}