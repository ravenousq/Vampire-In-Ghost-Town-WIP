using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect;

    private float xPosition;
    private float yPosition;
    private float length;
    private float height;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        length = GetComponentInChildren<SpriteRenderer>().bounds.size.x;

        xPosition = transform.position.x;
    }

    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, cam.transform.position.y);

        if(distanceMoved > xPosition + length)
            xPosition += length;
        else if(distanceMoved < xPosition - length)
            xPosition -= length;
    }
}