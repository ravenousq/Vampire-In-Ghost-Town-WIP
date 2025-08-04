using UnityEngine;
using UnityEngine.Tilemaps;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;

    private void Awake()
    {
        effector = GetComponentInChildren<PlatformEffector2D>();
    }

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsDefault;
    private Player player;
    private bool collisionActive;
    private float defaultArc;

    private void Start()
    {
        player = PlayerManager.instance.player;
        defaultArc = effector.surfaceArc;
    }

    private void Update()
    {
        if (collisionActive && player.IsCrouching() && Input.GetKeyDown(KeyCode.Space))
        {
            player.noGroundDetection = true;
        }

        effector.surfaceArc = player.noGroundDetection ? 0 : effector.surfaceArc;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>().GetComponent<CapsuleCollider2D>())
        {
            collisionActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>().GetComponent<CapsuleCollider2D>())
        {
            collisionActive = false;
            effector.surfaceArc = defaultArc;
        }
    }

}
