using UnityEngine;
using UnityEngine.UI;
public class MouseSettings : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;

    [SerializeField] private Player_Movement playerScript;
    void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        if (sensitivitySlider != null)
        {
            sensitivitySlider.SetValueWithoutNotify(savedSensitivity);
            sensitivitySlider.onValueChanged.RemoveAllListeners();
            sensitivitySlider.onValueChanged.AddListener(OnSliderChangeed);
        }

        ApplySensitivity(savedSensitivity);

    }

    public void RegisterPlayer(Player_Movement p)
    {
        playerScript = p;

        ApplySensitivity(PlayerPrefs.GetFloat("MouseSensitivity", 2f));

        //playerScript.SetSensitivity(PlayerPrefs.GetFloat("MouseSensitivity", 2f));
    }

    public void OnSliderChangeed(float value)
    {
        float sensClamped = Mathf.Clamp(value, 0.5f, 15f);
        PlayerPrefs.SetFloat("MouseSensitivity", sensClamped);
        PlayerPrefs.Save();
        ApplySensitivity(sensClamped);

    }

    public void RegisterSensitivitySlider(Slider slider)
    {
        sensitivitySlider = slider;
        float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        sensitivitySlider.SetValueWithoutNotify(savedSens);
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        sensitivitySlider.onValueChanged.AddListener(OnSliderChangeed);
        ApplySensitivity(savedSens);

    }

    void ApplySensitivity(float value)
    {
       playerScript.SetSensitivity(value);

    }

}
