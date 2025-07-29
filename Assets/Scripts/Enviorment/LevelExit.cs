using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private BoxCollider2D cd;

    void Awake()
    {
        cd = GetComponent<BoxCollider2D>();
    }

    [SerializeField] private Transform objective;
    private bool isTriggered;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered)
            return;

        if (other.GetComponent<Player>())
        {
            isTriggered = true;
            PlayerManager.instance.player.MoveTowardsObjective(objective);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTriggered = false;
    }
}
