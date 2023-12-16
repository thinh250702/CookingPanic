using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionalObject : InteractableObject
{
    [SerializeField] protected ContainerObjectSO functionalObjectSO;

    [SerializeField] protected Material[] signalMat;
    [SerializeField] protected FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] protected BurningRecipeSO[] burningRecipeSOArray;

    protected Animator animator;

    public ContainerObjectSO GetFunctionalObjectSO() {
        return functionalObjectSO;
    }

    public bool HasRecipeWithInput(IngredientObjectSO inputObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputObjectSO);
        return fryingRecipeSO != null;
    }

    public FryingRecipeSO GetFryingRecipeSOWithInput(IngredientObjectSO inputObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    public BurningRecipeSO GetBurningRecipeSOWithInput(IngredientObjectSO inputObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
