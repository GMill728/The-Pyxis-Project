using UnityEngine;
using UnityEngine.UI;


public class VolumeSliderBinder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindFirstObjectByType<SoundSettings>().RegisterVolumeSlider(GetComponent<Slider>());
    }
}