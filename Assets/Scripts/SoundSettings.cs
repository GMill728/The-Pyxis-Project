using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] private AudioSource previewSource;
    [SerializeField] private AudioClip previewClip;

    public void StartPreview()
    {
        if (previewSource == null|| previewClip == null) return;
        previewSource.clip = previewClip;
        previewSource.loop = true;
        previewSource.Play();


    }

    public void StopPreview()
    {
        if (previewSource == null) return;
        previewSource.Stop();
    }
        public void SetMusicVolume(float volume)
    {
        if (volume == 0)
        {
            masterMixer.SetFloat("MusicMixer", -80f);
            return;
        }
        volume = Mathf.Clamp(volume, 0f, 1f);
        volume = Mathf.Lerp(0.2f, 1f, volume);
        float db = Mathf.Log10(volume) * 20f;
        masterMixer.SetFloat("MusicMixer", db);

        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume == 0)
        {
            masterMixer.SetFloat("SFXMixer", -80f);
            return;
        }
        volume = Mathf.Clamp(volume, 0f, 1f);
        volume = Mathf.Lerp(0.2f, 1f, volume);
        float db = Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)) * 20f;
        masterMixer.SetFloat("SFXMixer", db);

        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public void RegisterVolumeSlider(Slider slider)
    {
        musicSlider = slider;
        float savedVolume = PlayerPrefs.GetFloat("MusicVol", 1f);
        musicSlider.SetValueWithoutNotify(savedVolume);
        musicSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        SetMusicVolume(savedVolume);


    }

    public void RegisterSFXSlider(Slider slider)
    {
        SFXSlider = slider;
        float savedVolume = PlayerPrefs.GetFloat("SFXVol", 1f);
        SFXSlider.SetValueWithoutNotify(savedVolume);
        SFXSlider.onValueChanged.RemoveAllListeners();
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);

        SetSFXVolume(savedVolume);

    }
}
