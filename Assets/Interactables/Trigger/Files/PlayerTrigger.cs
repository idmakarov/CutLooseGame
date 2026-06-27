using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    public UnityEvent OnPlayerEnterTrigger;
    public UnityEvent OnPlayerExitTrigger;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnPlayerEnterTrigger");
            OnPlayerEnterTrigger?.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnPlayerExitTrigger");
            OnPlayerExitTrigger?.Invoke();
        }
    }
}
