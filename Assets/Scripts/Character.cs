using UnityEngine;
public class Character : MonoBehaviour
{
    private Rigidbody _characterRigidbody;
    [SerializeField] private GameObject _pauseMenu;
    private bool _isPaused = false;
    Animator animator;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Start()
    {
        _characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        animator.SetFloat("Speed", _characterRigidbody.linearVelocity.magnitude);
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


