using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookUI : MonoBehaviour
{
    public static RecipeBookUI Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private TabGroup tabGroup;
    [SerializeField] private TextMeshProUGUI paginationText;

    [Header("Colors")]
    [SerializeField] private Color recipeHighlight;

    [Header("Menu Button")]
    [SerializeField] private Button burgersButton;
    [SerializeField] private Button friesButton;
    [SerializeField] private Button drinksButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Single Recipe UI")]
    [SerializeField] private Transform recipeContainer;
    [SerializeField] private Transform recipeTemplate;

    [Header("Recipe Detail UI")]
    [SerializeField] private Transform recipeDetailUI;

    const int PAGE_SIZE = 3;
    private int currIndex = 1;
    private List<List<RecipeSO>> chunkedList;

    private RecipeBookSingleRecipeUI selectedRecipe;
    private List<RecipeBookSingleRecipeUI> activeRecipeGroup;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("There is more than one Instance!");
        }
        Instance = this;

        burgersButton.onClick.AddListener(() => {
            currIndex = 1;
            chunkedList = ChunkRecipeSOList(RecipeSO.Type.Burger, recipeListSO.recipeSOList, PAGE_SIZE);
            RenderRecipePage();
        });
        friesButton.onClick.AddListener(() => {
            currIndex = 1;
            chunkedList = ChunkRecipeSOList(RecipeSO.Type.Fries, recipeListSO.recipeSOList, PAGE_SIZE);
            RenderRecipePage();
        });
        drinksButton.onClick.AddListener(() => {
            currIndex = 1;
            chunkedList = ChunkRecipeSOList(RecipeSO.Type.Drink, recipeListSO.recipeSOList, PAGE_SIZE);
            RenderRecipePage();
        });
        nextButton.onClick.AddListener(() => {  
            if (currIndex < chunkedList.Count) {
                currIndex++;
            }
            RenderRecipePage();
        });
        prevButton.onClick.AddListener(() => {
            if (currIndex > 1) {
                currIndex--;
            }
            RenderRecipePage();
        });
    }

    private void Start() {
        recipeTemplate.gameObject.SetActive(false);
        tabGroup.OnTabSelected(burgersButton.GetComponent<MenuTab>());
        chunkedList = ChunkRecipeSOList(RecipeSO.Type.Burger, recipeListSO.recipeSOList, PAGE_SIZE);
        RenderRecipePage();
    }

    private List<List<RecipeSO>> ChunkRecipeSOList(RecipeSO.Type type, List<RecipeSO> recipeSOList, int size) {
        
        var filteredList = recipeSOList.Where(x => x.type == type).ToList();
        var result = new List<List<RecipeSO>>();

        for (int i = 0; i < filteredList.Count; i += size) {
            result.Add(filteredList.GetRange(i, System.Math.Min(size, filteredList.Count - i)));
        }

        return result;
    }

    private void RenderRecipePage() {
        activeRecipeGroup = new List<RecipeBookSingleRecipeUI>();

        foreach (Transform child in recipeContainer) {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in chunkedList[currIndex - 1]) {
            Transform recipeTemplateTransform = Instantiate(recipeTemplate, recipeContainer);
            recipeTemplateTransform.gameObject.SetActive(true);

            RecipeBookSingleRecipeUI singleRecipeUI = recipeTemplateTransform.GetComponent<RecipeBookSingleRecipeUI>();
            singleRecipeUI.SetRecipeInfo(recipeSO);

            activeRecipeGroup.Add(singleRecipeUI);
        }

        paginationText.text = $"{currIndex} / {chunkedList.Count}";
    }

    public void OnRecipeEnter(RecipeBookSingleRecipeUI recipe) {
        ResetRecipes();
        recipe.background.color = recipeHighlight;
    }

    public void OnRecipeExit(RecipeBookSingleRecipeUI recipe) {
        ResetRecipes();
    }

    public void OnRecipeSelected(RecipeBookSingleRecipeUI recipe) {
        selectedRecipe = recipe;
        ResetRecipes();
        recipe.background.color = recipeHighlight;

        recipeDetailUI.GetComponent<RecipeBookDetailUI>().SetRecipeDetail(recipe.GetRecipeSO());
    }

    private void ResetRecipes() {
        foreach (RecipeBookSingleRecipeUI recipe in activeRecipeGroup) {
            if (selectedRecipe != null && recipe == selectedRecipe) { continue; }
            if (recipe.background != null) {
                recipe.background.color = Color.white;
            }
        }
    }

    public void OnBackClicked() {
        gameObject.SetActive(false);
    }
}
