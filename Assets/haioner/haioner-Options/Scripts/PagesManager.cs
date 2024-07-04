using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class PagesManager : MonoBehaviour
{
    [Header("Pages CACHE")]
    [SerializeField] private List<GameObject> pages = new List<GameObject>();
    [SerializeField] private List<GameObject> pagesButton = new List<GameObject>();

    [Header("State visual")] //Index 0 Disabled //Index 1 Enabled
    [SerializeField] private List<Sprite> spriteButtons = new List<Sprite>();
    [SerializeField] private List<Color> colorButtons = new List<Color>();

    private int _currentPageIndex = 0;

    private void Awake() => ChangeButtonVisual();

    #region pages
    public void PageButtonPress()
    {
        //Select current button
        var buttonName = EventSystem.current.currentSelectedGameObject.name;
        for (int i = 0; i < pagesButton.Count; i++)
        {
            if (buttonName == pagesButton[i].name)
                _currentPageIndex = i;
        }

        EnableOnePage();
        ChangeButtonVisual();
    }

    public void GetButtonColors(Button button, int colorValue)
    {
        var colors = button.GetComponent<Button>().colors;
        colors.normalColor = colorButtons[colorValue];
        colors.highlightedColor = colorButtons[colorValue];
        colors.pressedColor = colorButtons[colorValue];
        colors.selectedColor = colorButtons[colorValue];
        button.colors = colors;
    }

    private void EnableOnePage()
    {
        //Disable all pages
        for (int i = 0; i < pages.Count; i++)
            pages[i].SetActive(false);
        
        //Enable current page
        pages[_currentPageIndex].SetActive(true);
    }

    private void ChangeButtonVisual()
    {
        //Change Sprite
        if (spriteButtons.Count != 0)
        {
            for (int i = 0; i < pagesButton.Count; i++)
            {
                if (_currentPageIndex == i)
                    pagesButton[i].GetComponent<Image>().sprite = spriteButtons[1];
                else
                    pagesButton[i].GetComponent<Image>().sprite = spriteButtons[0];
            }
        }

        //Change Color
        if (colorButtons.Count != 0)
        {
            for (int i = 0; i < pagesButton.Count; i++)
            {
                if (_currentPageIndex == i)
                    GetButtonColors(pagesButton[i].GetComponent<Button>(), 1);
                else
                    GetButtonColors(pagesButton[i].GetComponent<Button>(), 0);
            }
        }
    }
    #endregion
}