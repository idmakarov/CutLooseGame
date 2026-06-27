using UnityEngine;

public class CursorShowHide : MonoBehaviour
{
    [SerializeField] bool show = true;

    void Start()
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
