using UnityEngine;

public class TerminalDemo : MonoBehaviour
{
    public SceneLoader sceneLoader;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Load Scene!");
            sceneLoader.LoadSceneByName("Demo");
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