using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class Bloodstain : MonoBehaviour
{
    public CircleCollider2D cd { get; private set; }
    private TextMeshProUGUI inputImage;
    private AudioSource au;

    private void Awake()
    {
        cd = GetComponent<CircleCollider2D>();
        inputImage = GetComponentInChildren<TextMeshProUGUI>();
        au = GetComponent<AudioSource>();
    }

    [SerializeField] private float fadeSpeed;
    [SerializeField] private float ambientRange;
    [SerializeField] private AudioMixer mixer;
    public int soulsToRecover { get; private set; }
    private bool allowPickUp;
    private float defaultLowPass;
    private bool isPickedUp;

    private void Start()
    {
        defaultLowPass = GetLowpassFrequency();
    }

    private void Update()
    {
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

        if (Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) < ambientRange)
        {
            SetLowPassFrequency(Mathf.Lerp(100f, 5000f, Mathf.InverseLerp(0, ambientRange, Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position))));
            au.volume = Mathf.Clamp01(Mathf.InverseLerp(ambientRange, 0, Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position)));
        }
        else
        {
            SetLowPassFrequency(defaultLowPass);
            au.volume = 0;
        }
    }

    public void SetUpBloodstain(Vector3 pos, int souls)
    {
        transform.position = pos;
        soulsToRecover = souls;
    }

    private void PickUpSouls()
    {
        SetLowPassFrequency(defaultLowPass);
        isPickedUp = true;

        PlayerManager.instance.RecoveredSouls(soulsToRecover);
        Destroy(gameObject, .5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
            allowPickUp = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
            allowPickUp = false;
    }

    public void SetLowPassFrequency(float value)
    {
        if (isPickedUp)
            return;

        mixer.SetFloat("Frequency", value);
    }

    public float GetLowpassFrequency()
    {
        if (mixer.GetFloat("Frequency", out float value))
            return value;

        return -1f;
    }
}
