using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryStateObject : InteractableObject
{
    public event EventHandler<EventArgs> OnObjectInteract;
    public override void Interact(Player player) {
        OnObjectInteract?.Invoke(this, EventArgs.Empty);
    }
    
}
