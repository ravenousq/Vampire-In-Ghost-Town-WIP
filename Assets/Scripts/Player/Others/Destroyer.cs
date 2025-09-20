using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToDestroy;

    private void Start()
    {
        foreach (GameObject obj in objectsToDestroy)
            Destroy(obj);
        
        Destroy(gameObject);
    }
}
