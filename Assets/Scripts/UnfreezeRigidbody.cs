using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnfreezeRigidbody : MonoBehaviour
{
    [SerializeField] float delaySec = 2f;

    void Start()
    {
        Invoke(nameof(Unfreeze), delaySec);
    }

    void Unfreeze()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
