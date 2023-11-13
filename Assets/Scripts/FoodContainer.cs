using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodContainer : ContainerObject {

    [SerializeField] private List<IngredientObjectSO> validObjectSOList;
    private List<IngredientObjectSO> ingredientObjectList;

    private void Awake() {
        ingredientObjectList = new List<IngredientObjectSO>();
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
