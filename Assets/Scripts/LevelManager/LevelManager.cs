using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // если используете TextMeshPro, иначе используйте UnityEngine.UI

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI progressText; // или обычный Text
    public GameObject levelCompletePanel; // панель, которая появляется при завершении

    [Header("Level Settings")]
    public string nextLevelName = "Level2"; // имя следующей сцены
    public float delayBeforeLoad = 2f;     // задержка перед загрузкой

    private int totalButtons;
    private int pressedButtons = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Ищем все кнопки на сцене автоматически (можно и вручную назначить)
        ButtonInteractable[] buttons = FindObjectsOfType<ButtonInteractable>();
        totalButtons = buttons.Length;
        UpdateUI();

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    // Вызывается кнопкой при нажатии
    public void RegisterButtonPress()
    {
        pressedButtons++;
        UpdateUI();

        if (pressedButtons >= totalButtons)
        {
            LevelComplete();
        }
    }

    private void UpdateUI()
    {
        if (progressText != null)
        {
            progressText.text = $"Кнопок: {pressedButtons} / {totalButtons}";
        }
    }

    private void LevelComplete()
    {
        Debug.Log("Все кнопки нажаты! Уровень пройден.");
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);

        // Загружаем следующий уровень через задержку
        Invoke(nameof(LoadNextLevel), delayBeforeLoad);
    }

    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
            SceneManager.LoadScene(nextLevelName);
        else
            Debug.LogWarning("Не указано имя следующего уровня!");
    }
}