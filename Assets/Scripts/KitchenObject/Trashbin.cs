using System;
using UnityEngine;

public class Trashbin : ContainerObject
{
    public static event EventHandler OnAnyObjectTrashed;

    private Vector3 dropPoint;
    private void Start() {
        dropPoint = new Vector3(GetObjectFollowTransform().position.x,
            GetObjectFollowTransform().position.y + 1f,
            GetObjectFollowTransform().position.z);
    }
    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {

            switch (player.GetChildrenObject()[0]) {
                case RaycastDropContainer container:
                    for (int i = container.GetChildrenObject().Count - 1; i >= 0; i--) {
                        container.GetChildrenObject()[i].NormalDropObject(this, dropPoint, Quaternion.identity);
                    }
                    break;
                case FoodPackage foodPackage:
                    for (int i = foodPackage.GetChildrenObject().Count - 1; i >= 0; i--) {
                        foodPackage.GetChildrenObject()[i].NormalDropObject(this, dropPoint, Quaternion.identity);
                    }
                    foodPackage.DropConcaveContainer(this, dropPoint, Quaternion.identity);
                    break;
                default:
                    player.GetChildrenObject()[0].NormalDropObject(this, dropPoint, Quaternion.identity);
                    break;
            }

            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
            StartCoroutine(DestroyChildrenObject(5f));
        }
    }
}
