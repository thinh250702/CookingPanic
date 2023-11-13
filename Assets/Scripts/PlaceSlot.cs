using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceSlot : ContainerObject
{
    [SerializeField] private ContainerObjectSO validObjectSO;
    [SerializeField] private GameObject previewObject;
    private void Start() {
        Player.Instance.OnSelectedObjectChanged += Instance_OnSelectedObjectChanged;
    }

    private void Instance_OnSelectedObjectChanged(object sender, Player.OnSelectedObjectChangedEventArgs e) {
        if (e.selectedObject == this) {
            if (!HasChildrenObject() && Player.Instance.HasChildrenObject() && Player.Instance.GetChildrenObject()[0] is ContainerObject) {
                ContainerObject playerContainer = Player.Instance.GetChildrenObject()[0] as ContainerObject;
                if (playerContainer.GetContainerObjectSO() == validObjectSO) {
                    previewObject.SetActive(true);
                }
            }
        } else {
            previewObject.SetActive(false);
        }
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject() && player.GetChildrenObject()[0] is ContainerObject) {
            // Player is carrying something
            ContainerObject container = player.GetChildrenObject()[0] as ContainerObject;
            if (container.GetContainerObjectSO() == validObjectSO) {
                container.DropConcaveContainer(this, this.GetObjectFollowTransform().position, Quaternion.identity);
            } else {
                Debug.Log("Cannot put object here");
            }
        } else {
            // Player is not carrying anything
        }
    }
}
