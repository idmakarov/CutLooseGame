using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    private bool _isPaused = false;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }   
        }*/
    }

    private void Pause()
    {
        _pauseMenu.SetActive(true);
        _isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Resume()
    {
        _pauseMenu.SetActive(false);
        _isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}


