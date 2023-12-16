using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastDropContainer : ContainerObject {

    public enum Type {
        MetalTray,
        ServingTray,
        AssemblyTable
    }

    [SerializeField] private bool canPick;
    [SerializeField] private Type type;

    public Type GetContainerType() {
        return this.type;
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {
            // Player is carrying something
            PickableObject playerHoldingObject = player.GetChildrenObject()[0];
            RaycastHit playerRaycastHit = player.GetPlayerRaycastHit();
            Vector3 dropPosition = new Vector3(playerRaycastHit.point.x, playerRaycastHit.point.y, playerRaycastHit.point.z);

            switch (type) {
                case Type.MetalTray:
                    // Only allow ingredient
                    if (playerHoldingObject is IngredientObject) {
                        playerHoldingObject.NormalDropObject(this, dropPosition, Quaternion.identity);
                    } else {
                        PopupMessageUI.Instance.SetMessage("Metal Tray only allow ingredient!");
                    }
                    break;
                case Type.ServingTray:
                    // Only allow food container
                    if (playerHoldingObject is FoodPackage) {
                        playerHoldingObject.DropConcaveContainer(this, dropPosition, Quaternion.identity);
                    } else {
                        PopupMessageUI.Instance.SetMessage("Serving Tray only allow food package!");
                    }
                    break;
                case Type.AssemblyTable:
                    // No constrain
                    if (playerHoldingObject is ContainerObject) {
                        playerHoldingObject.DropConcaveContainer(this, dropPosition, Quaternion.identity);
                    } else {
                        playerHoldingObject.NormalDropObject(this, dropPosition, Quaternion.identity);
                    }
                    break;
            }
        } else {
            // Player is not carrying anything - pickup that container
            if (canPick) {
                this.NormalPickObject(player, Quaternion.identity);
            }
        }
    }
}
