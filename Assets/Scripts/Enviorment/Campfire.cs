using UnityEngine;
using TMPro;
using System;

public class Campfire : MonoBehaviour, ISaveManager
{
    public Animator anim { get; private set; }
    public CircleCollider2D cd { get; private set; }
    public AudioSource au { get; private set; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CircleCollider2D>();
        au = GetComponent<AudioSource>();
    }

    [Header("Campfire")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private string campfireName;
    [SerializeField] private Transform audioPoint;

    [Header("Input")]
    [SerializeField] private TextMeshProUGUI inputImage;
    [SerializeField] private float fadeSpeed;
    private Player player;
    private bool menuActive;
    private bool used;

    private void Update()
    {
        if (player && Input.GetKeyDown(KeyCode.C) && !menuActive && Time.timeScale != 0)
        {
            used = true;
            player.RestAtCampfire();
            UI.instance.campfireUI.gameObject.SetActive(true);
            UI.instance.campfireUI.SetUpCampfire(this);
            menuActive = true;
        }

        AdjustDirectionalSound.Adjuster(au, PlayerManager.instance.player, 10);

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

    public void LoadData(GameData data)
    {

    }

    public void SaveData(ref GameData data)
    {
        if (!used)
            return;

        data.spawnPosition = null;
        data.spawnPosition = new float[] {spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z};
    }
}
