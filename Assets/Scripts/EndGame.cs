using TMPro;
using UnityEngine;
public class EndGame : MonoBehaviour
{
    /*[SerializeField] private GameObject _endMenu;
    [SerializeField] private TextMeshProUGUI messageText;
    private bool isPlayerInside = false;
    private void OnTriggerEnter(Collider other)
    {
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
            _endMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Взаимодействие с зоной!");
            ShowMessage("Конец игры");
        }
    }
    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    public void ShowMessage(string text)
    {
        messageText.text = text;
    }*/
}