using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDropContainer : ContainerObject {
    [SerializeField] protected bool canPick;
    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {
            // Player is carrying something
            PickableObject playerHoldingObject = player.GetChildrenObject()[0];
            RaycastHit playerRaycastHit = player.GetPlayerRaycastHit();
            Vector3 dropPosition = new Vector3(playerRaycastHit.point.x, playerRaycastHit.point.y, playerRaycastHit.point.z);
            if (playerHoldingObject is ContainerObject) {
                playerHoldingObject.DropConcaveContainer(this, dropPosition, Quaternion.identity);
            } else {
                playerHoldingObject.NormalDropObject(this, dropPosition, Quaternion.identity);
            }
        } else {
            // Player is not carrying anything - pickup that container
            if (canPick) {
                this.NormalPickObject(player, Quaternion.identity);
            }
        }
    }
}
