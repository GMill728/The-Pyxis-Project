using UnityEngine;

public class Billboard_Behavior : MonoBehaviour {

    private Camera targetCam;
    
    void Start() { targetCam = Camera.main; }

    void LateUpdate() {
    
        Vector3 direction = targetCam.transform.position - transform.position;

        direction.y = 0; // stops object from "hovering"

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

    }
}
