using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : PickableObject {

    [SerializeField] private IngredientObjectSO ingredientObjectSO;

    public IngredientObjectSO GetIngredientObjectSO() {
        return ingredientObjectSO;
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {
            // Player is carrying something
            if (player.GetChildrenObject()[0] is ContainerObject) {
                // Player is holding a container - put that ingredient to player's container
                Debug.Log("Player is holding an container");
                ContainerObject container = player.GetChildrenObject()[0] as ContainerObject;
                Debug.Log(container);
                if (container is RaycastDropContainer) {
                    Transform containerTransform = container.GetObjectFollowTransform();
                    Vector3 dropPoint = new Vector3(containerTransform.position.x, containerTransform.position.y + .2f, containerTransform.position.z);
                    this.NormalDropObject(container, dropPoint, Quaternion.identity);
                }
            } else {
                // Player is holding an ingredient - do nothing
            }
        } else {
            // Player is not carrying anything - pickup that ingredient
            Debug.Log("Doing Pickup");
            this.NormalPickObject(player, Quaternion.identity);
        }
    }

    public static IngredientObject SpawnKitchenObject(IngredientObjectSO ingredientObjectSO, IParentObject kitchenObjectParent, Transform spawnTransform) {
        Transform kitchenObjectTransform = Instantiate(ingredientObjectSO.prefab, spawnTransform.position, Quaternion.identity);
        IngredientObject kitchenObject = kitchenObjectTransform.GetComponent<IngredientObject>();
        kitchenObject.SetObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
