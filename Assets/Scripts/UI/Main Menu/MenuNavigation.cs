using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] protected List<TextMeshProUGUI> buttons;
    [SerializeField] protected GameObject[] lists;
    [SerializeField] protected PipSlider[] pipSliders;
    [SerializeField] protected Color highlightedColor;
    [SerializeField] protected float highlightedFontSize;
    protected Color defaultColor;
    protected float defaultFontSize;
    protected int currentButtonIndex = 0;
    protected Screens screenToSwitch = Screens.NullScreen;


    protected virtual void Start()
    {
        defaultFontSize = buttons[currentButtonIndex].fontSize;
        defaultColor = buttons[currentButtonIndex].color;

        SwitchTo(currentButtonIndex);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            OnUpPressed();

        if (Input.GetKeyDown(KeyCode.S))
            OnDownPressed();

        if (Input.GetKeyDown(KeyCode.A))
            OnLeftPressed();

        if (Input.GetKeyDown(KeyCode.D))
            OnRightPressed();

        if (Input.GetKeyDown(KeyCode.C))
            OnConfirmation();
    }

    protected virtual void OnUpPressed()
    {
        if (!CanNavigate())
            return;

        SwitchTo(currentButtonIndex == 0 ? buttons.Count - 1 : currentButtonIndex - 1);
    }

    protected virtual void OnDownPressed()
    {
        if (!CanNavigate())
            return;

        SwitchTo((currentButtonIndex + 1) % buttons.Count);
    }

    protected virtual void OnLeftPressed()
    {
        if (!CanNavigate())
            return;
    }

    protected virtual void OnRightPressed()
    {
        if (!CanNavigate())
            return;
    }

    protected virtual void OnConfirmation()
    {
        if (!CanNavigate())
            return;

        Remote();
    }

    protected virtual void SwitchTo(int index)
    {

        buttons[currentButtonIndex].color = defaultColor;
        buttons[currentButtonIndex].fontSize = defaultFontSize;

        currentButtonIndex = index;

        buttons[currentButtonIndex].color = highlightedColor;
        buttons[currentButtonIndex].fontSize = highlightedFontSize;
    }

    protected virtual void Remote()
    {
        if (screenToSwitch != Screens.NullScreen)
        {
            Invoke(nameof(ChangeScreen), 1.5f);
            MainMenu.instance.fadeScreen.FadeIn();
        }

    }

    protected virtual void ChangeScreen()
    {
        MainMenu.instance.SwitchTo(screenToSwitch);

        screenToSwitch = Screens.NullScreen;
    }

    protected virtual void ActivateList(GameObject list, bool activate = true)
    {
        TextMeshProUGUI listText = list.GetComponentInChildren<TextMeshProUGUI>();
        listText.fontSize = activate ? highlightedFontSize : defaultFontSize;
        listText.color = activate ? highlightedColor : defaultColor;
    }

    protected virtual void ActivateListByIndex(int index = -1)
    {
        for (int i = 0; i < lists.Length; i++)
            ActivateList(lists[i], index == i);
    }

    protected virtual void AddPipToCurrentList()
    {
        pipSliders[currentButtonIndex].AddPip();
    }

    protected virtual void RemovePipFromCurrentList()
    {
        pipSliders[currentButtonIndex].RemovePip();
    }

    protected virtual void HighlightCurrentPipSlider()
    {
        foreach (PipSlider slider in pipSliders)
            slider.Highlight(false);

        if(currentButtonIndex < pipSliders.Length)
            pipSliders[currentButtonIndex].Highlight();
    }

    protected virtual void ChangeOption(bool increment = true)
    {

    }

    public virtual bool CanNavigate() => !(MainMenu.instance.fadeScreen.isFadingIn || MainMenu.instance.fadeScreen.isFadingOut);
}
