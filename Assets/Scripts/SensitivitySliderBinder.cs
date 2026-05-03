using UnityEngine;
using UnityEngine.UI;


public class SensitivitySliderBinder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Slider slider = GetComponent<Slider>();
        MouseSettings settings = FindFirstObjectByType<MouseSettings>();

        if (settings != null && slider != null)
        {
            settings.RegisterSensitivitySlider(slider);
        }
    }

}
