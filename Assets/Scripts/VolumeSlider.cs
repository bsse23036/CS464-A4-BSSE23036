using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private string exposedParameter = "MusicVolume";

    private const string PrefKey = "MusicVolume";

    private void Start()
    {
        float saved = PlayerPrefs.GetFloat(PrefKey, 1f);
        if (slider != null)
        {
            slider.minValue = 0.0001f;
            slider.maxValue = 1f;
            slider.value = saved;
            slider.onValueChanged.AddListener(SetVolume);
        }
        ApplyDb(saved);
    }

    public void SetVolume(float value)
    {
        float clamped = Mathf.Clamp(value, 0.0001f, 1f);
        ApplyDb(clamped);
        PlayerPrefs.SetFloat(PrefKey, clamped);
        PlayerPrefs.Save();
    }

    private void ApplyDb(float linearValue)
    {
        if (mixer != null)
        {
            float dB = Mathf.Log10(linearValue) * 20f;
            mixer.SetFloat(exposedParameter, dB);
        }
    }
}