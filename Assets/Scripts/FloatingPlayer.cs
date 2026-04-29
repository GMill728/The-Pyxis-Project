using UnityEngine;

public class FloatingPlayer : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateeSpeed;

    private Vector2 direction = new Vector2(1, 1);
    [SerializeField] private RectTransform characterTransform;
    [SerializeField] private RectTransform canvasTransform;
    void Start()
    {
        speed = 200f;
        rotateeSpeed = 40f;
        characterTransform = GetComponent<RectTransform>();
        canvasTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();


        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        characterTransform.anchoredPosition += direction * speed * Time.deltaTime;
        characterTransform.Rotate(0,0, rotateeSpeed*Time.deltaTime);
        CheckBounds();
    }

    void CheckBounds()
    {
        Vector2 pos = characterTransform.anchoredPosition;
        float halfWidth = characterTransform.rect.width / 5;
        float halfHeight = characterTransform.rect.height / 3;

        float canvasW = canvasTransform.rect.width/2;
        float canvasH = canvasTransform.rect.height/2;

        if(pos.x + halfWidth >= canvasW)
        {
            pos.x = canvasW - halfWidth;
            direction.x *= -1;
            rotateeSpeed *= -1;
        }
        else if(pos.x - halfWidth <= -canvasW)
        {
            pos.x = -canvasW + halfWidth;
            direction.x *= -1;
            rotateeSpeed *= -1;
        }

        if (pos.y + halfHeight >= canvasH) 
        {
            pos.y = canvasH - halfHeight;
            direction.y *= -1;
           // rotateeSpeed *= -1;
        }
        else if (pos.y - halfHeight <= -canvasH)
        {
            pos.y = -canvasH + halfHeight;
            direction.y *= -1;
           // rotateeSpeed *= -1;
        }

        characterTransform.anchoredPosition = pos;
    }
}
