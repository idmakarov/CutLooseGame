using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class BrightnessSetting : MonoBehaviour
{
    public Volume volume;
    private LiftGammaGain liftGammaGain;
    [SerializeField] Slider gammaSlider;

    [SerializeField] float gamma;


    void Start()
    {
        gamma = PlayerPrefs.GetFloat("Gamma", 1.5f);
        gammaSlider.value = gamma;

        if (volume.profile.TryGet(out liftGammaGain))
        {
            liftGammaGain.gamma.value = new Vector4(1f, 1f, 1f, gamma);
        }

        gammaSlider.onValueChanged.AddListener(GammaChanged);
    }

    public void GammaChanged(float newValue)
    {
        gamma = newValue;
        liftGammaGain.gamma.value = new Vector4(1f, 1f, 1f, newValue);
        PlayerPrefs.SetFloat("Gamma", newValue);
    }
}
