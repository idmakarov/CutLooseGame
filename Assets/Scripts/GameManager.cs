using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] CinemachineOrbitalFollow cinemachineOrbitalFollow;


    void Start()
    {
        cinemachineOrbitalFollow.enabled = false;
        pauseMenu.Pause(noTimeStop: true, onResume: () => cinemachineOrbitalFollow.enabled = true);
    }
}
