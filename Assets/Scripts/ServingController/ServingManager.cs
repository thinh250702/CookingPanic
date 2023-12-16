using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServingManager : MonoBehaviour {
    public static ServingManager Instance { get; private set; }
    public ContainerObjectSO validObjectSO;

    public class OnOrderChangedEventArgs : EventArgs {
        public int slotIndex;
        public bool isCompleted;
    }
    public event EventHandler<OnOrderChangedEventArgs> OnOrderChanged;

    public class OnStarChangedEventArgs : EventArgs {
        public int slotIndex;
        public int starNumber;
    }
    public event EventHandler<OnStarChangedEventArgs> OnStarChanged;

    // event to play sound effect
    public event EventHandler OnServingSuccess;
    public event EventHandler OnServingFailed;

    [SerializeField] private RecipeListSO recipeListSO;
    [SerializeField] private CustomerListSO customerListSO;
    [SerializeField] private List<ServingSlot> servingSlotList;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        /*foreach (var slot in servingSlotList) {
            Debug.Log($"{slot.name}[{string.Join(",", slot.CurrentCustomer.GetCustomerOrderList())}]");
        }*/
        /*if (Input.GetKeyDown(KeyCode.Space)) {
            
        }*/
    }

    public void UpdateOrderSummary(int index, bool isCompleted) {
        OnOrderChanged?.Invoke(this, new OnOrderChangedEventArgs {
            slotIndex = index,
            isCompleted = isCompleted
        });
    }

    public void UpdateStarVisual(int index, int number) {
        OnStarChanged?.Invoke(this, new OnStarChangedEventArgs {
            slotIndex = index,
            starNumber = number
        });
    }

    public bool ServeOrder(ContainerObject servingTray, List<RecipeSO> customerOrder) {
        if (servingTray.GetChildrenObject().Count == customerOrder.Count) {
            Debug.Log("Have the same number of item");
            // The number of item in the tray equal the number of order item
            bool trayItemsMatchesRecipe = true;
            foreach (PickableObject item in servingTray.GetChildrenObject()) {
                // Loop through each item in the tray
                if (item.TryGetFoodPackage(out FoodPackage foodPackage)) {
                    Debug.Log("Item in the tray is type FoodContainer");
                    // The item is type FoodContainer
                    foreach (RecipeSO orderItem in customerOrder) {
                        // Loop through each item in order
                        var list1 = orderItem.componentList.Except(foodPackage.GetIngredientObjectSOList());
                        var list2 = foodPackage.GetIngredientObjectSOList().Except(orderItem.componentList);
                        var resultList = list1.Concat(list2).ToList();
                        // Compare two list of IngredientObjectSO
                        if (resultList.Count > 0) {
                            // There are some differences between 2 list
                            Debug.Log("differences");
                            trayItemsMatchesRecipe = false;
                            break;
                        } else {
                            // There are no difference between 2 list
                            Debug.Log("no difference");
                        }
                    }
                } else {
                    trayItemsMatchesRecipe = false;
                    break;
                }
            }
            if (trayItemsMatchesRecipe) {
                OnServingSuccess?.Invoke(this, EventArgs.Empty);
                return true;
            } else {
                OnServingFailed?.Invoke(this, EventArgs.Empty);
                return false;
            }
        }
        OnServingFailed?.Invoke(this, EventArgs.Empty);
        return false;
    }

    public RecipeListSO GetRecipeListSO() {
        return recipeListSO;
    }

    public CustomerListSO GetCustomerListSO() {
        return customerListSO;
    }

    public List<ServingSlot> GetServingSlotList() {
        return servingSlotList;
    }
}
