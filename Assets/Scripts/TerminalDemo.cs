using UnityEngine;

public class TerminalDemo : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Load Scene!");
            SceneLoader.LoadSceneByName("Puzzle");
            Debug.Log("Player entered the zone!");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left the zone!");
        }   
    }
}