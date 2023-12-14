using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerInfoSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image itemImage;

    public void SetIngredientObjectSO(IngredientObjectSO ingredientObjectSO) {
        itemNameText.text = ingredientObjectSO.ingredientName;
        itemImage.sprite = ingredientObjectSO.sprite;
    }

    public void SetContainerObjectSO(ContainerObjectSO containerObjectSO) {
        itemNameText.text = containerObjectSO.containerName;
        itemImage.sprite = containerObjectSO.sprite;
    }
}
