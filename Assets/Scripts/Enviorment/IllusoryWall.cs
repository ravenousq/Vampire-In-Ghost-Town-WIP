using UnityEngine;
using UnityEngine.Tilemaps;

public class IllusoryWall : MonoBehaviour
{
    private Tilemap tr;
    private Collider2D wall;
    private TilemapCollider2D tilemapCollider;

    private void Awake()
    {
        tr = GetComponent<Tilemap>();
        wall = GetComponent<Collider2D>();
        tilemapCollider = GetComponent<TilemapCollider2D>();
    }

    [SerializeField] private float fadeSpeed;
    [SerializeField] private IllusoryWall twinWall;
    private bool failSafe;

    private void Update()
    {
        if (!wall.enabled || failSafe)
            tr.color = new Color(tr.color.r, tr.color.g, tr.color.b, Mathf.MoveTowards(tr.color.a, 0, fadeSpeed * Time.deltaTime));

        if (tr.color.a == 0)
        {
            LevelManager.instance.IllusoryWallFound(this);
            Destroy(gameObject);
        }
    }

    public void Activate(bool manual = true)
    {
        if (tilemapCollider != null)
        {
            tilemapCollider.enabled = false;
            failSafe = true;
        }

        wall.enabled = false;
        if (manual)
        {
            AudioManager.instance.PlaySFX(13, false);

            if (twinWall != null)
                twinWall.Activate(false);
        }
    }
}
