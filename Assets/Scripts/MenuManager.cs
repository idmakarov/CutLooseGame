using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void StartGrayBox()
    {
        SceneManager.LoadScene("GrayBox");
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void Settings()
    {
        Debug.Log("Settings Menu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
