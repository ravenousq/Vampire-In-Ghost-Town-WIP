using System.Collections;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private BoxCollider2D cd;

    void Awake()
    {
        cd = GetComponent<BoxCollider2D>();
    }

    public Transform exitPoint;
    public Transform enterPoint;
    public int index;
    [SerializeField] private int targetIndex;
    [SerializeField] private string targetSceneName;
    private bool isTriggered;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered)
            return;

        if (other.GetComponent<Player>())
        {
            isTriggered = true;
            PlayerManager.instance.ExitLevel(exitPoint, targetSceneName, targetIndex);
        }
    }

    public void DisableCollider() => StartCoroutine(DisableColliderRoutine()); 

    private IEnumerator DisableColliderRoutine()
    {
        isTriggered = true;

        yield return new WaitForSecondsRealtime(2f);

        isTriggered = false;
    }

}
