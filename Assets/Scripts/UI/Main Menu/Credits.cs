using UnityEngine;

public class Credits : MenuNavigation
{
    [SerializeField] private RectTransform credtisText;
    [SerializeField] private float scrollSpeed = 200;
    private float defaultScrollSpeed;
    private float offScreenPosition;

    private bool switchingScreens;

    protected override void Start()
    {
        base.Start();

        defaultScrollSpeed = scrollSpeed;
    }

    private void OnEnable()
    {
        switchingScreens = false;
        credtisText.anchoredPosition = new Vector2(0, -credtisText.rect.height / 3 * 2);
        offScreenPosition = credtisText.rect.height / 2;
    }

    protected override void Update()
    {
        if (switchingScreens)
            return;

        base.Update();

        if (Input.GetKey(KeyCode.S) || Input.GetKeyUp(KeyCode.W))
            scrollSpeed = Mathf.Clamp(scrollSpeed * 2, defaultScrollSpeed / 2, defaultScrollSpeed * 2);
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKey(KeyCode.W))
            scrollSpeed = Mathf.Clamp(scrollSpeed / 2, defaultScrollSpeed / 2, defaultScrollSpeed * 2);


        if (!MainMenu.instance.fadeScreen.isFadingOut)
            credtisText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (credtisText.anchoredPosition.y > offScreenPosition || Input.GetKeyDown(KeyCode.Space))
            Remote();

        if (Input.GetKeyDown(KeyCode.Escape))
            Remote();
    }


    protected override void Remote()
    {
        if (switchingScreens)
            return;

        switchingScreens = true;

        screenToSwitch = Screens.TitleScreen;

        base.Remote();
    }
}
