using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    private void Awake()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 1);

        defaultFadeInSpeed = fadeInSpeed;
        defaultFadeOutSpeed = fadeOutSpeed;
    }

    [SerializeField] private float fadeInSpeed = 1;
    [SerializeField] private float fadeOutSpeed = 1;
    [SerializeField] private bool fadeOnStart = false;
    public bool isFadingIn { get; private set; }
    public bool isFadingOut { get; private set; }
    public float defaultFadeInSpeed { get; private set; }
    public float defaultFadeOutSpeed { get; private set; }
    private Image fadeImage;

    private void Start()
    {
        if (fadeOnStart)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
            FadeOut();
        }
    }

    private void Update()
    {
        if (isFadingIn)
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.MoveTowards(fadeImage.color.a, 1, fadeInSpeed * Time.unscaledDeltaTime));

        if (isFadingOut)
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.MoveTowards(fadeImage.color.a, 0, fadeOutSpeed * Time.unscaledDeltaTime));

        if (fadeImage.color.a >= 1 || fadeImage.color.a <= 0)
        {
            isFadingIn = false;
            isFadingOut = false;
            fadeInSpeed = defaultFadeInSpeed;
            fadeOutSpeed = defaultFadeOutSpeed;
        }
    }

    public void FadeIn(float speed = -1)
    {
        isFadingIn = true;
        fadeInSpeed = speed > 0 ? speed : defaultFadeInSpeed;
    }
    public void FadeOut(float speed = -1)
    {
        isFadingOut = true;
        fadeOutSpeed = speed > 0 ? speed : defaultFadeOutSpeed;
    }
}
