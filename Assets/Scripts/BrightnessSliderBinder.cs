using UnityEngine;
using UnityEngine.UI;


public class BrightnessSliderBinder : MonoBehaviour
{
    void Start()
    {
        FindFirstObjectByType<GlobalSettingsManager>().RegisterBrightnessSlider(GetComponent<Slider>());

    }
    
    public void Bind(GlobalSettingsManager manager)
    {
        manager.RegisterBrightnessSlider(GetComponent<Slider>());
    }


}
