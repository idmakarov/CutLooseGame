using TMPro;
using UnityEngine;
public class InteractionZone : MonoBehaviour
{
    private bool isPlayerInside = false;
    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошел именно игрок (по тегу)
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Игрок вошел в зону!");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Игрок вышел из зоны!");
        }
    }
    public void Interact()
    {
        if (isPlayerInside)
        {
            Debug.Log("Взаимодействие с зоной!");
            ShowMessage("Я БАНАН, Я Я БАНАН БАНАН");
        }
    }
    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public TextMeshProUGUI messageText;
    public void ShowMessage(string text)
    {
        messageText.text = text;
    }
}