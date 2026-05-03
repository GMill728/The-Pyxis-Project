using UnityEngine;
using UnityEngine.UI;


public class SFXSliderBinder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindFirstObjectByType<SoundSettings>().RegisterSFXSlider(GetComponent<Slider>());
    }
}