using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NotesUI : ItemsUI
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float defaultFade;
    [SerializeField] private GameObject noteContentDisplay;
    [SerializeField] private Scrollbar scroll;
    [SerializeField] private TextMeshProUGUI noteContentText;
    private bool focused;
    protected override void Start()
    {
        base.Start();

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        noteContentDisplay.SetActive(false);
    }

    protected override void Update()
    {
        if (!focused)
            base.Update();

        if (currentData != null && Input.GetKeyDown(KeyCode.C))
        {
            focused = !focused;
            noteContentDisplay.SetActive(focused);
            noteContentText.text = currentData.noteContent != null ? currentData.noteContent : "No content available.";
            StartCoroutine(ResetScrollBar());
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, focused ? defaultFade : 0);
        }

        if (Input.GetKey(KeyCode.W) && scroll.value != 1)
            scroll.value += .75f * Time.unscaledDeltaTime;
        else if (Input.GetKey(KeyCode.S) && scroll.value != 0)
            scroll.value -= .75f * Time.unscaledDeltaTime;
    }

    private IEnumerator ResetScrollBar()
    {
        yield return null;
        scroll.value = 1;
    }

    private void OnEnable()
    {
        if (focused)
        {
            focused = !focused;
            SwitchTo(0);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, focused ? defaultFade : 0);
            noteContentDisplay.SetActive(focused);
        }
    }

}
