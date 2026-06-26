using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LeverManager : MonoBehaviour
{
    static LeverManager _instance;
    public static LeverManager Instance {
        get {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<LeverManager>();
            }
            return _instance;
        }
    }
    
    [SerializeField] private TextMeshProUGUI _textBox;
    private int activatedCount = 0;
    public bool isComplete;

    public UnityEvent<bool> OnCompleteChanged;

    public void OnTurnLever(bool _isTurn)
    {
        if (_isTurn)
            activatedCount++;
        else
            activatedCount--;

        isComplete = activatedCount == 4;
        UpdateCounterUI();
        
        OnCompleteChanged?.Invoke(isComplete);
    }

    private void UpdateCounterUI()
    {
        _textBox.text = $"Switches turned on: {activatedCount}/4";
    }
}