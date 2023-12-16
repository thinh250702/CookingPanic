using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookDetailUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Image recipeImage;

    [SerializeField] private Transform ingredientContainer;
    [SerializeField] private Transform ingredientTemplate;

    private void Start() {
        ingredientTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeDetail(RecipeSO recipeSO) {
        recipeName.text = recipeSO.recipeName;
        recipeImage.sprite = recipeSO.recipeSprite;

        foreach (Transform child in ingredientContainer) {
            if (child == ingredientTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (var item in recipeSO.componentList)
        {
            Transform ingredientTemplateTransform = Instantiate(ingredientTemplate, ingredientContainer);
            ingredientTemplateTransform.gameObject.SetActive(true);

            RecipeBookSingleIngredientUI singleIngredientUI = ingredientTemplateTransform.GetComponent<RecipeBookSingleIngredientUI>();
            singleIngredientUI.SetIngredient(item);
        }
    }
}
