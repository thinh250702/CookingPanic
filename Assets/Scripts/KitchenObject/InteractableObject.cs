using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public virtual void Interact(Player player) {
        Debug.LogError("InteractableObject.Interact()");
    }

    public virtual void InteractAlternate(Player player) {
        Debug.LogError("InteractableObject.Interact()");
    }
}
