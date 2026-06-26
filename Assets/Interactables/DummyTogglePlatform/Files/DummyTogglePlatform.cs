using System.Collections.Generic;
using UnityEngine;

public class DummyTogglePlatform : MonoBehaviour
{
    [SerializeField] List<GameObject> dummies;
    [SerializeField] int correctId;
    int currentDummyId;

    public bool isCorrect = false;

    private void SetDummy(int id)
    {
        currentDummyId = id;
    
        for (int i = 0; i < dummies.Count; i++)
            dummies[i].SetActive(i == currentDummyId);
    
        isCorrect = currentDummyId == correctId;
    }
    
    private void Start()
    {
        if (dummies.Count == 0) return;
        SetDummy(Random.Range(0, dummies.Count));
    }
    
    public void ToggleNextDummy()
    {
        SetDummy((currentDummyId + 1) % dummies.Count);
    }
}