using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockObject : InteractableObject {

    [SerializeField] private KitchenObjectSO stockObjectSO;
    [SerializeField] private Transform spawnPosition;

    public override void Interact(Player player) {
        if (player.HasChildrenObject() && player.GetChildrenObject()[0] is RaycastDropContainer) {
            // Player is holding a MetalTray
            ContainerObject container = player.GetChildrenObject()[0] as ContainerObject;
            Transform containerTransform = container.GetObjectFollowTransform();
            Vector3 dropPoint = new Vector3(containerTransform.position.x, containerTransform.position.y + .2f, containerTransform.position.z);
            PickableObject.SpawnKitchenObject(stockObjectSO, container, dropPoint);
        } else {
            // Player is not carrying anything - do nothing
            Debug.Log("Player is not carrying anything -> Spawn to player");
            //Instantiate(stockObjectSO.prefab.GetComponent<Rigidbody>(), spawnPosition.position, Quaternion.identity);
            PickableObject.SpawnKitchenObject(stockObjectSO, player, player.GetObjectFollowTransform().position);
        }
    }
}
