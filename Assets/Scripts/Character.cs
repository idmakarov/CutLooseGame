using UnityEngine;
public class Character : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    private bool _isPaused = false;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        }
    }
    public void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        _isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        _isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
