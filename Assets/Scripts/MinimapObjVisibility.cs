using UnityEngine;

public class MinimapObjVisibility : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Collider room;
    public GameObject minimapRoom;

    bool isActive = false;
    
    void Start()
    {
       minimapRoom.SetActive(false);
    }
        
    // Update is called once per frame
    void Update(){}
    void OnTriggerEnter(Collider other)
    {
        minimapRoom.SetActive(true);
    }

}