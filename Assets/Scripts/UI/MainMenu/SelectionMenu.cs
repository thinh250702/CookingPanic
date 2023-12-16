using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private DynamicCalendarUI calendarMenu;
    [SerializeField] private RecipeBookUI recipeBookMenu;

    [Header("Menu Button")]
    [SerializeField] private Button calendarButton;
    [SerializeField] private Button recipeButton;

    [SerializeField] private TabGroup tabGroup;

    private void Awake() {
        calendarButton.onClick.AddListener(() => {
            calendarMenu.gameObject.SetActive(true);
            recipeBookMenu.gameObject.SetActive(false);
        });
        recipeButton.onClick.AddListener(() => {
            calendarMenu.gameObject.SetActive(false);
            recipeBookMenu.gameObject.SetActive(true);
        });
    }

    private void Start() {
        tabGroup.OnTabSelected(calendarButton.GetComponent<MenuTab>());
        calendarMenu.gameObject.SetActive(true);
        recipeBookMenu.gameObject.SetActive(false);
    }
}
