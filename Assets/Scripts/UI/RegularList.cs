using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegularList : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI listText;
    [SerializeField] private float defaultFontSize;
    [SerializeField] private float highlightedFontSize;
    [SerializeField] private Color defaultFontColor;
    [SerializeField] private Color highlightedFontColor;
    [Space]
    [SerializeField] private GameObject arrows;
    [SerializeField] private float defaultArrowSize = 75;
    [SerializeField] private float highlightedArrowSize = 80;
    [SerializeField] private float defaultArrowDistance = 600;
    [SerializeField] private float highlightedArrowDistance = 620;
    private List<string> possibleTexts;
    private int currentIndex = 0;

    public void SetUp(List<string> possibleTexts)
    {
        this.possibleTexts = new List<string>();

        foreach (string text in possibleTexts)
            this.possibleTexts.Add(text);

        listText.text = this.possibleTexts[currentIndex];
    }

    public void Proceed()
    {
        currentIndex = currentIndex == possibleTexts.Count - 1 ? 0 : currentIndex + 1;

        listText.text = possibleTexts[currentIndex];
    }

    public void Retract()
    {
        currentIndex = currentIndex == 0 ? possibleTexts.Count - 1 : currentIndex - 1;

        listText.text = possibleTexts[currentIndex];
    }

    public void Highlight(bool highlight = true)
    {
        listText.fontSize = highlight ? highlightedFontSize : defaultFontSize;
        listText.color = highlight ? highlightedFontColor : defaultFontColor;

        arrows.GetComponent<GridLayoutGroup>().cellSize = Vector2.one * (highlight ? highlightedArrowSize : defaultArrowSize);
        arrows.GetComponent<GridLayoutGroup>().spacing = new Vector2(highlight ? highlightedArrowDistance : defaultArrowDistance , 0);

        LayoutRebuilder.ForceRebuildLayoutImmediate(arrows.GetComponent<RectTransform>());
    }
}
