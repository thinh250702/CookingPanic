using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationPopupUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public void Show(UnityAction confirmAction, UnityAction cancelAction) {
        this.gameObject.SetActive(true);

        // remove any existing listeners just to make sure there aren't any previous ones hanging around
        // note - this only removes listeners added through code
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        // assign the onClick listeners
        confirmButton.onClick.AddListener(() => {
            Hide();
            confirmAction();
        });
        cancelButton.onClick.AddListener(() => {
            Hide();
            cancelAction();
        });
    }

    private void Hide() {
        this.gameObject.SetActive(false);
    }
}
