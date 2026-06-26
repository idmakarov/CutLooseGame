using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeverManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBox;
    private int activatedCount = 0;
    public void OnTurnLever(bool _isTurn)
    {
        if (_isTurn)
            activatedCount++;
        else
            activatedCount--;
        UpdateCounterUI();
    }
    private void UpdateCounterUI()
    {
        _textBox.text = $"Switches turned on: {activatedCount}/4";
    }
}