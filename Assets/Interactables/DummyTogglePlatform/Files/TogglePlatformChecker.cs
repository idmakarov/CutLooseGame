using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TogglePlatformChecker : MonoBehaviour
{
    [SerializeField] private List<DummyTogglePlatform> dummyTogglePlatforms;
    [SerializeField] private UnityEvent OnPlatformStatesChanged;

    private bool _lastAllCorrect;

    private void Start()
    {
        _lastAllCorrect = AreAllPlatformsCorrect();
    }

    private void Update()
    {
        bool allCorrect = AreAllPlatformsCorrect();

        if (allCorrect != _lastAllCorrect)
        {
            _lastAllCorrect = allCorrect;

            OnPlatformStatesChanged?.Invoke();
        }
    }

    private bool AreAllPlatformsCorrect()
    {
        foreach (var platform in dummyTogglePlatforms)
        {
            if (!platform.isCorrect)
                return false;
        }

        return true;
    }
}