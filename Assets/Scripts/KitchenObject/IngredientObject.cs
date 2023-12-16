using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : PickableObject {

    [SerializeField] private IngredientObjectSO ingredientObjectSO;
    private void Start() {
        if (TryGetComponent<Rigidbody>(out Rigidbody rigidbody)) {
            rigidbody.mass = 0.5f;
        }
    }

    public IngredientObjectSO GetIngredientObjectSO() {
        return ingredientObjectSO;
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {
            // Player is carrying something
            if (player.GetChildrenObject()[0] is RaycastDropContainer) {
                // Player is holding a container
                RaycastDropContainer container = player.GetChildrenObject()[0] as RaycastDropContainer;
                if (container.GetContainerType() == RaycastDropContainer.Type.MetalTray) {
                    // Only allow Metal Tray
                    Transform containerTransform = container.GetObjectFollowTransform();
                    Vector3 dropPoint = new Vector3(containerTransform.position.x, containerTransform.position.y + .2f, containerTransform.position.z);
                    this.NormalDropObject(container, dropPoint, Quaternion.identity);
                } else {
                    PopupMessageUI.Instance.SetMessage("Can't pick ingredient!");
                }
            } else {
                // Player is holding an ingredient - do nothing
                PopupMessageUI.Instance.SetMessage("Can't pick ingredient!");
            }
        } else {
            // Player is not carrying anything - pickup that ingredient
            this.NormalPickObject(player, Quaternion.identity);
        }
    }

    public static IngredientObject SpawnKitchenObject(IngredientObjectSO ingredientObjectSO, IParentObject kitchenObjectParent, Transform spawnTransform, Quaternion rotation) {
        Transform kitchenObjectTransform = Instantiate(ingredientObjectSO.prefab, spawnTransform.position, rotation);
        IngredientObject kitchenObject = kitchenObjectTransform.GetComponent<IngredientObject>();
        kitchenObject.SetObjectParent(kitchenObjectParent);
        return kitchenObject;
    }
}
