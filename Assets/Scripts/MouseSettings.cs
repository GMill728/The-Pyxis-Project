using UnityEngine;
using UnityEngine.UI;


public class MouseSettings : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
   
    [SerializeField] private Player_Movement playerScript;
    void Start()
    {
        float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
       if (sensitivitySlider != null )
        {
            sensitivitySlider.SetValueWithoutNotify(savedSensitivity);
            sensitivitySlider.onValueChanged.RemoveAllListeners();
            sensitivitySlider.onValueChanged.AddListener(OnSliderChangeed);
        }

    }
    
    public void RegisterPlayer(Player_Movement p)
    {
        playerScript = p;

        playerScript.SetSensitivity(PlayerPrefs.GetFloat("MouseSensitivity", 2f));
    }

    public void OnSliderChangeed(float value)
    {
        float sensClamped = Mathf.Clamp(value, 0.2f, 15f);
        PlayerPrefs.SetFloat("MouseSensitivity", sensClamped);
        PlayerPrefs.Save();
        
    }

    public void RegisterSensitivitySlider(Slider slider)
    {
        sensitivitySlider = slider;
        float savedSens = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        sensitivitySlider.SetValueWithoutNotify(savedSens);
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        sensitivitySlider.onValueChanged.AddListener(OnSliderChangeed);

    }

}
