using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private PlayerInput _playerInput;

    private InputAction pauseAction;
    private bool _isPaused = false;

    private Action _onResumeOnce;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        pauseAction = _playerInput.actions.FindAction("Pause");
    }

    private void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            if (!_isPaused)
                Pause();
            else
                Resume();
        }
    }

    // Optional callback is registered here
    public void Pause(bool noTimeStop = false, Action onResume = null)
    {
        _pauseMenu.SetActive(true);
        _isPaused = true;

        _onResumeOnce = onResume;

        if (!noTimeStop)
            Time.timeScale = 0.0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        _isPaused = false;

        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _onResumeOnce?.Invoke();
        _onResumeOnce = null;
    }
}