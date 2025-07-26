using TMPro;
using UnityEngine;

public class Bloodstain : MonoBehaviour
{
    public CircleCollider2D cd { get; private set; }
    private TextMeshProUGUI inputImage;

    private void Awake()
    {
        cd = GetComponent<CircleCollider2D>();
        inputImage = GetComponentInChildren<TextMeshProUGUI>();
    }

    [SerializeField] private float fadeSpeed;
    public int soulsToRecover { get; private set; }
    private bool allowPickUp;

    private void Update()
    {
        Debug.Log(allowPickUp);

        inputImage.color = new Color(
            inputImage.color.r,
            inputImage.color.g,
            inputImage.color.b,
            Mathf.MoveTowards(
                inputImage.color.a,
                allowPickUp ? 255 : 0,
                fadeSpeed * Time.unscaledDeltaTime
            )
        );

        if (allowPickUp && Input.GetKeyDown(KeyCode.C) && Time.timeScale == 1)
            PickUpSouls();
    }

    public void SetUpBloodstain(Vector3 pos, int souls)
    {
        transform.position = pos;
        soulsToRecover = souls;
    }

    private void PickUpSouls()
    {
        PlayerManager.instance.RecoveredSouls(soulsToRecover);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Player>())
            allowPickUp = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.GetComponent<Player>())
            allowPickUp = false;
    }
}
