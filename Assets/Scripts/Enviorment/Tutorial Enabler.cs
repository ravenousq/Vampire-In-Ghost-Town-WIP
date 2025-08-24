using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Linq;

public class TutorialEnabler : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;
    [SerializeField] private bool destoryAfterRead;
    [SerializeField] private ItemData[] requiredItems;
    private Image[] images;
    private TextMeshProUGUI[] texts;
    private bool asleep = true;
    private bool read;

    public void Start()
    {
        tutorial.gameObject.SetActive(true);

        images = tutorial.GetComponentsInChildren<Image>();
        texts = tutorial.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (Image image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        foreach (TextMeshProUGUI text in texts)
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }

    private void Update()
    {
        //        Debug.Log(HasItems());

        foreach (Image image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, asleep ? 0 : 1, Time.unscaledDeltaTime));

        foreach (TextMeshProUGUI text in texts)
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.MoveTowards(text.color.a, asleep ? 0 : 1, Time.unscaledDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CanBeTriggered(other))
        {
            asleep = false;
            read = true;
            LevelManager.instance.TutorialRead(this);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (CanBeTriggered(other) && asleep)
        {
            asleep = false;
            read = true;
            LevelManager.instance.TutorialRead(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
            asleep = true;
    }

    private bool CanBeTriggered(Collider2D other) => other.GetComponent<Player>() && HasItems() && LevelManager.instance.showTutorials && (destoryAfterRead ? !read : true);

    private bool HasItems()
    {
        foreach (ItemData item in requiredItems)
            if (!Inventory.instance.HasItem(item))
                return false;

        return true;
    }

    public void DestroyTutorial()
    {
        Destroy(tutorial); 
        Destroy(gameObject);
    }
    
    private void OnValidate()
    {
        gameObject.name = $"{(tutorial == null ? "Empty" : tutorial.gameObject.name)} Tutorial Enabler";
    }
}
