using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class SelectedObjectVisual : MonoBehaviour
{
    [SerializeField] private InteractableObject interactableObject;
    private Outline outline;
    private void Start() {
        Player.Instance.OnSelectedObjectChanged += Instance_OnSelectedObjectChanged;
        outline = GetComponent<Outline>();
        outline.enabled = false;
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
