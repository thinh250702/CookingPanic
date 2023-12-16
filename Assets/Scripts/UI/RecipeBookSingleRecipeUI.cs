using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeBookSingleRecipeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    [Header("Single Recipe UI Components")]
    public Image background;
    public Image recipeImage;
    public TextMeshProUGUI recipeName;
    public TextMeshProUGUI recipeDescription;
    public TextMeshProUGUI recipePrice;

    private RecipeSO recipeSO;

    public void SetRecipeInfo(RecipeSO recipeSO) {
        this.recipeSO = recipeSO;
        recipeImage.sprite = recipeSO.recipeSprite;
        recipeName.text = recipeSO.recipeName;
        recipeDescription.text = recipeSO.description;
        recipePrice.text = String.Format("${0:0.00}", recipeSO.price); 
    }

    public RecipeSO GetRecipeSO() {
        return this.recipeSO;
    }


    public void OnPointerClick(PointerEventData eventData) {
        RecipeBookUI.Instance.OnRecipeSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        RecipeBookUI.Instance.OnRecipeEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        RecipeBookUI.Instance.OnRecipeExit(this);
    }
}
