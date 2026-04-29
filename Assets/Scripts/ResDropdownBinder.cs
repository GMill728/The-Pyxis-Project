using TMPro;
using UnityEngine;

public class ResDropdownBinder : MonoBehaviour
{
    void Start()
    {
        FindFirstObjectByType<GlobalSettingsManager>().RegisterResDropdown(GetComponent<TMP_Dropdown>());
    }

    public void Bind(GlobalSettingsManager manager)
    {
        manager.RegisterResDropdown(GetComponent<TMP_Dropdown>());
    }


}
