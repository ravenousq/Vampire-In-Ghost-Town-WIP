using UnityEngine;

public class FullMapCameraController : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxZoom, minZoom;
    private float startZoom;

    private void Start()
    {
        startZoom = cam.orthographicSize;
    }

    private void OnEnable()
    {
        if(PlayerManager.instance)
            transform.position = PlayerManager.instance.player.transform.position + Vector3.back * 10;
    }

}
