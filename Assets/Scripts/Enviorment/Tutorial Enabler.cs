using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Linq;

public class TutorialEnabler : MonoBehaviour
{
    [SerializeField] private GameObject tutorial;
    [SerializeField] private bool destoryAfterRead = true;
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
        if (Input.GetKeyDown(KeyCode.C) && !asleep && requiredItems.Length == 0)
            DestroyTutorial();

        foreach (Image image in images)
            image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.MoveTowards(image.color.a, !asleep && HasItems() ? 1 : 0, Time.unscaledDeltaTime));

        foreach (TextMeshProUGUI text in texts)
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.MoveTowards(text.color.a, !asleep && HasItems() ? 1 : 0, Time.unscaledDeltaTime));

        if (images[0].color.a != 0 && !read)
        {
            read = true;
            LevelManager.instance.TutorialRead(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CanBeTriggered(other))
            asleep = false;
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
            asleep = true;
    }

    private bool CanBeTriggered(Collider2D other) => other.GetComponent<Player>() && LevelManager.instance.showTutorials && (destoryAfterRead ? !read : true);

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
