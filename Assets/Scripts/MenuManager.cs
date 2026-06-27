using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        if (LoadingFade.Instance != null)
            LoadingFade.Instance.LoadLevelFaded("Game");
        else
            SceneManager.LoadScene("Game");
    }
    public void StartGrayBox()
    {
        if (LoadingFade.Instance != null)
            LoadingFade.Instance.LoadLevelFaded("GrayBox");
        else
            SceneManager.LoadScene("GrayBox");
    }
    public void RestartGame()
    {
        if (LoadingFade.Instance != null)
            LoadingFade.Instance.LoadLevelFaded("Game");
        else
            SceneManager.LoadScene("Game");
    }
    public void Settings()
    {
        Debug.Log("Settings Menu");
    }
    public void ReturnToMainMenu()
    {
        if (LoadingFade.Instance != null)
            LoadingFade.Instance.LoadLevelFaded("Menu");
        else
            SceneManager.LoadScene("Menu");
    }
    public void Credits()
    {
        if (LoadingFade.Instance != null)
            LoadingFade.Instance.LoadLevelFaded("Credits");
        else
            SceneManager.LoadScene("Credits");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
