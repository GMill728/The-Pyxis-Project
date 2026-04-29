using UnityEngine;
using TMPro;

public class GraphicQualitySettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown qualityDropdown;

    void Start()
    {
        int savedQuality = PlayerPrefs.GetInt("QualityLevel", UnityEngine.QualitySettings.GetQualityLevel());

        ApplyQuality(savedQuality);
    }

    public void RegisterQualityDropdown(TMP_Dropdown dropdown)
    {
        qualityDropdown = dropdown;

        int savedQuality = PlayerPrefs.GetInt("QualityLevel", UnityEngine.QualitySettings.GetQualityLevel());
        qualityDropdown.SetValueWithoutNotify(savedQuality);
        qualityDropdown.onValueChanged.RemoveAllListeners();
        qualityDropdown.onValueChanged.AddListener(OnChange);
    }

    public void OnChange(int index)
    {
        ApplyQuality(index);

        PlayerPrefs.SetInt("QualityLevel", index);
        PlayerPrefs.Save();

    }

    private void ApplyQuality(int index)
    {
        UnityEngine.QualitySettings.SetQualityLevel(index, true);
    }
}
