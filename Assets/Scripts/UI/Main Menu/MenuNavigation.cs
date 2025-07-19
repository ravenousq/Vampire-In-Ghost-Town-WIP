using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Language
{
    English = 0,
    Polish = 1,
    Japanese = 2
}
public class MenuNavigation : MonoBehaviour
{
    protected virtual void Awake()
    {
        for (int i = 0; i < Enum.GetValues(typeof(Language)).Length; i++)
            languages.Add(((Language)i).ToString());

        booleans.Add("Enabled");
        booleans.Add("Disabled");

        for (int i = 0; i < Enum.GetValues(typeof(FullScreenMode)).Length; i++)
            screenModes.Add(MainMenu.AddSpacesToEnumName(((FullScreenMode)i).ToString()));

        foreach (var res in possibleResolutions)
            resolutions.Add($"{res[0]} x {res[1]}");
    }

    [SerializeField] protected List<TextMeshProUGUI> buttons;
    [SerializeField] protected RegularList[] lists;
    [SerializeField] protected PipSlider[] sliders;
    [SerializeField] protected Color highlightedColor;
    [SerializeField] protected float highlightedFontSize;
    protected Color defaultColor;
    protected float defaultFontSize;
    protected int currentButtonIndex = 0;
    protected Screens screenToSwitch = Screens.NullScreen;
    protected List<string> languages = new List<string>();
    protected List<string> booleans = new List<string>();
    protected List<string> screenModes = new List<string>();
    protected List<string> resolutions = new List<string>();
    protected int[][] possibleResolutions = new int[][]
    {
        new int[2] { 1920, 1080 },
        new int[2] { 1920, 1200 },
        new int[2] { 1920, 1440 },
        new int[2] { 1024, 768 },
        new int[2] { 1152, 864 },
        new int[2] { 1176, 664 },
        new int[2] { 1280, 1440 },
        new int[2] { 1280, 720 },
        new int[2] { 1280, 768 },
        new int[2] { 1280, 800 },
        new int[2] { 1280, 960 },
        new int[2] { 1280, 1024 },
        new int[2] { 1360, 768 },
        new int[2] { 1366, 768 },
        new int[2] { 1440, 900 },
        new int[2] { 1440, 1080 },
        new int[2] { 1600, 900 },
        new int[2] { 1600, 1024 },
        new int[2] { 1600, 1200 },
        new int[2] { 1680, 1050 },
        new int[2] { 800, 600 },
        new int[2] { 640, 480 },
        new int[2] { 720, 480 },
        new int[2] { 720, 576 },
        new int[2] { 800, 600 },
    };


    protected virtual void Start()
    {
        if (buttons.Count == 0)
            return;

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
        if (!CanNavigate() || buttons.Count == 0)
            return;

        SwitchTo(currentButtonIndex == 0 ? buttons.Count - 1 : currentButtonIndex - 1);
    }

    protected virtual void OnDownPressed()
    {
        if (!CanNavigate() || buttons.Count == 0)
            return;

        SwitchTo((currentButtonIndex + 1) % buttons.Count);
    }

    protected virtual void OnLeftPressed()
    {
        if (!CanNavigate())
            return;

        if (IsListActive())
            lists[currentButtonIndex].Retract();

        if (IsSliderActive())
            sliders[currentButtonIndex].RemovePip();
    }

    protected virtual void OnRightPressed()
    {
        if (!CanNavigate())
            return;

        if (IsListActive())
            lists[currentButtonIndex].Proceed();

        if (IsSliderActive())
            sliders[currentButtonIndex].AddPip();
    }

    protected virtual void OnConfirmation()
    {
        if (!CanNavigate())
            return;

        Remote();
    }

    protected virtual void SwitchTo(int index)
    {        
        if (buttons.Count == 0)
            return;

        buttons[currentButtonIndex].color = defaultColor;
        buttons[currentButtonIndex].fontSize = defaultFontSize;

        if (IsListActive())
            lists[currentButtonIndex].Highlight(false);

        if (IsSliderActive())
            sliders[currentButtonIndex].Highlight(false);

        currentButtonIndex = index;

        buttons[currentButtonIndex].color = highlightedColor;
        buttons[currentButtonIndex].fontSize = highlightedFontSize;

        if (IsListActive())
            lists[currentButtonIndex].Highlight();

        if (IsSliderActive())
            sliders[currentButtonIndex].Highlight();
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

    protected virtual void AddPipToCurrentList()
    {
        sliders[currentButtonIndex].AddPip();
    }

    protected virtual void RemovePipFromCurrentList()
    {
        sliders[currentButtonIndex].RemovePip();
    }

    protected virtual void ChangeOption(bool increment = true)
    {

    }

    public virtual bool CanNavigate() => !(MainMenu.instance.fadeScreen.isFadingIn || MainMenu.instance.fadeScreen.isFadingOut);

    protected virtual bool IsListActive() => lists.Length - 1 >= currentButtonIndex;
    
    protected virtual bool IsSliderActive() => sliders.Length - 1 >= currentButtonIndex;
}
