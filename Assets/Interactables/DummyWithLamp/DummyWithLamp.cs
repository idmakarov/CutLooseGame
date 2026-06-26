using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DummyWithLamp : MonoBehaviour
{
    [SerializeField] List<GameObject> enableOnFalling;
    [SerializeField] List<GameObject> disableOnFalling;

    [SerializeField] private string boxTag;
    [SerializeField] private string playerTag;
    [SerializeField] private Rigidbody dummyRb;

    private readonly HashSet<Collider> occupants = new();

    private bool initialized;

    private void OnTriggerStay(Collider other)
    {
        if (initialized)
            return;

        if (other.CompareTag(boxTag) || other.CompareTag(playerTag))
        {
            occupants.Add(other);
        }
    }

    private void LateUpdate()
    {
        if (initialized)
            return;

        // Wait one physics frame so OnTriggerStay has run at least once
        initialized = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(boxTag) || other.CompareTag(playerTag))
        {
            occupants.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(boxTag) || other.CompareTag(playerTag))
        {
            occupants.Remove(other);

            if (occupants.Count == 0)
                Fall();
        }
    }

    private void Fall()
    {
        dummyRb.isKinematic = false;
        
        foreach (GameObject go in enableOnFalling)
            go.SetActive(true);

        foreach (GameObject go in disableOnFalling)
            go.SetActive(false);
    }
}