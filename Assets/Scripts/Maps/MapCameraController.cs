using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D bounds;
    [SerializeField] private bool heedBounds = true;
    private float halfHeight;
    private float halfWidth;
    private Player player;

    private void Start()
    {
        player = PlayerManager.instance.player;

        halfHeight = GetComponent<Camera>().orthographicSize;
        halfWidth = halfHeight * GetComponent<Camera>().aspect;
    }

    private void Update()
    {
        if (heedBounds)
            transform.position = new Vector3(
                    Mathf.Clamp(player.transform.position.x, bounds.bounds.min.x + halfWidth, bounds.bounds.max.x - halfWidth),
                    Mathf.Clamp(player.transform.position.y, bounds.bounds.min.y + halfHeight, bounds.bounds.max.y - halfHeight),
                    transform.position.z
                );
        else
            transform.position = new Vector3(
                    player.transform.position.x,
                    player.transform.position.y,
                    transform.position.z
                );
    }
}
