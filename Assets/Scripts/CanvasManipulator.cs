using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManipulator : MonoBehaviour
{
    public Transform zoomCanvas;
    public Vector3 ogPOS;

    public float moveDuration = 1.5f;

    public GameObject firstCanvas;
    public GameObject secondCanvas;
    public GameObject thirdCanvas;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        ogPOS = cam.transform.position;
        secondCanvas.SetActive(false);
        thirdCanvas.SetActive(false);
    }

    public void ZoomIn()
    {
        firstCanvas.SetActive(false);
        secondCanvas.SetActive(true);
        StartCoroutine(MoveCamera(cam.transform.position, zoomCanvas.position));
    }

    public void ZoomOut()
    {
        StartCoroutine(MoveCamera(cam.transform.position, ogPOS, true));

    }

    IEnumerator MoveCamera(Vector3 start, Vector3 end, bool returning = false)
    {
        float elapsed = 0f;
        while(elapsed < moveDuration)
        {
            cam.transform.position = Vector3.Lerp(start, end, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.position = end;

        if(returning)
        {
            secondCanvas.SetActive(false);
            firstCanvas.SetActive(true);
        }
    }

    public void ThirdCanvasActive()
    {
        firstCanvas.SetActive(false);
        thirdCanvas.SetActive(true);
    }
    public void ThirdCanvasDeactivate()
    {
        thirdCanvas.SetActive(false);
        firstCanvas.SetActive(true);
    }

}
