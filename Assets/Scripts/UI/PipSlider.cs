using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class PipSlider : MonoBehaviour
{
    public int value { get; private set; } = 10;
    [SerializeField] private GameObject pipsParent;
    [SerializeField] private int defaultValue = 10;
    private void Awake()
    {
        pips = pipsParent.GetComponentsInChildren<Image>();
        value = defaultValue;

        for (int i = 0; i < MAX_PIPS - 1; i++)
            pips[i].sprite = emptyPip;

    }

    [SerializeField] private Sprite emptyPip;
    [SerializeField] private Sprite filledPip;
    [SerializeField] private float defaultPipSize = 50;
    [SerializeField] private float highlightedPipSize = 55;
    [Space]
    [SerializeField] private GameObject arrows;
    [SerializeField] private float defaultArrowSize = 75;
    [SerializeField] private float highlightedArrowSize = 80;
    [SerializeField] private float defaultArrowDistance = 600;
    [SerializeField] private float highlightedArrowDistance = 620;

    private const int MAX_PIPS = 10;
    private Image[] pips;


    private void Start()
    {
        for (int i = 0; i < value; i++)
            pips[i].sprite = filledPip;
    }

    public void RemovePip()
    {
        if (value == 0)
            return;

        value--;

        pips[value].sprite = emptyPip;
    }

    public void AddPip()
    {
        if (value == MAX_PIPS)
            return;

        value++;

        pips[value - 1].sprite = filledPip;
    }

    public void Highlight(bool highlight = true)
    {
        pipsParent.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * (highlight ? highlightedPipSize : defaultPipSize);

        LayoutRebuilder.ForceRebuildLayoutImmediate(pipsParent.GetComponent<RectTransform>());
        
        arrows.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * (highlight ? highlightedArrowSize : defaultArrowSize);
        arrows.GetComponent<GridLayoutGroup>().spacing = new Vector2(highlight ? highlightedArrowDistance : defaultArrowDistance , 0);

        LayoutRebuilder.ForceRebuildLayoutImmediate(arrows.GetComponent<RectTransform>());
    }

}
