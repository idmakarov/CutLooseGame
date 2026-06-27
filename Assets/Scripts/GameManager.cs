using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] PauseMenu pauseMenu;


    void Start()
    {
        pauseMenu.Pause();
    }
}
