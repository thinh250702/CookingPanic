using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoUI : MonoBehaviour {

    [SerializeField] private Transform rootContainer;
    [SerializeField] private TextMeshProUGUI headerText;

    [Space(10)]
    [Header("Container Info")]
    [SerializeField] private Transform containerInfoRoot;
    [SerializeField] private Transform containerInfoTemplate;

    [Space(10)]
    [Header("Station Info")]
    [SerializeField] private Transform stationInfoRoot;
    [SerializeField] private Transform stationInfoTemplate;

    [Space(10)]
    [Header("Stock Holder Info")]
    [SerializeField] private Transform stockContainerInfoRoot;
    [SerializeField] private Image barImage;
    [SerializeField] private Image stockImage;
    [SerializeField] private TextMeshProUGUI stockNameText;
    [SerializeField] private TextMeshProUGUI remainText;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Awake() {
        Debug.Log("Awake!");
        rootContainer.gameObject.SetActive(false);
        ResetAllChilds();
    }

    private void ResetAllChilds() {
        containerInfoTemplate.gameObject.SetActive(false);
        stationInfoTemplate.gameObject.SetActive(false);
        stockContainerInfoRoot.gameObject.SetActive(false);
    }

    private void Start() {
        Player.Instance.OnSelectedObjectChanged += Player_OnSelectedObjectChanged; ;
    }

    public void Player_OnSelectedObjectChanged(object sender, Player.OnSelectedObjectChangedEventArgs e) {
        //rootContainer.gameObject.SetActive(true);
        if (e.selectedObject is ContainerObject) {
            ContainerObject containerObject = e.selectedObject as ContainerObject;
            UpdateVisual(containerObject);
            
        } else if (e.selectedObject is FunctionalObject) {
            FunctionalObject functionalObject = e.selectedObject as FunctionalObject;
            UpdateVisual(functionalObject);
        } else if (e.selectedObject is IngredientObject) {
            IngredientObject ingredientObject = e.selectedObject as IngredientObject;
            UpdateVisual(ingredientObject);
        }
        else {
            rootContainer.gameObject.SetActive(false);
            ResetAllChilds();
        }
    }

    private void UpdateVisual(FunctionalObject functionalObject) {
        if (functionalObject.GetFunctionalObjectSO() != null) {
            headerText.text = functionalObject.GetFunctionalObjectSO().containerName;
            descriptionText.text = functionalObject.GetFunctionalObjectSO().description;
        } else {
            headerText.text = "";
            descriptionText.text = "";
        }

        foreach (Transform child in stationInfoRoot) {
            if (child == stationInfoTemplate) continue;
            Destroy(child.gameObject);
        }

        rootContainer.gameObject.SetActive(true);
        ResetAllChilds();
    }

    private void UpdateVisual(IngredientObject ingredientObject) {
        if (ingredientObject.GetIngredientObjectSO() != null) {
            headerText.text = ingredientObject.GetIngredientObjectSO().ingredientName;
            descriptionText.text = ingredientObject.GetIngredientObjectSO().description;
        } else {
            headerText.text = "";
            descriptionText.text = "";
        }

        rootContainer.gameObject.SetActive(true);
        ResetAllChilds();
    }

    private void UpdateVisual(ContainerObject containerObject) {
        if (containerObject.GetContainerObjectSO() != null) {
            headerText.text = containerObject.GetContainerObjectSO().containerName;
            descriptionText.text = containerObject.GetContainerObjectSO().description;
        } else {
            headerText.text = "";
            descriptionText.text = "";
        }
            
        foreach (Transform child in containerInfoRoot) {
            if (child == containerInfoTemplate) continue;
            Destroy(child.gameObject);
        }
        if (containerObject is RaycastDropContainer) {
            rootContainer.gameObject.SetActive(true);
            ResetAllChilds();
            RaycastDropContainer raycastDropContainer = containerObject as RaycastDropContainer;
            switch (raycastDropContainer.GetContainerType()) {
                case RaycastDropContainer.Type.MetalTray:
                    /*foreach (PickableObject item in raycastDropContainer.GetChildrenObject()) {
                        Debug.Log(item.name);
                        IngredientObject ingredient = item as IngredientObject;
                        Transform rowTransform = Instantiate(containerInfoTemplate, containerInfoRoot);
                        rowTransform.gameObject.SetActive(true);
                        rowTransform.GetComponent<ContainerInfoSingleUI>().SetIngredientObjectSO(ingredient.GetIngredientObjectSO());
                    }*/
                    break;
                case RaycastDropContainer.Type.ServingTray:
                    foreach (PickableObject item in raycastDropContainer.GetChildrenObject()) {
                        FoodPackage container = item as FoodPackage;
                        Transform rowTransform = Instantiate(containerInfoTemplate, containerInfoRoot);
                        rowTransform.gameObject.SetActive(true);
                        rowTransform.GetComponent<ContainerInfoSingleUI>().SetContainerObjectSO(container.GetContainerObjectSO());
                    }
                    break;
                case RaycastDropContainer.Type.AssemblyTable:
                    break;
            }
        } else if (containerObject is FoodPackage) {
            rootContainer.gameObject.SetActive(true);
            ResetAllChilds();
            foreach (PickableObject item in containerObject.GetChildrenObject()) {
                IngredientObject ingredient = item as IngredientObject;
                Transform rowTransform = Instantiate(containerInfoTemplate, containerInfoRoot);
                rowTransform.gameObject.SetActive(true);
                rowTransform.GetComponent<ContainerInfoSingleUI>().SetIngredientObjectSO(ingredient.GetIngredientObjectSO());
            }
        } else if (containerObject is StockHolderObject) {
            rootContainer.gameObject.SetActive(true);
            ResetAllChilds();
            stockContainerInfoRoot.gameObject.SetActive(true);
            StockHolderObject stockHolder = containerObject as StockHolderObject;
            StockObject stockObject = stockHolder.GetStockObject();

            stockImage.sprite = stockObject.GetStockObjectSO().sprite;
            stockNameText.text = stockObject.GetStockObjectSO().stockName;

            float maxQuantity = (float)stockObject.GetStockObjectSO().quantity;
            float currentQuantity = (float)stockObject.CurrentQuantity;

            float percentage = currentQuantity / maxQuantity;
            barImage.fillAmount = percentage;
            remainText.text = $"{(percentage * 100)}%";
            Debug.Log(currentQuantity.ToString());
        } else {
            rootContainer.gameObject.SetActive(false);
            ResetAllChilds();
        }
    }
}
