using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingFade : MonoBehaviour
{
    static LoadingFade _instance;
    public static LoadingFade Instance {
        get {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<LoadingFade>();
            }
            return _instance;
        }
    }
    
    [SerializeField] float unfadeTime = 1.5f;
    [SerializeField] Image fadeImage;

    void Start()
    {
        fadeImage.DOFade(0f, unfadeTime).OnComplete(() => fadeImage.raycastTarget = false);
    }

    public void LoadLevelFaded(string targetSceneName)
    {
        fadeImage.DOFade(1f, unfadeTime).OnComplete(() => SceneManager.LoadScene(targetSceneName));
    }
}
