using UnityEngine;
using UnityEngine.UI;

public class UIButtonSFX : MonoBehaviour
{
    private Button button;
    [SerializeField] private SFXType clickType = SFXType.ButtonClick;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayClick);
    }
    
    void PlayClick()
    {
        AudioManager.Instance.PlaySFX(clickType);
    }
}
