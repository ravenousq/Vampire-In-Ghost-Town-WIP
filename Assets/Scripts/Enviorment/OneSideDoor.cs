
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OneSideDoor : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D cd;
    private SpriteRenderer sr;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<BoxCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    [SerializeField] private bool requiredRight = true;
    [SerializeField] private float openDistance;
    [SerializeField] private GameObject inputPoint;
    [SerializeField] private TextMeshProUGUI inputImage;
    [SerializeField] private float fadeSpeed = 1;
    private bool inputBlocked = false;

    private void Start()
    {
        openDistance += cd.bounds.size.x / 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && InRange() && Time.timeScale == 1 && !inputBlocked)
        {
            if (CanBeOpened())
                Open();
            else
            {
                UI.instance.SetUpConfirmationDialogue("Door cannot be opened from this side", null, "Okay");
                UI.instance.confirmationDialogue.onConfirm += BlockInput;
                BlockInput();
            }
        }

        inputImage.color = new Color(
            inputImage.color.r,
            inputImage.color.g,
            inputImage.color.b,
            Mathf.MoveTowards(
                inputImage.color.a,
                InRange() ? 255 : 0,
                fadeSpeed * Time.unscaledDeltaTime
            )
        );

    }

    private void BlockInput() => inputBlocked = !inputBlocked;

    public void Open()
    {
        anim.SetTrigger("open");
        cd.enabled = false;
        LevelManager.instance.OneSideDoorOpened(this);
    }

    private bool CanBeOpened()
    {
        if (!cd.enabled)
            return false;
        Player player = PlayerManager.instance.player;
        float playerX = player.transform.position.x;
        float doorX = transform.position.x;
        return InRange() &&
            (playerX - doorX > 0 == requiredRight);
    }

    private bool InRange() => Vector3.Distance(PlayerManager.instance.player.transform.position, transform.position) < openDistance && cd.enabled;
    // private bool funkcja()
    // {
    //     Vector = Button;
    //     if button clicked = true
    //     then door = open
    //     ALE
    //     player on the left = 1
    //     door opened = Left = true
    //     player on the right = 1
    //     door opened = right = true
    // }
}
