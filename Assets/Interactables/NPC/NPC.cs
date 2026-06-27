using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    private bool isPlayerInside;
    [SerializeField] PlayerInput playerInput;
    private InputAction inputAction;

    [SerializeField] List<string> phrases;
    [SerializeField] bool showTutorial;
    

    void Start()
    {
        if (playerInput == null)
        {
            Debug.LogWarning("You should assign PlayerInput! Using .Find() for now.");
            playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();
        }

        inputAction = playerInput.actions.FindAction("Use");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Игрок вошел в зону!");
            CanvasHolder.Instance.talkTip.gameObject.SetActive(true);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Игрок вышел из зоны!");
            CanvasHolder.Instance.talkTip.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInside && inputAction.WasPressedThisFrame() && !Dialog.Instance.DialogInProgress)
        {
            Dialog.Instance.Open(phrases, showTutorial);
        }
    }
}
