using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    public event EventHandler<EventArgs> OnDoorInteract;
    public override void Interact(Player player) {
        OnDoorInteract?.Invoke(this, EventArgs.Empty);
    }
}
