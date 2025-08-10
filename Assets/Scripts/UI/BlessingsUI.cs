using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BlessingsUI : MonoBehaviour
{
    [SerializeField] private GameObject skillsParent;
    [SerializeField] public SkillButtonUI[] skills;
    [SerializeField] private Image skillImage;
    [SerializeField] private SkillButtonUI defaultIndex;
    [SerializeField] private SkillDisplay skillDisplay;
    [SerializeField] private GameObject informationText;

    [Header("Campfire")]
    [SerializeField] private GameObject darkImage;
    [SerializeField] private GameObject[] dividers;
    [SerializeField] private float dividerScale = .65f;
    [SerializeField] private float topDividerOffset = 2f;
    [SerializeField] private float bottomDividerOffset = 2f;
    [SerializeField] private GameObject snippets;
    [SerializeField] private GameObject quitSnippet;
    [SerializeField] private float snippetOffset = 95f;
    [SerializeField] private GameObject mask;
    [SerializeField] private string campfireSnippetText;
    [SerializeField] private GameObject fade;
    [SerializeField] private RectTransform soulsUI;
    [SerializeField] private Vector2 soulsUIOffset;
    private string regularSnippetText;

    private bool campfireActive = false;    

    private int currentIndex;
    public bool triedToPurchase { get; private set; }

    private void Start()
    {
        skills = skillsParent.GetComponentsInChildren<SkillButtonUI>(true);
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].SetIndex(i, this);
        }

        skills[0].Select(true);
        skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription(), skills[currentIndex].GetPrice());
        skillImage.sprite = skills[currentIndex].skillImage;
        regularSnippetText = quitSnippet.GetComponent<TextMeshProUGUI>().text;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            NavigateTo(KeyCode.W);

        if (Input.GetKeyDown(KeyCode.A))
            NavigateTo(KeyCode.A);

        if (Input.GetKeyDown(KeyCode.S))
            NavigateTo(KeyCode.S);

        if (Input.GetKeyDown(KeyCode.D))
            NavigateTo(KeyCode.D);

        if (Input.GetKeyDown(KeyCode.C) && !triedToPurchase)
        {
            if (campfireActive && PlayerManager.instance.CanAfford(skills[currentIndex].GetIntPrice()) && skills[currentIndex].canBePurchased)
                skills[currentIndex].SetPurchase(true);
            else
            {
                skills[currentIndex].NotEnoughCurrency();
                triedToPurchase = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.C))
            skills[currentIndex].SetPurchase(false);

        if (Input.GetKeyDown(KeyCode.X) && campfireActive)
        {
            SetUpCampfire();
            UI.instance.campfireUI.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void RemoveSouls(int price)
    {
        UI.instance.ModifySouls(-price);
    }

    public void UpdateAll()
    {
        foreach (SkillButtonUI skill in skills)
            skill.UpdatePurchase();
    }

    private void NavigateTo(KeyCode keyCode)
    {
        skills[currentIndex].Select(false);
        currentIndex = skills[currentIndex].GetNavigation(keyCode);
        skills[currentIndex].Select(true);
        skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription(), skills[currentIndex].GetPrice());
        skillImage.sprite = skills[currentIndex].GetImage();
    }

    public Row GetRowByIndex(int index)
    {
        if (index < 0 || index >= skills.Length)
            return Row.Default;

        return skills[index].row;
    }

    private void OnEnable() {
        Start();
        UI.instance.ModifySouls(0);
        skills[currentIndex].Select(false);
        currentIndex = 0;
        skills[currentIndex].Select(true);
        skillDisplay.SetUp(skills[currentIndex].GetName(), skills[currentIndex].GetDescription(), skills[currentIndex].GetPrice());
        skillImage.sprite = skills[currentIndex].skillImage;
    }

    public void StopBlockade() => triedToPurchase = false;

    public void UnlockSecretSkill(string name)
    {
        Start();

        foreach (SkillButtonUI skill in skills)
        {
            if (skill.IsSecret() && skill.GetName(false) == name)
            {
                skill.Purchase(true);

                UpdateAll();

                return;
            }
        }
    }

    public void SetUpCampfire()
    {
        if (!campfireActive)
        {
            campfireActive = true;
            darkImage.SetActive(true);
            foreach (GameObject divider in dividers)
                divider.transform.localScale = Vector3.one * dividerScale;

            dividers[0].transform.position = dividers[0].transform.position - new Vector3(0, topDividerOffset, 0);
            dividers[1].transform.position = dividers[1].transform.position + new Vector3(0, bottomDividerOffset, 0);

            mask.gameObject.transform.SetParent(darkImage.transform);

            snippets.transform.position = new Vector3(snippets.transform.position.x, snippets.transform.position.y + snippetOffset, snippets.transform.position.z);

            quitSnippet.GetComponent<TextMeshProUGUI>().text = campfireSnippetText;

            fade.SetActive(true);
            
            soulsUI.anchoredPosition = new Vector2(soulsUI.anchoredPosition.x - soulsUIOffset.x, soulsUI.anchoredPosition.y - soulsUIOffset.y);
            soulsUI.SetParent(transform);

            informationText.SetActive(false);
        }
        else
        {
            campfireActive = false;
            darkImage.SetActive(false);
            foreach (GameObject divider in dividers)
                divider.transform.localScale = Vector3.one;

            dividers[0].transform.position = dividers[0].transform.position + new Vector3(0, topDividerOffset, 0);
            dividers[1].transform.position = dividers[1].transform.position - new Vector3(0, bottomDividerOffset, 0);

            mask.gameObject.transform.SetParent(transform);

            snippets.transform.position = new Vector3(snippets.transform.position.x, snippets.transform.position.y - snippetOffset, snippets.transform.position.z);

            quitSnippet.GetComponent<TextMeshProUGUI>().text = regularSnippetText;

            informationText.SetActive(false);

            fade.SetActive(false);

            soulsUI.anchoredPosition = new Vector2(soulsUI.anchoredPosition.x + soulsUIOffset.x, soulsUI.anchoredPosition.y + soulsUIOffset.y);
            soulsUI.SetParent(UI.instance.GetInGameUI().transform);

            transform.SetParent(UI.instance.GetGameMenu().transform);
        }
    }
}
