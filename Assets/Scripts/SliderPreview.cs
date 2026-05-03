using UnityEngine;
using UnityEngine.EventSystems;
public class SliderPreview : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
   public SoundSettings settings;
    void Awake()
    {
        if(settings == null)
        settings = FindFirstObjectByType<SoundSettings>();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (settings == null) return;
        settings.StartPreview();
    }

    public void OnPointerUp(PointerEventData eventData) 
    {
        if (settings == null) return;
        settings.StopPreview();
    }
}
