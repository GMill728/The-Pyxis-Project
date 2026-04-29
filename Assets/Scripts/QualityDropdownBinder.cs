using TMPro;
using UnityEngine;

public class QualityDropdownBinder : MonoBehaviour
{
    void Start()
    {
        FindFirstObjectByType<GraphicQualitySettings>().RegisterQualityDropdown(GetComponent<TMP_Dropdown>());
    }

}
