using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class BloodstainText : MonoBehaviour
{
    [SerializeField] private Image frameImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float backgroundImageAlpha;
    [SerializeField] private float idleTime;
    [SerializeField] private float fadeSpeed;
    [SerializeField] private bool isAppearing;
    [SerializeField] private bool isDisappearing;
    [SerializeField] private float timer;

    private void OnEnable()
    {
        isAppearing = true;
        isDisappearing = false;
        timer = 10;
    }

    private void Update()
    {
        timer -= Time.unscaledDeltaTime;

        if (frameImage.color.a >= 1 && isAppearing)
        {
            timer = idleTime;
            isAppearing = false;
        }

        if (isAppearing)
        {
            frameImage.color = new Color(frameImage.color.r, frameImage.color.g, frameImage.color.b, Mathf.MoveTowards(frameImage.color.a, 1, fadeSpeed * Time.deltaTime));
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, Mathf.MoveTowards(backgroundImage.color.a, backgroundImageAlpha, fadeSpeed * Time.deltaTime));
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.MoveTowards(text.color.a, 1, fadeSpeed * Time.deltaTime));
        }
        
        if (timer < 0 && frameImage.color.a >= 1)
            StartDisappearing();

        if (isDisappearing)
        {
            frameImage.color = new Color(frameImage.color.r, frameImage.color.g, frameImage.color.b, Mathf.MoveTowards(frameImage.color.a, 0, fadeSpeed * Time.deltaTime));
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, Mathf.MoveTowards(backgroundImage.color.a, 0, fadeSpeed * Time.deltaTime));
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.MoveTowards(text.color.a, 0, fadeSpeed * Time.deltaTime));
        }

        if (isDisappearing && frameImage.color.a <= 0)
            gameObject.SetActive(false);
    }

    private void StartDisappearing() => isDisappearing = true;
    

}
