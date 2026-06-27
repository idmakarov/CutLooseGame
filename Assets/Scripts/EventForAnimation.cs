using UnityEngine;
using UnityEngine.Events;

public class EventForAnimation : MonoBehaviour
{
    [SerializeField] UnityEvent OnEvent;

    public void CallEvent()
    {
        OnEvent?.Invoke();
    }
}
