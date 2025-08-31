using UnityEngine;

public class MapCover : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    [SerializeField] private float fadeSpeed = 1;
    private bool trigger;

    void Update()
    {
        if (trigger)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(sr.color.a, 0, fadeSpeed * Time.unscaledDeltaTime));
        

        if (sr.color.a <= 0.01f)
            MapManager.instance.CoverDispelled(this);
    }

    public void Dispell() => trigger = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() && !trigger)
            Dispell();  
    }
}
