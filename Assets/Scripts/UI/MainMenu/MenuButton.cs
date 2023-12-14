using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private MenuButtonController buttonsController;
    

    private bool isDisable = false;
    public Image background;
    public TextMeshProUGUI buttonText;
    public string description;

    private Button buttonComponent;

    private void Start() {
        buttonComponent = GetComponent<Button>();
    }

    private void Update() {
        if (buttonComponent.interactable) {
            isDisable = false;
        } else {
            isDisable = true;
        }
        buttonsController.SetInteractableVisual(this, buttonComponent.interactable);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!isDisable) {
            buttonsController.OnButtonSelected(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isDisable) {
            buttonsController.OnButtonEnter(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!isDisable) {
            buttonsController.OnButtonExit(this);
        }
    }
}
