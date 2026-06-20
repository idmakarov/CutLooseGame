using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("Start Game");
    }
    public void Settings()
    {
        Debug.Log("Settings Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
