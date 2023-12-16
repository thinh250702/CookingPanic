using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject {
    public enum Type {
        Burger, Fries, Drink
    }

    public string recipeName;
    public Sprite recipeSprite;
    public Type type;
    public List<IngredientObjectSO> componentList;
    public float price;
    public string description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

    private void OnValidate() {
        price = calculatePrice();
    }

    private float calculatePrice() {
        float result = 0;
        foreach (var item in componentList)
        {
            result += item.ingredientCost;
        }
        return result;
    }
    
}
