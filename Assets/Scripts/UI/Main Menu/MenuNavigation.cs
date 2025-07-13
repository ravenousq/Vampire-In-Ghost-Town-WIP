using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] protected List<TextMeshProUGUI> buttons;
    [SerializeField] protected Color highlightColor;
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
            SwitchTo(currentButtonIndex == 0 ? buttons.Count - 1 : currentButtonIndex - 1);

        if (Input.GetKeyDown(KeyCode.S))
            SwitchTo((currentButtonIndex + 1) % buttons.Count);
    }

    protected virtual void SwitchTo(int index)
    {

        buttons[currentButtonIndex].color = defaultColor;
        buttons[currentButtonIndex].fontSize = defaultFontSize;

        currentButtonIndex = index;

        buttons[currentButtonIndex].color = highlightColor;
        buttons[currentButtonIndex].fontSize = highlightedFontSize;
    }

    protected virtual void Remote()
    {
        Invoke(nameof(ChangeScreen), 1.5f);
    }

    protected virtual void ChangeScreen()
    {
        if (screenToSwitch != Screens.NullScreen)
            MainMenu.instance.SwitchTo(screenToSwitch);

        screenToSwitch = Screens.NullScreen; //possible issue here
    }
}
