using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalSettingsManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resDropdown;
    private UnityEngine.Resolution[] resolutions;
    private List<UnityEngine.Resolution> filteredRes;
    private double currentRefreshRate;

    public static GlobalSettingsManager Instance;
    public Slider brightSlider;
    public PostProcessProfile brightnessProfile;
    public PostProcessLayer layer;
    ColorGrading colorGrade;
   

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {

        /*float savedBright = PlayerPrefs.GetFloat("SavedBrightness", 0.8f);

        AdjustBrightness(savedBright);
        */
        ApplyAllSettings();
    }

    public void SetUpResDropDown()
    {
        if (resDropdown == null)
        {
            return;
        }

        resolutions = Screen.resolutions;
        filteredRes = new List<UnityEngine.Resolution>();

        resDropdown.ClearOptions();
        int currentResIndex = 0;
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution res = resolutions[i];
            string option = resolutions[i].width + "x" + resolutions[i].height;
            if (!options.Contains(option))
            {
                options.Add(option);
                filteredRes.Add(resolutions[i]);
            }

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = filteredRes.Count - 1;
            }
            

        }

        resDropdown.AddOptions(options);
        resDropdown.SetValueWithoutNotify(currentResIndex);
        resDropdown.RefreshShownValue();
    }
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerPrefs.Save();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PostProcessVolume volume = FindFirstObjectByType<PostProcessVolume>();
        if (volume != null)
        {
            brightnessProfile = volume.profile;
            if(brightnessProfile.TryGetSettings(out colorGrade))
            {
                float saved = PlayerPrefs.GetFloat("SavedBrightness", 0.8f);
                colorGrade.postExposure.overrideState = true;
                colorGrade.postExposure.value = saved;
            }
        }


        ResDropdownBinder resBinder = FindFirstObjectByType<ResDropdownBinder>(FindObjectsInactive.Include);
        if(resBinder != null)
        {
            resBinder.Bind(this);
        }

        BrightnessSliderBinder brightBinder = FindFirstObjectByType<BrightnessSliderBinder>(FindObjectsInactive.Include);
        if (brightBinder != null)
        {
            brightBinder.Bind(this);
        }

        ApplyAllSettings();
    }

    public void RegisterBrightnessSlider(Slider slider)
    {
        brightSlider = slider;
        float savedBrightness = PlayerPrefs.GetFloat("SavedBrightness", 0.8f);
        brightSlider.SetValueWithoutNotify(savedBrightness);
        brightSlider.onValueChanged.RemoveAllListeners();
        brightSlider.onValueChanged.AddListener(AdjustBrightness);

    }

    public void RegisterResDropdown(TMP_Dropdown dropdown)
    {
        resDropdown = dropdown;

        SetUpResDropDown();
        int savedWidth = PlayerPrefs.GetInt("ResWidth", Screen.currentResolution.width);
        int savedHeight = PlayerPrefs.GetInt("ResHeight", Screen.currentResolution.height);

        int index = 0;
        for (int i = 0; i < filteredRes.Count; i++)
        {
            if (filteredRes[i].width == savedWidth && filteredRes[i].height == savedHeight)
            {
                index = i;
                break;
            }
        }

        resDropdown.SetValueWithoutNotify(index);
        resDropdown.onValueChanged.RemoveAllListeners();
        resDropdown.onValueChanged.AddListener(SetRes);
    }
    public void SetRes(int index)
    {
        if (filteredRes == null || filteredRes.Count == 0)
        {
            return;
        }

        UnityEngine.Resolution resolution = filteredRes[index];

        PlayerPrefs.SetInt("ResWidth", resolution.width);
        PlayerPrefs.SetInt("ResHeight", resolution.height);
        PlayerPrefs.Save();
        ApplyAllSettings();

    }
    public void AdjustBrightness(float value)
    {
        float ClampedValue = Mathf.Clamp(value, -3.0f, 1.8f);
        if (colorGrade != null)
        {
            colorGrade.postExposure.overrideState = true;
            colorGrade.postExposure.value = ClampedValue;
        }

        PlayerPrefs.SetFloat("SavedBrightness", ClampedValue);
    }

    public void ApplyAllSettings()
    {
        if (PlayerPrefs.HasKey("ResWidth"))
        {
            int w = PlayerPrefs.GetInt("ResWidth");
            int h = PlayerPrefs.GetInt("ResHeight");
            Screen.SetResolution(w, h, Screen.fullScreen);
        }

        if (colorGrade != null)
        {
            colorGrade.postExposure.overrideState = true;
            colorGrade.postExposure.value = PlayerPrefs.GetFloat("SavedBrightness", 0.8f);
        }
        
    }
}
