using UnityEngine;

public class Pickup_Handler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        
        if (other.CompareTag("Player")) {
         
            // Handle Pickup Actions

            Destroy(gameObject);
        }
    }
}