using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServingManagerSingleUI : MonoBehaviour
{
    [SerializeField] private Transform itemContainer;
    [SerializeField] private Transform itemTemplate;

    [SerializeField] private Transform starContainer;
    [SerializeField] private Transform starIconTemplate;

    private void Awake() {
        itemTemplate.gameObject.SetActive(false);
        starIconTemplate.gameObject.SetActive(false);
    }

    public void SetStarVisual(int number) {
        foreach (Transform child in starContainer) {
            if (child == starIconTemplate) continue;
            Destroy(child.gameObject);
        }
        for (int i = 0; i < number; i++)
        {
            Transform starIconTemplateTransform = Instantiate(starIconTemplate, starContainer);
            starIconTemplateTransform.gameObject.SetActive(true);
        }
    }

    public void SetRecipeSO(List<RecipeSO> orderItemList) {
        foreach (Transform child in itemContainer) {
            if (child == itemTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (RecipeSO recipeSO in orderItemList) {
            Transform itemTemplateTransform = Instantiate(itemTemplate, itemContainer);
            itemTemplateTransform.gameObject.SetActive(true);
            itemTemplateTransform.GetComponentInChildren<TextMeshProUGUI>().text = recipeSO.recipeName;
        }
    }
}
