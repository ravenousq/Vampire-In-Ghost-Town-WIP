using TMPro;
using UnityEngine;

public class ConfirmationDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private TextMeshProUGUI confirmButton;
    [SerializeField] private TextMeshProUGUI declineButton;
    [SerializeField] private Color highlightedColor;
    [SerializeField] private Color defaultColor;
    [SerializeField] private float highlightedFontSize = 20f;
    [SerializeField] private float defaultFontSize;
    public System.Action onConfirm;
    public System.Action onDecline;
    [SerializeField] private bool highlightedOption;
    private GameObject wakeUp;

    void Start()
    {
        defaultColor = confirmButton.color;
        defaultFontSize = confirmButton.fontSize;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            SwitchOption();

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (highlightedOption)
                onConfirm?.Invoke();
            else
                onDecline?.Invoke();
            
            onConfirm = null;
            onDecline = null;

            if (wakeUp != null)
                wakeUp.SetActive(true);

            gameObject.SetActive(false);
        }
    }

    public void SetUp(string information, GameObject wakeUp, string confirmationText, string cancelText = "")
    {
        declineButton.gameObject.SetActive(cancelText != "");
        highlightedOption = cancelText == "";
        this.wakeUp = wakeUp;

        if(wakeUp != null)
            wakeUp.SetActive(false);

        informationText.text = information;
        confirmButton.text = confirmationText;
        declineButton.text = cancelText;
        AdjustOptions();
    }

    private void AdjustOptions()
    {
        confirmButton.color = highlightedOption ? highlightedColor : defaultColor;
        confirmButton.fontSize = highlightedOption ? highlightedFontSize : defaultFontSize;

        declineButton.color = highlightedOption ? defaultColor : highlightedColor;
        declineButton.fontSize = highlightedOption ? defaultFontSize : highlightedFontSize;
    }

    public void SwitchOption()
    {
        if (!declineButton.gameObject.activeSelf)
            return;

        highlightedOption = !highlightedOption;

        AdjustOptions();
    }
    
    

}
