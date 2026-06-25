using UnityEngine;

public class ButtonInteractable : MonoBehaviour
{
    [Header("Визуальные настройки")]
    public Material pressedMaterial;   // материал для нажатой кнопки
    public float pressOffset = 0.1f;   // насколько кнопка "утапливается"

    private Material originalMaterial;
    private Renderer rend;
    private Vector3 originalPosition;
    private bool isPressed = false;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalMaterial = rend.material;

        originalPosition = transform.position;
    }

    // Этот метод вызывается при взаимодействии (например, по триггеру или клику)
    public void PressButton()
    {
        if (isPressed) return; // уже нажата

        isPressed = true;

        // Визуальное изменение
        if (rend != null && pressedMaterial != null)
            rend.material = pressedMaterial;

        // Опускаем кнопку вниз
        transform.position = originalPosition - Vector3.up * pressOffset;

        // Сообщаем менеджеру
        if (LevelManager.Instance != null)
            LevelManager.Instance.RegisterButtonPress();
        else
            Debug.LogError("LevelManager не найден на сцене!");

        // Опционально: звук, анимация и т.д.
    }

    // Если нужно сбросить кнопку (например, при перезапуске уровня)
    public void ResetButton()
    {
        isPressed = false;
        if (rend != null && originalMaterial != null)
            rend.material = originalMaterial;
        transform.position = originalPosition;
    }
}