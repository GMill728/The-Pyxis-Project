using System.Xml.Schema;
using UnityEngine;

public class EnableCursor : MonoBehaviour
{
    //Simple script to enable cursor on opening of scenes without pause controlers like the main menu and game over scenes
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
