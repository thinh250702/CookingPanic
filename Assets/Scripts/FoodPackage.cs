using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPackage : ContainerObject {

    public enum Type {
        BurgerBox,
        FriesCarton,
        PaperCup
    }

    [SerializeField] private Type type;
    [SerializeField] private List<IngredientObjectSO> validObjectSOList;

    private List<IngredientObjectSO> ingredientObjectList;

    private void Awake() {
        ingredientObjectList = new List<IngredientObjectSO>();
    }

    public override void Interact(Player player) {
        if (player.HasChildrenObject()) {
            if (player.GetChildrenObject()[0] is IngredientObject) {
                IngredientObject playerIngredient = player.GetChildrenObject()[0] as IngredientObject;
                if (TryAddIngredient(playerIngredient.GetIngredientObjectSO())) {
                    Transform containerTransform = this.GetObjectFollowTransform();
                    float height = GetCurrentHeight();
                    Vector3 dropPoint = new Vector3(containerTransform.position.x, 
                        containerTransform.position.y + height + .05f, 
                        containerTransform.position.z);

                    // case: add ketchup or mustard
                    if (playerIngredient.GetIngredientObjectSO().ingredientName == "Ketchup" || 
                        playerIngredient.GetIngredientObjectSO().ingredientName == "Mustard") {
                        Transform sauceTransform = Instantiate(playerIngredient.GetIngredientObjectSO().prefab, dropPoint, Quaternion.identity);
                        sauceTransform.GetComponent<IngredientObject>().DropSauce(this);
                    } 
                    // case: other ingredients
                    else {
                        playerIngredient.NormalDropObject(this, dropPoint, Quaternion.identity);
                    }
                    
                }
            } else {
                PopupMessageUI.Instance.SetMessage("Can not pick up food package!");
            }
        } else {
            this.NormalPickObject(player, Quaternion.identity);
        }
    }

    public Type GetPackageType() {
        return this.type;
    }

    private float GetCurrentHeight() {
        float tmp = 0;
        foreach (PickableObject item in holdingObjectList) {
            tmp = tmp + item.GetComponent<BoxCollider>().bounds.size.y;
        }
        return tmp;
    }

    protected bool TryAddIngredient(IngredientObjectSO ingredientObjectSO) {
        if (!validObjectSOList.Contains(ingredientObjectSO)) {
            return false;
        } else {
            ingredientObjectList.Add(ingredientObjectSO);
            return true;
        }
    }

    public List<IngredientObjectSO> GetIngredientObjectSOList() {
        return this.ingredientObjectList;
    }

}
