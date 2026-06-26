using System.Collections.Generic;
using UnityEngine;

public class DummyTogglePlatform : MonoBehaviour
{
    [SerializeField] List<GameObject> dummies;
    [SerializeField] int correctId;
    int currentDummyId;

    public bool isCorrect = false;

    public void ToggleNextDummy()
    {
        currentDummyId++;
        if (currentDummyId > dummies.Count - 1) currentDummyId = 0;
        
        foreach (GameObject dummy in dummies)
            dummy.SetActive(false);

        dummies[currentDummyId].SetActive(true);

        isCorrect = currentDummyId == correctId;
    }
}
