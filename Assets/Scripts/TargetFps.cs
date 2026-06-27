using UnityEngine;

public class TargetFps : MonoBehaviour
{
    [SerializeField] private int targetFPS = 120;

    void Awake()
    {
        Application.targetFrameRate = targetFPS;
        QualitySettings.vSyncCount = 0; // Ignored on WebGL, but good practice.
    }
}
