using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider soundSlider;
    [SerializeField] AudioMixer masterMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("SavedMasterVolume", 100f);
        ApplyVolumeToMixer(savedVolume);
    }

    public void ApplyFromSlider(float value)
    {
        ApplyVolumeToMixer(value);
        PlayerPrefs.SetFloat("SavedMasterVolume", value);
        PlayerPrefs.Save();
    }

    private void ApplyVolumeToMixer(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 100f);

        float mixerVal = Mathf.Log10(value / 100f) * 20f;
        masterMixer.SetFloat("MasterVolume", mixerVal);
    }

    public void RegisterVolumeSlider(Slider slider)
    {
        soundSlider = slider;
        float savedVolume = PlayerPrefs.GetFloat("SavedMasterVolume", 100f);
        soundSlider.SetValueWithoutNotify(savedVolume);
        soundSlider.onValueChanged.RemoveAllListeners();
        soundSlider.onValueChanged.AddListener(ApplyFromSlider);

    }

}
