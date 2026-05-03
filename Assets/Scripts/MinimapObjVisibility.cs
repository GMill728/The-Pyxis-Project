using UnityEngine;

public class MinimapObjVisibility : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Collider room;

    bool isActive = false;
    
    void Start()
    {
        activateMap(false);
    }
        

    // Update is called once per frame
    void Update(){}
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            activateMap(true);
            isActive = true;
        }
    }

    private void activateMap(bool active){
        // foreach (Transform child in transform)
        //         {
        //             child.gameObject.SetActive(active);
        //         }
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.enabled = active;
            }
    }
}