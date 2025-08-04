using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;
    public NPCShopUI npcShop { get; private set; }
    public CampfireUI campfireUI { get; private set; }
    public FadeScreen fadeScreen { get; private set; }
    public PauseMenu pauseMenu { get; private set; }
    public BloodstainText bloodstain { get; private set; }

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        npcShop = GetComponentInChildren<NPCShopUI>(true);
        campfireUI = GetComponentInChildren<CampfireUI>(true);
        fadeScreen = GetComponentInChildren<FadeScreen>(true);
        pauseMenu = GetComponentInChildren<PauseMenu>(true);
        bloodstain = GetComponentInChildren<BloodstainText>(true);
    }

    [Header("Inventory")]
    public Transform notesParent;
    public Transform keyItemsParent;
    public Transform charmsParent;
    public Transform equipedCharmsParent;
    public ItemDescriptionUI itemDescription;


    [Header("Game Menu")]
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameMenuButton[] menuButtons;
    [SerializeField] private ItemsUI itemsTab;
    [SerializeField] private GameObject mapTab;
    [SerializeField] private CharmsUI charmsTab;
    [SerializeField] private BlessingsUI blessingsTab;
    [SerializeField] private NotesUI notesTab;

    //[Header("Pause Menu")]

    [Space]
    [SerializeField] private GameObject InGameUI;
    [SerializeField] private SoulsUI inGameSoulsUI;
    public bool canTurnOnGameMenu { get; private set; } = true;
    public bool canTurnOnPauseMenu { get; private set; } = true;

    [Space]
    public ConfirmationDialogue confirmationDialogue;

    [Space]
    public ConcoctionUI concoctionUI;



    [Header("Debug")]
    [SerializeField] private NPC amelia;
    private int selectedIndex;

    private void Start()
    {
        gameMenu.SetActive(true);

        blessingsTab.TabSwitch();

        for (int i = 0; i < menuButtons.Length; i++)
        {
            menuButtons[i].Select(true);
            menuButtons[i].Select(false);

        }
        gameMenu.SetActive(false);
        campfireUI.gameObject.SetActive(false);
        confirmationDialogue.gameObject.SetActive(false);

        pauseMenu.gameObject.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab) && canTurnOnGameMenu)
        {
            // Cursor.visible = !Cursor.visible;
            // Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Confined : CursorLockMode.None;

            EnableUI(Time.timeScale == 0);

            gameMenu.SetActive(!gameMenu.activeSelf);

            canTurnOnPauseMenu = !gameMenu.activeSelf;

            if (gameMenu.activeSelf)
                DefaultMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && canTurnOnPauseMenu)
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = Cursor.lockState == CursorLockMode.None ? CursorLockMode.Confined : CursorLockMode.None;

            pauseMenu.gameObject.SetActive(!pauseMenu.gameObject.activeSelf);
        }

        if (gameMenu.activeSelf)
            NavigateTabs();
    }

    public void SwitchShop(NPC npc, int index)
    {
        Time.timeScale = !npcShop.gameObject.activeSelf ? 0 : 1;
        EnableNPCShop(!npcShop.gameObject.activeSelf);
        npcShop.SetUp(npc, index);
    }

    private void NavigateTabs()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (selectedIndex == 4)
                notesTab.Reset();
            else if (selectedIndex == 2)
                charmsTab.TabSwitch();

            menuButtons[selectedIndex].Select(false);
            selectedIndex--;

            if (selectedIndex < 0)
                selectedIndex = menuButtons.Length - 1;

            if (selectedIndex == 3)
                blessingsTab.TabSwitch();

            menuButtons[selectedIndex].Select(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (selectedIndex == 4)
                notesTab.Reset();
            else if (selectedIndex == 2)
                charmsTab.TabSwitch();
            else if (selectedIndex + 1 == 3)
                blessingsTab.TabSwitch();

            menuButtons[selectedIndex].Select(false);
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;

            if (selectedIndex == 3)
                blessingsTab.TabSwitch();

            menuButtons[selectedIndex].Select(true);
        }
    }

    public void EnableNPCShop(bool enable)
    {
        npcShop.gameObject.SetActive(enable);
    }

    private void DefaultMenu()
    {
        foreach (GameMenuButton button in menuButtons)
        {
            if (button == menuButtons[0])
                button.Select(true);
            else
                button.Select(false);
        }

        selectedIndex = 0;
    }

    public void EnableUI(bool enable)
    {
        if (!enable)
        {
            Time.timeScale = 0;

            Inventory.instance.UpdateSlotUI();
            itemsTab.SwitchTo();
        }
        else
        {
            notesTab.Reset();
            blessingsTab.TabSwitch();
            charmsTab.TabSwitch();
            Time.timeScale = 1;
        }
    }

    public void UnlockSecretSkill(string name) => blessingsTab.UnlockSecretSkill(name);


    public void ModifySouls(int souls = 0, bool wait = true)
    {
        if (souls != 0)
            inGameSoulsUI.ModifySouls(souls, wait);
        else
            inGameSoulsUI.UpdateSouls();
    }

    public void SetUpConfirmationDialogue(string information, GameObject wakeUp, string confirmationText, string cancelText = "")
    {
        confirmationDialogue.SetUp(information, wakeUp, confirmationText, cancelText);
        confirmationDialogue.gameObject.SetActive(true);
    }

    public void LockGameMenu() => canTurnOnGameMenu = !canTurnOnGameMenu;
    public void LockPauseMenu() => canTurnOnPauseMenu = !canTurnOnPauseMenu;
    public GameObject GetGameMenu() => gameMenu;
    public GameObject GetInGameUI() => InGameUI;
}
