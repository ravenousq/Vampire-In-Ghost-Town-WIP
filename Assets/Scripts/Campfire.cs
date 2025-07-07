using UnityEngine;
using TMPro;

public class Campfire : MonoBehaviour
{
    public Animator anim { get; private set; }
    public CircleCollider2D cd { get; private set; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CircleCollider2D>();
    }

    [Header("Campfire")]
    [SerializeField] private string campfireName;

    [Header("Input")]
    [SerializeField] private TextMeshProUGUI inputImage;
    [SerializeField] private float fadeSpeed;
    private Player player;
    private bool menuActive;
    private void Update()
    {
        if (player && Input.GetKeyDown(KeyCode.C) && !menuActive)
        {
            player.RestAtCampfire();
            UI.instance.campfireUI.gameObject.SetActive(true);
            UI.instance.campfireUI.SetUpCampfire(this);
            menuActive = true;
        }

        inputImage.color = new Color(
            inputImage.color.r,
            inputImage.color.g,
            inputImage.color.b,
            Mathf.MoveTowards(
                inputImage.color.a,
                player && !menuActive ? 255 : 0,
                fadeSpeed * Time.unscaledDeltaTime
            )
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        player = other.GetComponent<Player>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
            player = null;
    }

    public void StandUp()
    {
        UI.instance.campfireUI.gameObject.SetActive(false);
        menuActive = false;
        player.rest.StandUp();
    }
}
