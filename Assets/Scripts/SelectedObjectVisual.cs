using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjectVisual : MonoBehaviour
{
    [SerializeField] private InteractableObject interactableObject;
    private Outline outline;
    private void Start() {
        Player.Instance.OnSelectedObjectChanged += Instance_OnSelectedObjectChanged;
        outline = GetComponent<Outline>();
    }

    private void Instance_OnSelectedObjectChanged(object sender, Player.OnSelectedObjectChangedEventArgs e) {
        if (outline != null) {
            if (e.selectedObject == interactableObject) {
                outline.enabled = true;
            } else {
                outline.enabled = false;
            }
        }
    }
}
