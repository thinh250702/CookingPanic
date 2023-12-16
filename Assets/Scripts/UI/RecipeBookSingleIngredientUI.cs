using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookSingleIngredientUI : MonoBehaviour
{
    [SerializeField] private Image ingredientImage;
    [SerializeField] private TextMeshProUGUI ingredientName;

    public void SetIngredient(IngredientObjectSO ingredientObjectSO) {
        ingredientImage.sprite = ingredientObjectSO.sprite;
        ingredientName.text = ingredientObjectSO.ingredientName;
    }
}
