using UnityEngine;
using UnityEngine.UI;

public class LightTile : MonoBehaviour
{
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public PuzzleManager board;

    public bool isOn = true;

    private Image img;

    [Header("Colors")]
    public Color onColor = Color.yellow;
    public Color offColor = Color.black;

    void Start()
    {
        img = GetComponent<Image>();
        UpdateVisual();

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        board.PressTile(x, y);
    }

    public void Toggle()
    {
        isOn = !isOn;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        img.color = isOn ? onColor : offColor;
    }
}