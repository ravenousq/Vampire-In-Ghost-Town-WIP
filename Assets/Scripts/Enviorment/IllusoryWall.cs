using UnityEngine;
using UnityEngine.Tilemaps;

public class IllusoryWall : MonoBehaviour
{
    private Tilemap tr;

    private void Awake()
    {
        tr = GetComponent<Tilemap>();
    }

    [SerializeField] private int requiredFacingDir;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private BoxCollider2D wall;
    private Player player;

    private void Update() 
    {
        if (player && player.attack.shotTriggered && player.facingDir == requiredFacingDir && Time.timeScale != 0 && player.IsGroundDetected() && wall.enabled)
        {
            wall.enabled = false;
            AudioManager.instance.PlaySFX(13, false);
        }

        if(!wall.enabled)
            tr.color = new Color(tr.color.r, tr.color.g, tr.color.b, Mathf.MoveTowards(tr.color.a, 0, fadeSpeed * Time.deltaTime));

        if (tr.color.a == 0)
        {
            LevelManager.instance.IllusoryWallFound(this);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
            player  = other.GetComponent<Player>();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.GetComponent<Player>())
            player = null;
    }
}
