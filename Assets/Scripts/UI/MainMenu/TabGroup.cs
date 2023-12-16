using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<MenuTab> tabButtonList;

    public Color buttonNormal;
    public Color buttonHighlight;

    public Color textNormal;
    public Color textHighlight;

    private MenuTab selectedTab;

    private void OnEnable() {
        ResetTabs();
    }

    public void OnTabEnter(MenuTab tab) {
        ResetTabs();
        tab.background.color = buttonHighlight;
        tab.buttonText.color = textHighlight;
    }

    public void OnTabExit(MenuTab tab) {
        ResetTabs();
    }

    public void OnTabSelected(MenuTab tab) {
        selectedTab = tab;
        ResetTabs();
        tab.background.color = buttonHighlight;
        tab.buttonText.color = textHighlight;
    }

    public void ResetTabs() {
        foreach (MenuTab tab in tabButtonList) {
            if (selectedTab != null && tab == selectedTab) { continue; }
            tab.background.color = buttonNormal;
            tab.buttonText.color = textNormal;
        }
    }
}
