using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Trashbin : ContainerObject
{
    private Vector3 dropPoint;
    private void Start() {
        dropPoint = new Vector3(GetObjectFollowTransform().position.x,
            GetObjectFollowTransform().position.y + 1f,
            GetObjectFollowTransform().position.z);
    }
    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {
            if (player.GetChildrenObject()[0] is RaycastDropContainer) {
                RaycastDropContainer metalTray = player.GetChildrenObject()[0] as RaycastDropContainer;
                for (int i = metalTray.GetChildrenObject().Count - 1; i >= 0; i--) {
                    metalTray.GetChildrenObject()[i].NormalDropObject(this, dropPoint, Quaternion.identity);
                }
            } else if (player.GetChildrenObject()[0] is FoodPackage) {
                FoodPackage box = player.GetChildrenObject()[0] as FoodPackage;
                for (int i = box.GetChildrenObject().Count - 1; i >= 0; i--) {
                    box.GetChildrenObject()[i].NormalDropObject(this, dropPoint, Quaternion.identity);
                }
                box.DropConcaveContainer(this, dropPoint, Quaternion.identity);
            } else {
                player.GetChildrenObject()[0].NormalDropObject(this, dropPoint, Quaternion.identity);
            }
            StartCoroutine(DestroyChildrenObject(5f));
        }
    }
}
