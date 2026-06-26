using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingFade : MonoBehaviour
{
    [SerializeField] float unfadeTime = 1.5f;
    [SerializeField] Image fadeImage;

    void Start()
    {
        fadeImage.DOFade(0f, unfadeTime);
    }
}
