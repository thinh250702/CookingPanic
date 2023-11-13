using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerBox : FoodContainer {

    public override void Interact(Player player) {
        if (player.HasChildrenObject() && player.GetChildrenObject()[0] is IngredientObject) {
            IngredientObject playerIngredient = player.GetChildrenObject()[0] as IngredientObject;
            if (TryAddIngredient(playerIngredient.GetIngredientObjectSO())) {
                Transform containerTransform = this.GetObjectFollowTransform();
                float height = GetCurrentHeight();
                Vector3 dropPoint = new Vector3(containerTransform.position.x, containerTransform.position.y + height + .05f, containerTransform.position.z);
                playerIngredient.NormalDropObject(this, dropPoint, Quaternion.identity);
            }
        } else
        {
            this.NormalPickObject(player, Quaternion.identity);
        }
    }

    private float GetCurrentHeight() {
        float tmp = 0;
        foreach (PickableObject item in holdingObjectList) {
            tmp = tmp + item.GetComponent<BoxCollider>().bounds.size.y;
        }
        return tmp;
    }
}
