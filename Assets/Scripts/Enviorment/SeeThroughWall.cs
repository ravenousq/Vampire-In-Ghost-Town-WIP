using UnityEngine;
using UnityEngine.Tilemaps;

public class SeeThroughWall : MonoBehaviour
{
    private Tilemap[] tilemaps;

    private void Awake() => tilemaps = GetComponentsInChildren<Tilemap>();

    private bool fadeOut;
    private bool fadeIn;
    [SerializeField] private float fadeSpeed = 2f;

    private void Update()
    {
        if (fadeIn && !fadeOut)
        {
            foreach(Tilemap tilemap in tilemaps)
                tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, Mathf.MoveTowards(tilemap.color.a, 1, fadeSpeed * Time.unscaledDeltaTime));
            fadeIn = tilemaps[0].color.a != 1;
        }
        else if (fadeOut && !fadeIn)
        {
            foreach (Tilemap tilemap in tilemaps)
                tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, Mathf.MoveTowards(tilemap.color.a, 0, fadeSpeed * Time.unscaledDeltaTime));
            fadeOut = tilemaps[0].color.a != 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            fadeIn = false;
            fadeOut = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            fadeIn = true;
            fadeOut = false;
        }
    }
}
