using UnityEngine;

public class Minimap : MonoBehaviour
{
   public Transform playerPosition;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = playerPosition.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
