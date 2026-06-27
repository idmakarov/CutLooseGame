using TMPro;
using UnityEngine;

public class CanvasHolder : MonoBehaviour
{
    static CanvasHolder _instance;
    public static CanvasHolder Instance {
        get {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<CanvasHolder>();
            }
            return _instance;
        }
    }
    
    public TextMeshProUGUI useETip;
    public TextMeshProUGUI talkTip;
}
