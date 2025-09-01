using UnityEngine;

public class MapCover : MonoBehaviour
{
    private SpriteRenderer[] sr;

    void Awake()
    {
        sr = GetComponentsInChildren<SpriteRenderer>();
    }

    [SerializeField] private float fadeSpeed = 1;
    private bool trigger;

    void Update()
    {
        if (trigger)
            foreach (var sr in sr)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(sr.color.a, 0, fadeSpeed * Time.unscaledDeltaTime));

        if (sr[0].color.a <= 0.01f)
            MapManager.instance.CoverDispelled(this);
    }

    public void Dispell() => trigger = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && !trigger)
            Dispell();
    }
    
    private void OnValidate()
    {
        int index = transform.GetSiblingIndex() + 1; 
        gameObject.name = $"Map Cover - {index}";    
    }
}
