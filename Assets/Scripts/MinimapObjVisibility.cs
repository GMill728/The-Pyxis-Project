using UnityEngine;

public class MinimapObjVisibility : MonoBehaviour
{
    public Collider room;
    public GameObject minimapRoom;
    public GameObject floor;
    private Renderer mapRenderer;

    void Start()
    {
        minimapRoom.SetActive(false);
        mapRenderer = floor.GetComponent<Renderer>(); // fixed here
    }
        
    void OnTriggerEnter(Collider other)
    {
        minimapRoom.SetActive(true);
        mapRenderer.material.SetColor("_Color", Color.red);
    }

    void OnTriggerExit(Collider other)
    {
        minimapRoom.SetActive(true);
        mapRenderer.material.SetColor("_Color", Color.white);
    }
}