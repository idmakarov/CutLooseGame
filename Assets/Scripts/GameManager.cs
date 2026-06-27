using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<GameManager>();
            }
            return _instance;
        }
    }
    
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] CinemachineOrbitalFollow cinemachineOrbitalFollow;
    public Movement playerMovement;
    public Transform mainCameraT;


    void Start()
    {
        cinemachineOrbitalFollow.enabled = false;
        pauseMenu.Pause(noTimeStop: true, onResume: () => cinemachineOrbitalFollow.enabled = true);
    }
}
