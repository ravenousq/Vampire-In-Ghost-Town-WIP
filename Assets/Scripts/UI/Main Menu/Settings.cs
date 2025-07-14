using UnityEngine;

public class Settings : MenuNavigation
{
    protected override void Update()
    {
        base.Update();

        if(MainMenu.instance.fadeScreen.isFadingIn || MainMenu.instance.fadeScreen.isFadingOut)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            Remote();
            MainMenu.instance.fadeScreen.FadeIn();
        }
    }

    protected override void Remote()
    {
        switch (currentButtonIndex)
        {
            case 0:
                screenToSwitch = Screens.GameSettingsScreen;
                break;
            case 1:
                screenToSwitch = Screens.VideoSettingsScreen;
                break;
            case 2:
                screenToSwitch = Screens.AudioSettingsScreen;
                break;
            case 3:
                screenToSwitch = Screens.TitleScreen;
                break;
            default:
                Debug.LogWarning("Unknown button index: " + currentButtonIndex);
                break;
        }

        base.Remote();
    }
}
