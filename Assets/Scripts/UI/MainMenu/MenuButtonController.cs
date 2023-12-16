using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuButtonController : MonoBehaviour
{
    [SerializeField] private List<MenuButton> buttonGroup;

    public Color buttonNormal;
    public Color buttonActive;

    public Color textNormal;
    public Color textActive;
    public Color textDisabled;

    [SerializeField] private TextMeshProUGUI buttonDescription;

    private void OnEnable() {
        ResetButtons();
    }

    public void OnButtonEnter(MenuButton button) {
        ResetButtons();
        button.background.color = buttonActive;
        button.buttonText.color = textActive;
        if (buttonDescription != null) {
            buttonDescription.text = button.description;
        }
    }

    public void OnButtonExit(MenuButton button) {
        ResetButtons();
    }

    public void OnButtonSelected(MenuButton button) {
        ResetButtons();
        button.background.color = buttonActive;
        button.buttonText.color = textActive;
    }

    public void ResetButtons() {
        foreach (MenuButton button in buttonGroup)
        {
            button.background.color = buttonNormal;
            button.buttonText.color = textNormal;
        }
    }

    public void SetInteractableVisual(MenuButton button, bool isInteractable) {
        if (!isInteractable) {
            button.buttonText.color = textDisabled;
        } else {
            button.buttonText.color = textNormal;
        }
    }
}
