using UnityEngine;
using UnityEngine.InputSystem;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private PlayerInput _playerInput;
    private InputAction _gameplayAction;
    private bool _isPaused = false;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        _gameplayAction = _playerInput.actions.FindAction("Pause");
    }
    private void Update()
    {
        if(_gameplayAction.WasPressedThisFrame())
        {
            if (!_isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }   
        }
    }
    private void Pause()
    {
        _pauseMenu.SetActive(true);
        _isPaused = true;
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void Resume()
    {
        _pauseMenu.SetActive(false);
        _isPaused = false;
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}


