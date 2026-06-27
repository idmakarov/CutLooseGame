using UnityEngine;

public class RoomScissorsManager : MonoBehaviour
{
    [SerializeField] ButtonPhysical scissorsButtonPhysical;


    void Start()
    {
        LeverManager.Instance.OnCompleteChanged.AddListener(LeverManagerChanged);
        scissorsButtonPhysical.OnClick.AddListener(ScissorsButtonPressed);
    }

    void LeverManagerChanged(bool isComplete)
    {
        scissorsButtonPhysical.SetInteractable(isComplete);
    }

    void ScissorsButtonPressed()
    {
        Debug.Log("Game complete");
    }
}
